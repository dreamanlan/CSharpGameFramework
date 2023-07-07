rem working directory
set workdir=%~dp0
cd %workdir%

@taskkill /im node.exe /f
@taskkill /im RoomServer.exe /f
@taskkill /im UserServer.exe /f
@taskkill /im Lobby.exe /f
@taskkill /im DataCache.exe /f
@taskkill /im ServerCenter.exe /f
@taskkill /im ServerCenter.exe /f

