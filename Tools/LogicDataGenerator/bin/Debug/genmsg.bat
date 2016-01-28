@echo off
del /f/q ProtoFiles\*.js ProtoFiles\*.cs ProtoFiles\*.proto

LogicDataGenerator.exe genmsg

..\ProtoGen\protogen.exe -i:ProtoFiles\GameFrameworkGmMsg.proto -o:ProtoFiles\GmProtoMessage.cs

..\ProtoGen\protogen.exe -i:ProtoFiles\BigworldAndRoomServer.proto -o:ProtoFiles\BigworldAndRoomServerMessage.cs
..\ProtoGen\protogen.exe -i:ProtoFiles\Billing.proto -o:ProtoFiles\BillingMessage.cs
..\ProtoGen\protogen.exe -i:ProtoFiles\DataMessageDefine.proto -o:ProtoFiles\DataMessage.cs

rem xcopy ProtoFiles\*.cs ..\..\..\..\App\GeneratedCode\Message\ /y/d 
rem xcopy ProtoFiles\*.js ..\..\..\..\App\ServerModule\ServerEnv\bin\web\common\ /y/d 
@pause
