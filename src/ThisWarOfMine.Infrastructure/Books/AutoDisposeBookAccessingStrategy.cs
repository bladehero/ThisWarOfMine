using System.IO.Compression;

namespace ThisWarOfMine.Infrastructure.Books
{
    internal sealed class AutoDisposeBookAccessingStrategy : IBookAccessingStrategy
    {
        public Task UseAsync(string file, Func<ZipArchive, Task> action)
        {
            using var archive = ZipFile.Open(file, ZipArchiveMode.Update);
            return action(archive);
        }
    }
}
