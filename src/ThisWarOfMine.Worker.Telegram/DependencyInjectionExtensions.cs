using Telegram.Bot.Polling;

namespace ThisWarOfMine.Worker.Telegram;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTelegramWorker(this IServiceCollection services)
    {
        return services
            .AddHostedService<Worker>()
            .AddSingleton<IUpdateHandler, TelegramUpdateHandler>()
            .AddTransient<INotificationCreator, NotificationCreator>();
    }
}
