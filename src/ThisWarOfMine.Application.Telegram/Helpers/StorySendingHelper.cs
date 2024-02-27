using CSharpFunctionalExtensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using ThisWarOfMine.Application.Telegram.Abstraction;
using ThisWarOfMine.Application.Telegram.MessageHandlers.StoryPicked;
using ThisWarOfMine.Application.Telegram.States;
using ThisWarOfMine.Domain.Abstraction;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Application.Telegram.Helpers;

internal sealed class StorySendingHelper : IStorySendingHelper
{
    private const string StoryNotFound = nameof(StoryNotFound);

    private readonly IRandomAlternativePicker _randomAlternativePicker;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IStoryMessageBuilder _storyMessageBuilder;
    private readonly IResponseLocalizer _responseLocalizer;

    public StorySendingHelper(
        IRandomAlternativePicker randomAlternativePicker,
        ITelegramBotClient telegramBotClient,
        IStoryMessageBuilder storyMessageBuilder,
        IResponseLocalizer responseLocalizer
    )
    {
        _randomAlternativePicker = randomAlternativePicker;
        _telegramBotClient = telegramBotClient;
        _storyMessageBuilder = storyMessageBuilder;
        _responseLocalizer = responseLocalizer;
    }

    public Task<Result<Alternative, Error>> SendAsync(short storyNumber, long chatId, CancellationToken token)
    {
        return _randomAlternativePicker
            .GetRandomAsync(storyNumber, SendThatStoryNotFound, token)
            .Tap(SendAlternativeToUser)
            .TapError(error => throw new InvalidOperationException($"Cannot find any alternative: {error}"));

        #region Local Functions

        Task<Message> SendThatStoryNotFound()
        {
            return _telegramBotClient.SendTextMessageAsync(
                chatId,
                _responseLocalizer.GetString(StoryNotFound),
                cancellationToken: token
            );
        }

        Task SendAlternativeToUser(Alternative alternative)
        {
            var (text, markup) = _storyMessageBuilder.Build(alternative);
            return _telegramBotClient.SendTextMessageAsync(chatId, text, replyMarkup: markup, cancellationToken: token);
        }

        #endregion
    }
}
