using System.Text.Json.Serialization;
using ThisWarOfMine.Application.Telegram.Abstraction.Serialization;

namespace ThisWarOfMine.Application.Telegram.CallbackHandlers.Contracts;

[TelegramCallbackTypeName("ptad")]
internal sealed record PlainTextAnswerData(
    [property: JsonPropertyName("s")] short StoryNumber,
    [property: JsonPropertyName("o")] int OptionOrder
);
