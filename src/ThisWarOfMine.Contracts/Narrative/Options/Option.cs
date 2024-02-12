namespace ThisWarOfMine.Contracts.Narrative.Options;

public abstract class Option
{
    public OptionGroup Group { get; private init; }

    public abstract string Text { get; }

    public abstract bool IsRedirecting { get; }
    public virtual bool IsSelectable => true;

    protected Option(OptionGroup group) => Group = group;

    public Option Append(Option option) => ComplexOption.Create(Group, this, option);
    public Option Prepend(Option option) => ComplexOption.Create(Group, option, this);
}