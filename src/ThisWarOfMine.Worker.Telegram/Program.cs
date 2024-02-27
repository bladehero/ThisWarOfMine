using ThisWarOfMine.Application.Telegram;
using ThisWarOfMine.Common.Wrappers;
using ThisWarOfMine.Infrastructure.Books;
using ThisWarOfMine.Infrastructure.Telegram;
using ThisWarOfMine.Worker.Telegram;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        (context, services) =>
            services
                .AddCommonWrappers()
                .AddTelegramApplication()
                .AddTelegramInfrastructure(context.Configuration)
                .AddBookInfrastructure(context.Configuration)
                .AddTelegramWorker()
    )
    .Build();

host.Run();
