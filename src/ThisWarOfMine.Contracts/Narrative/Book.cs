using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Contracts.Narrative;

public sealed class Book
{
    private const int DefaultBookCapacity = 2048;
    private readonly List<Story> _stories;

    private Maybe<Story> _maybeCurrentStory;

    public IReadOnlyCollection<Story> Stories => _stories.AsReadOnly();

    private Book(int capacity) => _stories = new List<Story>(capacity);

    public Story NewStory(int number)
    {
        _maybeCurrentStory.Execute(ThrowIfStoryNumberIsNotHigherOfLastAdded);

        var story = Story.Create(this, number);
        _maybeCurrentStory = story;
        _stories.Add(story);
        return story;

        void ThrowIfStoryNumberIsNotHigherOfLastAdded(Story currentStory)
        {
            if (currentStory.Number >= number)
            {
                throw new InvalidOperationException("New story should have always higher number than the last one");
            }
        }

    }
    public static Book Create(int capacity = DefaultBookCapacity) => new(capacity);
}