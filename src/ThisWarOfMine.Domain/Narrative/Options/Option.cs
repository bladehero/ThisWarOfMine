using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Domain.Narrative.Options;

public abstract class Option : Abstraction.Entity<Guid>
{
    public OptionGroup Group { get; private init; }
    public int Order => Group.TakeWhile(x => x != this).Count();

    public abstract string Text { get; }

    public virtual bool IsRedirecting => GetRedirectionStoryNumber().HasValue;
    public virtual bool IsSelectable => true;

    public virtual Maybe<short> GetRedirectionStoryNumber() => Maybe.None;

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    protected Option(OptionGroup group, Guid guid)
    {
        Group = group;
        Id = guid;
    }

    internal Option Append(Option option) => ComplexOption.Create(Group, this, option);

    internal Option Prepend(Option option) => ComplexOption.Create(Group, option, this);
}
