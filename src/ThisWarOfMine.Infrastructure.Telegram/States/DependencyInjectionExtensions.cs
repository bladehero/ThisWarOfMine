using Microsoft.Extensions.DependencyInjection;
using ThisWarOfMine.Application.Telegram.States;

namespace ThisWarOfMine.Infrastructure.Telegram.States;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTelegramStates(this IServiceCollection services)
    {
        return services
            .AddScoped<ITelegramSettingsState, TelegramSettingsState>()
            .AddScoped<ITelegramChatAccessor, TelegramChatAccessor>()
            .AddScoped<IRandomAlternativePicker, RandomAlternativePicker>();
    }
}
