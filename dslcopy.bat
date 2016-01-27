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

set clidsl=%workdir%\Unity3d\Assets\StreamingAssets\Dsl
set dsldir=%workdir%\Resource\DslFile
set clitable=%workdir%\Unity3d\Assets\StreamingAssets
set tabledir=%workdir%\Resource\Tables

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
