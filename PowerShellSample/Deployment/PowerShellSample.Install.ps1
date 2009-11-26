# PowerShellSample.Install.ps1
# 05.06.2009
# 
# Install script demonstrating deployment functionality 
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

Log([String]::Format('*** Installation started: {0}', [DateTime]::Now))

# 1. Add the Event Log Application source for the app to log to
#    Uses the PowerShell registry provider to write new items

Log('*** Creating Event Log registry key')
New-Item -Path 'HKLM:\SYSTEM\CurrentControlSet\Services\Eventlog\Application\PowerShellSample' -Force
New-ItemProperty -Path 'HKLM:\SYSTEM\CurrentControlSet\Services\Eventlog\Application\PowerShellSample' -Name 'EventMessageFile' -PropertyType ExpandString -Value ‘C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\EventLogMessages.dll’ –Force
LogComplete

# 2. Create the expected receive location
#    Uses the PowerShell filesystem provider to write new items

Log('*** Creating receive location directory')
New-Item –Path 'c:\receiveLocations\x\y\z’ -ItemType Directory –Force
LogComplete

# 3. Add app settings to Enterprise SSO
#    Uses .NET classes from dependent assembly deployed to the GAC

Log('*** Importing settings to SSO application config store')
[Reflection.Assembly]::Load('SSOConfig, Version=1.1.0.0, Culture=neutral, PublicKeyToken=656a499478affdaf')
$configPath = [IO.Path]::Combine($env:BTAD_InstallDir, 'Deployment\PowerShellSample.ssoconfig')
$app = [SSOConfig.SSOApplication]::LoadFromXml($configPath)
$app.SaveToSSO()
LogComplete

# 4. Add log4net config elements to BTS service config file
#    Uses native PowerShell XML functionality & .NET base classes to manipulate XML

Log('*** Adding log4net settings to BTSNTSvc.exe.config')
$installPath = Get-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\BizTalk Server\3.0' -Name 'InstallPath'
$btsConfigPath = [IO.Path]::Combine($installPath.InstallPath, 'BTSNTSvc.exe.config')
$xml = [xml] (Get-Content $btsConfigPath)

$configSections = $xml.SelectSingleNode('configuration/configSections')
if ($configSections -eq $null) 
{
	$configSections = $xml.CreateElement('configSections')
	$firstChild = $xml.configuration.get_FirstChild()
	$xml.configuration.InsertBefore($configSections, $firstChild)
}

$log4netSection = $configSections.SelectSingleNode('section[@name="log4net"]')
if ($log4netSection -eq $null)
{
	$log4netSection = $xml.CreateElement('section')
	$log4netSection.SetAttribute('name', 'log4net')
	$log4netSection.SetAttribute('type', 'log4net.Config.Log4NetConfigurationSectionHandler, log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821')	
	$configSections.AppendChild($log4netSection )
}

$log4net = $xml.SelectSingleNode('configuration/log4net')
if ($log4net -eq $null) 
{
	$log4net = $xml.CreateElement('log4net')
	$xml.configuration.AppendChild($log4net)
}

$appender = $log4net.SelectSingleNode('appender[@name="Sixeyed.CacheAdapter.EventLogAppender"]')
if ($appender -eq $null)
{
	$appender = $xml.CreateElement('appender')
	$appender.SetAttribute('name', 'Sixeyed.CacheAdapter.EventLogAppender')
	$appender.SetAttribute('type', 'log4net.Appender.EventLogAppender, log4net, Version=1.2.10.0, Culture=neutral, PublicKeyToken=1b44e1d426115821')
	$appender.set_InnerXml('<param name="LogName" value="Application" /><param name="ApplicationName" value="Sixeyed.CacheAdapter" /><layout type="log4net.Layout.PatternLayout"><conversionPattern value="%date [%thread] %logger %level - %message%newline" /></layout>')
	$log4net.AppendChild($appender)
}

$logger = $log4net.SelectSingleNode('logger[@name="Sixeyed.CacheAdapter.Log"]')
if ($logger -eq $null)
{
	$logger = $xml.CreateElement('logger')
	$logger.SetAttribute('name', 'Sixeyed.CacheAdapter.Log')
	$logger.set_InnerXml('<level value="WARN" /><appender-ref ref="Sixeyed.CacheAdapter.EventLogAppender" />')
	$log4net.AppendChild($logger)
}

Log('*** Saving config changes')
$xml.Save($btsConfigPath)
LogComplete

Log([String]::Format('*** Installation completed: {0}', [DateTime]::Now))


