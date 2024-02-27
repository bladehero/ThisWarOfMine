using Telegram.Bot.Types.ReplyMarkups;
using ThisWarOfMine.Application.Telegram.Abstraction.Serialization;

namespace ThisWarOfMine.Application.Telegram.Abstraction;

internal sealed class InlineKeyBoardButtonProvider : IInlineKeyBoardButtonProvider
{
    private readonly ITelegramCallbackDataSerializer _telegramCallbackDataSerializer;

    public InlineKeyBoardButtonProvider(ITelegramCallbackDataSerializer telegramCallbackDataSerializer)
    {
        _telegramCallbackDataSerializer = telegramCallbackDataSerializer;
    }

    public InlineKeyboardButton Create<T>(string text, T payload)
    {
        var callbackData = _telegramCallbackDataSerializer.Serialize(payload!);
        return InlineKeyboardButton.WithCallbackData(text, callbackData);
    }
}
