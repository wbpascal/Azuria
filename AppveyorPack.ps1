function NugetPack([bool] $pre) 
{
    $Version = $env:GitVersion_MajorMinorPatch
    if ($pre)
    {
        echo "Creating pre-release package..."
        $VersionSuffix = "-ci$($env:GitVersion_BuildMetaData)"
    } 
    else 
    {
        echo "Creating stable package..."
    }

    Get-ChildItem $PSScriptRoot -Filter *.nuspec | 
    Foreach-Object {
        (Get-Content $_) | ForEach-Object { $_ -replace "%%VERSION%%", "$($VERSION)$($VersionSuffix)" } | Set-Content $_
    }
    
    nuget pack Azuria.Core.nuspec -version "$($VERSION)$($VersionSuffix)"
    nuget pack Azuria.Api.nuspec -version "$($VERSION)$($VersionSuffix)"
}

function MygetPush 
{
    if ($env:myget_key)
    {
        Get-ChildItem $PSScriptRoot -Filter *.nupkg | 
        Foreach-Object {
            nuget push $_ $env:myget_key -Source https://www.myget.org/F/infinitesoul/api/v2/package
        }
    }
}

NugetPack ($env:APPVEYOR_REPO_TAG -eq "false")
if ($env:APPVEYOR_REPO_BRANCH -eq "master") { MygetPush }