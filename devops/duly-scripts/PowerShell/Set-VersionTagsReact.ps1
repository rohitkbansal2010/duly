param (
    [parameter(Mandatory=$true)][string] $Path
)

Get-Item -Path Env:*

Write-Output ("param:Path: {0}" -f $Path)
Write-Output ("env:Build_BuildNumber: {0}" -f $Env:BUILD_BUILDNUMBER)
Write-Output ("env:Build_SourceBranch: {0}" -f $Env:BUILD_SOURCEBRANCH)

$htmlPath="$Path\src\index.html"

switch -Regex ($Env:BUILD_SOURCEBRANCH) {
    '.*(dev.*)' {
        [string]$data = "{0}.{1}-develop" -f "$(get-date -UFormat %y.%m.1%d)", ($Env:BUILD_BUILDNUMBER).split('.')[3].split('-')[0]
        [string]$commitID = ($Env:BUILD_SOURCEVERSION).Substring(0,7)
        (Get-Content -Path $htmlPath -Raw) -replace '<meta name="build-version" />', "<meta name=`"build-version`" content=`"$data`" />" | Set-Content -Path $htmlPath
        (Get-Content -Path $htmlPath -Raw) -replace '<meta name="commit-id" />', "<meta name=`"commit-id`" content=`"$commitID`" />" | Set-Content -Path $htmlPath
    }
    '.*(release.*)/(\d+).(\d+)' {
        [string]$data = "{0}{1}" -f "$(get-date -UFormat %y%m.1%d)", (($Env:BUILD_BUILDNUMBER).split('.')[3].split('-')[0] -replace '^.*(?=.{2}$)')
        [string]$commitID = ($Env:BUILD_SOURCEVERSION).Substring(0,7)  
        $buf1 = (($Env:BUILD_SOURCEBRANCH).split('/')).split('.')
        $value = "{0}.{1}.{2}-release" -f $buf1[3], $buf1[4], $data
        (Get-Content -Path $htmlPath -Raw) -replace '<meta name="build-version" />', "<meta name=`"build-version`" content=`"$value`" />" | Set-Content -Path $htmlPath
        (Get-Content -Path $htmlPath -Raw) -replace '<meta name="commit-id" />', "<meta name=`"commit-id`" content=`"$commitID`" />" | Set-Content -Path $htmlPath
    }
}

Get-Content -Path $htmlPath