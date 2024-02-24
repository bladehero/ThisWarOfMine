namespace ThisWarOfMine.Application.Telegram.Abstraction;

public interface ITelegramNotificationHandler
{
    Task<bool> CanHandleAsync(CancellationToken token);
    Task HandleAsync(CancellationToken token);
}
