using Microsoft.Extensions.Localization;
using ThisWarOfMine.Application.Telegram.MessageHandlers;
using ThisWarOfMine.Application.Telegram.MessageHandlers.Core;
using ThisWarOfMine.Infrastructure.Telegram.Resources.Core;

namespace ThisWarOfMine.Infrastructure.Telegram.Resources.Messages;

internal sealed class MessageResponseLocalizer : BaseLocalizer<Responses>, IMessageResponseLocalizer
{
    public MessageResponseLocalizer(IStringLocalizerFactory localizerFactory)
        : base(localizerFactory) { }
}
