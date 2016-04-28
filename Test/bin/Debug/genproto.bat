@echo off
rem working directory
set workdir=%~dp0

..\..\Protogen\protogen -i:table.proto -o:TableConfig.cs
move /y TableConfig.cs ..\..\TableConfig\

@pause