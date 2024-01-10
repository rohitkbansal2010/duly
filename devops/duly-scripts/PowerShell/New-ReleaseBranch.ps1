param (
    [parameter(Mandatory=$true)]
    [string] $PAT,

    [parameter(Mandatory=$true)]
    [string] $projectName,

    [parameter(Mandatory=$true)]
    [string] $repositoryName,

    [parameter(Mandatory=$true)]
    [int] $majorVersion,

    [parameter(Mandatory=$true)]
    [int] $minorVersion,

    [parameter()]
    [string] $commitId = "latest"
)


$cred = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes((":$PAT")))
$organizationName = "Next-Generation-Data-Platform"
$sourceBranchName = "dev"
$majorVersionString = $majorVersion.ToString()
$minorVersionString = $minorVersion.ToString()
$targetBranchName = "release/$($majorVersionString).$($minorVersionString)"

Write-Output "Use project: $projectName"
Write-Output "Use repository: $repositoryName"
Write-Output "Use source branch: $sourceBranchName"
Write-Output "Use commitId: $commitId"
Write-Output "Target branch name: $targetBranchName"

### Phase 1: perform necessary checks and create target branch

# Get list of repositories in the project and find repository id by its name
$repositoriesUrl = "https://dev.azure.com/$organizationName/$projectName/_apis/git/repositories?api-version=6.0"
try {
    $repositories = Invoke-RestMethod -Uri $repositoriesUrl -ContentType "application/json" -Headers @{Authorization = "Basic $cred"} -Method GET -ErrorAction Stop
} catch {
    Throw "Failed to get list of repositories for the project $($projectName): $($_.Exception.Message)"
}
$repositoryId = ($repositories.value | Where-Object {$_.name -eq $repositoryName}).id
if (!$repositoryId) {
    Throw "Failed to find repository id of the repository $($repositoryName) in the project $($projectName)"
}
Write-Output "The repository $($repositoryName) has id $($repositoryId)"

# Get list of branches from the repository and check if the source branch exists
$branchesUrl = "https://dev.azure.com/$organizationName/$projectName/_apis/git/repositories/$repositoryId/refs/heads?api-version=6.0"
try {
    $branches = Invoke-RestMethod -Uri $branchesUrl -ContentType "application/json" -Headers @{Authorization = "Basic $cred"} -Method GET -ErrorAction Stop
} catch {
    Throw "Failed to get a list of branches for repository $($repositoryName): $($_.Exception.Message)"
}
if (!($branches.value | Where-Object {$_.name -eq "refs/heads/$sourceBranchName"})) {
    Throw "The source branch $($sourceBranchName) doesn't exist"
}

# Get commits from source branch
# If the commitId is provided use it for creating a new branch, otherwise use latest commit from the source branch
$commitsUrl = "https://dev.azure.com/$organizationName/$projectName/_apis/git/repositories/$repositoryId/commits?searchCriteria.itemVersion.version=$sourceBranchName&api-version=6.0"
try {
    $commits = Invoke-RestMethod -Uri $commitsUrl -ContentType "application/json" -headers @{Authorization = "Basic $cred"} -Method GET -ErrorAction Stop
} catch {
    Throw "Failed to get a list of commits for the branch $($sourceBranchName): $($_.Exception.Message)"
}
if ($commitId -eq "latest") {
    $sourceCommit = $commits.value[0]
} else {
    if ($commitId.Length -eq 40) {
        $sourceCommit = $commits.value | Where-Object {$_.commitId -eq $commitId}
    } elseif ($commitId.Length -eq 8) {
        $sourceCommit = $commits.value | Where-Object {$_.commitId -like "$commitId*"}
    } else {
        Throw "Specified commit must be either the full 40 chars or the first 8 chars."
    }
    if (!$sourceCommit) {
        Throw "Specified commit $($commitId) doesn't match any of the commits in source branch $($sourceBranchName)"
    }  
}

