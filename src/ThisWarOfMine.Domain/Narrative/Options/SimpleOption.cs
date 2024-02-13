namespace ThisWarOfMine.Domain.Narrative.Options;

internal sealed class SimpleOption : Option
{
    public override string Text { get; }
    public override bool IsRedirecting => false;
    private SimpleOption(OptionGroup group, string text) : base(group) => Text = text;

    internal static SimpleOption Create(OptionGroup group, string text) => new(group, text);
}