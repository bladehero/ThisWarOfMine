namespace ThisWarOfMine.Contracts.Narrative;

public sealed class Alternative
{
    public string Text { get; init; } = null!;
    public Language Language { get; init; } = null!;
    public bool IsOriginal { get; init; }
}