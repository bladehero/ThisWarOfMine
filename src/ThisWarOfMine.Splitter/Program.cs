using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThisWarOfMine.Contracts;
using ThisWarOfMine.Contracts.Narrative;
using ThisWarOfMine.Splitter;

var builder = Host.CreateApplicationBuilder(args);
builder
    .Services
    .AddScoped<IBookSplitter, BookSplitter>()
    .AddScoped<IStoryCreator, StoryCreator>();
using var host = builder.Build();

await host.StartAsync();

const string bookFile = "book.txt";
var list = new List<Story>();
await foreach (var story in host.Services.GetRequiredService<IBookSplitter>().SplitAsync(bookFile, Language.Russian))
{
    list.Add(story);
}

;