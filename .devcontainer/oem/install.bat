@echo off
rem Runs once, at the end of the unattended Windows installation (dockur copies this folder to C:\OEM).
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0install.ps1" > C:\OEM\install.log 2>&1
