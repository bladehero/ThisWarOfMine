using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ThisWarOfMine.Application.Telegram.Abstraction.Serialization;

public static class DependencyInjectionExtensions
{
    private static readonly Assembly ThisAssembly = typeof(DependencyInjectionExtensions).Assembly;

    public static IServiceCollection AddTelegramCallbackDataSerialization(this IServiceCollection services)
    {
        return services
            .AddJsonExtractors()
            .AddTransient<ITelegramCallbackDataSerializer, TelegramCallbackDataSerializer>();
    }

    private static IServiceCollection AddJsonExtractors(this IServiceCollection services)
    {
        var extractors = ThisAssembly
            .DefinedTypes.Where(x => x.GetCustomAttribute<TelegramCallbackTypeNameAttribute>() is not null)
            .Select(type => new TelegramCallbackDataJsonExtractor(type))
            .ToArray();

        ThrowIfDuplications(extractors);

        foreach (var extractor in extractors)
        {
            services.AddSingleton<ITelegramCallbackDataJsonExtractor>(extractor);
        }

        return services;
    }

    private static void ThrowIfDuplications(IEnumerable<TelegramCallbackDataJsonExtractor> extractors)
    {
        var duplications = extractors.GroupBy(x => x.Attribute.Name).Where(x => x.Count() > 1).ToArray();
        if (duplications.Any())
        {
            throw new InvalidOperationException(
                $"All extractors' type names should be unique but found duplications for keys: {string.Join(", ", duplications.Select(x => x.Key))}"
            );
        }
    }
}
