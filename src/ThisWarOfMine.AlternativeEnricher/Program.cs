using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ThisWarOfMine.Common;
using ThisWarOfMine.Common.Wrappers;
using ThisWarOfMine.Domain;
using ThisWarOfMine.Domain.Narrative;
using ThisWarOfMine.Infrastructure.Books;
using ThisWarOfMine.Infrastructure.ChatGpt;

var assemblies = AssemblyHelper.LoadAssemblies<Program>("ThisWarOfMine");
var builder = Host.CreateApplicationBuilder(args);
builder
    .Services.AddCommonWrappers()
    .AddChatGptInfrastructure(builder.Configuration)
    .AddBookInfrastructure(builder.Configuration);
using var host = builder.Build();

await host.StartAsync();

var repository = host.Services.GetRequiredService<IBookRepository>();
var chatGpt = host.Services.GetRequiredService<IChatGpt>();
var book = await repository
    .FindByNameAsync(Constants.BookOfScripts)
    .OnFallback(error => throw new InvalidOperationException(error.Message));
foreach (var story in book.Stories)
{
    var text = await chatGpt.AskAsync($"Can you retell this text with saving idea at most: \"{story.Original.Text}\"");
    book.AddTranslationAlternative(story.Number, Language.Russian, Guid.NewGuid(), text);
}
