using System.Text.Json.Serialization;
using ThisWarOfMine.Application.Telegram.Abstraction.Serialization;

namespace ThisWarOfMine.Application.Telegram.CallbackHandlers.Contracts;

[TelegramCallbackTypeName("rtsd")]
public record RedirectToStoryData([property: JsonPropertyName("nsn")] short NextStoryNumber);
