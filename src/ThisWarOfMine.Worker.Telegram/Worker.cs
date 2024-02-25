using Telegram.Bot;
using Telegram.Bot.Polling;

namespace ThisWarOfMine.Worker.Telegram;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<Worker> _logger;

    public Worker(IServiceScopeFactory serviceScopeFactory, ILogger<Worker> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var client = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
            var handler = scope.ServiceProvider.GetRequiredService<IUpdateHandler>();
            await client.ReceiveAsync(handler, new ReceiverOptions(), stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}
