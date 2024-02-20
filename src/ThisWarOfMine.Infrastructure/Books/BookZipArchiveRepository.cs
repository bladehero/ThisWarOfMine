using MediatR;
using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Infrastructure.Books;

internal sealed class BookZipArchiveRepository : DispatchableRepository<Book, Guid>, IBookRepository
{
    private readonly IBookNameResolver _bookNameResolver;
    private readonly IZipBookCreator _zipBookCreator;

    public BookZipArchiveRepository(
        IMediator mediator,
        IBookNameResolver bookNameResolver,
        IZipBookCreator zipBookCreator
    )
        : base(mediator)
    {
        _bookNameResolver = bookNameResolver;
        _zipBookCreator = zipBookCreator;
    }

    protected override Task SaveAsync(Book aggregate, CancellationToken cancellationToken)
    {
        var file = _bookNameResolver.IfNotExistsGetFileNameFor(aggregate);
        if (file.HasValue)
        {
            return _zipBookCreator.CreateAsync(file.Value, aggregate);
        }

        return Task.CompletedTask;
    }

    public override Task<Book> LoadAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
