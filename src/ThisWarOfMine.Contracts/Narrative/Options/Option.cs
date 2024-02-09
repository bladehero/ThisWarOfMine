namespace ThisWarOfMine.Contracts.Narrative.Options;

public abstract class Option
{
    public OptionGroup Group { get; private init; }

    public abstract string Text { get; }

    public abstract bool IsRedirecting { get; }

    protected Option(OptionGroup group) => Group = group;
}