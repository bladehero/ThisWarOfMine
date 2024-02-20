namespace ThisWarOfMine.Domain.Narrative.Options;

internal sealed class RemarkOption : Option
{
    private RemarkOption(OptionGroup group, Guid guid, string text)
        : base(group, guid) => Text = text;

    public override string Text { get; }
    public override bool IsRedirecting => false;
    public override bool IsSelectable => false;

    internal static RemarkOption Create(OptionGroup group, Guid guid, string text) => new(group, guid, text);
}
