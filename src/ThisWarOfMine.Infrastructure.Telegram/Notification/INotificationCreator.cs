using MediatR;
using Telegram.Bot.Types;

namespace ThisWarOfMine.Infrastructure.Telegram.Notification
{
    internal interface INotificationCreator
    {
        INotification CreateFrom(Update update);
    }
}
