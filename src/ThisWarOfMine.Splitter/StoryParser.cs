using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative;
using ThisWarOfMine.Splitter.Options;

namespace ThisWarOfMine.Splitter;

internal sealed class StoryParser : IStoryParser
{
    private readonly IOptionParser _optionParser;

    public StoryParser(IOptionParser optionParser)
    {
        _optionParser = optionParser;
    }

    public Result<Story> ParseIn(Book book, Language language, IReadOnlyCollection<string> rows)
    {
        var body = rows.Skip(1).ToArray();
        var indexOfFirstOption = IndexOfFirstOption(body);

        return rows.TryFirst()
            .ToResult("Rows are empty")
            .Bind(ParsingStoryNumber)
            .Bind(AddingNewStory)
            .Bind(TranslatingStory)
            .Bind(WritingStory)
            .Bind(DefiningOptions);

        Result<short> AddingNewStory(short number)
        {
            return book.AddStory(number).Map(_ => number).MapError(x => x.Message);
        }

        Result<short> TranslatingStory(short number)
        {
            return book.TranslateStory(number, language).Map(_ => number).MapError(x => x.Message);
        }

        Result<(short StoryNumber, Guid AlternativeId)> WritingStory(short number)
        {
            var contentRows = body[..indexOfFirstOption];

            return Result
                .SuccessIf(contentRows.Any, "Content of the story should be always present")
                .Bind(AddingNewAlternative)
                .Map(alternative => (number, alternative.Id));

            Result<Alternative> AddingNewAlternative()
            {
                return book.AddTranslationAlternative(
                        number,
                        language,
                        Guid.NewGuid(),
                        string.Join(Environment.NewLine, contentRows)
                    )
                    .MapError(x => x.Message);
            }
        }

        Result<Story> DefiningOptions((short Number, Guid AlternativeId) data)
        {
            var (number, alternativeId) = data;
            var optionRows = body[indexOfFirstOption..];
            if (!optionRows.Any())
            {
                throw new InvalidOperationException("The story should have at least one option");
            }

            var order = 0;
            foreach (var optionRow in optionRows)
            {
                var result = _optionParser
                    .Parse(optionRow, order)
                    .Bind(optionData =>
                        book.AddAlternativeOption(number, language, alternativeId, optionData).MapError(x => x.Message)
                    );

                if (result.IsFailure)
                {
                    return Result.Failure<Story>(result.Error);
                }

                order++;
            }

            return book.StoryBy(number).MapError(x => x.Message);
        }
    }

    private static Result<short> ParsingStoryNumber(string source) =>
        Result.SuccessIf(short.TryParse(source, out var number), number, $"Cannot parse title as number: {source}");

    private static int IndexOfFirstOption(IReadOnlyList<string> body)
    {
        var index = 0;
        while (index < body.Count)
        {
            if (body[index].StartsWith(Constants.OptionMarker))
            {
                return index;
            }

            index++;
        }

        return -1;
    }
}
