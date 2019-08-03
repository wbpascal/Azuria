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
    
    nuget pack Azuria.nuspec -symbols -version "$($env:GitVersion_LegacySemVer)"
}

NugetPack ($env:APPVEYOR_REPO_TAG -eq "false")