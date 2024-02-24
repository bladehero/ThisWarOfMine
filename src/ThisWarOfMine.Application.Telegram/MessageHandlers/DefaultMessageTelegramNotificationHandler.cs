using Telegram.Bot;
using Telegram.Bot.Types;
using ThisWarOfMine.Application.Telegram.Abstraction;

namespace ThisWarOfMine.Application.Telegram.MessageHandlers;

internal sealed class DefaultMessageTelegramNotificationHandler : DefaultTelegramNotificationHandler<Message>
{
    public override Task HandleAsync(CancellationToken token) =>
        Client.SendTextMessageAsync(Payload.Chat.Id, "Command not recognized", cancellationToken: token);
}
