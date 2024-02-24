using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Domain.Abstraction
{
    public interface IRepository<TRoot, in TKey>
        where TRoot : AggregateRoot<TKey>
        where TKey : IComparable<TKey>
    {
        Task SaveAsync(TRoot aggregate, CancellationToken cancellationToken);
        Task<Result<TRoot, Error>> LoadAsync(TKey id, CancellationToken cancellationToken = default);
    }
}
