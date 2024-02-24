using System.Diagnostics.CodeAnalysis;
using ThisWarOfMine.Domain.Abstraction;

namespace ThisWarOfMine.Domain.Narrative.Options
{
    public abstract class Option : Entity<Guid>
    {
        public OptionGroup Group { get; private init; }
        public int Order => Group.TakeWhile(x => x != this).Count();

        public abstract string Text { get; }

        public abstract bool IsRedirecting { get; }
        public virtual bool IsSelectable => true;

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        protected Option(OptionGroup group, Guid guid)
        {
            Group = group;
            Id = guid;
        }

        internal Option Append(Option option) => ComplexOption.Create(Group, this, option);

        internal Option Prepend(Option option) => ComplexOption.Create(Group, option, this);
    }
}
