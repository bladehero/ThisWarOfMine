using Microsoft.Extensions.DependencyInjection;

namespace ThisWarOfMine.Common.Wrappers;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddCommonWrappers(this IServiceCollection services)
    {
        var assembly = typeof(IWrapper).Assembly;
        return services.AddAll<IWrapper>(new[] { assembly }, ServiceLifetime.Transient);
    }
}
