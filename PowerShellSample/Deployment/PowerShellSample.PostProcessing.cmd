

cd "%BTAD_InstallDir%\Deployment"
if "%BTAD_InstallMode%" == "Install" ( powershell ".\PowerShellSample.Install.ps1" >> PowerShellSample.Install.ps1.log)
if "%BTAD_InstallMode%" == "Uninstall" ( powershell ".\PowerShellSample.Uninstall.ps1" >> PowerShellSample.Uninstall.ps1.log)

