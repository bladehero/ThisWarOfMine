using MediatR;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace ThisWarOfMine.Worker.Telegram;

internal sealed class TelegramUpdateHandler : IUpdateHandler
{
    private readonly IMediator _mediator;
    private readonly INotificationCreator _notificationCreator;
    private readonly ILogger<TelegramUpdateHandler> _logger;

    public TelegramUpdateHandler(
        IMediator mediator,
        INotificationCreator notificationCreator,
        ILogger<TelegramUpdateHandler> logger
    )
    {
        _mediator = mediator;
        _notificationCreator = notificationCreator;
        _logger = logger;
    }

    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var notification = _notificationCreator.CreateFrom(update);
        return _mediator.Publish(notification, cancellationToken);
    }

    public Task HandlePollingErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(exception, "Happened during polling");
        return Task.CompletedTask;
    }
}
