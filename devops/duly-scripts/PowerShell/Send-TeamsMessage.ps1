[CmdletBinding()]
param (
    [Parameter(Mandatory = $false)]
    [string] $messageTitle = "Build Failed"
)

Get-Item -Path Env:*
$utcDateTime = Get-Date -Format "MM/dd/yyyy HH:mm K"

# MS Teams team channel webhook connector URL
$msTeamsConnectorURL = 'https://epam.webhook.office.com/webhookb2/7bb1aa4c-b9b0-4d20-9a6e-e9fc8549149a@b41b72d0-4e9f-4c26-8a69-f949f367c91d/IncomingWebhook/c11c2f9ed773495c8198d4308cb1699d/218ac730-25cb-4504-9403-7a0be3563bf5'

$title = "$messageTitle $utcDateTime"

$buildNumber = $Env:BUILD_BUILDNUMBER
$buildId = $Env:BUILD_BUILDID
$repository = $Env:BUILD_REPOSITORY_NAME
$branch = $Env:BUILD_SOURCEBRANCH
$organizationURI = $Env:System_TeamFoundationCollectionUri
$projectName = $Env:SYSTEM_TEAMPROJECT
$stageName = $Env:SYSTEM_STAGENAME

$buildURL = "https://dev.azure.com/Next-Generation-Data-Platform/$projectName/_build/results?buildId=$buildId&view=results"

Write-Output "buildNumber: $buildNumber"
Write-Output "buildId : $buildId "
Write-Output "buildURL: $buildURL"
Write-Output "repository: $repository"
Write-Output "branch: $branch"
Write-Output "systemTeamProject: $projectName"

$messageBody = [ordered]@{
    "@type" = "MessageCard"
    "@context" = "http://schema.org/extensions"
    "themeColor" = "0076D7"
    "summary" = "Build Alert"
    "sections" = @(
        @{
        "activityTitle" = $title
        "activitySubtitle" = $projectName
        "facts" = @(
             @{
               "name" = "Build Name:"
               "value" = $buildNumber
              },
             @{
              "name" = "Repository:"
              "value" = $repository
             },
             @{
              "name" = "Branch:"
              "value" = $branch
              },
             @{
              "name" = "Stage Name:"
              "value" = $stageName
              })
         }
     )
     "potentialAction" = @(
        @{
          "@type" = "OpenUri"
          "name" = "Build Link"
          "targets" = @(
            @{ 
              "os" = "default"
              "uri" = $buildURL
             }
          )
        }
     )
}

$messageBody = $messageBody | ConvertTo-Json -Depth 5

Invoke-RestMethod -Method post -ContentType 'Application/Json' -Body $messageBody -Uri $msTeamsConnectorURL
