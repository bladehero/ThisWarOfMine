using ChatGPT.Net.DTO;
using Microsoft.Extensions.Options;

namespace ThisWarOfMine.Infrastructure.ChatGpt;

internal sealed class ChatGptWrapper : IChatGpt
{
    private readonly ChatGPT.Net.ChatGptUnofficial _bot;

    public ChatGptWrapper(IOptions<ChatGptConfiguration> options) =>
        _bot = new ChatGPT.Net.ChatGptUnofficial(
            options.Value.SecretKey!,
            new ChatGptUnofficialOptions { BaseUrl = "http://localhost:3000" }
        );

    public Task<string> AskAsync(string value) => _bot.Ask(value);
}
