param (
    [parameter(Mandatory=$true)]
    [string] $PAT,

    [parameter(Mandatory=$true)]
    [string] $organizationName,

    [parameter(Mandatory=$true)]
    [string] $agentPoolName
)

$cred = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes((":$PAT")))
$header = @{Authorization = "Basic $cred"}

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

$onlineAgents = $agents.value | Where-Object {$_.status -eq "online"}
$offlineAgents = $agents.value | Where-Object {$_.status -eq "offline"}
Write-Output "Found $($onlineAgents.Count) agent(s) online and $($offlineAgents.Count) agent(s) offline."

# Delete offline agents
if ($offlineAgents) {
    Write-Output "Starting offline agents removal..."
    foreach ($agent in $offlineAgents) {
        Write-Output "Processing agent $($agent.name)"
        try {
            $result = Invoke-RestMethod -Method DELETE -ContentType "application/json" -Headers $header `
                -Uri "https://dev.azure.com/$organizationName/_apis/distributedtask/pools/$agentPoolId/agents/$($agent.id)?api-version=6.0" `
                -ErrorAction Stop
            Write-Output "The agent $($agent.name) has been removed successfully."
        } catch {
            Write-Warning "Failed to delete agent $($agent.name): $($_.Exception.Message)"
        }
    }
}
