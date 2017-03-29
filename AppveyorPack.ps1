function NugetPack([bool] $pre) 
{
    $Version = gitversion /output json /showvariable MajorMinorPatch
    if ($pre)
    {
        echo "Creating pre-release package..."
        $VersionSuffix = "-ci$(gitversion /output json /showvariable BuildMetaData)"
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

NugetPack $env:APPVEYOR_REPO_TAG
if ($env:APPVEYOR_REPO_BRANCH -eq "master") { MygetPush }