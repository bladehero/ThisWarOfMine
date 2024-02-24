using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Infrastructure.Books
{
    internal interface IZipBookCreator
    {
        Task CreateAsync(string path, Book book);
    }
}
