using System.Reflection;
using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Application.Telegram.Abstraction.Serialization;

internal sealed class TelegramCallbackDataJsonExtractor : ITelegramCallbackDataJsonExtractor
{
    public TelegramCallbackDataJsonExtractor(Type type)
    {
        Type = type;
        Attribute =
            type.GetCustomAttribute<TelegramCallbackTypeNameAttribute>()
            ?? throw new InvalidOperationException(
                $"Serializable part should always have attribute {nameof(TelegramCallbackTypeNameAttribute)}"
            );
    }

    public Type Type { get; }
    internal TelegramCallbackTypeNameAttribute Attribute { get; }

    public Maybe<string> TryExtract(string source)
    {
        var typeName = Attribute.Name;
        if (!source.StartsWith(typeName, StringComparison.Ordinal))
        {
            return Maybe.None;
        }

        return source[(typeName.Length + 1)..];
    }
}
