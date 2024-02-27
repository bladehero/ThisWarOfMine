using System.Text;
using CSharpFunctionalExtensions;
using Telegram.Bot.Types.ReplyMarkups;
using ThisWarOfMine.Application.Telegram.Abstraction;
using ThisWarOfMine.Application.Telegram.CallbackHandlers.Contracts;
using ThisWarOfMine.Domain.Narrative;
using ThisWarOfMine.Domain.Narrative.Options;

namespace ThisWarOfMine.Application.Telegram.MessageHandlers.StoryPicked;

internal sealed class StoryMessageBuilder : IStoryMessageBuilder
{
    private const string OptionsHeader = nameof(OptionsHeader);
    private const int MaxOptionsRowLength = 4;

    private readonly IResponseLocalizer _responseLocalizer;
    private readonly IInlineKeyBoardButtonProvider _inlineKeyBoardButtonProvider;

    public StoryMessageBuilder(
        IResponseLocalizer responseLocalizer,
        IInlineKeyBoardButtonProvider inlineKeyBoardButtonProvider
    )
    {
        _responseLocalizer = responseLocalizer;
        _inlineKeyBoardButtonProvider = inlineKeyBoardButtonProvider;
    }

    public StoryMessage Build(Alternative alternative)
    {
        var builder = new StringBuilder(alternative.Text).AppendLine().AppendLine();
        AppendOptions(alternative, builder);

        var chunks = alternative.Options.Where(x => x.IsSelectable).Select(AsOptionButton).Chunk(MaxOptionsRowLength);
        var markup = new InlineKeyboardMarkup(chunks);
        return new StoryMessage(builder.ToString(), markup);
    }

    private void AppendOptions(Alternative alternative, StringBuilder builder)
    {
        if (alternative.Options.Count > 1)
        {
            builder.Append(_responseLocalizer.GetString(OptionsHeader)).Append(':').AppendLine();
        }

        var number = 1;
        foreach (var option in alternative.Options)
        {
            if (option.IsSelectable)
            {
                builder.Append(number).Append('.').Append(' ');
                number++;
            }

            builder.AppendLine(GetOptionText(option));
        }
    }

    private static string GetOptionText(Option option) =>
        option switch
        {
            ComplexOption complexOption => complexOption.Current.Text,
            _ => option.Text
        };

    private InlineKeyboardButton AsOptionButton(Option option, int index)
    {
        return _inlineKeyBoardButtonProvider.Create($"{index + 1}", CallbackDataFrom(option));
    }

    private static object CallbackDataFrom(Option option)
    {
        var (hasNextStory, nextStoryNumber) = option.GetRedirectionStoryNumber();
        if (hasNextStory)
        {
            return new RedirectToStoryData(nextStoryNumber);
        }

        var alternative = option.Group.Alternative;
        var storyNumber = alternative.Translation.Story.Number;
        return option switch
        {
            BackToGameOption => new BackToGameAnswerData(),
            SimpleOption => new BackToGameAnswerData(),
            ComplexOption complexOption => new PlainTextAnswerData(storyNumber, option.Order),
            _ => throw new InvalidOperationException($"Cannot construct callback data for {option}")
        };
    }
}
