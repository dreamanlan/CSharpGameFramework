@echo off
del /f/q DataProto\*.cs DataProto\*.proto DataProto\*.sql

LogicDataGenerator.exe

..\ProtoGen\protogen.exe -i:DataProto\Data.proto -o:DataProto\DataStruct.cs

xcopy DataProto\*.cs ..\..\..\..\App\GeneratedCode\DataAccess\ /y/d 
@pause
