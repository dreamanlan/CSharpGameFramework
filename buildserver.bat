@echo off
if NOT "%1" EQU "" (
  set server_cfg=%1
) else (
  set server_cfg=Debug
)
if NOT "%2" EQU "" (
  set server_pause=%2
) else (
  set server_pause=True
)

rem working directory
set workdir=%~dp0
cd %workdir%

BatchCommand.exe ./Tools/Batch/buildserver.dsl %server_cfg% %server_pause%

if %server_pause% EQU True (
  pause
  exit /b %ec%
)
