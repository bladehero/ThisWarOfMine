using System.IO.Compression;
using ThisWarOfMine.Domain.Narrative.Events.Options;

namespace ThisWarOfMine.Infrastructure.Books.Options
{
    internal interface IOptionDataSerializer
    {
        string Serialize(IOptionData optionData);
        Task<IOptionData> DeserializeAsync(ZipArchiveEntry entry, CancellationToken token = default);
    }
}
