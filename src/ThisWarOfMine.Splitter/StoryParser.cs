using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain;
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

    public void ParseIn(Book book, Language language, IReadOnlyCollection<string> rows)
    {
        var title = rows.First();
        var number = TryParseAsNumber(title);
        if (number.HasNoValue)
        {
            throw new InvalidOperationException($"Cannot parse title as number: {title}");
        }

        var translation = book.NewStory(number.Value).TranslateTo(language);

        var body = rows.Skip(1).ToArray();
        var indexOfFirstOption = IndexOfFirstOption(body);
        var contentRows = body[..indexOfFirstOption];
        if (!contentRows.Any())
        {
            throw new InvalidOperationException("Content of the story should be always present");
        }

        var optionRows = body[indexOfFirstOption..];
        if (!optionRows.Any())
        {
            throw new InvalidOperationException("The story should have at least one option");
        }

        var alternative = translation.Retell(string.Join(Environment.NewLine, contentRows));

        foreach (var optionRow in optionRows)
        {
            _optionParser.ParseIn(alternative.Options, optionRow);
        }
    }

    private static Maybe<int> TryParseAsNumber(string source) =>
        int.TryParse(source, out var number) ? number : Maybe.None;

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