#Collin Milligan
#Date Created 9/11/2025
#Date Modified 4/16/2026

#Services Disabled
if (-not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Host "This script must be run as Administrator"
    Pause
    exit
}

$Disable = @(
    "sshd",
    "seclogon",
    "Fax",
    "DPS",
    "BthServ",
    "RemoteRegistry",
    "WerSvc",
    "TabletInputService",
    "FDResPub",
    "WaaSMedicSvc"
)

#Stopping each service in the above list
foreach ($service in $Disable) {
    if (Get-Service -Name $service -ErrorAction SilentlyContinue) {
        Stop-Service -Name $service -Force -ErrorAction SilentlyContinue
        Set-Service -Name $service -StartupType Disabled -ErrorAction SilentlyContinue
    }
}

#Disable ping services for IPv4 and IPv6
if (-not (Get-NetFirewallRule -DisplayName "Block ICMPv4" -ErrorAction SilentlyContinue)) {
    New-NetFirewallRule -DisplayName "Block ICMPv4" -Protocol ICMPv4 -IcmpType 8 -Direction Inbound -Action Block
    Write-Host "Ping Disabled for IPV4"
}

if (-not (Get-NetFirewallRule -DisplayName "Block ICMPv6" -ErrorAction SilentlyContinue)) {
    New-NetFirewallRule -DisplayName "Block ICMPv6" -Protocol ICMPv6 -IcmpType 128 -Direction Inbound -Action Block
    Write-Host "Ping Disabled for IPV6"
}

Write-Host "Baseline Complete. Closing in 3 seconds..."
Start-Sleep -Seconds 3
exit