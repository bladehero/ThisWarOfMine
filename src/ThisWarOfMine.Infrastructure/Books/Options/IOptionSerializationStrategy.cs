using System.IO.Compression;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Infrastructure.Books.Options;

internal interface IOptionSerializationStrategy<TOption> : IOptionSerializationStrategy
    where TOption : IOptionData
{
    string Serialize(TOption optionData);
    new Task<Maybe<TOption>> TryDeserialize(ZipArchiveEntry entry, CancellationToken token);

    bool IOptionSerializationStrategy.CompatibleFor(IOptionData data) => typeof(TOption) == data.GetType();

    string IOptionSerializationStrategy.Serialize(IOptionData optionData) => Serialize((TOption)optionData);

    Task<Maybe<IOptionData>> IOptionSerializationStrategy.TryDeserialize(
        ZipArchiveEntry entry,
        CancellationToken token
    ) => TryDeserialize(entry, token).Map(x => (IOptionData)x);
}

internal interface IOptionSerializationStrategy
{
    bool CompatibleFor(IOptionData data);
    string Serialize(IOptionData optionData);
    Task<Maybe<IOptionData>> TryDeserialize(ZipArchiveEntry entry, CancellationToken token);
}
