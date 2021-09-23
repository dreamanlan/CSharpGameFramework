@echo off
if NOT "%1" EQU "" (
  set cfg=%1
) else (
  set cfg=Debug
)
if NOT "%2" EQU "" (
  set is_pause=%2
) else (
  set is_pause=True
)

rem lang: chs cht kr
set lang=chs

rem working directory
set workdir=%~dp0

set svrbin=%workdir%\App\ServerModule\ServerEnv\bin

set svrdsl=%workdir%\App\ServerModule\ServerEnv\bin\Dsl
set dsldir=%workdir%\Resource\DslFile
set svrtable=%workdir%\App\ServerModule\ServerEnv\bin\Tables
set tabledir=%workdir%\Resource\Tables

set logdir=%workdir%\BuildLog
set libdir=%workdir%\ExternalLibrary

set msbuild=%workdir%\Tools\msbuild\msbuild.exe

rem mdb generator
set pdb2mdb=%workdir%\Tools\mono\mono.exe %workdir%\Tools\lib\mono\4.5\pdb2mdb.exe

rem dsl copy and convert *.dsl from txt to binary (only release version)
if "%cfg%" EQU "Release" (
  set dslcopy=%workdir%\Tools\DslCopy\bin\Debug\DslCopy.exe
) else (
  set dslcopy=%workdir%\Tools\DslCopy\bin\Release\DslCopy.exe
)

rem tabel copy
if "%cfg%" EQU "Release" (
  set tablecopy=%workdir%\Tools\TableReaderGenerator\bin\Debug\TableReaderGenerator.exe
) else (
  set tablecopy=%workdir%\Tools\TableReaderGenerator\bin\Debug\TableReaderGenerator.exe
)

rem show msbuild version
%msbuild% /version
echo.

rem make build log dir
mkdir %logdir%

echo building Server.sln ...
%msbuild% /m /nologo /noconsolelogger /property:Configuration=%cfg% ^
         /flp:LogFile=%logdir%\Server.sln.log;Encoding=UTF-8 ^
		     /t:clean;rebuild ^
         %workdir%\Server.sln
if NOT %ERRORLEVEL% EQU 0 (
  echo build failed, check %logdir%\Server.sln.log.
  goto error_end
) else (
  echo done.
)

rem copy dll to server's bin directory
echo "update binaries"
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
if NOT %ERRORLEVEL% EQU 0 (
  echo copy failed, exclusive access error? check your running process and retry.
  goto error_end
) else (
  echo done.
)

echo [server]: generate *mdb debug files for mono
pushd %workdir%\App\ServerModule\ServerEnv\bin
for /r %%i in (*.pdb) do (
  %pdb2mdb% %%~dpni.dll
)
del /a /f *.pdb
popd
echo done. & echo.

echo update dsl and table files
%dslcopy% %dsldir% %svrdsl%
%tablecopy% %tabledir% %svrtable% isserver

goto good_end

:error_end
set ec=1
goto end
:good_end
set ec=0
echo All Done, Good to Go.
:end
if %is_pause% EQU True (
  pause
  exit /b %ec%
)
