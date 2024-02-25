using CSharpFunctionalExtensions;
using Telegram.Bot;
using ThisWarOfMine.Application.Telegram.MessageHandlers.Core;
using ThisWarOfMine.Application.Telegram.States;
using ThisWarOfMine.Domain.Narrative;

namespace ThisWarOfMine.Application.Telegram.MessageHandlers;

internal sealed class ChangeLanguageTelegramNotificationHandler : TextCommandTelegramNotificationHandler
{
    private const string NoTranslationForThisLanguage = nameof(NoTranslationForThisLanguage);
    private const string LanguageWasUpdated = nameof(LanguageWasUpdated);
    private readonly ITelegramSettingsState _telegramSettingsState;
    private readonly IMessageResponseLocalizer _messageResponseLocalizer;

    public ChangeLanguageTelegramNotificationHandler(
        ITelegramSettingsState telegramSettingsState,
        IMessageResponseLocalizer messageResponseLocalizer
    )
    {
        _telegramSettingsState = telegramSettingsState;
        _messageResponseLocalizer = messageResponseLocalizer;
    }

    protected override string Command => "language";

    public override async Task<bool> CanHandleAsync(CancellationToken token)
    {
        var cannotHandle = !await base.CanHandleAsync(token);
        if (cannotHandle)
        {
            return false;
        }

        var optionsAreEmpty = CommandOptions?.Any() != true;
        if (optionsAreEmpty)
        {
            return false;
        }

        return true;
    }

    public override Task HandleAsync(CancellationToken token)
    {
        var (canBeParsed, language) = Language.TryFromShortName(CommandOptions![0]);
        if (!canBeParsed)
        {
            return Client.SendTextMessageAsync(
                Message.Chat.Id,
                _messageResponseLocalizer.GetString(NoTranslationForThisLanguage),
                cancellationToken: token
            );
        }

        _telegramSettingsState.Change(x => x.Language = language);
        return Client.SendTextMessageAsync(
            Message.Chat.Id,
            _messageResponseLocalizer.GetString(LanguageWasUpdated),
            cancellationToken: token
        );
    }
}
