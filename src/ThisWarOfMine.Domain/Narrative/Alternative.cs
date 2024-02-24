using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative.Events.Options;
using ThisWarOfMine.Domain.Narrative.Options;

namespace ThisWarOfMine.Domain.Narrative
{
    public sealed class Alternative : Abstraction.Entity<Guid>
    {
        public string Text { get; private init; } = null!;
        public bool IsOriginal { get; private init; }
        public OptionGroup Options { get; private set; } = null!;
        public Translation Translation { get; private init; }

        private Alternative(Translation translation)
        {
            Translation = translation;

            Register<AlternativeOptionAddedToBookEvent>(Apply);
        }

        internal static Result<Alternative, Error> Create(Translation translation, Guid guid, string text)
        {
            return Result.FailureIf(
                string.IsNullOrWhiteSpace(text),
                NewAlternative(),
                Error.Because("Alternative text should never be null or whitespace")
            );

            Alternative NewAlternative()
            {
                var alternative = new Alternative(translation)
                {
                    Id = guid,
                    Text = text,
                    IsOriginal = IfTheVeryFirstAlternative(translation)
                };
                alternative.Options = OptionGroup.Empty(alternative);
                return alternative;
            }
        }

        #region Event Handling

        private UnitResult<Error> Apply(AlternativeOptionAddedToBookEvent @event)
        {
            var (_, _, _, _, _, optionData) = @event;

            Result<Option, Error> result = optionData switch
            {
                BackToGameOptionData data => Options.WithOnlyBackToGame(data.Id),
                RemarkOptionData data => Options.Note(data.Id, data.Remark),
                TextOptionData data => Options.AppendWithText(data.Id, data.Text, data.WithBackToGame),
                RedirectionOptionData data
                    => Options.AppendWithRedirection(data.Id, data.Text, data.StoryNumber, data.Appendix),
                _ => Error.Because($"Option data `{optionData}` was in incorrect format or type is not supported")
            };

            return result;
        }

        #endregion

        private static bool IfTheVeryFirstAlternative(Translation translation) =>
            translation.Story.Translations.Count == 1 && translation.IsEmpty;
    }
}
