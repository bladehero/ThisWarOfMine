using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThisWarOfMine.Contracts;
using ThisWarOfMine.Splitter;

var builder = Host.CreateApplicationBuilder(args);
builder
    .Services
    .AddScoped<IBookSplitter, BookSplitter>()
    .AddScoped<IStoryCreator, StoryCreator>();
using var host = builder.Build();

await host.StartAsync();

await foreach (var story in host.Services.GetRequiredService<IBookSplitter>().SplitAsync(Language.Russian))
{
    Console.WriteLine(story);
}