param (
    [switch]$debug
)

dotnet publish "../BleBatterySubscribe.csproj" -c $(if ($debug) { "Debug" } else { "Release" })

# If making a debug build, rewrite the package layout
$PackagingLayoutFile = "PackagingLayout.xml"

if ($debug) {
    (Get-Content $PackagingLayoutFile) -replace "bin/Release/", "bin/Debug/" | Out-File -Encoding utf8 "$env:temp\$PackagingLayoutFile"
    $PackagingLayoutFile = "$env:temp\$PackagingLayoutFile"
}

# Find makeappx.exe if it isn't already on the path
if ($null -eq (Get-Command "makeappx.exe" -ErrorAction SilentlyContinue)) {
    $sdkFiles = Get-ChildItem -Path "C:/Program Files (x86)/Windows Kits/10/bin/*/x64/makeappx.exe" | Sort-Object
    $path = $sdkFiles[-1].DirectoryName

    if ($null -eq $path) {
        $kitFile = Get-Item "C:/Program Files (x86)/Windows Kits/10/App Certification Kit/makeappx.exe" -ErrorAction SilentlyContinue
        if ($kitFile) {
            $path = $kitFile.DirectoryName
        }
        else {
            Write-Error "Cannot find makeappx.exe" -ErrorAction Stop
        }
    }

    $env:PATH = "$env:PATH;$path"
}

makeappx build /overwrite /f $PackagingLayoutFile /op bin\