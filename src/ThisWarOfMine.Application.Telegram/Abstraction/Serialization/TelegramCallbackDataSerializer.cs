using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using ThisWarOfMine.Common.Wrappers;

namespace ThisWarOfMine.Application.Telegram.Abstraction.Serialization;

internal sealed class TelegramCallbackDataSerializer : ITelegramCallbackDataSerializer
{
    private static readonly JsonSerializerOptions SerializerOptions =
        new() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = false };
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IEnumerable<ITelegramCallbackDataJsonExtractor> _extractors;

    public TelegramCallbackDataSerializer(
        IJsonSerializer jsonSerializer,
        IEnumerable<ITelegramCallbackDataJsonExtractor> extractors
    )
    {
        _jsonSerializer = jsonSerializer;
        _extractors = extractors;
    }

    public string Serialize(object value)
    {
        var type = value.GetType();
        if (_extractors.All(x => x.Type != type))
        {
            throw new InvalidOperationException($"Type `{type}` is not allowed to be serialized");
        }

        var typeName = GetTypeName(type);
        var json = _jsonSerializer.Serialize(value, SerializerOptions);
        return $"{typeName}:{json}";
    }

    public Maybe<T> TryDeserialize<T>(string source)
    {
        var extractor = _extractors.Single(x => x.Type == typeof(T));
        return extractor.TryExtract(source).Map(json => _jsonSerializer.Deserialize<T>(json, SerializerOptions))!;
    }

    private static string GetTypeName(MemberInfo member) =>
        member.GetCustomAttribute<TelegramCallbackTypeNameAttribute>()?.Name
        ?? throw new InvalidOperationException(
            "Cannot serialize value that is not marked with TelegramCallbackTypeNameAttribute attribute"
        );
}
