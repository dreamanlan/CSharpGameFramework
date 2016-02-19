@echo off
del /f/q ProtoFiles\*.js ProtoFiles\*.cs ProtoFiles\*.proto

LogicDataGenerator.exe genmsg

..\ProtoGen\protogen.exe -i:ProtoFiles\RoomMsg.proto -o:ProtoFiles\RoomMsg.cs
..\ProtoGen\protogen.exe -i:ProtoFiles\LobbyMsg.proto -o:ProtoFiles\LobbyMsg.cs
..\ProtoGen\protogen.exe -i:ProtoFiles\LobbyGmMsg.proto -o:ProtoFiles\LobbyGmMsg.cs

..\ProtoGen\protogen.exe -i:ProtoFiles\BigworldAndRoomServer.proto -o:ProtoFiles\BigworldAndRoomServerMessage.cs
..\ProtoGen\protogen.exe -i:ProtoFiles\DataMessageDefine.proto -o:ProtoFiles\DataMessage.cs

xcopy ProtoFiles\*.cs ..\..\..\..\App\GeneratedCode\Message\ /y/d 
xcopy ProtoFiles\*.js ..\..\..\..\App\ServerModule\ServerEnv\bin\nodejs\ /y/d 
@pause
