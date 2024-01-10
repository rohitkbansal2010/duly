param (
    [parameter(Mandatory= $true, HelpMessage = "Root folder for recursive search")]
    [string] $WorkingDirectory,
    [parameter(Mandatory= $false, HelpMessage = "Projects to include")]
    [string] $IncludeProjects = "*Tests.csproj",
    [parameter(Mandatory= $false, HelpMessage = "Package to add into the project")]
    [string] $PackageToAdd = "coverlet.msbuild"
)

$testsProjects = Get-Childitem -Path $WorkingDirectory -Recurse -Include $IncludeProjects

foreach ($testProject in $testsProjects) {
    $res = Start-Process dotnet -ArgumentList "add $($testProject.FullName) package -n -s https://api.nuget.org/v3/index.json $PackageToAdd" -PassThru -Wait -NoNewWindow
    if ($res.ExitCode -ne 0) {
        Write-Warning "$PackageToAdd package was not added to `"$($testProject.FullName)`" project. Please check the log above."
    }
}