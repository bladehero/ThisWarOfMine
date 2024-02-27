using Telegram.Bot.Types.ReplyMarkups;

namespace ThisWarOfMine.Application.Telegram.Abstraction;

internal interface IInlineKeyBoardButtonProvider
{
    InlineKeyboardButton Create<T>(string text, T payload);
}
