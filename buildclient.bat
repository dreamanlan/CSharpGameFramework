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

set plugindir=%workdir%\Unity3d\Assets\Plugins
set clidsl=%workdir%\Unity3d\Assets\StreamingAssets\Dsl
set dsldir=%workdir%\Resource\DslFile
set clitable=%workdir%\Unity3d\Assets\StreamingAssets
set tabledir=%workdir%\Resource\Tables

set logdir=%workdir%\BuildLog
set libdir=%workdir%\ExternalLibrary

rem set msbuild=%workdir%\Tools\msbuild\msbuild.exe
set msbuild="C:\Program Files\JetBrains\JetBrains Rider 2022.1.1\tools\MSBuild\Current\Bin\MSBuild.exe"

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

echo building Client.sln ...
%msbuild% /m /nologo /noconsolelogger /property:Configuration=%cfg% ^
         /flp:LogFile=%logdir%\Client.sln.log;Encoding=UTF-8 ^
		     /t:clean;rebuild ^
         %workdir%\Client.sln
if NOT %ERRORLEVEL% EQU 0 (
  echo build failed, check %logdir%\Client.sln.log.
  goto error_end
) else (
  echo done.
)

echo [client]: generate *mdb debug files for mono

pushd %workdir%\App\ClientModule\PluginFramework\bin\%cfg%
for /r %%i in (*.pdb) do (
  %pdb2mdb% %%~dpni.dll
)
popd
echo done. & echo.

rem copy dll to unity3d's plugin directory
echo "update binaries"
xcopy %workdir%\App\ClientModule\PluginFramework\bin\%cfg%\*.dll %plugindir% /y /q
xcopy %workdir%\App\ClientModule\PluginFramework\bin\%cfg%\*.mdb %plugindir% /y /q
del /a /f %plugindir%\Library.dll
del /a /f %plugindir%\UnityEngine.dll
del /a /f %plugindir%\UnityEngine.UI.dll
del /a /f %plugindir%\UnityEditor.dll
if NOT %ERRORLEVEL% EQU 0 (
  echo copy failed, exclusive access error? check your running process and retry.
  goto error_end
) else (
  echo done.
)

echo update dsl and table files
%dslcopy% %dsldir% %clidsl%
%tablecopy% %tabledir% %clitable%

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
