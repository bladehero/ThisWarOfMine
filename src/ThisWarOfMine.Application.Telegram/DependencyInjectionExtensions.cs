using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ThisWarOfMine.Application.Telegram.Abstraction;
using ThisWarOfMine.Application.Telegram.Abstraction.Serialization;
using ThisWarOfMine.Application.Telegram.Helpers;
using ThisWarOfMine.Application.Telegram.MessageHandlers.StoryPicked;

namespace ThisWarOfMine.Application.Telegram;

public static class DependencyInjectionExtensions
{
    private static readonly Assembly ThisAssembly = typeof(DependencyInjectionExtensions).Assembly;

    public static IServiceCollection AddTelegramApplication(this IServiceCollection services)
    {
        return services
            .AddMemoryCache()
            .AddHelpers()
            .AddMessageHandling()
            .AddInlineMarkup()
            .AddTelegramCallbackDataSerialization()
            .AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(ThisAssembly);
                configuration.NotificationPublisherType = typeof(TaskWhenAllOrDefaultTelegramNotificationPublisher);
            });
    }

    private static IServiceCollection AddMessageHandling(this IServiceCollection services)
    {
        return services.AddTransient<IStoryMessageBuilder, StoryMessageBuilder>();
    }

    private static IServiceCollection AddInlineMarkup(this IServiceCollection services)
    {
        return services.AddTransient<IInlineKeyBoardButtonProvider, InlineKeyBoardButtonProvider>();
    }

    private static IServiceCollection AddHelpers(this IServiceCollection services)
    {
        return services.AddScoped<IStorySendingHelper, StorySendingHelper>();
    }
}
