using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Infrastructure.Books;

internal sealed class FileNameResolver : IFileNameResolver
{
    private readonly IOptionsSnapshot<BookConfiguration> _options;

    public FileNameResolver(IOptionsSnapshot<BookConfiguration> options)
    {
        _options = options;
    }

    public Maybe<string> IfNotExistsGetFileNameFor(Book aggregate)
    {
        var configuration = _options.Value;
        Directory.CreateDirectory(configuration.Folder);
        var file = Path.ChangeExtension(aggregate.Id.ToString(), "zip");
        var path = Path.Combine(configuration.Folder, file);
        if (File.Exists(path))
        {
            return Maybe.None;
        }

        return path;
    }
}
