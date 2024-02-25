using System.Reflection;
using System.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ThisWarOfMine.Application.Telegram.States;

namespace ThisWarOfMine.Infrastructure.Telegram.Resources.Core;

internal sealed class TelegramStringLocalizerFactory : ResourceManagerStringLocalizerFactory
{
    private readonly ITelegramSettingsState _telegramSettingsState;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IResourceNamesCache _resourceNamesCache = new ResourceNamesCache();

    public TelegramStringLocalizerFactory(
        ITelegramSettingsState telegramSettingsState,
        IOptions<LocalizationOptions> localizationOptions,
        ILoggerFactory loggerFactory
    )
        : base(localizationOptions, loggerFactory)
    {
        _telegramSettingsState = telegramSettingsState;
        _loggerFactory = loggerFactory;
    }

    protected override ResourceManagerStringLocalizer CreateResourceManagerStringLocalizer(
        Assembly assembly,
        string baseName
    )
    {
        return new TelegramStringLocalizer(
            _telegramSettingsState,
            new ResourceManager(baseName, assembly),
            assembly,
            baseName,
            _resourceNamesCache,
            _loggerFactory.CreateLogger<ResourceManagerStringLocalizer>()
        );
    }
}
