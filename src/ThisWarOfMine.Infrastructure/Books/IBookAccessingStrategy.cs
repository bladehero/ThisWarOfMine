using System.IO.Compression;

namespace ThisWarOfMine.Infrastructure.Books;

public interface IBookAccessingStrategy
{
    Task UseAsync(string file, Func<ZipArchive, Task> action);
}
