using MediatR;
using Telegram.Bot.Types;

namespace ThisWarOfMine.Worker.Telegram;

internal interface INotificationCreator
{
    INotification CreateFrom(Update update);
}
