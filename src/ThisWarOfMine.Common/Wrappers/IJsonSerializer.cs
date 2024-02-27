using System.Text.Json;

namespace ThisWarOfMine.Common.Wrappers;

public interface IJsonSerializer : IWrapper
{
    string Serialize<TValue>(TValue value, JsonSerializerOptions? options = null);
    T? Deserialize<T>(ReadOnlySpan<char> json, JsonSerializerOptions? options = null);
}
