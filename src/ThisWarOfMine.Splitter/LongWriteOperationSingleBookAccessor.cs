using System.IO.Compression;

namespace ThisWarOfMine.Splitter;

internal sealed class LongWriteOperationSingleBookAccessor : IDisposable
{
    private readonly object _synchronization = new();
    private ZipArchive? _archive;

    public ZipArchive Open(string file)
    {
        lock (_synchronization)
        {
            _archive ??= ZipFile.Open(file, ZipArchiveMode.Update);
        }

        return _archive;
    }

    public void Dispose() => _archive?.Dispose();
}
