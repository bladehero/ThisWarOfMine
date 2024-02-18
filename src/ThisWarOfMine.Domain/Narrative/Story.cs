using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative.Events;

namespace ThisWarOfMine.Domain.Narrative;

public sealed class Story : Abstraction.Entity<StoryNumber>
{
    private readonly List<Translation> _translations = new();

    public StoryNumber Number => Id;
    public IReadOnlyCollection<Translation> Translations => _translations.AsReadOnly();

    public Alternative Original =>
        _translations.TryFirst(x => x.Original.HasValue).Value.Original
            .GetValueOrThrow("Cannot get original source as it's not been initialized yet");

    public Book Book { get; private init; }

    private Story(Book book)
    {
        Book = book;
        
        Register<StoryTranslationAddedToBookEvent>(Apply);
    }

    public Result<Translation, Error> TranslationBy(Language language) => _translations.TryFirst(x => x.HasSame(language))
        .ToResult(Error.Because($"Not defined translation with a language: `{language}` for story: {Number}"));

    public bool HasTranslationTo(Language language) => _translations.Any(x => x.Language == language);

    internal static Result<Story, Error> Create(Book book, short number)
    {
        return Result.SuccessIf(
            NumberIsHigherThanZero,
            NewStory(),
            Error.Because($"Story number should be always higher than zero but found: `{number}`")
        );

        bool NumberIsHigherThanZero() => number > 0;

        Story NewStory()
        {
            StoryNumber storyNumber = number;
            var story = new Story(book) { Id = storyNumber };
            storyNumber.Assign(story);
            return story;
        }
    }

    #region Event Handling

    private void Apply(StoryTranslationAddedToBookEvent @event)
    {
        if (HasTranslationTo(@event.Language))
        {
            return;
        }
        
        var translation = Translation.Create(this, @event.Language);
        _translations.Add(translation);
    }

    #endregion
}