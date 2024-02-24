using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Common.Wrappers;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Splitter.Options
{
    internal sealed partial class OnlyBackToGameOptionParsingStrategy : IOptionParsingStrategy
    {
        private static readonly Regex OnlyBackToGameRule = GetOnlyBackToGameRegex();

        private readonly IGuidProvider _guidProvider;

        public OnlyBackToGameOptionParsingStrategy(IGuidProvider guidProvider)
        {
            _guidProvider = guidProvider;
        }

        public Maybe<IOptionData> TryParse(string optionRow, int order)
        {
            if (!OnlyBackToGameRule.IsMatch(optionRow))
            {
                return Maybe.None;
            }

            return new BackToGameOptionData(_guidProvider.NewGuid(), order);
        }

        [GeneratedRegex($"^\\s*\\?\\s*({Constants.BackToGameMarker})\\s*\\.?\\s*$")]
        private static partial Regex GetOnlyBackToGameRegex();
    }
}
