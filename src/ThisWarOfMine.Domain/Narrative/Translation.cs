using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Domain.Narrative;

public sealed class Translation
{
    private readonly List<Alternative> _alternatives = new();

    public Language Language { get; private init; } = null!;
    public IReadOnlyCollection<Alternative> Alternatives => _alternatives.AsReadOnly();
    public Maybe<Alternative> Original => _alternatives.TryFirst(x => x.IsOriginal);
    public bool IsEmpty => !_alternatives.Any();
    public Story Story { get; private init; }

    private Translation(Story story) => Story = story;

    public Alternative Retell(string text)
    {
        var alternative = Alternative.Create(this, text);
        _alternatives.Add(alternative);
        return alternative;
    }

    internal static Translation Create(Story story, Language language) => new(story) { Language = language };
}