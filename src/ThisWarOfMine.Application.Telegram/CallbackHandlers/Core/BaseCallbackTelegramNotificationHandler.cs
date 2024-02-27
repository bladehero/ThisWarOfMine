using CSharpFunctionalExtensions;
using Telegram.Bot.Types;
using ThisWarOfMine.Application.Telegram.Abstraction;
using ThisWarOfMine.Application.Telegram.Abstraction.Serialization;

namespace ThisWarOfMine.Application.Telegram.CallbackHandlers.Core;

internal abstract class BaseCallbackTelegramNotificationHandler<T> : BaseTelegramNotificationHandler<CallbackQuery>
{
    private readonly ITelegramCallbackDataSerializer _callbackDataSerializer;

    protected BaseCallbackTelegramNotificationHandler(ITelegramCallbackDataSerializer callbackDataSerializer) =>
        _callbackDataSerializer = callbackDataSerializer;

    protected CallbackQuery CallbackQuery => Payload;
    protected Message Message => Payload.Message!;
    protected Chat Chat => Message.Chat;
    protected T? CallbackData { get; private set; }

    public override Task<bool> CanHandleAsync(CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(Payload.Data))
        {
            return Task.FromResult(false);
        }

        var (canBeSerialized, callbackData) = _callbackDataSerializer.TryDeserialize<T>(Payload.Data);
        if (!canBeSerialized)
        {
            return Task.FromResult(false);
        }

        CallbackData = callbackData;
        return Task.FromResult(true);
    }
}
