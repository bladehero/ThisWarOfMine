using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using ThisWarOfMine.Application.Telegram;
using ThisWarOfMine.Application.Telegram.States;
using ThisWarOfMine.Common;
using ThisWarOfMine.Infrastructure.Telegram.Resources.Core;
using ThisWarOfMine.Infrastructure.Telegram.States;

namespace ThisWarOfMine.Infrastructure.Telegram.Resources;

public static class DependencyInjectionExtensions
{
    private static readonly Assembly ThisAssembly = typeof(DependencyInjectionExtensions).Assembly;

    public static IServiceCollection AddTelegramResources(this IServiceCollection services) =>
        services
            .AddLocalization()
            .Replace(ServiceDescriptor.Scoped<IStringLocalizerFactory, TelegramStringLocalizerFactory>())
            .AddAll<ILocalizer>(new[] { ThisAssembly }, ServiceLifetime.Transient);
}
