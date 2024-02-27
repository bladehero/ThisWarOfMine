using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Application.Telegram.MessageHandlers.StoryPicked;

internal interface IStoryMessageBuilder
{
    StoryMessage Build(Alternative alternative);
}
