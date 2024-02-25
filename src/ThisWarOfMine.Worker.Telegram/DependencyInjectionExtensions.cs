namespace ThisWarOfMine.Worker.Telegram;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTelegramWorker(this IServiceCollection services)
    {
        return services.AddHostedService<Worker>();
    }
}
