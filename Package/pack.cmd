@echo off
setlocal

set VERSION=1.0.0

set OUTPUT=c:\NuGet\

if exist "%ProgramFiles%\MSBuild\12.0\Bin\msbuild.exe" (
    set BUILD="%ProgramFiles%\MSBuild\12.0\Bin\msbuild.exe"
)
if exist "%ProgramFiles(x86)%\MSBuild\12.0\Bin\msbuild.exe" (
    set BUILD="%ProgramFiles(x86)%\MSBuild\12.0\Bin\msbuild.exe"
)

REM Clean
call .\clean.cmd

REM Build
%BUILD% .\pack.sln /maxcpucount /target:build /nologo /p:Configuration=Release /p:Platform=AnyCPU

REM Pack
%OUTPUT%nuget.exe pack MMaitre.MediaCaptureExtensions.nuspec -OutputDirectory %OUTPUT%Packages -Prop NuGetVersion=%VERSION% 
:: -NoPackageAnalysis
%OUTPUT%nuget.exe pack MMaitre.MediaCaptureExtensions.Symbols.nuspec -OutputDirectory %OUTPUT%Symbols -Prop NuGetVersion=%VERSION%
:: -NoPackageAnalysis
