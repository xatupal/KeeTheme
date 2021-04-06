param (
	[string]$apiKey = ""
)

If ($apiKey -eq "")
{
	Write-Host "Choco API key was not provided. Trying to read from choco.key file."
	$apiKey = Get-Content choco.key
}

If ($apiKey -eq "")
{
	Write-Host "Choco API key not found!"
	Break
}

Write-Host "Getting latest KeeTheme version..."
$AllProtocols = [System.Net.SecurityProtocolType]'Ssl3,Tls,Tls11,Tls12'
[System.Net.ServicePointManager]::SecurityProtocol = $AllProtocols

$latestUrl = "https://github.com/xatupal/KeeTheme/releases/latest"
$request = [System.Net.WebRequest]::Create($latestUrl)
$request.AllowAutoRedirect=$false
$response=$request.GetResponse()

If ($response.StatusCode -ne "Found")
{
	Write-Host "Unable to find latest version!"
	Break
}

$latestTagUrl = $response.GetResponseHeader("Location")
$latestTag = $latestTagUrl -Split "/" | Select-Object -Last 1
$version = $latestTag.TrimStart("v")
Write-Host "Latest version: " $version

Write-Host "Downloading KeeTheme.plgx..."
$downloadUrl = "https://github.com/xatupal/KeeTheme/releases/download/$latestTag/KeeTheme.plgx"
Invoke-WebRequest -Uri $downloadUrl -OutFile "KeeTheme.plgx"
Write-Host "KeeTheme plugin downloaded"

Write-Host "Updating Choco files..."
$content = Get-Content -path "keetheme.nuspec"
$content -Replace '\<version\>(.+)\</version\>', "<version>$version</version>" |  Out-File -Encoding "UTF8" "keetheme.nuspec"

$content = Get-Content -path "tools\common.ps1"
$content -Replace '\$url = ''(.+)''', "`$url = '$downloadUrl'" |  Out-File -Encoding "UTF8" "tools\common.ps1"

$checksum = (Get-FileHash -algorithm sha256 "KeeTheme.plgx").Hash
$content = Get-Content -path "tools\common.ps1"
$content -Replace '\$checksum = ''(.+)''', "`$checksum = '$checksum'" |  Out-File -Encoding "UTF8" "tools\common.ps1"
Write-Host "Choco files updated"

Write-Host "Creating Choco package..."
choco pack
Write-Host "Choco package keepass-plugin-keetheme.$version.nupkg created!"

$reply = Read-Host -Prompt "Ready to push?[y/n]"
If ($reply -match "[yY]") 
{
	Write-Host "Pushing Choco package..."
	choco push keepass-plugin-keetheme.$version.nupkg -s https://chocolatey.org --api-key=$apiKey
	Write-Host "Choco package pushed!"
}
pause
