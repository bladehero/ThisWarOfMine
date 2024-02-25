using System.IO.Compression;

namespace ThisWarOfMine.Infrastructure.Books;

public sealed class NoDisposeBookAccessingStrategy : IBookAccessingStrategy
{
    private readonly LongWriteOperationSingleBookAccessor _longWriteOperationSingleBookAccessor;

    public NoDisposeBookAccessingStrategy(LongWriteOperationSingleBookAccessor longWriteOperationSingleBookAccessor)
    {
        _longWriteOperationSingleBookAccessor = longWriteOperationSingleBookAccessor;
    }

    public Task UseAsync(string file, Func<ZipArchive, Task> action)
    {
        var archive = _longWriteOperationSingleBookAccessor.Open(file);
        return action(archive);
    }
}
