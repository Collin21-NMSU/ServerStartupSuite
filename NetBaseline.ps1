# Zach Lalonde
# Date Modified: 4/14/2026
# System Summary Script

#Admin check For Admin Perm
if (-not ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Host "Please run as Administrator" -ForegroundColor Red
    Pause
    exit
}

$now = Get-Date
$cs = Get-CimInstance -ClassName Win32_ComputerSystem
$os = Get-CimInstance -ClassName Win32_OperatingSystem

$uptime = $now - $os.LastBootUpTime
$totalRamBytes = $cs.TotalPhysicalMemory
$totalRamGiB = [math]::Round($totalRamBytes / 1GB, 2)

$domainLabel = if ($cs.PartOfDomain) { 'Domain' } else { 'Workgroup' }
$domainValue = $cs.Domain

Write-Host '--- Server Summary ---' -ForegroundColor Cyan
Write-Host "Computer name : $($cs.Name)"
Write-Host "$($domainLabel.PadRight(14)): $domainValue"
Write-Host "Current time  : $($now.ToString('yyyy-MM-dd HH:mm:ss'))"
Write-Host "Uptime        : $($uptime.Days)d $($uptime.Hours)h $($uptime.Minutes)m $($uptime.Seconds)s"
Write-Host "Since         : $($os.LastBootUpTime.ToString('yyyy-MM-dd HH:mm:ss'))"
Write-Host "Total RAM     : $totalRamGiB GiB"
Write-Host 'IPv4 addresses:' -ForegroundColor Cyan

# Clean Network Output
Get-NetIPAddress -AddressFamily IPv4 -ErrorAction SilentlyContinue |
    Where-Object { $_.IPAddress -notmatch '^(127\.|169\.254\.)' } |
    Sort-Object InterfaceIndex, IPAddress |
    ForEach-Object {
        Write-Host ("  {0,-24} {1}" -f $_.InterfaceAlias, $_.IPAddress)
    }

#Keep Window Open
Write-Host "`nPress any key to return to the Dashboard..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")