@echo off
del /f/q ProtoFiles\*.js ProtoFiles\*.cs ProtoFiles\*.proto

net6.0\LogicDataGenerator.exe genmsg

..\ProtoGen\protogen.exe --proto_path=ProtoFiles --csharp_out=ProtoFiles +names=original RoomMsg.proto
..\ProtoGen\protogen.exe --proto_path=ProtoFiles --csharp_out=ProtoFiles +names=original LobbyMsg.proto
..\ProtoGen\protogen.exe --proto_path=ProtoFiles --csharp_out=ProtoFiles +names=original LobbyGmMsg.proto

..\ProtoGen\protogen.exe --proto_path=ProtoFiles --csharp_out=ProtoFiles +names=original BigworldAndRoomServer.proto
..\ProtoGen\protogen.exe --proto_path=ProtoFiles --csharp_out=ProtoFiles +names=original DataMessageDefine.proto

xcopy ProtoFiles\*.cs ..\..\..\..\App\GeneratedCode\Message\ /y/d 
xcopy ProtoFiles\*.js ..\..\..\..\App\ServerModule\ServerEnv\bin\nodejs\ /y/d 
@pause
