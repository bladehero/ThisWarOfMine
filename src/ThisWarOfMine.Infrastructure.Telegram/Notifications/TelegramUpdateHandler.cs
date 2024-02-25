using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using ThisWarOfMine.Application.Telegram.States;
using ThisWarOfMine.Infrastructure.Telegram.States;

namespace ThisWarOfMine.Infrastructure.Telegram.Notifications;

internal sealed class TelegramUpdateHandler : IUpdateHandler
{
    private readonly IMediator _mediator;
    private readonly TelegramChatAccessor _telegramChatAccessor;
    private readonly INotificationCreator _notificationCreator;
    private readonly ILogger<TelegramUpdateHandler> _logger;

    public TelegramUpdateHandler(
        IMediator mediator,
        ITelegramChatAccessor telegramChatAccessor,
        INotificationCreator notificationCreator,
        ILogger<TelegramUpdateHandler> logger
    )
    {
        _mediator = mediator;
        _telegramChatAccessor = (TelegramChatAccessor)telegramChatAccessor;
        _notificationCreator = notificationCreator;
        _logger = logger;
    }

    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        _telegramChatAccessor.Initialize(update);
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
