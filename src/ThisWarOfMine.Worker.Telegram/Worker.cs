using Telegram.Bot;
using Telegram.Bot.Polling;

namespace ThisWarOfMine.Worker.Telegram;

public class Worker : BackgroundService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IUpdateHandler _updateHandler;
    private readonly ILogger<Worker> _logger;

    public Worker(ITelegramBotClient telegramBotClient, IUpdateHandler updateHandler, ILogger<Worker> logger)
    {
        _telegramBotClient = telegramBotClient;
        _updateHandler = updateHandler;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await _telegramBotClient.ReceiveAsync(_updateHandler, new ReceiverOptions(), stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
