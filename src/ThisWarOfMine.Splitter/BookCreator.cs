using ThisWarOfMine.Domain;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Splitter;

internal sealed class BookCreator : IBookCreator
{
    private readonly IBookSplitter _bookSplitter;
    private readonly IStoryParser _storyParser;

    public BookCreator(IBookSplitter bookSplitter, IStoryParser storyParser)
    {
        _bookSplitter = bookSplitter;
        _storyParser = storyParser;
    }

    public async Task<Book> CreateAsync(string path, Language language, CancellationToken token = default)
    {
        var book = Book.Create();
        await foreach (var rows in _bookSplitter.SplitAsync(path, token))
        {
            _storyParser.ParseIn(book, language, rows);
        }
        return book;
    }
}