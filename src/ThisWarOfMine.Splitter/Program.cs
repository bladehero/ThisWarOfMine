using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThisWarOfMine.Common;
using ThisWarOfMine.Domain.Narrative;
using ThisWarOfMine.Infrastructure;
using ThisWarOfMine.Splitter;

var assemblies = AssemblyHelper.LoadAssemblies<Program>("ThisWarOfMine");
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration, assemblies).AddSplitter();
using var host = builder.Build();

await host.StartAsync();

const string bookName = "Book of Scripts";
const string bookFile = "book.txt";

var bookCreator = host.Services.GetRequiredService<IBookCreator>();
var book = bookCreator.InitializeAsync(bookName);
await foreach (var story in bookCreator.FulFillAsync(bookFile, Language.Russian))
{
    ;
}
