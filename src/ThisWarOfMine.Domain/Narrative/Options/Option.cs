namespace ThisWarOfMine.Domain.Narrative.Options;

public abstract class Option
{
    public OptionGroup Group { get; private init; }

    public abstract string Text { get; }

    public abstract bool IsRedirecting { get; }
    public virtual bool IsSelectable => true;

    protected Option(OptionGroup group) => Group = group;

    internal Option Append(Option option) => ComplexOption.Create(Group, this, option);

    internal Option Prepend(Option option) => ComplexOption.Create(Group, option, this);
}
