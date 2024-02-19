using Microsoft.Extensions.DependencyInjection;
using ThisWarOfMine.Splitter.Options;

namespace ThisWarOfMine.Splitter;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSplitter(this IServiceCollection services) =>
        services
            .AddScoped<IBookCreator, BookCreator>()
            .AddScoped<IBookSplitter, BookSplitter>()
            .AddScoped<IStoryParser, StoryParser>()
            .AddScoped<IOptionParser, OptionParser>()
            .AddScoped<IOptionParsingStrategy, OnlyBackToGameOptionParsingStrategy>()
            .AddScoped<IOptionParsingStrategy, RemarkOptionParsingStrategy>()
            .AddScoped<IOptionParsingStrategy, BackToStoryOptionParsingStrategy>()
            .AddScoped<IOptionParsingStrategy, RedirectOptionParsingStrategy>()
            .AddScoped<IOptionParsingStrategy, TextOptionParsingStrategy>();
}
