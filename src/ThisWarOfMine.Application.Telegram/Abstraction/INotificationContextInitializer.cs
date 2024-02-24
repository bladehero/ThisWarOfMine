using MediatR;
using Telegram.Bot;

namespace ThisWarOfMine.Application.Telegram.Abstraction;

public interface INotificationContextInitializer
{
    void SetContext(INotification notification, ITelegramBotClient client);
}
