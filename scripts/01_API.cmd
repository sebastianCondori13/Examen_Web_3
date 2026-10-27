@echo off
chcp 65001 >nul
cd /d "%~dp0..\backend\SportHub.Api"
dotnet restore
if errorlevel 1 goto fin
dotnet run
:fin
pause
