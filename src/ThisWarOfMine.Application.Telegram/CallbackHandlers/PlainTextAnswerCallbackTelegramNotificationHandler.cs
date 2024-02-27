using CSharpFunctionalExtensions;
using Telegram.Bot;
using ThisWarOfMine.Application.Telegram.Abstraction.Serialization;
using ThisWarOfMine.Application.Telegram.CallbackHandlers.Contracts;
using ThisWarOfMine.Application.Telegram.CallbackHandlers.Core;
using ThisWarOfMine.Application.Telegram.States;
using ThisWarOfMine.Domain.Narrative.Options;

namespace ThisWarOfMine.Application.Telegram.CallbackHandlers;

internal sealed class PlainTextAnswerCallbackTelegramNotificationHandler
    : BaseCallbackTelegramNotificationHandler<PlainTextAnswerData>
{
    private readonly IRandomAlternativePicker _randomAlternativePicker;

    public PlainTextAnswerCallbackTelegramNotificationHandler(
        ITelegramCallbackDataSerializer callbackDataSerializer,
        IRandomAlternativePicker randomAlternativePicker
    )
        : base(callbackDataSerializer)
    {
        _randomAlternativePicker = randomAlternativePicker;
    }

    public override Task HandleAsync(CancellationToken token)
    {
        var (storyNumber, optionOrder) = CallbackData!;
        return _randomAlternativePicker
            .GetRandomAsync(storyNumber, token: token)
            .Map(x => x.Options[optionOrder])
            .Tap(SendAnswerAndDeletePreviousMessage);

        async Task SendAnswerAndDeletePreviousMessage(Option option)
        {
            var sending = Client.SendTextMessageAsync(Chat.Id, GetOptionText(option), cancellationToken: token);
            var deleting = Client.DeleteMessageAsync(Chat.Id, Message.MessageId, cancellationToken: token);
            await Task.WhenAll(sending, deleting);
        }
    }

    private static string GetOptionText(Option option) =>
        option switch
        {
            ComplexOption complexOption => complexOption.Next.Text,
            _ => option.Text
        };
}
