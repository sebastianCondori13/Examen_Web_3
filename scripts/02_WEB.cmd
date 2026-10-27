@echo off
chcp 65001 >nul
cd /d "%~dp0..\web"
if exist node_modules goto iniciar
call npm ci
if errorlevel 1 goto fin
:iniciar
call npm run dev
:fin
pause
