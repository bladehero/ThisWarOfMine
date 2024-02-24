using System.IO.Compression;

namespace ThisWarOfMine.Infrastructure.Books
{
    internal interface IBookAccessor
    {
        Task UseAsync(Guid bookId, Func<ZipArchive, Task> configure);
    }
}
