@echo off
del /f/q DataProto\*.cs DataProto\*.proto DataProto\*.sql

LogicDataGenerator.exe

..\ProtoGen\protogen.exe -i:DataProto\Data.proto -o:DataProto\DataMessage.cs

rem xcopy DataProto\*.cs ..\..\..\..\App\GeneratedCode\DataMessage\ /y/d 
@pause
