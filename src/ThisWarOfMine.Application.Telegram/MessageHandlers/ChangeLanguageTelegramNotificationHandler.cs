using CSharpFunctionalExtensions;
using Telegram.Bot;
using ThisWarOfMine.Application.Telegram.Abstraction;
using ThisWarOfMine.Application.Telegram.MessageHandlers.Core;
using ThisWarOfMine.Application.Telegram.States;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Application.Telegram.MessageHandlers;

internal sealed class ChangeLanguageTelegramNotificationHandler : TextCommandWithOptionsTelegramNotificationHandler
{
    private const string NoTranslationForThisLanguage = nameof(NoTranslationForThisLanguage);
    private const string LanguageWasUpdated = nameof(LanguageWasUpdated);
    private readonly ITelegramSettingsState _telegramSettingsState;
    private readonly IResponseLocalizer _responseLocalizer;

    public ChangeLanguageTelegramNotificationHandler(
        ITelegramSettingsState telegramSettingsState,
        IResponseLocalizer responseLocalizer
    )
    {
        _telegramSettingsState = telegramSettingsState;
        _responseLocalizer = responseLocalizer;
    }

    protected override string Command => "language";

    protected override bool OptionsAreRequired => true;

    public override Task HandleAsync(CancellationToken token)
    {
        var (canBeParsed, language) = Language.TryFromShortName(CommandOptions![0]);
        if (!canBeParsed)
        {
            return Client.SendTextMessageAsync(
                Chat.Id,
                _responseLocalizer.GetString(NoTranslationForThisLanguage),
                cancellationToken: token
            );
        }

        _telegramSettingsState.Change(x => x.Language = language);
        return Client.SendTextMessageAsync(
            Chat.Id,
            _responseLocalizer.GetString(LanguageWasUpdated),
            cancellationToken: token
        );
    }
}
