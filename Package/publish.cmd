@echo off
setlocal

set VERSION=1.0.0

set OUTPUT=c:\NuGet\

%OUTPUT%nuget push %OUTPUT%Packages\MMaitre.MediaCaptureExtensions.%VERSION%.nupkg
%OUTPUT%nuget push %OUTPUT%Symbols\MMaitre.MediaCaptureExtensions.Symbols.%VERSION%.nupkg -Source http://nuget.gw.symbolsource.org/Public/NuGet 