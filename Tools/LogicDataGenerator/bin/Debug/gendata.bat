@echo off
del /f/q DataProto\*.cs DataProto\*.proto DataProto\*.sql

net6.0\LogicDataGenerator.exe

..\ProtoGen\protogen.exe --proto_path=DataProto --csharp_out=DataProto +names=original Data.proto

xcopy DataProto\*.cs ..\..\..\..\App\GeneratedCode\DataAccess\ /y/d 
@pause
