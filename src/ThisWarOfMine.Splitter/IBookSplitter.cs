using System.Runtime.CompilerServices;
using ThisWarOfMine.Contracts;
using ThisWarOfMine.Contracts.Narrative;

namespace ThisWarOfMine.Splitter;

internal interface IBookSplitter
{
    IAsyncEnumerable<Story> SplitAsync(Language language, CancellationToken token = default);
}