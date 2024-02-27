using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Application.Telegram.Abstraction.Serialization;

internal interface ITelegramCallbackDataSerializer
{
    string Serialize(object value);
    Maybe<T> TryDeserialize<T>(string source);
}
