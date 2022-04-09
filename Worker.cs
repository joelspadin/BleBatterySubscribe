namespace BleBatterySubscribe;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Worker : BackgroundService
{
    private readonly BatteryServiceWatcher _watcher;
    private readonly ILogger<Worker> _logger;

    public Worker(BatteryServiceWatcher watcher, ILogger<Worker> logger)
    {
        _watcher = watcher;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _watcher.ServiceAdded += async (BatteryServiceWatcher sender, BatteryServiceInformation info) =>
        {
            await foreach (var characteristic in ServiceHelper.GetBatteryLevelCharacteristicsAsync(info.Service))
            {
                if (await ServiceHelper.WriteNotify(characteristic))
                {
                    _logger.LogInformation($"Subscribed to {info.Device.Name}");
                }
                else
                {
                    _logger.LogWarning($"Failed to subscribe to {info.Device.Name}");
                }
            }
        };

        _watcher.Start();
        await stoppingToken;
        _watcher.Stop();
    }
}
