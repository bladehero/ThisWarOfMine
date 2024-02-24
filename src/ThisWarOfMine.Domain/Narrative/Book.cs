using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative.Events;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Domain.Narrative
{
    public sealed class Book : AggregateRoot<Guid>
    {
        private const int DefaultBookCapacity = 2048;
        private readonly List<Story> _stories;

        public string Name { get; private set; } = null!;
        public IReadOnlyCollection<Story> Stories => _stories.AsReadOnly();

        private Book(int capacity)
        {
            _stories = new List<Story>(capacity);

            Register<BookCreatedEvent>(Apply);
            Register<StoryAddedToBookEvent>(Apply);
            Register<StoryTranslationAddedToBookEvent>(Apply);
            Register<TranslationAlternativeAddedToBookEvent, Alternative>(Apply);
            Register<AlternativeOptionAddedToBookEvent>(Apply);
        }

        #region Accessing

        public Result<Story, Error> StoryBy(StoryNumber number)
        {
            if (number.Assigned)
            {
                return number.Story.Value;
            }

            return _stories
                .TryFirst(story => story.HasSame(number))
                .ToResult(Error.Because($"There is no story with number: {number}"));
        }

        #endregion

        #region Mutation

        public Result<Book, Error> AddStory(StoryNumber number)
        {
            return Result
                .FailureIf(
                    LastStoryNumberIsHigherThanNewOne,
                    this,
                    Error.Because(
                        "Number of newly added story should be always higher than any number of existing stories"
                    )
                )
                .Tap(() => ApplyChange(new StoryAddedToBookEvent(Id, DateTime.UtcNow, number)));

            bool LastStoryNumberIsHigherThanNewOne() => _stories.LastOrDefault()?.Number > number;
        }

        public Result<Book, Error> TranslateStory(StoryNumber number, Language language)
        {
            return StoryBy(number)
                .Bind(story =>
                    ApplyChange(new StoryTranslationAddedToBookEvent(Id, DateTime.UtcNow, story.Number, language))
                )
                .Map(() => this);
        }

        public Result<Alternative, Error> AddTranslationAlternative(
            StoryNumber number,
            Language language,
            Guid guid,
            string text
        )
        {
            return StoryBy(number)
                .Bind(story =>
                    ApplyChange<TranslationAlternativeAddedToBookEvent, Alternative>(
                        new TranslationAlternativeAddedToBookEvent(
                            Id,
                            DateTime.UtcNow,
                            story.Number,
                            language,
                            guid,
                            text
                        )
                    )
                );
        }

        public Result<Book, Error> AddAlternativeOption(
            StoryNumber number,
            Language language,
            Guid alternativeId,
            IOptionData optionData
        )
        {
            return StoryBy(number).Bind(story => ApplyChange(NewOptionEventFor(story))).Map(() => this);

            BaseBookEvent NewOptionEventFor(Story story) =>
                new AlternativeOptionAddedToBookEvent(
                    Id,
                    DateTime.UtcNow,
                    story.Number,
                    language,
                    alternativeId,
                    optionData
                );
        }

        #endregion

        public static Result<Book, Error> Create(Guid guid, string name, int capacity = DefaultBookCapacity)
        {
            return Result
                .FailureIf(NameIsNull, new Book(capacity), Error.Because("Name of book should be always filled"))
                .Tap(book => book.ApplyChange(new BookCreatedEvent(guid, DateTime.UtcNow, name)));

            bool NameIsNull() => string.IsNullOrWhiteSpace(name);
        }

        #region Event Handling

        private void Apply(BookCreatedEvent @event)
        {
            var (id, _, name) = @event;
            Id = id;
            Name = name;
        }

        private UnitResult<Error> Apply(StoryAddedToBookEvent @event)
        {
            var (_, _, storyNumber) = @event;
            return Story
                .Create(this, storyNumber)
                .Tap(story => _stories.Add(story))
                .Tap(story => storyNumber.Assign(story));
        }

        private UnitResult<Error> Apply(StoryTranslationAddedToBookEvent @event)
        {
            return StoryBy(@event.Number).Bind(story => story.Route(@event));
        }

        private Result<Alternative, Error> Apply(TranslationAlternativeAddedToBookEvent @event)
        {
            var (_, _, storyNumber, language, _, _) = @event;
            return StoryBy(storyNumber)
                .Bind(story => story.TranslationBy(language))
                .Bind(translation => translation.Route<TranslationAlternativeAddedToBookEvent, Alternative>(@event));
        }

        private UnitResult<Error> Apply(AlternativeOptionAddedToBookEvent @event)
        {
            var (_, _, storyNumber, language, alternativeId, _) = @event;
            return StoryBy(storyNumber)
                .Bind(story => story.TranslationBy(language))
                .Bind(translation => translation.AlternativeBy(alternativeId))
                .Bind(alternative => alternative.Route(@event));
        }

        #endregion
    }
}
