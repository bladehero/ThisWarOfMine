using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ThisWarOfMine.Splitter
{
    internal sealed partial class BookSplitter : IBookSplitter
    {
        private const int RegularStoryTextRowsCount = 8;
        private const int BufferSize = 1024;
        private static readonly Regex OnlyNumberRule = GetNumberOnlyRegex();

        public async IAsyncEnumerable<IReadOnlyCollection<string>> SplitAsync(
            string path,
            [EnumeratorCancellation] CancellationToken token = default
        )
        {
            await using var stream = BookStream(path);
            using var reader = new StreamReader(stream);

            var rows = new List<string?>(RegularStoryTextRowsCount);
            while (!token.IsCancellationRequested)
            {
                var line = (await reader.ReadLineAsync(token))?.Trim();

                rows.Add(line);

                if (rows.Count < 3)
                {
                    continue;
                }

                if (LineIsNotANumber(line))
                {
                    continue;
                }

                // TODO: Refactor (maybe remove RemoveAt method if possible)
                rows.RemoveAt(rows.Count - 1);
                yield return rows.ToArray()!;
                rows.Clear();
                rows.Add(line);

                if (line is null)
                {
                    yield break;
                }
            }
        }

        private static bool LineIsNotANumber(string? line) => line is not null && !OnlyNumberRule.IsMatch(line);

        private static FileStream BookStream(string path) =>
            new(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                BufferSize,
                FileOptions.Asynchronous | FileOptions.SequentialScan
            );

        [GeneratedRegex("^\\s*\\d+\\s*$")]
        private static partial Regex GetNumberOnlyRegex();
    }
}
