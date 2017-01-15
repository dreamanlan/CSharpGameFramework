rem working directory
set workdir=%~dp0
cd %workdir%

cd App\ServerModule\ServerEnv\bin
start "center" ServerCenter.exe bigworld
start "center" ServerCenter.exe
start "node" node.exe nodejs/app.js
start "lobby" Lobby.exe nostore
start "datacache" DataCache.exe
start "userserver" UserServer.exe
start "roomserver" RoomServer.exe
rem start "roomserver" RoomServer.exe 21
rem start "roomserver" RoomServer.exe 22

