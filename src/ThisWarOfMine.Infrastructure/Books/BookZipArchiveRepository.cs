using System.IO.Compression;
using CSharpFunctionalExtensions;
using MediatR;
using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative;
using ThisWarOfMine.Domain.Narrative.Events.Options;
using ThisWarOfMine.Infrastructure.Books.Options;

namespace ThisWarOfMine.Infrastructure.Books;

internal sealed class BookZipArchiveRepository : DispatchableRepository<Book, Guid>, IBookRepository
{
    private readonly IBookNameResolver _bookNameResolver;
    private readonly IZipBookCreator _zipBookCreator;
    private readonly IOptionDataSerializer _optionDataSerializer;

    public BookZipArchiveRepository(
        IMediator mediator,
        IBookNameResolver bookNameResolver,
        IZipBookCreator zipBookCreator,
        IOptionDataSerializer optionDataSerializer
    )
        : base(mediator)
    {
        _bookNameResolver = bookNameResolver;
        _zipBookCreator = zipBookCreator;
        _optionDataSerializer = optionDataSerializer;
    }

    protected override Task SaveAsync(Book aggregate, CancellationToken cancellationToken)
    {
        var file = _bookNameResolver.IfNotExistsGetFileNameFor(aggregate);
        if (file.HasValue)
        {
            return _zipBookCreator.CreateAsync(file.Value, aggregate);
        }

        return Task.CompletedTask;
    }

    public override Task<Result<Book, Error>> LoadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var file = _bookNameResolver.GetFileNameFor(id);
        using var archive = ZipFile.OpenRead(file);
        return LoadAsync(id, archive, cancellationToken);
    }

    public async Task<Result<Book, Error>> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        foreach (var (guid, file) in _bookNameResolver.GetPossibleBookArchives())
        {
            using var archive = ZipFile.OpenRead(file);
            if (archive.Comment == name)
            {
                return await LoadAsync(guid, archive, cancellationToken);
            }
        }

        return Error.Because($"Cannot find book with a name: {name}");
    }

    private Task<Result<Book, Error>> LoadAsync(Guid id, ZipArchive archive, CancellationToken cancellationToken)
    {
        var sections = GetSections();
        return Book.Create(id, archive.Comment, sections.Length)
            .Bind(LoadingStories)
            .Bind(LoadingAlternatives)
            .Tap(x => x.Commit());

        #region Local Functions

        Section[] GetSections() =>
            archive
                .Entries.Select(AsSections)
                .GroupBy(x => x.Section)
                .Select(group =>
                    group.Key with
                    {
                        OptionIds = group.Where(x => x.OptionId.HasValue).Select(x => x.OptionId!.Value)
                    }
                )
                .ToArray();

        Task<Result<Book, Error>> LoadingStories(Book book)
        {
            var results = sections.Select(entry =>
                book.AddStory(entry.StoryNumber).Bind(x => x.TranslateStory(entry.StoryNumber, entry.Language))
            );
            var finalResult = Result.Combine(results).Map(_ => book);
            return Task.FromResult(finalResult);
        }

        async Task<Result<Book, Error>> LoadingAlternatives(Book book)
        {
            var container = new List<UnitResult<Error>>();
            foreach (var section in sections)
            {
                if (archive.GetEntry(section.GetContentPath()) is not { } entry)
                {
                    return Error.Because("Cannot find entry for alternative");
                }

                var (storyNumber, language, alternativeId, _) = section;
                var text = await GetTextAsync(entry, cancellationToken);
                var result = await book.AddTranslationAlternative(storyNumber, language, alternativeId, text)
                    .Bind(async _ =>
                    {
                        var tasks = section
                            .GetOptionPaths()
                            .Select(optionPath =>
                            {
                                var optionEntry = archive.GetEntry(optionPath);
                                return _optionDataSerializer.DeserializeAsync(optionEntry!, cancellationToken);
                            })
                            .ToArray();

                        var options = new List<IOptionData>(tasks.Length);
                        foreach (var task in tasks)
                        {
                            options.Add(await task);
                        }

                        return Result.Combine(
                            options
                                .OrderBy(x => x.Order)
                                .Select(option =>
                                    book.AddAlternativeOption(storyNumber, language, alternativeId, option)
                                )
                        );
                    });
                container.Add(result);
            }

            return Result.Combine(container).Map(() => book);
        }

        #endregion
    }

    #region Helpers

    private static async Task<string> GetTextAsync(ZipArchiveEntry entry, CancellationToken cancellationToken)
    {
        await using var stream = entry.Open();
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync(cancellationToken);
    }

    private static (Section Section, Guid? OptionId) AsSections(ZipArchiveEntry entry)
    {
        if (entry.FullName.Split('/') is not { Length: > 2 } sections)
        {
            throw new InvalidOperationException($"Cannot map sections: `{entry.FullName}`");
        }

        Guid? guid = null;
        if (sections.Length == 5)
        {
            guid = Guid.Parse(sections[4]);
        }

        var section = new Section(
            short.Parse(sections[0]),
            Language.FromShortName(sections[1]),
            Guid.Parse(sections[2])
        );
        return (section, guid);
    }

    private readonly record struct Section(
        short StoryNumber,
        Language Language,
        Guid AlternativeId,
        IEnumerable<Guid>? OptionIds = null
    )
    {
        public override string ToString() => $"{StoryNumber}/{Language.ShortName}/{AlternativeId}/";

        public string GetContentPath() => $"{ToString()}content";

        public IEnumerable<string> GetOptionPaths()
        {
            var baseString = ToString();
            return OptionIds?.Select(optionId => $"{baseString}options/{optionId}") ?? Array.Empty<string>();
        }
    }

    #endregion
}
