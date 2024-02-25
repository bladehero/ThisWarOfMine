using Microsoft.Extensions.Localization;
using ThisWarOfMine.Application.Telegram;

namespace ThisWarOfMine.Infrastructure.Telegram.Resources.Core;

public abstract class BaseLocalizer<TResource> : ILocalizer
{
    private readonly IStringLocalizer _localizer;

    protected BaseLocalizer(IStringLocalizerFactory localizerFactory)
    {
        ArgumentNullException.ThrowIfNull(localizerFactory);

        _localizer = localizerFactory.Create(typeof(TResource));
    }

    public Type ResourceType => typeof(TResource);

    public string GetString(string key) => _localizer.GetString(key);
}
