using System.Runtime.CompilerServices;
using ThisWarOfMine.Common;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Splitter;

internal sealed class BookCreator : IBookCreator
{
    private readonly IBookSplitter _bookSplitter;
    private readonly IStoryParser _storyParser;
    private readonly IBookRepository _bookRepository;

    private Book? _book;

    public BookCreator(IBookSplitter bookSplitter, IStoryParser storyParser, IBookRepository bookRepository)
    {
        _bookSplitter = bookSplitter;
        _storyParser = storyParser;
        _bookRepository = bookRepository;
    }

    public async Task<Book> InitializeAsync(string name, CancellationToken token = default)
    {
        _book = Book.Create(Guid.NewGuid(), name)
            .OnFallback(error =>
                throw new InvalidOperationException($"Cannot create book because of: {error.Message}")
            );
        await _bookRepository.SaveAsync(_book, token);
        return _book;
    }

    public async IAsyncEnumerable<Story> FulFillAsync(
        string path,
        Language language,
        [EnumeratorCancellation] CancellationToken token = default
    )
    {
        if (_book is null)
        {
            throw new InvalidOperationException("Cannot fulfill book before it's been initialized");
        }

        await foreach (var rows in _bookSplitter.SplitAsync(path, token))
        {
            var story = _storyParser.ParseIn(_book, language, rows).OnFallback(ThrowOnError);
            await _bookRepository.SaveAsync(_book, token);
            yield return story;
        }
    }

    private static Story ThrowOnError(string error) => throw new InvalidOperationException(error);
}
