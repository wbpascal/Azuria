@echo off

if [%APPVEYOR_REPO_TAG%]==[true] (
    nuget_pack.bat
) else (
    if [%APPVEYOR_REPO_BRANCH%]==[master] (nuget_pack.bat /pre)
)