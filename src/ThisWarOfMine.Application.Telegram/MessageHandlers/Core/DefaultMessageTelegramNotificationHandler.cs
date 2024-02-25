using Telegram.Bot;
using Telegram.Bot.Types;
using ThisWarOfMine.Application.Telegram.Abstraction;

namespace ThisWarOfMine.Application.Telegram.MessageHandlers.Core;

internal sealed class DefaultMessageTelegramNotificationHandler : DefaultTelegramNotificationHandler<Message>
{
    public const string NotRecognizedMessageCommand = nameof(NotRecognizedMessageCommand);
    private readonly IMessageResponseLocalizer _localizer;

    public DefaultMessageTelegramNotificationHandler(IMessageResponseLocalizer localizer)
    {
        _localizer = localizer;
    }

    public override Task HandleAsync(CancellationToken token) =>
        Client.SendTextMessageAsync(
            Payload.Chat.Id,
            _localizer.GetString(NotRecognizedMessageCommand),
            cancellationToken: token
        );
}
