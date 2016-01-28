#!/bin/bash

./TableReaderGenerator.exe

mv FilePathDefine.cs ../../../../EntitySystem/GameObjects/TableReader/
mv DataReader.cs ../../../../EntitySystem/GameObjects/TableReader/

exit 0
