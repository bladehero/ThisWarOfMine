using CSharpFunctionalExtensions;
using ThisWarOfMine.Contracts;
using ThisWarOfMine.Contracts.Narrative;

namespace ThisWarOfMine.Splitter;

internal sealed class StoryParser : IStoryParser
{
    private const char OptionMarker = '?';

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
            // TODO: extract to specific strategies
            if (optionRow.StartsWith(OptionMarker))
            {
                alternative.Options.AppendWithText(optionRow);
            }
            else
            {
                alternative.Options.Note(optionRow);
            }
        }
    }

    private static Maybe<int> TryParseAsNumber(string source) =>
        int.TryParse(source, out var number) ? number : Maybe.None;

    private static int IndexOfFirstOption(IReadOnlyList<string> body)
    {
        var index = 0;
        while (index < body.Count)
        {
            if (body[index].StartsWith(OptionMarker))
            {
                return index;
            }

            index++;
        }

        return -1;
    }
}