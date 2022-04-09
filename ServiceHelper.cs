using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace BleBatterySubscribe;

public static class ServiceHelper
{
    public static async IAsyncEnumerable<GattCharacteristic> GetBatteryLevelCharacteristicsAsync(GattDeviceService batteryService)
    {
        var result = await batteryService.GetCharacteristicsForUuidAsync(GattCharacteristicUuids.BatteryLevel);
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