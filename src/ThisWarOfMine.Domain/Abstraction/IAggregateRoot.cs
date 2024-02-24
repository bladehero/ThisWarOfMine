using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Domain.Abstraction
{
    public interface IAggregateRoot
    {
        bool HasChanges { get; }
        IReadOnlyCollection<IBaseDomainEvent> GetUncommittedChanges();
        UnitResult<Error> Load(IEnumerable<IBaseDomainEvent> history);
        void Commit();
    }
}
