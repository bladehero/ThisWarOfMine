using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ThisWarOfMine.Application.Telegram.Abstraction;

namespace ThisWarOfMine.Application.Telegram.MessageHandlers.Core;

internal sealed class DefaultMessageTelegramNotificationHandler : DefaultTelegramNotificationHandler<Message>
{
    public const string NotRecognizedMessageCommand = nameof(NotRecognizedMessageCommand);
    private readonly IResponseLocalizer _localizer;

    public DefaultMessageTelegramNotificationHandler(IResponseLocalizer localizer)
    {
        _localizer = localizer;
    }

    public override Task<bool> CanHandleAsync(CancellationToken token)
    {
        return Task.FromResult(Payload.Type is MessageType.Text);
    }

    public override Task HandleAsync(CancellationToken token) =>
        Client.SendTextMessageAsync(
            Payload.Chat.Id,
            _localizer.GetString(NotRecognizedMessageCommand),
            cancellationToken: token
        );
}
