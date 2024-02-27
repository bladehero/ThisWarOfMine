using System.Collections.Immutable;
using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Domain.Narrative.Options;

public sealed class RedirectOption : Option
{
    private static readonly ImmutableSortedDictionary<Language, string> Map = new SortedDictionary<Language, string>
    {
        { Language.English, "see" },
        { Language.Ukrainian, "див." },
        { Language.Russian, "см." },
    }.ToImmutableSortedDictionary();

    private readonly short _storyNumber;

    public override Maybe<short> GetRedirectionStoryNumber() => _storyNumber;

    public override string Text => $"{Map.ValueRef(Group.Alternative.Translation.Language)} {_storyNumber}";

    private RedirectOption(OptionGroup group, Guid guid, short storyNumber)
        : base(group, guid)
    {
        _storyNumber = storyNumber;
    }

    internal static RedirectOption Create(OptionGroup group, Guid guid, short storyNumber) =>
        new(group, guid, storyNumber);
}
