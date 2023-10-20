@echo off
if NOT "%1" EQU "" (
  set cfg=%1
) else (
  set cfg=Debug
)

rem working directory
set workdir=%~dp0

set svrbin=%workdir%\App\ServerModule\ServerEnv\bin

xcopy %workdir%\App\ServerModule\RoomServer\bin\%cfg%\net7.0\Room*.exe %svrbin% /y /q
xcopy %workdir%\App\ServerModule\RoomServer\bin\%cfg%\net7.0\Room*.dll %svrbin% /y /q
xcopy %workdir%\App\ServerModule\RoomServer\bin\%cfg%\net7.0\Room*.pdb %svrbin% /y /q
xcopy %workdir%\App\ServerModule\RoomServer\bin\%cfg%\net7.0\Room*.json %svrbin% /y /q
xcopy %workdir%\App\ServerModule\RoomServer\bin\%cfg%\net7.0\Room*.config %svrbin% /y /q

@pause