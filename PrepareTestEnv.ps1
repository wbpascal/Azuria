$url = "https://ci.appveyor.com/api/buildjobs/56jt50hluqai7e4e/artifacts/main%2Fbin%2Fzip%2Fopencover.4.6.690.zip"
$output = "$PSScriptRoot\opencover.zip"
$opencover_path = "$PSScriptRoot\OpenCover"
$start_time = Get-Date

(New-Object System.Net.WebClient).DownloadFile($url, $output)

Write-Output "Downloaded OpenCover.zip in $((Get-Date).Subtract($start_time).Seconds) second(s)"

Add-Type -AssemblyName System.IO.Compression.FileSystem

[System.IO.Compression.ZipFile]::ExtractToDirectory($output, $opencover_path)