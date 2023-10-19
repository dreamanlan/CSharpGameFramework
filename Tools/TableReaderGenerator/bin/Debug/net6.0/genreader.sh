#!/bin/bash

./TableReaderGenerator.exe

mv FilePathDefine.cs ../../../../../InformationService/GameObjects/TableReader/
mv DataReader.cs ../../../../../InformationService/GameObjects/TableReader/

exit 0
