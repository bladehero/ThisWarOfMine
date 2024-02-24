using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThisWarOfMine.Common;
using ThisWarOfMine.Common.Wrappers;
using ThisWarOfMine.Domain.Narrative;
using ThisWarOfMine.Infrastructure;
using ThisWarOfMine.Infrastructure.Books;
using ThisWarOfMine.Splitter;

var assemblies = AssemblyHelper.LoadAssemblies<Program>("ThisWarOfMine");
var longWriteOperationAccessor = new LongWriteOperationSingleBookAccessor();
var strategy = new NoDisposeBookAccessingStrategy(longWriteOperationAccessor);
var builder = Host.CreateApplicationBuilder(args);
builder
    .Services.AddCommonWrappers()
    .AddBookInfrastructure(builder.Configuration, assemblies)
    .ConfigureBookAccessor(x => x.AccessingStrategy = strategy)
    .AddSplitter();
using var host = builder.Build();

await host.StartAsync();

const string bookFile = "book.txt";

var bookCreator = host.Services.GetRequiredService<IBookCreator>();
var book = await bookCreator.InitializeAsync(Constants.BookOfScripts);
await foreach (var story in bookCreator.FulFillAsync(bookFile, Language.Russian))
{
    Console.WriteLine(story.Number);
}

longWriteOperationAccessor.Dispose();

var repository = host.Services.GetRequiredService<IBookRepository>();
var result = await repository.LoadAsync(book.Id);
;
