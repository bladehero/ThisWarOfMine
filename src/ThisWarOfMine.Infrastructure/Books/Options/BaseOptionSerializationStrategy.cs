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

    Maybe<TOption> IOptionSerializationStrategy<TOption>.TryDeserialize(ZipArchiveEntry entry, string content)
    {
        if (content.StartsWith(Prefix))
        {
            return Deserialize(Guid.Parse(entry.Name), int.Parse(entry.Comment), content[Prefix.Length..].Trim());
        }

        return Maybe.None;
    }
}
