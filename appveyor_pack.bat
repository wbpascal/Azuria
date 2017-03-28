@echo off

if [%APPVEYOR_REPO_TAG%]==[true] (
    nuget_pack.bat
) else (
    nuget_pack.bat /pre
)