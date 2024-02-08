using CSharpFunctionalExtensions;
using ThisWarOfMine.Contracts;
using ThisWarOfMine.Contracts.Narrative;

namespace ThisWarOfMine.Splitter;

internal sealed class StoryCreator : IStoryCreator
{
    public Story Create(Language language, IReadOnlyCollection<string> rows)
    {
        var title = rows.First();
        var body = rows.Skip(1);
        var number = TryParseAsNumber(title);
        if (number.HasNoValue)
        {
            throw new InvalidOperationException($"Cannot parse title as number: {title}");
        }

        return Story.Create(number.Value, language, body);
    }

    private static Maybe<int> TryParseAsNumber(string source) =>
        int.TryParse(source, out var number) ? number : Maybe.None;
}