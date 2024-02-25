using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ThisWarOfMine.Common;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddConfiguration<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        string? name = null
    )
        where T : class => services.Configure<T>(configuration.GetSection(name ?? typeof(T).Name));
}
