using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ThisWarOfMine.Common
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddAll<T>(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies,
            ServiceLifetime lifetime = ServiceLifetime.Scoped
        )
        {
            var markerImplementations = assemblies
                .SelectMany(x => x.DefinedTypes)
                .Where(x => !x.IsAbstract && x.ImplementedInterfaces.Any(i => i == typeof(T)));

            foreach (var provider in markerImplementations)
            {
                foreach (var implementedInterface in provider.ImplementedInterfaces)
                {
                    var descriptor = ServiceDescriptor.Describe(implementedInterface, provider, lifetime);
                    services.Add(descriptor);
                }
            }

            return services;
        }
    }
}
