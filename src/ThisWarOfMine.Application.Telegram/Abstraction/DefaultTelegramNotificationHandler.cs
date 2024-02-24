namespace ThisWarOfMine.Application.Telegram.Abstraction;

internal abstract class DefaultTelegramNotificationHandler<T> : BaseTelegramNotificationHandler<T>
{
    public override Task<bool> CanHandleAsync(CancellationToken token) => Task.FromResult(true);
}
