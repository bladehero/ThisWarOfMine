using System.IO.Compression;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Infrastructure.Books.Options;

internal sealed class OptionDataSerializer : IOptionDataSerializer
{
    private readonly IEnumerable<IOptionSerializationStrategy> _strategies;

    public OptionDataSerializer(IEnumerable<IOptionSerializationStrategy> strategies)
    {
        _strategies = strategies;
    }

    public string Serialize(IOptionData optionData) =>
        _strategies
            .TryFirst(strategy => strategy.CompatibleFor(optionData))
            .GetValueOrThrow($"Not supported serialization of data `{optionData}`")
            .Serialize(optionData);

    public async Task<IOptionData> DeserializeAsync(ZipArchiveEntry entry, CancellationToken token = default)
    {
        var tasks = _strategies.Select(x => x.TryDeserialize(entry, token));
        var maybes = await Task.WhenAll(tasks);

        var options = maybes.Where(x => x.HasValue).Select(x => x.Value).ToArray();
        return options.Length switch
        {
            0 => throw new InvalidOperationException($"Cannot find proper option data type for entry: `{entry.Name}`"),
            1 => options.First(),
            _
                => throw new InvalidOperationException(
                    $"Ambiguous option type: ({string.Join(", ", options.Select(x => x.GetType()))})"
                )
        };
    }
}
