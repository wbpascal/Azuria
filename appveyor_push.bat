@echo off

if exist Azuria.%VERSION%.nupkg (
    if defined myget_key (
        nuget push Azuria.%VERSION%.nupkg %myget_key% -Source https://www.myget.org/F/infinitesoul/api/v2/package
    )
)