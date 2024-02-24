using CSharpFunctionalExtensions;
using MediatR;

namespace ThisWarOfMine.Domain.Abstraction
{
    public abstract class DispatchableRepository<TRoot, TKey> : IRepository<TRoot, TKey>
        where TRoot : AggregateRoot<TKey>
        where TKey : IComparable<TKey>
    {
        private readonly IMediator _mediator;

        protected DispatchableRepository(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected abstract Task SaveAsync(TRoot aggregate, CancellationToken cancellationToken);

        async Task IRepository<TRoot, TKey>.SaveAsync(TRoot aggregate, CancellationToken cancellationToken)
        {
            await SaveAsync(aggregate, cancellationToken);
            await DispatchDomainEventsAsync(aggregate, cancellationToken);
        }

        public abstract Task<Result<TRoot, Error>> LoadAsync(TKey id, CancellationToken cancellationToken = default);

        private async Task DispatchDomainEventsAsync(TRoot aggregateRoot, CancellationToken cancellationToken)
        {
            var domainEvents = aggregateRoot.GetUncommittedChanges();

            var tasks = domainEvents.Select(domainEvent => _mediator.Publish(domainEvent, cancellationToken));
            await Task.WhenAll(tasks);

            aggregateRoot.Commit();
        }
    }
}
