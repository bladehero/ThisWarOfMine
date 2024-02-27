using CSharpFunctionalExtensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ThisWarOfMine.Application.Telegram.Abstraction;

namespace ThisWarOfMine.Application.Telegram.MessageHandlers.Core;

internal abstract class TextCommandTelegramNotificationHandler : BaseTelegramNotificationHandler<Message>
{
    private const int StartingOffset = 0;
    protected Message Message => Payload;
    protected Chat Chat => Payload.Chat;
    protected MessageEntity? CommandEntity { get; private set; }
    protected abstract string Command { get; }

    public override Task<bool> CanHandleAsync(CancellationToken token)
    {
        if (Message.Type is not MessageType.Text)
        {
            return Task.FromResult(false);
        }

        if (Message.Entities is null)
        {
            return Task.FromResult(false);
        }

        var (itIsACommand, entity) = Message.Entities.TryFirst(x =>
            x.Type == MessageEntityType.BotCommand && x.Offset == StartingOffset
        );
        if (!itIsACommand)
        {
            return Task.FromResult(false);
        }

        CommandEntity = entity;
        var text = Message.Text!.Substring(entity.Offset, entity.Length);
        return Task.FromResult(CommandIsValid(text));
    }

    private bool CommandIsValid(string text) => $"/{Command}".Equals(text, StringComparison.OrdinalIgnoreCase);
}
