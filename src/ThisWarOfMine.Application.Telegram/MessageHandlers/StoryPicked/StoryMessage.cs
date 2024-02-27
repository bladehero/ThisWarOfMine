using Telegram.Bot.Types.ReplyMarkups;

namespace ThisWarOfMine.Application.Telegram.MessageHandlers.StoryPicked;

public record StoryMessage(string Text, IReplyMarkup Markup);
