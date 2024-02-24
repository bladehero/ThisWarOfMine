namespace ThisWarOfMine.Splitter
{
    public interface IBookSplitter
    {
        IAsyncEnumerable<IReadOnlyCollection<string>> SplitAsync(string path, CancellationToken token = default);
    }
}
