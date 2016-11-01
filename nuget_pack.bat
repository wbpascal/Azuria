@echo off

for /f %%i in ('gitversion /output json /showvariable MajorMinorPatch') do set VERSIONPRE=%%i

set arg1=%1
if /i [%arg1%]==[/pre] (
    echo Creating pre-release package...
    for /f %%i in ('gitversion /output json /showvariable BuildMetaData') do set VERSION=%VERSIONPRE%-ci%%i
) else (
    echo Creating stable package...
    for /f %%i in ('gitversion /output json /showvariable BuildMetaData') do set VERSION=%VERSIONPRE%.%%i
)

nuget pack Azuria.nuspec -version %VERSION%