function NugetPack([bool] $pre) 
{
    $Version = $env:GitVersion_MajorMinorPatch
    if ($pre)
    {
        echo "Creating pre-release package..."
    } 
    else 
    {
        echo "Creating stable package..."
    }
    
    nuget pack Azuria.nuspec -version "$($env:GitVersion_FullSemVer)"
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