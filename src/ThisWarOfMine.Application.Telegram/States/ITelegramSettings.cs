using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Application.Telegram.States;

public interface ITelegramSettings
{
    Language Language { get; }
}
