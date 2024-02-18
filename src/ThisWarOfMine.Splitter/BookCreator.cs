using CSharpFunctionalExtensions;
using ThisWarOfMine.Common;
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

    public ValueTask<Book> CreateAsync(string name, string path, Language language, CancellationToken token = default)
    {
        return Book.Create(name)
            .Tap(async book =>
            {
                await foreach (var rows in _bookSplitter.SplitAsync(path, token))
                {
                    _storyParser.ParseIn(book, language, rows).TapError(ThrowOnError);
                }
            })
            .OnFallback(error =>
                throw new InvalidOperationException($"Cannot create book because of: {error.Message}")
            );
    }

    private static void ThrowOnError(string error) => throw new InvalidOperationException(error);
}
