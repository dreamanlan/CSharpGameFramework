@echo off
if NOT "%1" EQU "" (
  set cfg=%1
) else (
  set cfg=Debug
)

rem working directory
set workdir=%~dp0

set svrbin=%workdir%\App\ServerModule\ServerEnv\bin

xcopy %workdir%\App\ServerModule\Lobby\bin\%cfg%\*.exe %svrbin% /y /q
xcopy %workdir%\App\ServerModule\Lobby\bin\%cfg%\*.dll %svrbin% /y /q
xcopy %workdir%\App\ServerModule\Lobby\bin\%cfg%\*.pdb %svrbin% /y /q

xcopy %workdir%\App\ServerModule\UserServer\bin\%cfg%\*.exe %svrbin% /y /q
xcopy %workdir%\App\ServerModule\UserServer\bin\%cfg%\*.dll %svrbin% /y /q
xcopy %workdir%\App\ServerModule\UserServer\bin\%cfg%\*.pdb %svrbin% /y /q

xcopy %workdir%\App\ServerModule\RoomServer\bin\%cfg%\*.exe %svrbin% /y /q
xcopy %workdir%\App\ServerModule\RoomServer\bin\%cfg%\*.dll %svrbin% /y /q
xcopy %workdir%\App\ServerModule\RoomServer\bin\%cfg%\*.pdb %svrbin% /y /q

xcopy %workdir%\App\ServerModule\DataCache\bin\%cfg%\*.exe %svrbin% /y /q
xcopy %workdir%\App\ServerModule\DataCache\bin\%cfg%\*.dll %svrbin% /y /q
xcopy %workdir%\App\ServerModule\DataCache\bin\%cfg%\*.pdb %svrbin% /y /q

@pause