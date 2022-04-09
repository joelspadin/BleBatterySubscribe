
namespace BleBatterySubscribe;

using System;
using Microsoft.Extensions.Logging;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;

public class BatteryServiceInformation
{
    public BatteryServiceInformation(BluetoothLEDevice device, GattDeviceService service)
    {
        Device = device;
        Service = service;
    }

    public BluetoothLEDevice Device { get; }
    public GattDeviceService Service { get; }
}

public class BatteryServiceWatcher
{


    private DeviceWatcher _watcher;
    private ILogger<WindowsBackgroundService> _logger;

    public event TypedEventHandler<BatteryServiceWatcher, BatteryServiceInformation>? ServiceAdded;

    public BatteryServiceWatcher(ILogger<WindowsBackgroundService> logger)
    {
        _logger = logger;

        var selector = BluetoothLEDevice.GetDeviceSelectorFromConnectionStatus(BluetoothConnectionStatus.Connected);
        _watcher = DeviceInformation.CreateWatcher(selector);

        _watcher.Added += DeviceAdded;

        // https://docs.microsoft.com/en-us/uwp/api/windows.devices.enumeration.devicewatcher.added
        // An app must subscribe to all of the added, removed, and updated events
        // to be notified when there are device additions, removals or updates.
        // If an app handles only the added event, it will not receive an update
        // if a device is added to the system after the initial device enumeration
        // 0completes.
        _watcher.Updated += DeviceUpdated;
        _watcher.Removed += DeviceRemoved;
    }

    public void Start() => _watcher.Start();
    public void Stop() => _watcher.Stop();

    private async void DeviceAdded(DeviceWatcher sender, DeviceInformation info)
    {
        var device = await BluetoothLEDevice.FromIdAsync(info.Id);
        if (device == null)
        {
            _logger.LogWarning($"Failed to get BLE device {info.Name} ({info.Id})");
            return;
        }

        var result = await device.GetGattServicesForUuidAsync(GattServiceUuids.Battery);
        if (result == null)
        {
            _logger.LogDebug($"No battery service on BLE device {info.Name}");
            return;
        }

        _logger.LogDebug($"New BLE device {info.Name}");

        foreach (var service in result.Services)
        {
            OnServiceAdded(new BatteryServiceInformation(device, service));
        }
    }

    private void DeviceUpdated(DeviceWatcher sender, DeviceInformationUpdate update) { }
    private void DeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate update) { }

    protected virtual void OnServiceAdded(BatteryServiceInformation e)
    {
        ServiceAdded?.Invoke(this, e);
    }
}
