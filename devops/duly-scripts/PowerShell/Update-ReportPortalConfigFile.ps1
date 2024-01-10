param (
    [parameter(Mandatory=$true)]
    [ValidateScript({
        if (-not (Test-Path -PathType Leaf $_) ) {
            Throw "The file $_ doesn't exist!"
        }
        return $true
    })]
    [string] $filePath,

    [parameter(Mandatory=$true)]
    [string] $reportPortalProject,

    [parameter(Mandatory=$true)]
    [string] $accessToken,

    [parameter()]
    [string] $reportPortalServerUrl = "http://reportportal.dulydevops.privatelink.northcentralus.azmk8s.io",

    [parameter()]
    [string] $description,

    [parameter()]
    [string[]] $attributes
)

# Get content of the config file
$jsonConfig = Get-Content $filePath | ConvertFrom-Json

# Update reportPortalProject string because Reportportal doesn't allow project names with dots.
if ($reportPortalProject.Contains('.')) {
    $reportPortalProject = $reportPortalProject.Replace('.','-')
}
# Show parameters (except access token)
$jsonConfig | Format-List
Write-Output "Reportportal Server URL: $reportPortalServerUrl"
Write-Output "Reportportal project: $reportPortalProject"
if ($description) {
    Write-Output "Description: $description"
}
if ($attributes) {
    Write-Output "Attributes: $attributes"
}

# Update parameters
$jsonConfig.enabled = $true
$jsonConfig.server.url = "$reportPortalServerUrl"
$jsonConfig.server.project = "$reportPortalProject"
$jsonConfig.server.authentication.uuid = "$accessToken"
if ($description) {
    $jsonConfig.launch.description = $description 
}
if ($attributes) {
    if ($jsonConfig.launch.attributes) {
        $jsonConfig.launch.attributes = $attributes
    } else {
        $jsonConfig.launch | Add-Member -MemberType NoteProperty -Name "attributes" -Value $attributes
    }
}

# Save file
try {
    $jsonConfig | ConvertTo-Json | Set-Content $filePath -ErrorAction Stop
} catch {
    Write-Warning "Failed to update config file $($filePath): $($_.Exception.Message)"
}
