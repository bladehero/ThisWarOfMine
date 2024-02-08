namespace ThisWarOfMine.Contracts.Narrative;

public sealed class Story
{
    private readonly List<Alternative> _alternatives = new();

    public int Number { get; init; }
    public IReadOnlyCollection<Alternative> Alternatives => _alternatives.AsReadOnly();
    public IReadOnlyCollection<Option> Options { get; set; } = new List<Option>();

    private Story() { }

    public static Story Create(int number, Language language, IEnumerable<string> rows) =>
        new()
        {
            Number = number,
            _alternatives =
            {
                new Alternative
                {
                    Language = language,
                    IsOriginal = true,
                    Text = string.Join(Environment.NewLine, rows)
                }
            }
        };
}