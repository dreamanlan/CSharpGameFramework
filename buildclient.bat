@echo off
if NOT "%1" EQU "" (
  set client_cfg=%1
) else (
  set client_cfg=Debug
)
if NOT "%2" EQU "" (
  set client_pause=%2
) else (
  set client_pause=True
)

rem working directory
set workdir=%~dp0
cd %workdir%

BatchCommand.exe ./Tools/Batch/buildclient.dsl %client_cfg% %client_pause%

if %client_pause% EQU True (
  pause
  exit /b %ec%
)
