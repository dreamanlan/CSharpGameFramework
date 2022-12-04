@echo off

rem working directory
set workdir=%~dp0
cd %workdir%

rem mdb generator
set pdb2mdb=%workdir%\Tools\mono\mono.exe %workdir%\Tools\lib\mono\4.5\pdb2mdb.exe

pushd %workdir%\App\ClientModule\Cs2LuaScript\bin\Debug
for /r %%i in (*.pdb) do (
  %pdb2mdb% %%~dpni.dll
)
popd

%workdir%\Tools\cs2lua\Cs2Lua.exe %workdir%\App\ClientModule\Cs2LuaScript\Cs2LuaScript.csproj
xcopy %workdir%\App\ClientModule\Cs2LuaScript\lua\*.txt %workdir%\Unity3d\Assets\Slua\Resources /y /q

xcopy %workdir%\App\ClientModule\Cs2LuaScript\bin\Debug\Cs2LuaScript.dll %workdir%\Unity3d\Assets\Plugins /y /q
xcopy %workdir%\App\ClientModule\Cs2LuaScript\bin\Debug\Cs2LuaScript.dll.mdb %workdir%\Unity3d\Assets\Plugins /y /q
xcopy %workdir%\App\ClientModule\Cs2LuaScript\bin\Debug\Cs2DslUtility.dll %workdir%\Unity3d\Assets\Plugins /y /q

del /a /f %workdir%\Unity3d\Assets\StreamingAssets\Cs2LuaScript.dll
del /a /f %workdir%\Unity3d\Assets\StreamingAssets\Cs2DslUtility.dll