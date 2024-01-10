[CmdletBinding()]
param(
    [parameter(Mandatory=$true)]
    [String] $apimRgName,
    [parameter(Mandatory=$true)]
    [String] $apimName
)

Write-Output "Get APIM Context"
$apimContext = New-AzApiManagementContext -ResourceGroupName $apimRgName -ServiceName $apimName

Write-Output "Get APIM Access Key"
$accessKey = Get-AzApiManagementTenantAccess -Context $apimContext | select-object -ExpandProperty SecondaryKey

$identifier = 'integration'
$expiry = [DateTime]::UtcNow.AddHours(1)

Write-Output "Create Cryptography HMAC"
$HMAC = New-Object System.Security.Cryptography.HMACSHA512
$HMAC.key = [Text.Encoding]::UTF8.GetBytes("$accessKey")

Write-Output "Create CultureInfo"
$culture = [System.Globalization.CultureInfo]::InvariantCulture

$dataToSign = $identifier + "`n" + $expiry.ToString("O", $culture)

Write-Output "HMAC Get Bytes"
$hash = $HMAC.ComputeHash([Text.Encoding]::UTF8.GetBytes($dataToSign))

Write-Output "Create Signature"
$signature = [Convert]::ToBase64String($hash)

$SASToken = "SharedAccessSignature uid={0}&ex={1:o}&sn={2}" -f $identifier, $expiry, $signature

# Set a pipeline variable accessKey to the value of SASToken
echo "##vso[task.setvariable variable=sasAccessKey;]$SASToken"