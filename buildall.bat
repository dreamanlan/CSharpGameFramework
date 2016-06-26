@echo off
if NOT "%1" EQU "" (
  set cfg=%1
) else (
  set cfg=Debug
)
if NOT "%2" EQU "" (
  set pause_arg=%2
) else (
  set pause_arg=True
)

rem working directory
set workdir=%~dp0

cd %workdir%

call buildclient.bat %cfg% False
call buildserver.bat %cfg% False

if %pause_arg% EQU True (
  pause
  exit /b %ec%
)
