using System.Diagnostics.CodeAnalysis;
using Telegram.Bot;

namespace ThisWarOfMine.Infrastructure.Telegram
{
    internal sealed class TelegramBotConfiguration
    {
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string? Token { get; set; }

        public static implicit operator TelegramBotClientOptions(TelegramBotConfiguration configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration.Token))
            {
                throw new InvalidOperationException("Bot should never has empty token");
            }

            return new TelegramBotClientOptions(configuration.Token);
        }
    }
}
