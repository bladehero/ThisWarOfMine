using Telegram.Bot;
using ThisWarOfMine.Application.Telegram.Abstraction;
using ThisWarOfMine.Application.Telegram.Helpers;
using ThisWarOfMine.Application.Telegram.MessageHandlers.Core;

namespace ThisWarOfMine.Application.Telegram.MessageHandlers.StoryPicked;

internal sealed class StoryPickerTelegramNotificationHandler : TextCommandWithOptionsTelegramNotificationHandler
{
    private const string NotValidStoryNumber = nameof(NotValidStoryNumber);

    private readonly IStorySendingHelper _storySendingHelper;
    private readonly IResponseLocalizer _responseLocalizer;

    public StoryPickerTelegramNotificationHandler(
        IStorySendingHelper storySendingHelper,
        IResponseLocalizer responseLocalizer
    )
    {
        _storySendingHelper = storySendingHelper;
        _responseLocalizer = responseLocalizer;
    }

    protected override string Command => "story";

    protected override bool OptionsAreRequired => true;

    public override Task HandleAsync(CancellationToken token)
    {
        if (!short.TryParse(CommandOptions![0], out var storyNumber))
        {
            return Client.SendTextMessageAsync(
                Chat.Id,
                _responseLocalizer.GetString(NotValidStoryNumber),
                cancellationToken: token
            );
        }

        return _storySendingHelper.SendAsync(storyNumber, Chat.Id, token);
    }
}
