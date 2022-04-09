#Requires -RunAsAdministrator

# Workaround for https://github.com/PowerShell/PowerShell/issues/13138
if ($PSVersionTable.PSVersion.Major -ge 7) {
    Import-Module Appx -UseWindowsPowerShell
}

Add-AppxPackage ./bin/BleBatterySubscribe.msixbundle