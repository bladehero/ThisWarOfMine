namespace ThisWarOfMine.Application.Telegram;

public interface ILocalizer
{
    Type ResourceType { get; }
    string GetString(string key);
}
