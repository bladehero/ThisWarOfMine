using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Abstraction;

namespace ThisWarOfMine.Domain.Narrative
{
    public interface IBookRepository : IRepository<Book, Guid>
    {
        Task<Result<Book, Error>> FindByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
