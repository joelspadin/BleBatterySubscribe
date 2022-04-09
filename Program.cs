namespace BleBatterySubscribe;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseWindowsService(options =>
            {
                options.ServiceName = "BLE Battery Service Subscriber";
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<BatteryServiceWatcher>();
                services.AddHostedService<WindowsBackgroundService>();
            });
}

