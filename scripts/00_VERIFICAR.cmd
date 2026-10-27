@echo off
chcp 65001 >nul
echo Herramientas de SportHub
node --version
call npm --version
dotnet --list-sdks
git --version
pause
