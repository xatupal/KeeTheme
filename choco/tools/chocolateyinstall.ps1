$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

function Install-Plugin {
	$pluginPath = Get-KeePassPluginsPath
	$pluginFile = Join-Path $toolsDir $fileName
	
	Write-Verbose "Downloading into tools dir."
	Get-ChocolateyWebFile -PackageName "$packageName" -FileFullPath "$pluginFile" `
		-Url "$url" `
		-Checksum "$checksum" `
		-ChecksumType "$checksumType"
		
	# rename PLGX file so it is clear which plugins are managed via choco
	$pluginChocoFile = Join-Path $pluginPath "$($packageName).plgx"
	Move-Item -Path $pluginFile -Destination $pluginChocoFile -Force
	Write-Verbose "Moving to plugins directory."

	if ( Get-Process -Name "KeePass" -ErrorAction SilentlyContinue ) {
		Write-Warning "$($packageSearch) is currently running. Plugin will be available at next restart of $($packageSearch)." 
	} else {
		Write-Host "$($packageName) will be loaded the next time KeePass is started."
	}
}

. "${toolsDir}\common.ps1"
Install-Plugin
