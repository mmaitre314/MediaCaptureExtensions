@echo off
setlocal

REM Clean up
if exist .\Release rmdir /S /Q .\Release
if exist .\obj rmdir /S /Q .\obj
if exist .\AnyCPU rmdir /S /Q .\AnyCPU
