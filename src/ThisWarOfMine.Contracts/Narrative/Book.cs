namespace ThisWarOfMine.Contracts.Narrative;

public sealed class Book
{
    private const int DefaultBookCapacity = 2048;
    private readonly List<Story> _stories;

    public IReadOnlyCollection<Story> Story => _stories.AsReadOnly();

    private Book(int capacity) => _stories = new List<Story>(capacity);

    public static Book Create(int capacity = DefaultBookCapacity) => new(capacity);
}