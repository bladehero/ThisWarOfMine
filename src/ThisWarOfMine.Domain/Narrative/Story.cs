using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Domain.Narrative;

public sealed class Story
{
    private readonly List<Translation> _translations = new();

    public int Number { get; private init; }
    public IReadOnlyCollection<Translation> Translations => _translations.AsReadOnly();

    public Alternative Original =>
        _translations.TryFirst(x => x.Original.HasValue).Value.Original
            .GetValueOrThrow("Cannot get original source as it's not been initialized yet");

    public Book Book { get; private init; }

    private Story(Book book) => Book = book;

    public Translation TranslateTo(Language language)
    {
        return _translations.TryFirst(x => x.Language == language).GetValueOrDefault(UseExisting, OrCreateNew);

        Translation UseExisting(Translation translation) => translation;

        Translation OrCreateNew()
        {
            var translation = Translation.Create(this, language);
            _translations.Add(translation);
            return translation;
        }
    }

    internal static Story Create(Book book, int number)
    {
        if (number <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(number));
        }

        return new Story(book) { Number = number };
    }
}