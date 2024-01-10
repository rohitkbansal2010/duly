param (
    [parameter(Mandatory=$true)][string] $Path,
    [parameter()][switch] $skipBranchSuffix
)

Get-Item -Path Env:* | Out-Null

Write-Output ("param:Path: {0}" -f $Path)
Write-Output ("env:Build_BuildNumber: {0}" -f $Env:BUILD_BUILDNUMBER)
Write-Output ("env:Build_SourceBranch: {0}" -f $Env:BUILD_SOURCEBRANCH)

switch -Regex ($Env:BUILD_SOURCEBRANCH) {
    '.*(dev.*)' {
        [string]$build = "{0}.{1}" -f (Get-Date -UFormat '%y.%m.1%d'), $([int]($Env:BUILD_BUILDNUMBER).split('.')[3].split('-')[0]).ToString('0#')
    }
    '.*(main.*)' {
        [string]$build = "{0}.{1}" -f (Get-Date -UFormat '%y.%m.1%d'), $([int]($Env:BUILD_BUILDNUMBER).split('.')[3].split('-')[0]).ToString('0#')
    }
    '.*(release.*)/(\d+).(\d+)' {
        [string]$build = "{0}.{1}.{2}{3}" -f $Matches[2], $Matches[3], (Get-Date -UFormat '%y%m.1%d'), $([int]($Env:BUILD_BUILDNUMBER).split('.')[3].split('-')[0] -replace '^.*(?=.{2}$)').ToString()
    }
}

Get-ChildItem -Path $Path -Recurse | Where-Object { $_.Name -like "*.csproj" } | ForEach-Object {
    $csprojFullName = $_.FullName
    Write-Output ("Updating csproj: {0}" -f $csprojFullName)
    [xml]$xml = (Get-Content -Path $csprojFullName)
    $node = $xml.Project.FirstChild

    if ($node.AssemblyVersion -ne $null) {
        Write-Output ("`tAssemblyVersion: {0}" -f $build)
        $node.AssemblyVersion = $build
    }

    if ($node.FileVersion -ne $null) {
        Write-Output ("`tFileVersion: {0}" -f $build)
        $node.FileVersion = $build
    }

    if ($node.Version -ne $null) {
       # Sometimes we don't need to add a branch suffix to the version, for example when we need to push a release version of a NuGet package
       if ($skipBranchSuffix) {
           Write-Output ("`tVersion: {0}" -f $build)
           $node.Version = $build
       } else {
           Write-Output ("`tVersion: {0}-{1}" -f $build, $Matches[1])
           $node.Version = $("{0}-{1}" -f $build, $Matches[1])
       }    
    }

    $xml.Save($csprojFullName)
}
