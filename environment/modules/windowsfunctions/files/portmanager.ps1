Param(
  [string]$name,
  [int]$port,
  [bool]$enableTcp
)

Write-Host "Enabling port: $name, $port, $enableTcp"

$NET_FW_IP_PROTOCOL_UDP = 17 
$NET_FW_IP_PROTOCOL_TCP = 6 
$NET_FW_PROFILE_PUBLIC = 2

$fwMgr = New-Object -ComObject HNetCfg.FwMgr 

$profile = $fwMgr.LocalPolicy.GetProfileByType($NET_FW_PROFILE_PUBLIC)

if ($profile.FirewallEnabled -eq $True) { 
	Write-Host "Windows Firewall is Enabled " 
}
else { 
	Write-Host "Windows Firewall is Enabled " 
}

$firewallPort = New-Object -ComObject HNetCfg.FWOpenPort 
$firewallPort.Name = $name + "_tcp"

$firewallPort.Port = $port 
$firewallPort.RemoteAddresses = "*" 
$firewallPort.Enabled = $enableTcp 
$firewallPort.Protocol = $NET_FW_IP_PROTOCOL_TCP

$profile.GloballyOpenPorts.Add($firewallPort) 