# Create the target branch if it doesn't exist
$targetBranch = $branches.value | Where-Object {$_.name -eq "refs/heads/$targeBranchName"}
if ($targetBranch) {
    Write-Warning "The branch $($targetBranchName) already exists, skipping:"
    Write-Warning "Branch - $($targetBranch.name), latest commit id - $($targetBranch.objectId), created by $($targetBranch.creator.displayName)"

} else {
    Write-Output "Creating new branch $($targetBranchName) from source branch $($sourceBranchName) based on the following commit:"
    Write-Output "$($sourceCommit.commitId.Substring(0,8)): $($sourceCommit.comment) [$($sourceCommit.author.name) ($($sourceCommit.author.email))]"
    $targetBranchUrl = "https://dev.azure.com/$organizationName/$projectName/_apis/git/repositories/$repositoryId/refs?api-version=5.1"
    $jsonBody = ConvertTo-Json @(
    @{
        name = "refs/heads/$targetBranchName"
        newObjectId = $sourceCommit.commitId
        oldObjectId = "0000000000000000000000000000000000000000"

    })
    try {
        $targetBranch = Invoke-RestMethod -Uri $targetBranchUrl -ContentType "application/json" -Body $jsonBody -headers @{Authorization = "Basic $cred"} -Method POST -ErrorAction Stop
    } catch {
        Throw "Failed to create branch $($targetBranchName): $($_.Exception.Message)"
    }

    # Perform a double check if the result of creation is 'success' or not. Sometimes AzDO cannot create a new branch but it doesn't throw an exception.
    if ($targetBranch.value[0].success -ne "true") {
        Throw "Failed to create branch $($targetBranchName), see the status: $($targetBranch.value[0].updateStatus)"
    }
}

### Phase 2: clone branch policies from source branch and apply them to the target branch

# Get list of policies
$policiesUrl = "https://dev.azure.com/$organizationName/$projectName/_apis/policy/configurations?api-version=6.0"
try {
    $policies = Invoke-RestMethod -Uri $policiesUrl -ContentType "application/json" -Headers @{Authorization = "Basic $cred"} -Method GET -ErrorAction Stop
} catch {
    Throw "Failed to get a list of policies for the project $($projectName): $($_.Exception.Message)"
}
$sourcePolicies = $policies.value | Where-Object {$_.settings.scope[0].repositoryId -eq $repositoryId -and $_.settings.Scope.refName -eq "refs/heads/$sourceBranchName"}
$targetPolicies = $policies.value | Where-Object {$_.settings.scope[0].repositoryId -eq $repositoryId -and $_.settings.Scope.refName -eq "refs/heads/$targetBranchName"}

if ($sourcePolicies) {
    foreach ($policy in $sourcePolicies) {
        # Check if the policy already exists for target branch
        $existPolicy = $targetPolicies | Where-Object {$_.type.displayName -eq $policy.type.displayName}
        if ($existPolicy) {
            if ($existPolicy.type.displayName -eq "Status") {
                # "Status" policy contains codecoverage and quality gate policies so we need to check them separately
                if (($policy.settings.statusName -in $existPolicy.settings.statusName) -and ($policy.settings.defaultDisplayName -in $existPolicy.settings.defaultDisplayName)) {
                    Write-Output "Skipping status policy $($policy.settings.defaultDisplayName) because it already exists"
                    continue
                }
            } elseif ($existPolicy.type.displayName -eq "Build") {
                # "Build" policy can contain more than one policy so we need to check them separately
                if ($policy.settings.buildDefinitionId -in $existPolicy.settings.buildDefinitionId) {
                    Write-Output "Skipping build policy $($policy.settings.displayName) because it already exists"
                    continue
                }
            } else {
                Write-Output "Skipping policy $($policy.type.displayName) because it already exists"
                continue
            }
        }
   
        # Remove specific properties from policy such as id, createdDate etc.
        $policyProperties = $policy.PSObject.Properties
        $policyProperties.remove('createdBy')
        $policyProperties.remove('createdDate')
        $policyProperties.remove('revision')
        $policyProperties.remove('id')
        $policyProperties.remove('url')
        $policy._links.PSObject.Properties.Remove('self')

        # Change reference in policy to apply it to the target branch
        $policy.settings.scope[0].refName = "refs/heads/$targetBranchName"
        
        if ($policy.type.displayName -eq "Build") {
             Write-Output "Applying policy '$($policy.type.displayName): $($policy.settings.displayName)' to the target branch $($targetBranchName)"
        } elseif ($policy.type.displayName -eq "Status") {
             Write-Output "Applying policy '$($policy.type.displayName): $($policy.settings.defaultDisplayName)' to the target branch $($targetBranchName)"
        } else {
             Write-Output "Applying policy $($policy.type.displayName) to the target branch $($targetBranchName)"
        }
        
        try {
            $newPolicy = Invoke-RestMethod -Uri $policiesUrl -ContentType "application/json" -Body (ConvertTo-Json -Depth 100 $policy) -headers @{Authorization = "Basic $cred"} -Method POST -ErrorAction Stop 
        } catch {
            Write-Error "Failed to apply branch policy $($policy.type.displayName) to the target branch $($targetBranchName): $($_.Exception.Message)"
        }
    }
} else {
    Write-Warning "Could not find branch policies for the branch $($sourceBranchName) and repository $($repositoryName), nothing to clone."
}





 



