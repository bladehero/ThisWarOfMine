using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThisWarOfMine.Domain.Narrative;
using ThisWarOfMine.Splitter;
using ThisWarOfMine.Splitter.Options;
using OptionParser = ThisWarOfMine.Splitter.Options.OptionParser;

var builder = Host.CreateApplicationBuilder(args);
builder
    .Services
    .AddScoped<IBookCreator, BookCreator>()
    .AddScoped<IBookSplitter, BookSplitter>()
    .AddScoped<IStoryParser, StoryParser>()
    .AddScoped<IOptionParser, OptionParser>()
    .AddScoped<IOptionParsingStrategy, OnlyBackToGameOptionParsingStrategy>()
    .AddScoped<IOptionParsingStrategy, RemarkOptionParsingStrategy>()
    .AddScoped<IOptionParsingStrategy, BackToStoryOptionParsingStrategy>()
    .AddScoped<IOptionParsingStrategy, RedirectOptionParsingStrategy>()
    .AddScoped<IOptionParsingStrategy, TextOptionParsingStrategy>();
using var host = builder.Build();

await host.StartAsync();

const string bookFile = "book.txt";

var bookCreator = host.Services.GetRequiredService<IBookCreator>();
var book = await bookCreator.CreateAsync("Book of Scripts", bookFile, Language.Russian);
;