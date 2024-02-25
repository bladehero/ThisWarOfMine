using CSharpFunctionalExtensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ThisWarOfMine.Application.Telegram.Abstraction;

namespace ThisWarOfMine.Application.Telegram.MessageHandlers.Core;

internal abstract class TextCommandTelegramNotificationHandler : BaseTelegramNotificationHandler<Message>
{
    private const int StartingOffset = 0;
    protected Message Message => Payload;
    protected string[]? CommandOptions { get; private set; }
    protected abstract string Command { get; }

    public override Task<bool> CanHandleAsync(CancellationToken token)
    {
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

        var text = Message.Text!.Substring(entity.Offset, entity.Length);
        if (!CommandIsValid(text))
        {
            return Task.FromResult(false);
        }

        CommandOptions = Message
            .Text[entity.Length..]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return Task.FromResult(true);
    }

    private bool CommandIsValid(string text) => $"/{Command}".Equals(text, StringComparison.OrdinalIgnoreCase);
}
