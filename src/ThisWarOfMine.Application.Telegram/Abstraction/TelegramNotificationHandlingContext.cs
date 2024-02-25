using Telegram.Bot;

namespace ThisWarOfMine.Application.Telegram.Abstraction;

internal sealed class TelegramNotificationHandlingContext<T>
{
    public TelegramNotification<T> TelegramNotification { get; }
    public ITelegramBotClient Client { get; }

    public TelegramNotificationHandlingContext(TelegramNotification<T> telegramNotification, ITelegramBotClient client)
    {
        TelegramNotification = telegramNotification;
        Client = client;
    }
}
