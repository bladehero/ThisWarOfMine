using MediatR;
using Telegram.Bot.Types;

namespace ThisWarOfMine.Infrastructure.Telegram.Notifications;

internal interface INotificationCreator
{
    INotification CreateFrom(Update update);
}
