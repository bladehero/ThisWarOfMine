using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Domain.Narrative.Options;

public sealed class ComplexOption : Option
{
    public Option Current { get; init; }
    public Option Next { get; init; }

    public override string Text => $"{Current.Text} ► {Next.Text}";

    public override Maybe<short> GetRedirectionStoryNumber() =>
        Current.GetRedirectionStoryNumber().Or(Next.GetRedirectionStoryNumber());

    private ComplexOption(OptionGroup group, Option current, Option next)
        : base(group, current.Id)
    {
        if (current.IsRedirecting && next.IsRedirecting)
        {
            throw new InvalidOperationException("Only one of options can redirect");
        }

        Current = current;
        Next = next;
    }

    internal static ComplexOption Create(OptionGroup group, Option current, Option next) => new(group, current, next);
}
