# The script installs Google Chrome browser on Windows machines for automated UI tests
Write-Output "Starting to install latest version of Google Chrome browser..."
mkdir -Path $env:temp\chromeinstall -ErrorAction SilentlyContinue | Out-Null
$logFile = "C:\Windows\Temp\chrome_installer.log"
$installer = Join-Path $env:temp\chromeinstall "chrome_installer.exe"
Invoke-RestMethod -Method Get -Uri "https://dl.google.com/chrome/install/latest/chrome_installer.exe" -OutFile $installer
$process = Start-Process -FilePath $installer -Wait -ArgumentList "/silent /install" -PassThru -Verb RunAs
if ($process.ExitCode -ne 0) {
    Write-Warning "Failed to install Google Chrome!"
    if (Test-Path -PathType Leaf $logFile) {
        Get-Content $logFile
    } else {
        Write-Output "The log file $logFile was not found!"
    }
} else {    
    Write-Output "Google Chrome has been installed successfully, current version:"
    (Get-Item (Get-ItemProperty 'HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe').'(Default)').VersionInfo.ProductVersion
}