namespace ThisWarOfMine.Infrastructure.ChatGpt;

public interface IChatGpt
{
    Task<string> AskAsync(string value);
}
