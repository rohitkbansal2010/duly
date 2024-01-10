param (
    [parameter()][switch] $skipBranchSuffix
)
Get-Item -Path Env:* | Out-Null

Write-Output ("env:Build_BuildNumber: {0}" -f $Env:BUILD_BUILDNUMBER)
Write-Output ("env:Build_SourceBranch: {0}" -f $Env:BUILD_SOURCEBRANCH)

switch -Regex ($Env:BUILD_SOURCEBRANCH) {
    '.*(dev.*)' {
        [string]$build = "{0}.{1}" -f (Get-Date -UFormat '%y.%m.1%d'), $([int]($Env:BUILD_BUILDNUMBER).split('.')[1]).ToString('0#')
    }
    '.*(main.*)' {
        [string]$build = "{0}.{1}" -f (Get-Date -UFormat '%y.%m.1%d'), $([int]($Env:BUILD_BUILDNUMBER).split('.')[1]).ToString('0#')
    }
    '.*(release.*)/(\d+).(\d+)' {
        [string]$build = "{0}.{1}.{2}{3}" -f $Matches[2], $Matches[3], (Get-Date -UFormat '%y%m.1%d'), $([int]($Env:BUILD_BUILDNUMBER).split('.')[1]).ToString('0#')
    }
}

if ($skipBranchSuffix) {
    # Sometimes we don't need to add a branch suffix to the version, for example when we need to push a release version of a NuGet package
    $version = $build
} else {
    $version = "{0}-{1}" -f $build, $Matches[1]
}


Write-Output ("##vso[task.setvariable variable=packversion;]{0}" -f $version)
Write-Output ("Setting Azure DevOps buildnumber: {0}" -f $version)
Write-Output ("##vso[build.updatebuildnumber]{0}" -f $version)
