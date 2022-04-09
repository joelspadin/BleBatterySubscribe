namespace BleBatterySubscribe;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class WindowsBackgroundService : BackgroundService
{
    private readonly BatteryServiceWatcher _watcher;
    private readonly ILogger<WindowsBackgroundService> _logger;

    public WindowsBackgroundService(BatteryServiceWatcher watcher, ILogger<WindowsBackgroundService> logger)
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
                    _logger.LogInformation($"Subscribed to battery service notifications for {info.Device.Name}");
                }
                else
                {
                    _logger.LogWarning($"Failed to subscribe to battery service notifications for {info.Device.Name}");
                }
            }
        };

        _watcher.Start();
        await stoppingToken;
        _watcher.Stop();
    }
}
