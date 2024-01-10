[CmdletBinding()]
param(
    [parameter(Mandatory=$true)]
    [String] $apimRgName,
    [parameter(Mandatory=$true)]
    [String] $apimName,
    [parameter(Mandatory=$true)]
    [String]$saName,
    [parameter(Mandatory=$true)]
    [String]$saRgName,
    [parameter(Mandatory=$true)]
    [String]$containerName,
    [parameter(Mandatory=$true)]
    [String]$backupName
)

Write-Verbose "Starting APIM configuration restore from backup" -Verbose

Write-Verbose "Retrieve storage access key" -Verbose
$storageKey = (Get-AzStorageAccountKey -ResourceGroupName $saRgName -StorageAccountName $saName)[1].Value

Write-Verbose "Establishing storage context" -Verbose
$storageContext = New-AzStorageContext -StorageAccountName $saName -StorageAccountKey $storageKey

Write-Verbose "Performing APIM configuration restore" -Verbose
Restore-AzApiManagement -ResourceGroupName $apimRgName -Name $apimName -StorageContext $storageContext -SourceContainerName $containerName -SourceBlobName "$backupName"

Write-Verbose "APIM configuration restore from backup has been finished" -Verbose