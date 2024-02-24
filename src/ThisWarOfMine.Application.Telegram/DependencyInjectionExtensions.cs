using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ThisWarOfMine.Application.Telegram.Abstraction;

namespace ThisWarOfMine.Application.Telegram
{
    public static class DependencyInjectionExtensions
    {
        private static readonly Assembly ThisAssembly = typeof(DependencyInjectionExtensions).Assembly;

        public static IServiceCollection AddTelegramApplication(this IServiceCollection services)
        {
            return services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(ThisAssembly);
                configuration.NotificationPublisherType = typeof(TaskWhenAllOrDefaultTelegramNotificationPublisher);
            });
        }
    }
}
