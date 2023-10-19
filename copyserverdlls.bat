@echo off
if NOT "%1" EQU "" (
  set cfg=%1
) else (
  set cfg=Debug
)

rem working directory
set workdir=%~dp0

set svrbin=%workdir%\App\ServerModule\ServerEnv\bin

xcopy %workdir%\App\ServerModule\Lobby\bin\%cfg%\net6.0\*.exe %svrbin% /y /q
xcopy %workdir%\App\ServerModule\Lobby\bin\%cfg%\net6.0\*.dll %svrbin% /y /q
xcopy %workdir%\App\ServerModule\Lobby\bin\%cfg%\net6.0\*.pdb %svrbin% /y /q
xcopy %workdir%\App\ServerModule\Lobby\bin\%cfg%\net6.0\*.json %svrbin% /y /q

xcopy %workdir%\App\ServerModule\UserServer\bin\%cfg%\net6.0\*.exe %svrbin% /y /q
xcopy %workdir%\App\ServerModule\UserServer\bin\%cfg%\net6.0\*.dll %svrbin% /y /q
xcopy %workdir%\App\ServerModule\UserServer\bin\%cfg%\net6.0\*.pdb %svrbin% /y /q
xcopy %workdir%\App\ServerModule\UserServer\bin\%cfg%\net6.0\*.json %svrbin% /y /q

xcopy %workdir%\App\ServerModule\RoomServer\bin\%cfg%\net6.0\*.exe %svrbin% /y /q
xcopy %workdir%\App\ServerModule\RoomServer\bin\%cfg%\net6.0\*.dll %svrbin% /y /q
xcopy %workdir%\App\ServerModule\RoomServer\bin\%cfg%\net6.0\*.pdb %svrbin% /y /q
xcopy %workdir%\App\ServerModule\RoomServer\bin\%cfg%\net6.0\*.json %svrbin% /y /q

xcopy %workdir%\App\ServerModule\DataCache\bin\%cfg%\net6.0\*.exe %svrbin% /y /q
xcopy %workdir%\App\ServerModule\DataCache\bin\%cfg%\net6.0\*.dll %svrbin% /y /q
xcopy %workdir%\App\ServerModule\DataCache\bin\%cfg%\net6.0\*.pdb %svrbin% /y /q
xcopy %workdir%\App\ServerModule\DataCache\bin\%cfg%\net6.0\*.json %svrbin% /y /q

@pause