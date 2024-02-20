using System.IO.Compression;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Infrastructure.Books.Options;

internal abstract class BaseOptionSerializationStrategy<TOption> : IOptionSerializationStrategy<TOption>
    where TOption : IOptionData
{
    private static string Prefix => $"> [{typeof(TOption).Name}]{Environment.NewLine}";

    protected abstract string Serialize(TOption optionData);

    protected abstract TOption Deserialize(Guid guid, int order, string @string);

    string IOptionSerializationStrategy<TOption>.Serialize(TOption optionData) =>
        $"{Prefix}{Serialize(optionData)}{Environment.NewLine}";

    async Task<Maybe<TOption>> IOptionSerializationStrategy<TOption>.TryDeserialize(
        ZipArchiveEntry entry,
        CancellationToken token
    )
    {
        await using var stream = entry.Open();
        using var reader = new StreamReader(stream);
        var @string = await reader.ReadToEndAsync(token);
        var index = @string.LastIndexOf(Prefix, StringComparison.Ordinal);
        if (index == -1)
        {
            return Maybe.None;
        }

        return Deserialize(Guid.Parse(entry.Name), int.Parse(entry.Comment), @string[index..].Trim());
    }
}
