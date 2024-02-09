namespace ThisWarOfMine.Contracts.Narrative.Options;

internal sealed class ComplexOption : Option
{
    public Option Current { get; init; }
    public Option Next { get; init; }

    public override string Text => $"{Current.Text} ► {Next.Text}";
    public override bool IsRedirecting => Next.IsRedirecting;

    private ComplexOption(OptionGroup group, Option current, Option next) : base(group)
    {
        Current = current;
        Next = next;
        
    }

    internal static ComplexOption Create(OptionGroup group, Option current, Option next) => new(group, current, next);
}