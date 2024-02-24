using System.IO.Compression;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Infrastructure.Books
{
    internal sealed class ZipBookCreator : IZipBookCreator
    {
        public async Task CreateAsync(string path, Book book)
        {
            await using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
            using var archive = new ZipArchive(stream, ZipArchiveMode.Create, false);
            archive.Comment = book.Name;
        }
    }
}
