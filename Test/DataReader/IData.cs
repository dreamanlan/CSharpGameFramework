using System;
using System.Collections.Generic;
//using System.Diagnostics;

namespace DemoCommon
{
    public interface IDataRecord
    {
        bool ReadFromBinary(BinaryTable table, int index);
        void WriteToBinary(BinaryTable table);
    }
    public interface IDataRecord<KeyType>
    {
        bool ReadFromBinary(BinaryTable table, int index);
        void WriteToBinary(BinaryTable table);
        KeyType GetId();
    }
}
