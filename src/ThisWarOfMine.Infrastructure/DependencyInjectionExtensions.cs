using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThisWarOfMine.Common;
using ThisWarOfMine.Domain.Narrative;
using ThisWarOfMine.Infrastructure.Books;
using ThisWarOfMine.Infrastructure.Books.Options;

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
            .AddScoped<IBookNameResolver, BookNameResolver>()
            .AddScoped<IZipBookCreator, ZipBookCreator>()
            .AddScoped<IOptionDataSerializer, OptionDataSerializer>()
            .AddAll<IOptionSerializationStrategy>(assemblies)
            .AddBookAccessor()
            .AddBookConfiguration(configuration)
            .AddMediatR(x => x.RegisterServicesFromAssemblies(assemblies));

    public static IServiceCollection ConfigureBookAccessor(
        this IServiceCollection services,
        Action<BookAccessorConfiguration>? configure = null
    )
    {
        var builder = services.AddOptions<BookAccessorConfiguration>();
        if (configure is not null)
        {
            builder.PostConfigure(configure);
        }

        return services;
    }

    private static IServiceCollection AddBookAccessor(this IServiceCollection services) =>
        services.AddScoped<IBookAccessor, BookAccessor>().ConfigureBookAccessor();

    private static IServiceCollection AddBookConfiguration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var builder = services.AddOptions<BookConfiguration>();
        var section = configuration.GetSection($"{nameof(BookConfiguration)}:Folder");
        if (!string.IsNullOrWhiteSpace(section.Value))
        {
            builder.Configure(x => x.Folder = section.Value);
            return services;
        }

        var settings = new BookFolderPath.BookFolderSettings();
        section.Bind(settings);
        builder.Configure(x => x.Folder = settings);
        return services;
    }
}
