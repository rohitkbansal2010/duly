param (
    [parameter(Mandatory=$true)]
    [string] $PAT,

    [parameter(Mandatory=$true)]
    [string] $organizationName,

    [parameter(Mandatory=$true)]
    [string] $agentPoolName,

    [parameter(Mandatory=$true)]
    [ValidateSet("Enable","Disable")]
    [string] $action
)

$cred = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes((":$PAT")))
$header = @{Authorization = "Basic $cred"}

if ($action -eq "Enable") {
    $enableAgent = $true
} elseif ($action -eq "Disable") {
    $enableAgent = $false
}

# Get agent pool id
try {
    $agentPool = Invoke-RestMethod -Method GET -ContentType "application/json" -Headers $header `
        -Uri "https://dev.azure.com/$organizationName/_apis/distributedtask/pools?poolName=$agentPoolName&api-version=6.1-preview" `
        -ErrorAction Stop
    $agentPoolId = $agentPool.value.id
} catch {
    Throw "Failed to get information about agent pool $($agentPoolName): $($_.Exception.Message)"
}

# Get agents list from the pool
try {
    $agents = Invoke-RestMethod -Method GET -ContentType "application/json" -Headers $header `
        -Uri "https://dev.azure.com/$organizationName/_apis/distributedtask/pools/$agentPoolId/agents?api-version=6.1-preview" `
        -ErrorAction Stop
} catch {
    Throw "Failed to get information about agents in the pool $($agentPoolName): $($_.Exception.Message)"
}

if (!$agents) {
    Throw "No agents found in pool $agentPoolName"
}

# Enable or disable agents
foreach ($agent in $agents) {
   Write-Output "Performing action '$($action)' on agent $($agent.name)"
   $agentUri = "https://dev.azure.com/$organizationName/_apis/distributedtask/pools/$agentPoolId/agents/$($agent.id)?api-version=6.0"
   $jsonBody = @{
       id = $agent.id
       enabled = $enableAgent
   } | ConvertTo-Json

   try {
       $result = Invoke-RestMethod -Method PATCH -ContentType "application/json" -Headers $header -Uri $agentUri -Body $jsonBody -ErrorAction Stop
   } catch {
       Write-Warning "Failed to perform the action '$($action)' on agent $($agent.name): $($_.Exception.Message)"
   }
}
