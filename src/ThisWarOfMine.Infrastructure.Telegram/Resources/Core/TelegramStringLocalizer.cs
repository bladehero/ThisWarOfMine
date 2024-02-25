using System.Globalization;
using System.Reflection;
using System.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ThisWarOfMine.Application.Telegram.States;

namespace ThisWarOfMine.Infrastructure.Telegram.Resources.Core;

internal sealed class TelegramStringLocalizer : ResourceManagerStringLocalizer
{
    private readonly ITelegramSettingsState _telegramSettingsState;
    private readonly string _resourceBaseName;

    public TelegramStringLocalizer(
        ITelegramSettingsState telegramSettingsState,
        ResourceManager resourceManager,
        Assembly resourceAssembly,
        string resourceBaseName,
        IResourceNamesCache resourceNamesCache,
        ILogger logger
    )
        : base(resourceManager, resourceAssembly, resourceBaseName, resourceNamesCache, logger)
    {
        _telegramSettingsState = telegramSettingsState;
        _resourceBaseName = resourceBaseName;
    }

    public override LocalizedString this[string name]
    {
        get
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var value = GetStringSafely(name, CurrentCulture);

            return new LocalizedString(
                name,
                value ?? name,
                resourceNotFound: value == null,
                searchedLocation: _resourceBaseName
            );
        }
    }

    public override LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var format = GetStringSafely(name, null);
            var value = string.Format(CurrentCulture, format ?? name, arguments);

            return new LocalizedString(
                name,
                value,
                resourceNotFound: format == null,
                searchedLocation: _resourceBaseName
            );
        }
    }

    private CultureInfo CurrentCulture => _telegramSettingsState.Get().Language.Culture;
}
