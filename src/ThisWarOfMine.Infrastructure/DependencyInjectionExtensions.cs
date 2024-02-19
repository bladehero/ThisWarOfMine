using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThisWarOfMine.Common;
using ThisWarOfMine.Domain.Narrative;
using ThisWarOfMine.Infrastructure.Books;

namespace ThisWarOfMine.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly[] assemblies
    ) =>
        services
            .AddScoped<IBookRepository, BookZipArchiveRepository>()
            .AddConfiguration<BookConfiguration>(configuration)
            .AddMediatR(x => x.RegisterServicesFromAssemblies(assemblies));
}
