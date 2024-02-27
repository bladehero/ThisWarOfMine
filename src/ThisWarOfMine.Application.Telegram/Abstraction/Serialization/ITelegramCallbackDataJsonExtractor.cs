using CSharpFunctionalExtensions;

namespace ThisWarOfMine.Application.Telegram.Abstraction.Serialization;

public interface ITelegramCallbackDataJsonExtractor
{
    Type Type { get; }
    Maybe<string> TryExtract(string source);
}
