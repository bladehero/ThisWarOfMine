using System.IO.Compression;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Infrastructure.Books.Options
{
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
            await using var stream = entry.Open();
            using var reader = new StreamReader(stream);
            var content = await reader.ReadToEndAsync(token);
            foreach (var strategy in _strategies)
            {
                var (canDeserialize, optionData) = strategy.TryDeserialize(entry, content, token);
                if (canDeserialize)
                {
                    return optionData;
                }
            }

            throw new InvalidOperationException($"Cannot find proper option data type for entry: `{entry.Name}`");
        }
    }
}
