using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BleBatterySubscribe;

public static class ServiceHelper
{
    private static readonly Guid BatteryLeveluuid = new Guid("00002A19-0000-1000-8000-00805F9B34FB");

    public static async IAsyncEnumerable<GattCharacteristic> GetBatteryLevelCharacteristicsAsync(GattDeviceService batteryService)
    {
        var result = await batteryService.GetCharacteristicsForUuidAsync(BatteryLeveluuid);
        if (result != null)
        {
            foreach (var characteristic in result.Characteristics)
            {
                yield return characteristic;
            }
        }
    }

    /// <summary>
    /// Writes to the GATT characteristic to be notified when it changes.
    /// </summary>
    public static async Task<bool> WriteNotify(GattCharacteristic characteristic)
    {
        if (!characteristic.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Notify))
        {
            return false;
        }

        var status = await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
            GattClientCharacteristicConfigurationDescriptorValue.Notify);

        return status == GattCommunicationStatus.Success;
    }
}