using ThisWarOfMine.Domain;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Splitter;

internal interface IBookCreator
{
    Task<Book> CreateAsync(string path, Language language, CancellationToken token = default);
}