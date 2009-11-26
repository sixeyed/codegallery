# PowerShellSample.Install.ps1
# 05.06.2009
# 
# Uninstall script demonstrating deployment cleanup functionality 
# for BizTalk post-processing scripts

# Script logging functions

function Log([string] $message)
{
	Write-Output $message
}

function LogComplete()
{
	Log('*** Complete.')
}

#

Log([String]::Format('*** Uninstallation started: {0}', [DateTime]::Now))

# 1. Remove the Event Log Application source 
#    Uses the PowerShell registry provider to remove

Log('*** Removing Event Log registry key')
Remove-Item -Path 'HKLM:\SYSTEM\CurrentControlSet\Services\Eventlog\Application\PowerShellSample' -Recurse -Force
LogComplete

# 2. Delete the expected receive location
#    Uses the PowerShell filesystem provider to remove

Log('*** Removing receive location directory')
Remove-Item –Path 'c:\receiveLocations’ -Recurse –Force
LogComplete

# 3. Delete app settings from Enterprise SSO
#    Uses .NET classes from dependent assembly deployed to the GAC

Log('*** Removing settings from SSO application config store')
[Reflection.Assembly]::Load('SSOConfig, Version=1.1.0.0, Culture=neutral, PublicKeyToken=656a499478affdaf')
$app = [SSOConfig.SSOApplication]::LoadFromSSO('PowerShellSample')
if ($app -ne $null)
{
	$app.DeleteFromSSO()
}
LogComplete

# 4. Remove log4net config elements from BTS service config file
#    Note that we don't record which elements we created, so remove the specific
#    elements and then remove the generic ones if empty
#    Uses native PowerShell XML functionality & .NET base classes to manipulate XML

Log('*** Removing log4net settings from BTSNTSvc.exe.config')
$installPath = Get-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\BizTalk Server\3.0' -Name 'InstallPath'
$btsConfigPath = [IO.Path]::Combine($installPath.InstallPath, 'BTSNTSvc.exe.config')
$xml = [xml] (Get-Content $btsConfigPath)

$log4net = $xml.SelectSingleNode('configuration/log4net')
if ($log4net -ne $null) 
{
	$appender = $log4net.SelectSingleNode('appender[@name="Sixeyed.CacheAdapter.EventLogAppender"]')
	if ($appender -ne $null)
	{	
		$log4net.RemoveChild($appender)
	}

	$logger = $log4net.SelectSingleNode('logger[@name="Sixeyed.CacheAdapter.Log"]')
	if ($logger -ne $null)
	{
		$log4net.RemoveChild($logger)
	}

	if ($log4net.get_HasChildNodes() -eq $false)
	{
		$xml.configuration.RemoveChild($log4net)

	}
}

$log4net = $xml.SelectSingleNode('configuration/log4net')
if ($log4net -eq $null) 
{
	#if there's no log4net section, remove it from configSections:
	$configSections = $xml.SelectSingleNode('configuration/configSections')
	if ($configSections -ne $null) 
	{
		$log4netSection = $configSections.SelectSingleNode('section[@name="log4net"]')
		if ($log4netSection -ne $null)
		{
			$configSections.RemoveChild($log4netSection)
		}

		if ($configSections.get_HasChildNodes() -eq $false)
		{
			$xml.configuration.RemoveChild($configSections)

		}
	}		
}


Log('*** Saving config changes')
$xml.Save($btsConfigPath)
LogComplete

Log([String]::Format('*** Installation completed: {0}', [DateTime]::Now))