$url = "https://github.com/OpenCover/opencover/releases/download/4.6.519/opencover.4.6.519.zip"
$output = "$PSScriptRoot\opencover.zip"
$opencover_path = "$PSScriptRoot\OpenCover"
$start_time = Get-Date

(New-Object System.Net.WebClient).DownloadFile($url, $output)

Write-Output "Downloaded OpenCover.zip in $((Get-Date).Subtract($start_time).Seconds) second(s)"

Add-Type -AssemblyName System.IO.Compression.FileSystem

[System.IO.Compression.ZipFile]::ExtractToDirectory($output, $opencover_path)