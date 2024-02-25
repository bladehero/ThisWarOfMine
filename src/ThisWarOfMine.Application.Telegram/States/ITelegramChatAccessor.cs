using Telegram.Bot.Types;

namespace ThisWarOfMine.Application.Telegram.States;

public interface ITelegramChatAccessor
{
    Chat? Chat { get; }
    User? User { get; }
}
