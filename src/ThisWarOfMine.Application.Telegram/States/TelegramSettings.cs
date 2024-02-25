using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Application.Telegram.States;

public sealed class TelegramSettings : ITelegramSettings
{
    public Language Language { get; set; } = Language.English;
}
