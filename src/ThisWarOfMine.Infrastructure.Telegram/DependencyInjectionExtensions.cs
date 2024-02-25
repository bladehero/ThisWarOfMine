using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using ThisWarOfMine.Infrastructure.Telegram.Notifications;
using ThisWarOfMine.Infrastructure.Telegram.Resources;
using ThisWarOfMine.Infrastructure.Telegram.States;

namespace ThisWarOfMine.Infrastructure.Telegram;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTelegramInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<TelegramBotClient>? configure = null
    ) => services.AddTelegramHandling(configuration, configure).AddTelegramResources().AddTelegramStates();
}
