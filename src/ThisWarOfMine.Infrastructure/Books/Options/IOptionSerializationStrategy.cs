using System.IO.Compression;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Infrastructure.Books.Options;

internal interface IOptionSerializationStrategy<TOption> : IOptionSerializationStrategy
    where TOption : IOptionData
{
    string Serialize(TOption optionData);
    new Maybe<TOption> TryDeserialize(ZipArchiveEntry entry, string content);

    bool IOptionSerializationStrategy.CompatibleFor(IOptionData data) => typeof(TOption) == data.GetType();

    string IOptionSerializationStrategy.Serialize(IOptionData optionData) => Serialize((TOption)optionData);

    Maybe<IOptionData> IOptionSerializationStrategy.TryDeserialize(
        ZipArchiveEntry entry,
        string content,
        CancellationToken token
    ) => TryDeserialize(entry, content).Map(x => (IOptionData)x);
}

internal interface IOptionSerializationStrategy
{
    bool CompatibleFor(IOptionData data);
    string Serialize(IOptionData optionData);
    Maybe<IOptionData> TryDeserialize(ZipArchiveEntry entry, string content, CancellationToken token);
}
