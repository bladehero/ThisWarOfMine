using System.Collections.Concurrent;
using ThisWarOfMine.Application.Telegram.States;

namespace ThisWarOfMine.Infrastructure.Telegram.States;

internal sealed class TelegramSettingsState : ITelegramSettingsState
{
    private static readonly ConcurrentDictionary<long, TelegramSettings> Settings = new();
    private readonly ITelegramChatAccessor _telegramChatAccessor;

    public TelegramSettingsState(ITelegramChatAccessor telegramChatAccessor)
    {
        _telegramChatAccessor = telegramChatAccessor;
    }

    public TelegramSettings Get() => Settings.GetOrAdd(_telegramChatAccessor.Chat!.Id, _ => new TelegramSettings());

    public void Change(Action<TelegramSettings> configure)
    {
        var settings = Get();
        configure.Invoke(settings);
    }
}
