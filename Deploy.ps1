#Requires -RunAsAdministrator

param(
    [switch]$debug
)

# Build package
$configuration = if ($debug) { "Debug" } else { "Release" }
dotnet publish -c $configuration

(Get-Content "$PSScriptRoot/installer/appxmanifest.xml") `
    -replace "BleBatterySubscribe.exe", "$configuration/net7.0-windows10.0.19041.0/win-x64/publish/BleBatterySubscribe.exe" `
| Out-File -Encoding utf8 "bin/appxmanifest.xml"

# Stop the service if it's already running
$service = Get-Service "BLE Battery Service Subscriber" -ErrorAction SilentlyContinue

if ($service -and ($service.Status -eq "Running")) {
    $service.Stop()
}

# Workaround for https://github.com/PowerShell/PowerShell/issues/13138
if ($PSVersionTable.PSVersion.Major -ge 7) {
    Import-Module Appx -UseWindowsPowerShell
}

Get-AppxPackage -Name "*JoelSpadin.BleBatterySubscribe*" | foreach-object {
    Remove-AppxPackage $_.PackageFullName
}

Add-AppxPackage -Register "$PSScriptRoot/bin/appxmanifest.xml"
