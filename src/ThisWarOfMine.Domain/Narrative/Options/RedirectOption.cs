using System.Collections.Immutable;

namespace ThisWarOfMine.Domain.Narrative.Options;

public sealed class RedirectOption : Option
{
    private static readonly ImmutableSortedDictionary<Language, string> Map = new SortedDictionary<Language, string>
    {
        { Language.English, "see" },
        { Language.Ukrainian, "див." },
        { Language.Russian, "см." },
    }.ToImmutableSortedDictionary();

    public int StoryNumber { get; init; }

    public override string Text => $"{Map.ValueRef(Group.Alternative.Translation.Language)} {StoryNumber}";
    public override bool IsRedirecting => true;

    public RedirectOption(OptionGroup group)
        : base(group) { }

    internal static RedirectOption Create(OptionGroup group, int storyNumber) =>
        new(group) { StoryNumber = storyNumber };
}
