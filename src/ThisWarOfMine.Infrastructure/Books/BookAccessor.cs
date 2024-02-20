using System.IO.Compression;
using Microsoft.Extensions.Options;

namespace ThisWarOfMine.Infrastructure.Books;

internal sealed class BookAccessor : IBookAccessor
{
    private readonly IBookNameResolver _bookNameResolver;
    private readonly IOptionsSnapshot<BookAccessorConfiguration> _options;

    public BookAccessor(IBookNameResolver bookNameResolver, IOptionsSnapshot<BookAccessorConfiguration> options)
    {
        _bookNameResolver = bookNameResolver;
        _options = options;
    }

    public Task UseAsync(Guid bookId, Func<ZipArchive, Task> action)
    {
        var file = _bookNameResolver.GetFileNameFor(bookId);
        return _options.Value.AccessingStrategy.UseAsync(file, action);
    }
}
