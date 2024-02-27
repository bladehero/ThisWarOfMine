using System.Text.Json;

namespace ThisWarOfMine.Common.Wrappers;

internal sealed class JsonSerializerWrapper : IJsonSerializer
{
    public string Serialize<TValue>(TValue value, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Serialize(value, options);
    }

    public T? Deserialize<T>(ReadOnlySpan<char> json, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Deserialize<T>(json, options);
    }
}
