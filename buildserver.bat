@echo off

rem working directory
set workdir=%~dp0
cd %workdir%

BatchCommand.exe ./Tools/Batch/buildserver.dsl Debug
