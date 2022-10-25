namespace DebugLaunchIssue;

public class QueueProcessingService : BackgroundService
{
    private ILogger<QueueProcessingService> Logger { get; }
    private Queue<string> Queue { get; }
    private Task? _longRunningTask;

    public QueueProcessingService(ILogger<QueueProcessingService> logger)
    {
        Logger = logger;
        Queue = new Queue<string>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation("Starting queue processor");
        _longRunningTask = Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (Queue.Count == 0)
                    {
                        continue;
                    }

                    string content = Queue.Dequeue();
                    Logger.LogInformation("{Content} has been processed", content);
                }
            },
            stoppingToken
        );
    }
}