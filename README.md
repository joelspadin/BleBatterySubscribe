# Window BLE Battery Service Subscriber

This project implements a Windows service which fixes Windows to correctly update the battery level for some Bluetooth devices. If the battery levels in **Settings > Devices > Bluetooth & other devices** don't seem to update properly, installing this service may help.

For some reason, Windows doesn't ask BLE devices to notify it when their battery levels change. This service fixes that by watching for connected BLE devices, checking if they have a GATT battery service, and if so registering for notifications for the battery level characteristic.

## Requirements

* [.NET SDK 7.0 or newer](https://dotnet.microsoft.com/en-us/download/dotnet)
* [PowerShell](https://aka.ms/powershell-release?tag=stable)

## Build and Install

To run the program as a console application, open the project folder in a terminal and run
```ps
dotnet run
```

To run the program in Visual Studio Code's debugger, use the ".NET Core Launch (console)" launch configuration.

To install the Windows service, first open **Settings > Update & Security > For developers** and enable the **Developer Mode** switch. Then, open the project folder in a PowerShell window as administrator and run

```ps
# Builds and deploys a release build
./Deploy
```
or
```ps
# Builds and deploys a debug build
./Deploy -debug
```

To verify that the service is running, open "Services" from the start menu or press **Windows Key + R**, type `services.msc`, and press OK. Check that there is a row for "BLE Battery Service Subscriber" and it shows "Running" for its status.

## Credits

Installer scripts are based in part on [microsoft/PowerToys](https://github.com/microsoft/PowerToys) ([license](https://github.com/microsoft/PowerToys/blob/main/LICENSE)).
