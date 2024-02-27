namespace ThisWarOfMine.Application.Telegram.MessageHandlers.Core;

internal abstract class TextCommandWithOptionsTelegramNotificationHandler : TextCommandTelegramNotificationHandler
{
    protected abstract bool OptionsAreRequired { get; }
    protected string[]? CommandOptions { get; private set; }

    public override async Task<bool> CanHandleAsync(CancellationToken token)
    {
        var cannotHandle = !await base.CanHandleAsync(token);
        if (cannotHandle)
        {
            return false;
        }

        CommandOptions = Message
            .Text![CommandEntity!.Length..]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return !OptionsAreRequired || CommandOptions.Any();
    }
}
