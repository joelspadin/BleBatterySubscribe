#Requires -RunAsAdministrator

# Workaround for https://github.com/PowerShell/PowerShell/issues/13138
if ($PSVersionTable.PSVersion.Major -ge 7) {
    Import-Module Appx -UseWindowsPowerShell
}

Get-AppxPackage -Name "JoelSpadin.BleBatterySubscribe" | Select-Object -ExpandProperty "PackageFullName" | Remove-AppxPackage