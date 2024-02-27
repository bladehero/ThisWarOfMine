using Microsoft.Extensions.Localization;
using ThisWarOfMine.Application.Telegram.Abstraction;
using ThisWarOfMine.Infrastructure.Telegram.Resources.Core;

namespace ThisWarOfMine.Infrastructure.Telegram.Resources.Messages;

internal sealed class ResponseLocalizer : BaseLocalizer<Responses>, IResponseLocalizer
{
    public ResponseLocalizer(IStringLocalizerFactory localizerFactory)
        : base(localizerFactory) { }
}
