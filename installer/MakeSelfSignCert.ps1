#Requires -RunAsAdministrator

$expirationDate = { Get-Date }.Invoke().AddYears(5)
$pass = ConvertTo-SecureString -String "12345" -Force -AsPlainText

$thumbprint = (New-SelfSignedCertificate -notafter $expirationDate -Type CodeSigningCert `
        -Subject "CN=Joel Spadin, C=US" -FriendlyName "BleBatterySubscribe Test Certificate" `
        -KeyDescription "BleBatterySubscribe Test Certificate" -KeyFriendlyName "BleBatterySubscribe Test Key" `
        -KeyUsage "DigitalSignature" -CertStoreLocation Cert:\LocalMachine\My).Thumbprint

Export-PfxCertificate -Cert cert:\LocalMachine\My\$thumbprint -FilePath BleBatterySubscribe_TemporaryKey.pfx -Password $pass
Import-PfxCertificate -CertStoreLocation Cert:\LocalMachine\Root -FilePath BleBatterySubscribe_TemporaryKey.pfx -Password $pass