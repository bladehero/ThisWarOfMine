using Telegram.Bot;
using ThisWarOfMine.Application.Telegram.Abstraction.Serialization;
using ThisWarOfMine.Application.Telegram.CallbackHandlers.Contracts;
using ThisWarOfMine.Application.Telegram.CallbackHandlers.Core;
using ThisWarOfMine.Application.Telegram.Helpers;

namespace ThisWarOfMine.Application.Telegram.CallbackHandlers;

internal sealed class RedirectToStoryCallbackTelegramNotificationHandler
    : BaseCallbackTelegramNotificationHandler<RedirectToStoryData>
{
    private readonly IStorySendingHelper _storySendingHelper;

    public RedirectToStoryCallbackTelegramNotificationHandler(
        ITelegramCallbackDataSerializer callbackDataSerializer,
        IStorySendingHelper storySendingHelper
    )
        : base(callbackDataSerializer)
    {
        _storySendingHelper = storySendingHelper;
    }

    public override async Task HandleAsync(CancellationToken token)
    {
        var sending = _storySendingHelper.SendAsync(CallbackData!.NextStoryNumber, Chat.Id, token);
        var deleting = Client.DeleteMessageAsync(Chat.Id, Message.MessageId, cancellationToken: token);
        await Task.WhenAll(sending, deleting);
    }
}
