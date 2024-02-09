using ThisWarOfMine.Contracts;
using ThisWarOfMine.Contracts.Narrative;

namespace ThisWarOfMine.Splitter;

internal interface IBookCreator
{
    Task<Book> CreateAsync(string path, Language language, CancellationToken token = default);
}