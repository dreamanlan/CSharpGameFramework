cd App\ServerModule\ServerEnv\bin
start "center" ServerCenter.exe bigworld
start "center" ServerCenter.exe
start "node" node.exe nodejs/app.js
start "lobby" Lobby.exe nostore
rem start "datacache" DataCache.exe
start "userserver" UserServer.exe
start "roomserver" RoomServer.exe

