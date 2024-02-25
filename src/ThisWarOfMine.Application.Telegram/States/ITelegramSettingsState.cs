namespace ThisWarOfMine.Application.Telegram.States;

public interface ITelegramSettingsState
{
    TelegramSettings Get();
    void Change(Action<TelegramSettings> configure);
}
