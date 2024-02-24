using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using ThisWarOfMine.Common;

namespace ThisWarOfMine.Infrastructure.Telegram;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTelegramInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<TelegramBotClient>? configure = null
    )
    {
        return services
            .AddConfiguration<TelegramBotConfiguration>(configuration)
            .AddSingleton<ITelegramBotClient>(ClientFactory(configure));
    }

    private static Func<IServiceProvider, TelegramBotClient> ClientFactory(Action<TelegramBotClient>? configure) =>
        provider =>
        {
            var options = provider.GetRequiredService<IOptions<TelegramBotConfiguration>>();
            var client = new TelegramBotClient(options.Value);
            configure?.Invoke(client);
            return client;
        };
}
