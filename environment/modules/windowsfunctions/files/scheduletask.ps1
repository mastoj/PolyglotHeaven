Param(
	[string]$name,
	[string]$action,
	[string]$arguments, 
	[int]$minutes, 
	[int]$startDelay
)
Write-Host "Going to schedule $name with interval $minutes"
Write-Host "Action is: $action"
Write-Host "It is starting in approx. $startDelay"

try {
	$startTime = $(Get-Date).AddMinutes($startDelay);
	$interval = (New-TimeSpan $(Get-Date) $(Get-Date).AddMinutes($minutes));
	$trigger = New-ScheduledTaskTrigger -Once -At $startTime -RepetitionInterval $interval -RepetitionDuration $([TimeSpan]::MaxValue);
	if([string]::IsNullOrWhiteSpace($arguments)) {
		$scheduleAction = New-ScheduledTaskAction -Execute "$action";
	}
	else {
		$scheduleAction = New-ScheduledTaskAction -Execute "$action" -Argument "$arguments";
	}
	$task = New-ScheduledTask -Action $scheduleAction -Trigger $trigger;
	Register-ScheduledTask $name -InputObject $task;
}
catch {
	$message = $_.Exception.Message
	Write-Error $message
	Exit 1
}