[CmdletBinding()]
param(
    [parameter(Mandatory=$true)]
    [String] $apimRgName,
    [parameter(Mandatory=$true)]
    [String] $apimName,
    [parameter(Mandatory=$true)]
    [String]$saName,
    [parameter(Mandatory=$true)]
    [String] $saRgName,
    [parameter(Mandatory=$true)]
    [String]$containerName,
    [parameter(Mandatory=$false)]
    [String]$blobPrefix = "backup_",
    [parameter(Mandatory=$false)]
    [Int32]$RetentionDays = 30
)

Write-Verbose "Starting APIM configuration backup" -Verbose

Write-Verbose "Retrieve storage access key" -Verbose
$storageKey = (Get-AzStorageAccountKey -ResourceGroupName $saRgName -StorageAccountName $saName)[1].Value

Write-Verbose "Establishing storage context" -Verbose
$storageContext = New-AzStorageContext -StorageAccountName $saName -StorageAccountKey $storageKey

Write-Verbose "Get current date" -Verbose
$bkpDate = Get-Date -Format yyyyMMdd

Write-Verbose "Performing APIM configuration backup" -Verbose
Backup-AzApiManagement -ResourceGroupName $apimRgName -Name $apimName -StorageContext $storageContext -TargetContainerName $containerName -TargetBlobName "$blobPrefix$apimName$bkpDate"

Write-Output "Removing backups older than '$retentionDays' days from container: '$containerName'"
$isOldDate = [DateTime]::UtcNow.AddDays(-$retentionDays)
$blobs = Get-AzStorageBlob -Container $containerName -Context $storageContext | Where-Object { $_.LastModified.UtcDateTime -lt $isOldDate -and $_.BlobType -eq "BlockBlob" }

foreach ($blob in $blobs) {
    Write-Verbose ("Removing blob: " + $blob.Name) -Verbose
    Remove-AzStorageBlob -Blob $blob.Name -Container $containerName -Context $storageContext
}

Write-Verbose "APIM configuration backup finished" -Verbose