@echo off

set arg1=%1
if /i [%arg1%]==[/pre] (
    echo Creating pre-release package...
    for /f %%i in ('gitversion /output json /showvariable LegacySemVer') do set VERSION=%%i
) else (
    echo Creating stable package...
    for /f %%i in ('gitversion /output json /showvariable MajorMinorPatch') do set VERSION=%%i
)

nuget pack Azuria.nuspec -version %VERSION%
