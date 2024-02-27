using Telegram.Bot;
using ThisWarOfMine.Application.Telegram.Abstraction;
using ThisWarOfMine.Application.Telegram.Abstraction.Serialization;
using ThisWarOfMine.Application.Telegram.CallbackHandlers.Contracts;
using ThisWarOfMine.Application.Telegram.CallbackHandlers.Core;

namespace ThisWarOfMine.Application.Telegram.CallbackHandlers;

internal sealed class BackToGameAnswerCallbackTelegramNotificationHandler
    : BaseCallbackTelegramNotificationHandler<BackToGameAnswerData>
{
    public const string BackToGame = nameof(BackToGame);
    private readonly IResponseLocalizer _responseLocalizer;

    public BackToGameAnswerCallbackTelegramNotificationHandler(
        ITelegramCallbackDataSerializer callbackDataSerializer,
        IResponseLocalizer responseLocalizer
    )
        : base(callbackDataSerializer)
    {
        _responseLocalizer = responseLocalizer;
    }

    public override async Task HandleAsync(CancellationToken token)
    {
        var sending = Client.SendTextMessageAsync(
            Chat.Id,
            _responseLocalizer.GetString(BackToGame),
            cancellationToken: token
        );
        var deleting = Client.DeleteMessageAsync(Chat.Id, Message.MessageId, cancellationToken: token);
        await Task.WhenAll(sending, deleting);
    }
}
