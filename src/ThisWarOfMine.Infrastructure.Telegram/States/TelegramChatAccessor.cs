using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ThisWarOfMine.Application.Telegram.States;

namespace ThisWarOfMine.Infrastructure.Telegram.States;

internal sealed class TelegramChatAccessor : ITelegramChatAccessor
{
    public Chat? Chat { get; private set; }
    public User? User { get; private set; }

    public void Initialize(Update update)
    {
        Chat = ExtractChatFrom(update);
        User = ExtractUserFrom(update);
    }

    private static Chat? ExtractChatFrom(Update update)
    {
        return update.Type switch
        {
            UpdateType.Unknown => null,
            UpdateType.Message => update.Message!.Chat,
            UpdateType.InlineQuery => null,
            UpdateType.ChosenInlineResult => null,
            UpdateType.CallbackQuery => update.CallbackQuery!.Message!.Chat,
            UpdateType.EditedMessage => update.EditedMessage!.Chat,
            UpdateType.ChannelPost => update.ChannelPost!.Chat,
            UpdateType.EditedChannelPost => update.EditedChannelPost!.Chat,
            UpdateType.ShippingQuery => null,
            UpdateType.PreCheckoutQuery => null,
            UpdateType.Poll => null,
            UpdateType.PollAnswer => null,
            UpdateType.MyChatMember => update.MyChatMember!.Chat,
            UpdateType.ChatMember => update.ChatMember!.Chat,
            UpdateType.ChatJoinRequest => update.ChatJoinRequest!.Chat,
            _ => null
        };
    }

    private static User? ExtractUserFrom(Update update)
    {
        return update.Type switch
        {
            UpdateType.Unknown => null,
            UpdateType.Message => update.Message!.From,
            UpdateType.InlineQuery => update.InlineQuery!.From,
            UpdateType.ChosenInlineResult => update.ChosenInlineResult!.From,
            UpdateType.CallbackQuery => update.CallbackQuery!.From,
            UpdateType.EditedMessage => update.EditedMessage!.From,
            UpdateType.ChannelPost => update.ChannelPost!.From,
            UpdateType.EditedChannelPost => update.EditedChannelPost!.From,
            UpdateType.ShippingQuery => update.ShippingQuery!.From,
            UpdateType.PreCheckoutQuery => update.PreCheckoutQuery!.From,
            UpdateType.Poll => null,
            UpdateType.PollAnswer => update.PollAnswer!.User,
            UpdateType.MyChatMember => update.MyChatMember!.From,
            UpdateType.ChatMember => update.ChatMember!.From,
            UpdateType.ChatJoinRequest => update.ChatJoinRequest!.From,
            _ => null
        };
    }
}
