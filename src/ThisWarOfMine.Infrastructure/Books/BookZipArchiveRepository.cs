using MediatR;
using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Infrastructure.Books;

internal sealed class BookZipArchiveRepository : DispatchableRepository<Book, Guid>, IBookRepository
{
    private readonly IFileNameResolver _fileNameResolver;
    private readonly IZipBookCreator _zipBookCreator;

    public BookZipArchiveRepository(
        IMediator mediator,
        IFileNameResolver fileNameResolver,
        IZipBookCreator zipBookCreator
    )
        : base(mediator)
    {
        _fileNameResolver = fileNameResolver;
        _zipBookCreator = zipBookCreator;
    }

    protected override Task SaveAsync(Book aggregate, CancellationToken cancellationToken)
    {
        var file = _fileNameResolver.IfNotExistsGetFileNameFor(aggregate);
        if (file.HasNoValue)
        {
            return Task.CompletedTask;
        }

        return _zipBookCreator.CreateAsync(file.Value, aggregate);
    }

    public override Task<Book> LoadAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
