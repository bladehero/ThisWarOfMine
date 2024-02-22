using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Infrastructure.Books;

internal sealed class BookNameResolver : IBookNameResolver
{
    private readonly IOptionsSnapshot<BookConfiguration> _options;

    public BookNameResolver(IOptionsSnapshot<BookConfiguration> options)
    {
        _options = options;
    }

    public string GetFileNameFor(Guid bookId)
    {
        var configuration = _options.Value;
        var file = Path.ChangeExtension(bookId.ToString(), "zip");
        return Path.Combine(configuration.Folder!.Path, file);
    }

    public Maybe<string> IfNotExistsGetFileNameFor(Book aggregate)
    {
        var configuration = _options.Value;
        Directory.CreateDirectory(configuration.Folder!.Path);
        var path = GetFileNameFor(aggregate.Id);
        if (File.Exists(path))
        {
            return Maybe.None;
        }

        return path;
    }
}
