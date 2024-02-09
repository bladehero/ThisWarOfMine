using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThisWarOfMine.Contracts;
using ThisWarOfMine.Splitter;

var builder = Host.CreateApplicationBuilder(args);
builder
    .Services
    .AddScoped<IBookCreator, BookCreator>()
    .AddScoped<IBookSplitter, BookSplitter>()
    .AddScoped<IStoryParser, StoryParser>();
using var host = builder.Build();

await host.StartAsync();

const string bookFile = "book.txt";
var book = await host.Services.GetRequiredService<IBookCreator>().CreateAsync(bookFile, Language.Russian);

;