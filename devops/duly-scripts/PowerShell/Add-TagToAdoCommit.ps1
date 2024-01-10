param (
    [parameter(Mandatory=$true)]
    [string] $PAT,

    [parameter(Mandatory=$true)]
    [string] $organizationUri,

    [parameter(Mandatory=$true)]
    [string] $projectName,

    [parameter(Mandatory=$true)]
    [string] $repositoryID,

    [parameter(Mandatory=$true)]
    [string] $commitID,

    [parameter(Mandatory=$true)]
    [string] $tagName,

    [parameter(Mandatory=$true)]
    [string] $tagMessage
)

$cred = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes((":$PAT")))
$header = @{Authorization = "Basic $cred"}

$jsonBody = @{
    name = $tagName
    message = $tagMessage
    taggedObject = @{
        objectId = $commitID
    }
} | ConvertTo-Json

$uri = "$($organizationUri)$($projectName)/_apis/git/repositories/$repositoryID/annotatedtags?api-version=6.0-preview.1"

# Write log for debug
Write-Output "Uri: $uri"
Write-Output "Request: $jsonBody"

# Add tag to commit
try {
    Invoke-RestMethod -Method POST -ContentType "application/json" -Headers $header -Uri $uri -Body $jsonBody -ErrorAction Stop
} catch {
    Write-Warning "Failed to add tag $tagName to the commit $($commitID): $($_.Exception.Message)"
}
