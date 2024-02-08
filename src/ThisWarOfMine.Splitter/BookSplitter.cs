using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ThisWarOfMine.Contracts;
using ThisWarOfMine.Contracts.Narrative;

namespace ThisWarOfMine.Splitter;

internal sealed partial class BookSplitter : IBookSplitter
{
    private const int RegularStoryTextRowsCount = 8;
    private const int BufferSize = 1024;
    private const string BookFile = "book.txt";
    private static readonly Regex OnlyNumberRule = GetNumberOnlyRegex();

    private readonly IStoryCreator _storyCreator;

    public BookSplitter(IStoryCreator storyCreator) => _storyCreator = storyCreator;

    public async IAsyncEnumerable<Story> SplitAsync(Language language, [EnumeratorCancellation] CancellationToken token = default)
    {
        await using var stream = BookStream();
        using var reader = new StreamReader(stream);

        var rows = new List<string>(RegularStoryTextRowsCount);
        while (!token.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync(token);
            if (line is null)
            {
                break;
            }

            rows.Add(line);

            if (rows.Count < 3)
            {
                continue;
            }

            if (!OnlyNumberRule.IsMatch(line))
            {
                continue;
            }

            // TODO: Refactor (maybe remove RemoveAt method if possible)
            rows.RemoveAt(rows.Count - 1);
            yield return _storyCreator.Create(language, rows);
            rows.Clear();
            rows.Add(line);
        }
    }

    private static FileStream BookStream() =>
        new(BookFile, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize,
            FileOptions.Asynchronous | FileOptions.SequentialScan);

    [GeneratedRegex("^\\s*\\d+\\s*$")]
    private static partial Regex GetNumberOnlyRegex();
}