using System.Collections.Immutable;

namespace ThisWarOfMine.Domain.Narrative.Options;

public sealed class BackToGameOption : Option
{
    private static readonly ImmutableSortedDictionary<Language, string> Map = new SortedDictionary<Language, string>
    {
        { Language.English, "BACK TO GAME" },
        { Language.Ukrainian, "НАЗАД У ГРУ" },
        { Language.Russian, "НАЗАД В ИГРУ" },
    }.ToImmutableSortedDictionary();

    public override string Text => Map.ValueRef(Group.Alternative.Translation.Language);
    public override bool IsRedirecting => false;

    private BackToGameOption(OptionGroup group, Guid guid)
        : base(group, guid) { }

    internal static BackToGameOption Create(OptionGroup group, Guid guid) => new(group, guid);
}
