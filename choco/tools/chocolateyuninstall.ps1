function Uninstall-Plugin {
	Write-Verbose "Checking KeePass is not running..."
	if (Get-Process -Name "KeePass" -ErrorAction SilentlyContinue) {
		Write-Warning "$($packageSearch) is running. Please save any opened databases and close $($packageSearch) before attempting to uninstall KeePass plugins."
		exit 1
	}

	$pluginPath = Get-KeePassPluginsPath
	$pluginFile = Join-Path $pluginPath $fileName
	$pluginChocoFile = Join-Path $pluginPath "$($packageName).plgx"
	if (Test-Path $pluginFile) {
		Write-Warning "$(fileName) was found but it is not managed by Chocolatey."
		Write-Warning "If you intend to remove it anyway, please do it manually."
	}
	if (Test-Path $pluginChocoFile) {
		Remove-Item -Path $pluginChocoFile -Force -ErrorAction Continue
	} else {
		Write-Warning "The plugin has not been found!"
	}
}

$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
. "${toolsDir}\common.ps1"
Uninstall-Plugin
