# Window BLE Battery Service Subscriber

This project implements a Windows service which fixes Windows to correctly update the battery level for some Bluetooth devices. If the battery levels in **Settings > Devices > Bluetooth & other devices** don't seem to update properly, installing this service may help.

For some reason, Windows doesn't ask BLE devices to notify it when their battery levels change. This service fixes that by watching for connected BLE devices, checking if they have a GATT battery service, and if so registering for notifications for the battery level characteristic.

## Installation

This service requires the [.NET Desktop Runtime 5.0](https://dotnet.microsoft.com/en-us/download/dotnet/5.0). Find the ".NET Desktop Runtime" section on that page and click the "x64" link in the table, then run the installer it downloads.

Download the the .msix file from the [latest release](https://github.com/joelspadin/BleBatterySubscribe/releases) and double click it to install the service.

To verify that the service is running, open "Services" from the start menu or press **Windows Key + R**, type `services.msc`, and press OK. Check that there is a row for "BLE Battery Service Subscriber" and it shows "Running" for its status.

## Development

### Requirements

* [.NET SDK 5.0 or newer](https://dotnet.microsoft.com/en-us/download/dotnet)
* [.NET Desktop Runtime 5.0](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
* [PowerShell](https://aka.ms/powershell-release?tag=stable)

### Testing

To run the program as a console application, open the project folder in a terminal and run
```ps
dotnet run
```

To run the program in Visual Studio Code's debugger, use the ".NET Core Launch (console)" launch configuration.

To install the Windows service in development mode, first open **Settings > Update & Security > For developers** and enable the **Developer Mode** switch. Then, open the project folder in a PowerShell window as administrator and run

```ps
# Builds and deploys a release build
./Deploy
```
or
```ps
# Builds and deploys a debug build
./Deploy -debug
```

### Packaging

To install the package, it must be signed. To make a self-sign certificate for testing, open a PowerShell window as administrator and run

```ps
cd installer
./MakeSelfSignCert
```

For more details, see [create a certificate for package signing](https://docs.microsoft.com/en-us/windows/msix/package/create-certificate-package-signing).

To build and sign the installer, open a PowerShell window and run

```ps
cd installer
./MakeMsix
./SignMsix
```

The SignMsix script assumes you are using the self-sign certificate generated by MakeSelfSignCert. To use a different certificate, see [sign an app package using SignTool](https://docs.microsoft.com/en-us/windows/msix/package/sign-app-package-using-signtool).

To install the package, simply run the generated `installer/bin/BleBatterySubscribe-x64.msix` file.

## Credits

Installer scripts are based in part on [microsoft/PowerToys](https://github.com/microsoft/PowerToys) ([license](https://github.com/microsoft/PowerToys/blob/main/LICENSE)).
