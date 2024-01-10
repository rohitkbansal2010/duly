$needsUpdate = $false

# Get list of MS Edge latest versions
$products = Invoke-RestMethod -ContentType "application/json" -Method Get -Uri "https://edgeupdates.microsoft.com/api/products"
$stableReleases = ($products | Where-Object {$_.Product -eq "Stable"}).Releases
$targetRelease = $stableReleases | Where-Object {$_.Platform -eq "Windows" -and $_.Architecture -eq "x64"}

# Check which version is installed already
$currentVersion = (Get-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{56EB18F8-B008-4CBD-B6D2-8C97FE7E9062}").pv
if ($currentVersion) {
    Write-Output "The current version is $currentVersion"
       if ($currentVersion -eq $targetRelease.ProductVersion) {
          Write-Output "The MS Edge is up-to-date"
       } else {
          Write-Output "Starting to update MS Edge, latest version is $($targetRelease.ProductVersion)"
          $needsUpdate = $true
       }
} else {
    Write-Output "Unable to determine the current version, starting to install the version $($targetRelease.ProductVersion)"
    $needsUpdate = $true
}

# Update MS Edge to latest version if needed
if ($needsUpdate) {
   mkdir -Path $env:temp\edgeinstall -erroraction SilentlyContinue | Out-Null
   $edgeMsi = join-path $env:temp\edgeinstall MicrosoftEdgeEnterpriseX64.msi
   Invoke-RestMethod -Method Get -Uri "$($targetRelease.Artifacts.Location)" -OutFile $edgeMsi
   $logFile = join-path $env:temp\edgeinstall install.log
   $installArguments = @("/i","$edgeMsi","/quiet","/norestart","/l*v","$logFile")
   $process = Start-Process "msiexec.exe" -Wait -ArgumentList $installArguments -PassThru -Verb RunAs
   if ($process.ExitCode -ne 0) {
       Write-Warning "Failed to install MS Edge!"
          if (Test-Path -PathType Leaf $logFile) {
              Get-Content $logFile
          } else {
              Write-Output "The log file $logFile was not found!"
          }
   }
   Write-Output "The current version now is:"
   (Get-ItemProperty -Path "HKLM:\SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{56EB18F8-B008-4CBD-B6D2-8C97FE7E9062}").pv
}