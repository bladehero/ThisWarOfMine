namespace ThisWarOfMine.Domain.Abstraction;

public interface IRepository<TRoot, in TKey>
    where TRoot : AggregateRoot<TKey>
    where TKey : IComparable<TKey>
{
    Task SaveAsync(TRoot aggregate, CancellationToken cancellationToken);
    Task<TRoot> LoadAsync(TKey id, CancellationToken cancellationToken);
}
