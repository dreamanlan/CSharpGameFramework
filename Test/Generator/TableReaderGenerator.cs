using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using Dsl;
using DemoCommon;

namespace TableReaderGenerator
{
    internal enum TableFileTypeEnum
    {
        CLIENT = 0,
        PUBLIC,
        SERVER,
        MULTIFILE,
    }
    internal class TypeDef
    {
        internal string m_TypeName;
        internal string m_TypeCode;
        internal string m_BinaryCode;
        internal string m_RecordCode = "record.{0} = DataRecordUtility.SetValue(table, {0}, {1});";

        internal TypeDef() { }
        internal TypeDef(string typeName, string typeCode, string textCode, string binaryCode)
        {
            m_TypeName = typeName;
            m_TypeCode = typeCode;
            m_BinaryCode = binaryCode;
            if (m_TypeCode.CompareTo("string") == 0) {
                m_RecordCode = "record.{0} = DataRecordUtility.SetValue(table, {0}, \"{1}\");";
            } else {
                m_RecordCode = "record.{0} = DataRecordUtility.SetValue(table, {0}, {1});";
            }
        }
        internal TypeDef(string typeName, string typeCode, string textCode, string binaryCode, string recordcode)
        {
            m_TypeName = typeName;
            m_TypeCode = typeCode;
            m_BinaryCode = binaryCode;
            m_RecordCode = recordcode;
        }
    }
    internal class FieldDef
    {
        internal string m_MemberName;
        internal string m_FieldName;
        internal string m_Type;
        internal string m_Default;
        internal string m_Access;
    }
    internal class TableDef
    {
        internal string m_TableName;
        internal string m_Type;
        internal List<FieldDef> m_Fields = new List<FieldDef>();
        internal TableFileTypeEnum m_FileType;
        internal string m_RecordName;
        internal string m_ProviderName;
        internal string m_RecordModifier = "";
        internal string m_ProviderModifier = "";
        internal string m_CsFileName = "DataReader";
        internal string m_KeyFieldTypeCode = string.Empty;
        internal string m_KeyFieldMemberName = string.Empty;
        internal string m_ContainerType = string.Empty;

        internal int GetLayoutFieldCount()
        {
            int ct = 0;
            for (int i = 0; i < m_Fields.Count; ++i) {
                ++ct;
            }
            return ct;
        }
    }
    internal class TableDslParser
    {
        internal bool Init(string dslFile)
        {
            try {
                DslFile file = new DslFile();
                if (!file.Load(dslFile, LogSystem.Log)) {
                    return false;
                }
                bool haveError = false;
                foreach (DslInfo info in file.DslInfos) {
                    if (info.GetId() == "tool") {
                        if (info.Functions.Count == 1) {
                            FunctionData funcData = info.First;
                            if (null != funcData && funcData.HaveExternScript()) {
                                string toolFile;
                                CallData callData = funcData.Call;
                                if (null != callData && callData.GetParamNum() == 1) {
                                    toolFile = callData.GetParamId(0);
                                } else {
                                    toolFile = m_DefToolCsFileName;
                                }
                                List<string> codes;
                                if (!m_ToolCodes.TryGetValue(toolFile, out codes)) {
                                    codes = new List<string>();
                                    m_ToolCodes.Add(toolFile, codes);
                                }
                                codes.Add(funcData.GetExternScript());
                            }
                        } else {
                            LogSystem.Error("tool {0} must end with ';' ! line {1}", info.ToScriptString(), info.GetLine());
                            haveError = true;
                        }
                    } else if (info.GetId() == "global") {
                        if (info.Functions.Count == 1) {
                            FunctionData funcData = info.First;
                            if (null != funcData && funcData.HaveExternScript()) {
                                string globalFile;
                                CallData callData = funcData.Call;
                                if (null != callData && callData.GetParamNum() == 1) {
                                    globalFile = callData.GetParamId(0);
                                } else {
                                    globalFile = m_DefGlobalCsFileName;
                                }
                                List<string> codes;
                                if (!m_GlobalCodes.TryGetValue(globalFile, out codes)) {
                                    codes = new List<string>();
                                    m_GlobalCodes.Add(globalFile, codes);
                                }
                                codes.Add(funcData.GetExternScript());
                            }
                        } else {
                            LogSystem.Error("global {0} must end with ';' ! line {1}", info.ToScriptString(), info.GetLine());
                            haveError = true;
                        }
                    } else if (info.GetId() == "typedef") {
                        if (info.Functions.Count == 1) {
                            FunctionData funcData = info.First;
                            if (null != funcData) {
                                CallData callData = funcData.Call;
                                if (null != callData && callData.GetParamNum() == 1) {
                                    string typeName = callData.GetParamId(0);

                                    TypeDef typeDef = new TypeDef();
                                    typeDef.m_TypeName = typeName;
                                    if (m_Types.ContainsKey(typeName)) {
                                        m_Types[typeName] = typeDef;
                                    } else {
                                        m_Types.Add(typeName, typeDef);
                                    }

                                    foreach (ISyntaxComponent comp in funcData.Statements) {
                                        FunctionData item = comp as FunctionData;
                                        if (null != item) {
                                            if (item.HaveExternScript()) {
                                                if (item.GetId() == "binary") {
                                                    typeDef.m_BinaryCode = item.GetExternScript();
                                                } else if (item.GetId() == "record") {
                                                    typeDef.m_RecordCode = item.GetExternScript();
                                                }
                                            } else {
                                                LogSystem.Error("typedef {0} must contains code ! line {1}", comp.ToScriptString(), comp.GetLine());
                                                haveError = true;
                                            }
                                        } else {
                                            CallData call = comp as CallData;
                                            if (null != call && call.GetParamNum() == 1) {
                                                if (call.GetId() == "type") {
                                                    typeDef.m_TypeCode = call.GetParamId(0);
                                                }
                                            } else {
                                                LogSystem.Error("typedef {0} must have params or end with ';' ! line {1}", comp.ToScriptString(), comp.GetLine());
                                                haveError = true;
                                            }
                                        }
                                    }
                                } else {
                                    LogSystem.Error("typedef {0} must have 1 params ! line {1}", info.ToScriptString(), info.GetLine());
                                    haveError = true;
                                }
                            }
                        } else {
                            LogSystem.Error("typedef {0} must end with ';' ! line {1}", info.ToScriptString(), info.GetLine());
                            haveError = true;
                        }
                    } else if (info.GetId() == "tabledef") {
                        if (info.Functions.Count == 1) {
                            FunctionData funcData = info.First;
                            if (null != funcData) {
                                CallData callData = funcData.Call;
                                if (null != callData && callData.GetParamNum() == 3) {
                                    TableFileTypeEnum fileType = TableFileTypeEnum.PUBLIC;
                                    string tableName, tableType, recordName, providerName;
                                    tableName = callData.GetParamId(0);
                                    tableType = callData.GetParamId(1);
                                    recordName = tableName;
                                    providerName = tableName + "Provider";

                                    string fileTypeStr = callData.GetParamId(2);
                                    if (0 == fileTypeStr.CompareTo("client")) {
                                        fileType = TableFileTypeEnum.CLIENT;
                                    } else if (0 == fileTypeStr.CompareTo("server")) {
                                        fileType = TableFileTypeEnum.SERVER;
                                    } else if (0 == fileTypeStr.CompareTo("multifile")) {
                                        fileType = TableFileTypeEnum.MULTIFILE;
                                    } else {
                                        fileType = TableFileTypeEnum.PUBLIC;
                                    }

                                    TableDef tableDef = new TableDef();
                                    tableDef.m_TableName = tableName;
                                    tableDef.m_Type = tableType;
                                    tableDef.m_FileType = fileType;
                                    tableDef.m_RecordName = recordName;
                                    tableDef.m_ProviderName = providerName;
                                    if (m_Tables.ContainsKey(tableName)) {
                                        m_Tables[tableName] = tableDef;
                                    } else {
                                        m_Tables.Add(tableName, tableDef);
                                    }

                                    foreach (ISyntaxComponent comp in funcData.Statements) {
                                        CallData field = comp as CallData;
                                        if (null != field) {
                                            if (field.GetId() == "recordname") {
                                                tableDef.m_RecordName = field.GetParamId(0);
                                                tableDef.m_ProviderName = tableDef.m_RecordName + "Provider";
                                            } else if (field.GetId() == "providername") {
                                                tableDef.m_ProviderName = field.GetParamId(0);
                                            } else if (field.GetId() == "filename") {
                                                tableDef.m_CsFileName = field.GetParamId(0);
                                            } else if (field.GetId() == "recordmodifier") {
                                                tableDef.m_RecordModifier = field.GetParamId(0);
                                                if (tableDef.m_RecordModifier.Length > 0 && tableDef.m_RecordModifier[0] != ' ') {
                                                    tableDef.m_RecordModifier = " " + tableDef.m_RecordModifier;
                                                }
                                            } else if (field.GetId() == "providermodifier") {
                                                tableDef.m_ProviderModifier = field.GetParamId(0);
                                                if (tableDef.m_ProviderModifier.Length > 0 && tableDef.m_ProviderModifier[0] != ' ') {
                                                    tableDef.m_ProviderModifier = " " + tableDef.m_ProviderModifier;
                                                }
                                            } else if (field.GetId() == "fielddef" && field.GetParamNum() >= 3) {
                                                FieldDef fieldDef = new FieldDef();
                                                fieldDef.m_MemberName = field.GetParamId(0);
                                                fieldDef.m_FieldName = field.GetParamId(1);
                                                fieldDef.m_Type = field.GetParamId(2);
                                                if (field.GetParamNum() >= 4) {
                                                    ISyntaxComponent param = field.GetParam(3);
                                                    if (null != param) {
                                                        fieldDef.m_Default = param.GetId();
                                                    }
                                                } else {
                                                    if (0 == fieldDef.m_Type.CompareTo("string")) {
                                                        fieldDef.m_Default = "";
                                                    } else if (fieldDef.m_Type.Contains("_list") || fieldDef.m_Type.Contains("_array")) {
                                                        fieldDef.m_Default = "null";
                                                    } else if (0 == fieldDef.m_Type.CompareTo("bool")) {
                                                        fieldDef.m_Default = "false";
                                                    } else {
                                                        fieldDef.m_Default = "0";
                                                    }
                                                }
                                                if (field.GetParamNum() >= 5) {
                                                    fieldDef.m_Access = field.GetParamId(4);
                                                } else {
                                                    fieldDef.m_Access = "public";
                                                }
                                                tableDef.m_Fields.Add(fieldDef);
                                            } else {
                                                LogSystem.Error("field {0} must have name (member and field) and type ! line {1}", comp.ToScriptString(), comp.GetLine());
                                                haveError = true;
                                            }
                                        } else {
                                            LogSystem.Error("field {0} must have name (member and field) and type and end with ';' ! line {1}", comp.ToScriptString(), comp.GetLine());
                                            haveError = true;
                                        }
                                    }
                                } else {
                                    LogSystem.Error("tabledef {0} must have 3 params ! line {1}", info.ToScriptString(), info.GetLine());
                                    haveError = true;
                                }
                            }
                        } else {
                            LogSystem.Error("tabledef {0} must end with ';' ! line {1}", info.ToScriptString(), info.GetLine());
                            haveError = true;
                        }
                    } else {
                        LogSystem.Error("Unknown part {0}, line {1}", info.GetId(), info.GetLine());
                        haveError = true;
                    }
                }
                m_Types.Add("string", new TypeDef("string", "string", "{0} = DataRecordUtility.ExtractString(node, \"{1}\", \"{2}\");", "{0} = DataRecordUtility.ExtractString(table, record.{0}, \"{1}\");"));
                m_Types.Add("bool", new TypeDef("bool", "bool", "{0} = DataRecordUtility.ExtractBool(node, \"{1}\", {2});", "{0} = DataRecordUtility.ExtractBool(table, record.{0}, {1});"));
                m_Types.Add("int", new TypeDef("int", "int", "{0} = DataRecordUtility.ExtractNumeric<int>(node, \"{1}\", {2});", "{0} = DataRecordUtility.ExtractInt(table, record.{0}, {1});"));
                m_Types.Add("int32", new TypeDef("int", "int", "{0} = DataRecordUtility.ExtractNumeric<int>(node, \"{1}\", {2});", "{0} = DataRecordUtility.ExtractInt(table, record.{0}, {1});"));
                m_Types.Add("float", new TypeDef("float", "float", "{0} = DataRecordUtility.ExtractNumeric<float>(node, \"{1}\", {2});", "{0} = DataRecordUtility.ExtractFloat(table, record.{0}, {1});"));
                m_Types.Add("long", new TypeDef("long", "long", "{0} = DataRecordUtility.ExtractNumeric<long>(node, \"{1}\", {2});", "{0} = DataRecordUtility.ExtractLong(table, record.{0}, {1});"));
                m_Types.Add("int64", new TypeDef("long", "long", "{0} = DataRecordUtility.ExtractNumeric<long>(node, \"{1}\", {2});", "{0} = DataRecordUtility.ExtractLong(table, record.{0}, {1});"));
                m_Types.Add("string_list", new TypeDef("string_list", "List<string>", "{0} = DataRecordUtility.ExtractStringList(node, \"{1}\", {2});", "{0} = DataRecordUtility.ExtractStringList(table, record.{0}, {1});"));
                m_Types.Add("string_array", new TypeDef("string_array", "string[]", "{0} = DataRecordUtility.ExtractStringArray(node, \"{1}\", {2});", "{0} = DataRecordUtility.ExtractStringArray(table, record.{0}, {1});"));
                m_Types.Add("int_list", new TypeDef("int_list", "List<int>", "{0} = DataRecordUtility.ExtractNumericList<int>(node, \"{1}\", {2});", "{0} = DataRecordUtility.ExtractIntList(table, record.{0}, {1});"));
                m_Types.Add("int_array", new TypeDef("int_array", "int[]", "{0} = DataRecordUtility.ExtractNumericArray<int>(node, \"{1}\", {2});", "{0} = DataRecordUtility.ExtractIntArray(table, record.{0}, {1});"));
                m_Types.Add("float_list", new TypeDef("float_list", "List<float>", "{0} = DataRecordUtility.ExtractNumericList<float>(node, \"{1}\", {2});", "{0} = DataRecordUtility.ExtractFloatList(table, record.{0}, {1});"));
                m_Types.Add("float_array", new TypeDef("float_array", "float[]", "{0} = DataRecordUtility.ExtractNumericArray<float>(node, \"{1}\", {2});", "{0} = DataRecordUtility.ExtractFloatArray(table, record.{0}, {1});"));
                m_Types.Add("int32_list", new TypeDef("int_list", "List<int>", "{0} = DataRecordUtility.ExtractNumericList<int>(node, \"{1}\", {2});", "{0} = DataRecordUtility.ExtractIntList(table, record.{0}, {1});"));
                m_Types.Add("int32_array", new TypeDef("int_array", "int[]", "{0} = DataRecordUtility.ExtractNumericArray<int>(node, \"{1}\", {2});", "{0} = DataRecordUtility.ExtractIntArray(table, record.{0}, {1});"));

                foreach (var pair in m_Tables) {
                    TableDef tableDef = pair.Value;
                    if (0 == tableDef.m_Type.CompareTo("dictionary")) {
                        if (tableDef.m_Fields.Count > 0) {
                            tableDef.m_KeyFieldMemberName = tableDef.m_Fields[0].m_MemberName;
                            TypeDef keyTypeDef;
                            if (m_Types.TryGetValue(tableDef.m_Fields[0].m_Type, out keyTypeDef)) {
                                tableDef.m_KeyFieldTypeCode = keyTypeDef.m_TypeCode;
                                tableDef.m_ContainerType = string.Format("DataDictionaryMgr<{0},{1}>", tableDef.m_TableName, tableDef.m_KeyFieldTypeCode);
                            } else {
                                Console.WriteLine("Can't find type {0}'s definition ! table {1}", tableDef.m_KeyFieldTypeCode, pair.Key);
                                haveError = true;
                            }
                        }
                    } else {
                        tableDef.m_ContainerType = string.Format("DataListMgr<{0}>", tableDef.m_TableName);
                    }
                }

                return !haveError;
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
            return false;
        }
        internal void GenAllReaders(Dictionary<string, string> tableFiles)
        {
            System.Diagnostics.Process p = System.Diagnostics.Process.Start("cmd", "/c del /f/q *.cs");
            p.WaitForExit();

            using (StreamWriter sw = new StreamWriter("FilePathDefine.cs", true)) {
                sw.WriteLine("//----------------------------------------------------------------------------");
                sw.WriteLine("//！！！不要手动修改此文件，此文件由TableReaderGenerator按table.dsl生成！！！");
                sw.WriteLine("//----------------------------------------------------------------------------");
                sw.WriteLine("using System;");
                sw.WriteLine("using System.Collections.Generic;");
                sw.WriteLine("using System.IO;");
                sw.WriteLine("using System.Text;");
                sw.WriteLine();
                sw.WriteLine("internal class FilePathDefine_Client");
                sw.WriteLine("{");
                sw.WriteLine("\tconst string C_RootPath = FilePathDefineBase.C_RootPath;");
                foreach (var pair in tableFiles) {
                    string filename = pair.Key;
                    string filepath = pair.Value;
                    if (filepath.Contains("Public") || filepath.Contains("Client")) {
                        sw.WriteLine("\tpublic const string C_{0} = C_RootPath + \"{1}\";", filename, Path.ChangeExtension(filepath, "dat"));
                        sw.WriteLine("\tpublic const string C_Proto{0} = C_RootPath + \"{1}\";", filename, Path.ChangeExtension(filepath, "data"));
                    }
                }
                sw.WriteLine("}");
                sw.WriteLine("internal class FilePathDefine_Server");
                sw.WriteLine("{");
                sw.WriteLine("\tconst string C_RootPath = FilePathDefineBase.C_RootPath;");
                foreach (var pair in tableFiles) {
                    string filename = pair.Key;
                    string filepath = pair.Value;
                    if (filepath.Contains("Public") || filepath.Contains("Server")) {
                        sw.WriteLine("\tpublic const string C_{0} = C_RootPath + \"{1}\";", filename, Path.ChangeExtension(filepath, "dat"));
                        sw.WriteLine("\tpublic const string C_Proto{0} = C_RootPath + \"{1}\";", filename, Path.ChangeExtension(filepath, "data"));
                    }
                }
                sw.WriteLine("}");
                sw.Close();
            }
            HashSet<string> files = new HashSet<string>();            
            foreach (var codePair in m_GlobalCodes) {
                string fileName = codePair.Key;
                List<string> codes = codePair.Value;
                string file = fileName + ".cs";
                if (!files.Contains(file)) {
                    files.Add(file);
                    try {
                        using (StreamWriter sw = new StreamWriter(file, true)) {
                            sw.WriteLine("//----------------------------------------------------------------------------");
                            sw.WriteLine("//！！！不要手动修改此文件，此文件由TableReaderGenerator按table.dsl生成！！！");
                            sw.WriteLine("//----------------------------------------------------------------------------");
                            sw.WriteLine("using System;");
                            sw.WriteLine("using System.Collections.Generic;");
                            sw.WriteLine("using System.Runtime.InteropServices;");
                            sw.WriteLine("using System.IO;");
                            sw.WriteLine("using System.Text;");
                            sw.WriteLine("using DemoCommon;");
                            sw.Close();
                        }
                    } catch (Exception ex) {
                        Console.WriteLine(ex);
                    }
                }
                using (StreamWriter sw = new StreamWriter(file, true)) {
                    sw.WriteLine();
                    sw.WriteLine("namespace BinaryTableConfig");
                    sw.WriteLine("{");
                    foreach (string code in codes) {
                        sw.WriteLine(IndentCode("\t", code));
                    }
                    sw.WriteLine("}");
                    sw.Close();
                }
            }
            foreach (var tablePair in m_Tables) {
                string table = tablePair.Key;
                TableDef tableDef = tablePair.Value;
                string file = tableDef.m_CsFileName + ".cs";
                if (!files.Contains(file)) {
                    files.Add(file);
                    try {
                        using (StreamWriter sw = new StreamWriter(file, true)) {
                            sw.WriteLine("//----------------------------------------------------------------------------");
                            sw.WriteLine("//！！！不要手动修改此文件，此文件由TableReaderGenerator按table.dsl生成！！！");
                            sw.WriteLine("//----------------------------------------------------------------------------");
                            sw.WriteLine("using System;");
                            sw.WriteLine("using System.Collections.Generic;");
                            sw.WriteLine("using System.Runtime.InteropServices;");
                            sw.WriteLine("using System.IO;");
                            sw.WriteLine("using System.Text;");
                            sw.WriteLine("using DemoCommon;");
                            sw.Close();
                        }
                    } catch (Exception ex) {
                        Console.WriteLine(ex);
                    }
                }
                GenReader(table, file, true);
            }
        }
        internal void GenReader(string table, string file, bool append)
        {
            TableDef tableDef;
            if (m_Tables.TryGetValue(table, out tableDef)) {
                try {
                    using (StreamWriter sw = new StreamWriter(file, append)) {
                        sw.WriteLine();
                        sw.WriteLine("namespace BinaryTableConfig");
                        sw.WriteLine("{");
                        if (0 == tableDef.m_Type.CompareTo("dictionary")) {
                            sw.WriteLine("\tpublic sealed{0} class {1} : IDataRecord<{2}>", tableDef.m_RecordModifier, tableDef.m_RecordName, tableDef.m_KeyFieldTypeCode);
                        } else {
                            sw.WriteLine("\tpublic sealed{0} class {1} : IDataRecord", tableDef.m_RecordModifier, tableDef.m_RecordName);
                        }
                        sw.WriteLine("\t{");
                        sw.WriteLine("\t\t[StructLayout(LayoutKind.Auto, Pack = 1, Size = {0})]", tableDef.GetLayoutFieldCount() * sizeof(int));
                        sw.WriteLine("\t\tprivate struct {0}Record", tableDef.m_RecordName);
                        sw.WriteLine("\t\t{");
                        foreach (FieldDef fieldDef in tableDef.m_Fields) {
                            TypeDef typeDef;
                            if (m_Types.TryGetValue(fieldDef.m_Type, out typeDef)) {
                                sw.WriteLine("\t\t\tinternal {0} {1};", GetRecordType(typeDef.m_TypeCode), fieldDef.m_MemberName);
                            } else {
                                Console.WriteLine("Can't find type {0}'s definition ! table {1}", fieldDef.m_Type, table);
                            }
                        }
                        sw.WriteLine("\t\t}");
                        sw.WriteLine();
                        foreach (FieldDef fieldDef in tableDef.m_Fields) {
                            TypeDef typeDef;
                            if (m_Types.TryGetValue(fieldDef.m_Type, out typeDef)) {
                                sw.WriteLine("\t\t{0} {1} {2};", fieldDef.m_Access, typeDef.m_TypeCode, fieldDef.m_MemberName);
                            } else {
                                Console.WriteLine("Can't find type {0}'s definition ! table {1}", fieldDef.m_Type, table);
                            }
                        }
                        sw.WriteLine();                        
                        sw.WriteLine("\t\t{0}", "public bool ReadFromBinary(BinaryTable table, int index)");
                        sw.WriteLine("\t\t{");
                        sw.WriteLine("\t\t\t{0}Record record = GetRecord(table,index);", tableDef.m_RecordName);
                        foreach (FieldDef fieldDef in tableDef.m_Fields) {
                            TypeDef typeDef;
                            if (m_Types.TryGetValue(fieldDef.m_Type, out typeDef)) {
                                sw.WriteLine(IndentCode("\t\t\t", typeDef.m_BinaryCode), fieldDef.m_MemberName, fieldDef.m_Default);
                            } else {
                                Console.WriteLine("Can't find type {0}'s definition ! table {1}", fieldDef.m_Type, table);
                            }
                        }
                        sw.WriteLine("\t\t\treturn true;");
                        sw.WriteLine("\t\t}");
                        sw.WriteLine();
                        sw.WriteLine("\t\tpublic void WriteToBinary(BinaryTable table)");
                        sw.WriteLine("\t\t{");
                        sw.WriteLine("\t\t\t{0}Record record = new {1}Record();", tableDef.m_RecordName, tableDef.m_RecordName);
                        foreach (FieldDef fieldDef in tableDef.m_Fields) {
                            TypeDef typeDef;
                            if (m_Types.TryGetValue(fieldDef.m_Type, out typeDef)) {
                                sw.WriteLine(IndentCode("\t\t\t", typeDef.m_RecordCode), fieldDef.m_MemberName, fieldDef.m_Default);
                            } else {
                                Console.WriteLine("Can't find type {0}'s definition ! table {1}", fieldDef.m_Type, table);
                            }
                        }
                        sw.WriteLine("\t\t\tbyte[] bytes = GetRecordBytes(record);");
                        sw.WriteLine("\t\t\ttable.Records.Add(bytes);");
                        sw.WriteLine("\t\t}");
                        sw.WriteLine();
                        if (0 == tableDef.m_Type.CompareTo("dictionary")) {
                            sw.WriteLine("\t\tpublic {0} GetId()", tableDef.m_KeyFieldTypeCode);
                            sw.WriteLine("\t\t{");
                            sw.WriteLine("\t\t\treturn {0};", tableDef.m_KeyFieldMemberName);
                            sw.WriteLine("\t\t}");
                            sw.WriteLine();
                        }
                        sw.WriteLine("\t\tprivate unsafe {0}Record GetRecord(BinaryTable table, int index)", tableDef.m_RecordName);
                        sw.WriteLine("\t\t{");
                        sw.WriteLine("\t\t\t{0}Record record;", tableDef.m_RecordName);
                        sw.WriteLine("\t\t\tbyte[] bytes = table.Records[index];");
                        sw.WriteLine("\t\t\tfixed (byte* p = bytes) {");
                        sw.WriteLine("\t\t\t\trecord = *({0}Record*)p;", tableDef.m_RecordName);
                        sw.WriteLine("\t\t\t}");
                        sw.WriteLine("\t\t\treturn record;");
                        sw.WriteLine("\t\t}");
                        sw.WriteLine("\t\tprivate static unsafe byte[] GetRecordBytes({0}Record record)", tableDef.m_RecordName);
                        sw.WriteLine("\t\t{");
                        sw.WriteLine("\t\t\tbyte[] bytes = new byte[sizeof({0}Record)];", tableDef.m_RecordName);
                        sw.WriteLine("\t\t\tfixed (byte* p = bytes) {");
                        sw.WriteLine("\t\t\t\t{0}Record* temp = ({1}Record*)p;", tableDef.m_RecordName, tableDef.m_RecordName);
                        sw.WriteLine("\t\t\t\t*temp = record;");
                        sw.WriteLine("\t\t\t}");
                        sw.WriteLine("\t\t\treturn bytes;");
                        sw.WriteLine("\t\t}");
                        sw.WriteLine("\t}");
                        sw.WriteLine();
                        sw.WriteLine("\tpublic sealed{0} class {1}", tableDef.m_ProviderModifier, tableDef.m_ProviderName);
                        sw.WriteLine("\t{");
                        if (tableDef.m_FileType == TableFileTypeEnum.CLIENT || tableDef.m_FileType == TableFileTypeEnum.PUBLIC) {
                            sw.WriteLine("\t\tpublic void LoadForClient()");
                            sw.WriteLine("\t\t{");
                            sw.WriteLine("\t\t\tLoad(FilePathDefine_Client.C_{0});", table);
                            sw.WriteLine("\t\t}");
                        }
                        if (tableDef.m_FileType == TableFileTypeEnum.SERVER || tableDef.m_FileType == TableFileTypeEnum.PUBLIC) {
                            sw.WriteLine("\t\tpublic void LoadForServer()");
                            sw.WriteLine("\t\t{");
                            sw.WriteLine("\t\t\tLoad(FilePathDefine_Server.C_{0});", table);
                            sw.WriteLine("\t\t}");
                        }
                        sw.WriteLine("\t\tpublic void Load(string file)");
                        sw.WriteLine("\t\t{");
                        sw.WriteLine("\t\t\tif (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {");
                        sw.WriteLine("\t\t\t\tm_{0}Mgr.LoadFromBinary(file);", tableDef.m_RecordName);
                        sw.WriteLine("\t\t\t} else {");
                        sw.WriteLine("\t\t\t\tLogSystem.Error(\"{0} is not a table !\");", tableDef.m_RecordName);
                        sw.WriteLine("\t\t\t}");
                        sw.WriteLine("\t\t}");
                        sw.WriteLine("\t\tpublic void Save(string file)");
                        sw.WriteLine("\t\t{");
                        sw.WriteLine("\t\t#if DEBUG");
                        sw.WriteLine("\t\t\tm_{0}Mgr.SaveToBinary(file);", tableDef.m_RecordName);
                        sw.WriteLine("\t\t#endif");
                        sw.WriteLine("\t\t}");
                        sw.WriteLine("\t\tpublic void Clear()");
                        sw.WriteLine("\t\t{");
                        sw.WriteLine("\t\t\tm_{0}Mgr.Clear();", tableDef.m_RecordName);
                        sw.WriteLine("\t\t}");
                        sw.WriteLine();
                        if (0 == tableDef.m_Type.CompareTo("dictionary")) {
                            sw.WriteLine("\t\tpublic {0} {1}Mgr", tableDef.m_ContainerType, tableDef.m_RecordName);
                            sw.WriteLine("\t\t{");
                            sw.WriteLine("\t\t\tget {{ return m_{0}Mgr; }}", tableDef.m_RecordName);
                            sw.WriteLine("\t\t}");
                            sw.WriteLine();
                            sw.WriteLine("\t\tpublic int Get{0}Count()", tableDef.m_RecordName);
                            sw.WriteLine("\t\t{");
                            sw.WriteLine("\t\t\treturn m_{0}Mgr.GetDataCount();", tableDef.m_RecordName);
                            sw.WriteLine("\t\t}");
                            sw.WriteLine();
                            sw.WriteLine("\t\tpublic {0} Get{1}({2} id)", tableDef.m_RecordName, tableDef.m_RecordName, tableDef.m_KeyFieldTypeCode);
                            sw.WriteLine("\t\t{");
                            sw.WriteLine("\t\t\treturn m_{0}Mgr.GetDataById(id);", tableDef.m_RecordName);
                            sw.WriteLine("\t\t}");
                            sw.WriteLine();
                            sw.WriteLine("\t\tprivate {0} m_{1}Mgr = new {2}();", tableDef.m_ContainerType, tableDef.m_RecordName, tableDef.m_ContainerType);
                        } else {
                            sw.WriteLine("\t\tpublic {0} {1}Mgr", tableDef.m_ContainerType, tableDef.m_RecordName);
                            sw.WriteLine("\t\t{");
                            sw.WriteLine("\t\t\tget {{ return m_{0}Mgr; }}", tableDef.m_RecordName);
                            sw.WriteLine("\t\t}");
                            sw.WriteLine();
                            sw.WriteLine("\t\tpublic int Get{0}Count()", tableDef.m_RecordName);
                            sw.WriteLine("\t\t{");
                            sw.WriteLine("\t\t\treturn m_{0}Mgr.GetDataCount();", tableDef.m_RecordName);
                            sw.WriteLine("\t\t}");
                            sw.WriteLine();
                            sw.WriteLine("\t\tprivate {0} m_{1}Mgr = new {2}();", tableDef.m_ContainerType, tableDef.m_RecordName, tableDef.m_ContainerType);
                        }
                        sw.WriteLine();
                        if (tableDef.m_FileType != TableFileTypeEnum.MULTIFILE) {
                            sw.WriteLine("\t\tpublic static {0} Instance", tableDef.m_ProviderName);
                            sw.WriteLine("\t\t{");
                            sw.WriteLine("\t\t\tget { return s_Instance; }");
                            sw.WriteLine("\t\t}");
                            sw.WriteLine("\t\tprivate static {0} s_Instance = new {1}();", tableDef.m_ProviderName, tableDef.m_ProviderName);
                        }
                        sw.WriteLine("\t}");
                        sw.WriteLine("}");
                        sw.Close();
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                }
            } else {
                Console.WriteLine("Can't find table {0}'s definition !", table);
            }
        }

        private string IndentCode(string indent, string code)
        {
            string[] lines = code.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int ix = 0; ix < lines.Length; ++ix) {
                lines[ix] = string.Format("{0}{1}", indent, lines[ix]);
            }
            return string.Join("\r\n", lines);
        }
        private string GetRecordType(string type)
        {
            if (0 == type.CompareTo("int") ||
              0 == type.CompareTo("float")) {
                return type;
            } else {
                return "int";
            }
        }

        private const string m_DefGlobalCsFileName = "Global";
        private const string m_DefToolCsFileName = "Tool";
        private SortedDictionary<string, List<string>> m_GlobalCodes = new SortedDictionary<string, List<string>>();
        private SortedDictionary<string, List<string>> m_ToolCodes = new SortedDictionary<string, List<string>>();
        private SortedDictionary<string, TypeDef> m_Types = new SortedDictionary<string, TypeDef>();
        private SortedDictionary<string, TableDef> m_Tables = new SortedDictionary<string, TableDef>();
    }
    internal class TableReaderGenerator
    {
        public static void Generate(bool isGenerateCode)
        {
            ResourceReadProxy.OnReadAsArray = (string path) => {
                byte[] buffer = null;
                try {
                    buffer = File.ReadAllBytes(path);
                } catch {
                }
                return buffer;
            };
            if (isGenerateCode) {
                GenAllDsl();
                GenDataReader();

                GenAllProto();
                //GenTableConfig();

                System.Diagnostics.Process p = System.Diagnostics.Process.Start("cmd", "/c move /y *.cs ../../TableReader/");
                p.WaitForExit();

                Console.WriteLine("TableReader generated, please recompile Test project.");
            } else {
                Convert();
            }
        }

        private static void GenDataReader()
        {
            TableDslParser parser = new TableDslParser();
            parser.Init("table.dsl");
            parser.GenAllReaders(GetAllTableFiles());
        }
        private static Dictionary<string, string> GetAllTableFiles()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            try {
                string basePath = "../TestData";
                string[] files = Directory.GetFiles(basePath, "*.txt", SearchOption.AllDirectories);
                foreach (string file in files) {
                    string s = file.Substring(basePath.Length + 1);
                    s = s.Replace("\\", "/");
                    string filename = Path.GetFileNameWithoutExtension(s);
                    if (dict.ContainsKey(filename)) {
                        Console.WriteLine("[Error] file name duplication, fileName: " + filename);
                    }
                    dict.Add(filename, s);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
            return dict;
        }

        private static void GenAllDsl()
        {
            try {
                File.Delete("table.dsl");
                string basePath = "../TestData";
                string[] files = Directory.GetFiles(basePath, "*.txt", SearchOption.AllDirectories);
                foreach (string file in files) {
                    GenDsl(file);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }
        private static void GenDsl(string file)
        {
            string[] lines = File.ReadAllLines(file);
            if (lines.Length >= 2) {
                string[] types = lines[0].Split('\t');
                string[] fields = lines[1].Split('\t');

                string dirName = Path.GetDirectoryName(file);
                string fileName = Path.GetFileName(file);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                string fileType;
                if (dirName.IndexOf("Scenes") >= 0)
                    fileType = "multifile";
                else if (dirName.IndexOf("Client") >= 0)
                    fileType = "client";
                else if (dirName.IndexOf("Server") >= 0)
                    fileType = "server";
                else
                    fileType = "public";

                string tableType = "dictionary";
                if (s_ListTables.Contains(fileName)) {
                    tableType = "list";
                }

                using (StreamWriter sw = new StreamWriter("table.dsl", true)) {
                    sw.WriteLine("tabledef({0}, {1}, {2})", fileNameWithoutExtension, tableType, fileType);
                    sw.WriteLine("{");
                    sw.WriteLine("\trecordmodifier(partial);");
                    sw.WriteLine("\tprovidermodifier(partial);");

                    int ct = fields.Length;
                    for (int ix = 0; ix < ct; ++ix) {
                        if (string.IsNullOrEmpty(fields[ix]) || string.IsNullOrEmpty(types[ix]))
                            continue;
                        string type = types[ix].Trim();
                        if (type.EndsWith("[]")) {
                            sw.WriteLine("\tfielddef({0}, {1}, {2}_list);", fields[ix], fields[ix], type.Substring(0, type.Length - 2));
                        } else {
                            sw.WriteLine("\tfielddef({0}, {1}, {2});", fields[ix], fields[ix], type);
                        }
                    }

                    sw.WriteLine("};");
                    sw.Close();
                }
            }
        }

        private static void GenAllProto()
        {
            try {
                File.Delete("table.proto");

                using (StreamWriter sw = new StreamWriter("table.proto", true)) {
                    sw.WriteLine("package TableConfig;");
                    sw.WriteLine();
                }
                string basePath = "../TestData";
                string[] files = Directory.GetFiles(basePath, "*.txt", SearchOption.AllDirectories);
                foreach (string file in files) {
                    GenProto(file);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }
        private static void GenProto(string file)
        {
            string[] lines = File.ReadAllLines(file);
            if (lines.Length >= 2) {
                string[] types = lines[0].Split('\t');
                string[] fields = lines[1].Split('\t');

                string dirName = Path.GetDirectoryName(file);
                string fileName = Path.GetFileName(file);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                
                using (StreamWriter sw = new StreamWriter("table.proto", true)) {
                    sw.WriteLine("message {0}List", fileNameWithoutExtension);
                    sw.WriteLine("{");
                    sw.WriteLine("\trepeated {0} items = 1;", fileNameWithoutExtension);
                    sw.WriteLine("}");
                    sw.WriteLine("message {0}", fileNameWithoutExtension);
                    sw.WriteLine("{");
                    int ct = fields.Length;
                    int index = 1;
                    for (int ix = 0; ix < ct; ++ix) {
                        if (string.IsNullOrEmpty(fields[ix]) || string.IsNullOrEmpty(types[ix]))
                            continue;
                        string type = types[ix].Trim();
                        if (type.EndsWith("[]")) {
                            sw.WriteLine("\trepeated {0} {1} = {2};", type.Substring(0, type.Length - 2), fields[ix], index);
                        } else {
                            //为与python版本一致，目前字段都使用required，改为使用optional可以省很多空间
                            sw.WriteLine("\trequired {0} {1} = {2};", type, fields[ix], index);
                        }
                        ++index;
                    }

                    sw.WriteLine("}");
                    sw.Close();
                }
            }
        }
        
        private static void Convert()
        {
            string path = HomePath.GetAbsolutePath("TableConfig.dll");
            Assembly assembly = Assembly.LoadFile(path);
            Encoding encoding = Encoding.GetEncoding(936);
            string[] files = Directory.GetFiles("../TestData", "*.txt", SearchOption.AllDirectories);
            foreach (string file in files) {
                string outFile = Path.ChangeExtension(file, "dat");
                string outFile2 = Path.ChangeExtension(file, "data");
                Txt2Binary.Convert(file, outFile, encoding);
                ConvertToProtoData(file, outFile2, encoding, assembly);
            }
        }

        private static bool ConvertToProtoData(string file, string outFile, Encoding encoding, Assembly assembly)
        {
            try {
                string filename = Path.GetFileNameWithoutExtension(file);
                string listClassName = string.Format("TableConfig.{0}List", filename);
                string className = string.Format("TableConfig.{0}", filename);

                string[] lines = File.ReadAllLines(file, encoding);
                if (lines.Length >= 2) {
                    string[] types = lines[0].Split('\t');
                    string[] fields = lines[1].Split('\t');

                    string dirName = Path.GetDirectoryName(file);
                    string fileName = Path.GetFileName(file);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                    int fieldCount = 0;
                    int excelColumnCount = types.Length;
                    if (fields.Length != excelColumnCount) {
                        LogSystem.Error("[line:2] “{0}” field count != {1}", lines[1], excelColumnCount);
                        return false;
                    }
                    for (int ix = 0; ix < excelColumnCount; ++ix) {
                        if (string.IsNullOrEmpty(types[ix]) || string.IsNullOrEmpty(fields[ix]))
                            continue;
                        ++fieldCount;
                    }
                    object list = assembly.CreateInstance(listClassName);
                    Type listClassType = list.GetType();
                    Type skillClassType = assembly.GetType(className);
                    for (int rowIndex = 2; rowIndex < lines.Length; ++rowIndex) {
                        if (lines[rowIndex].StartsWith("#") || lines[rowIndex].StartsWith("//"))
                            continue;
                        int colIndex = 0;
                        string[] fieldValues = lines[rowIndex].Split('\t');
                        if (fieldValues.Length != excelColumnCount) {
                            LogSystem.Error("[line:{0}] “{1}” field count != {2}", rowIndex + 1, lines[rowIndex], excelColumnCount);
                            continue;
                        }
                        object obj = assembly.CreateInstance(className);
                        ListPropertyAdd(listClassType, list, "items", obj);
                        for (int ix = 0; ix < excelColumnCount; ++ix) {
                            if (string.IsNullOrEmpty(fields[ix]) || string.IsNullOrEmpty(types[ix]))
                                continue;
                            string field=fields[ix].Trim();
                            string type = types[ix].Trim();
                            string val = fieldValues[ix].Trim();
                            try {
                                if (0 == type.CompareTo("int") || 0 == type.CompareTo("int32") || 0 == type.CompareTo("long") || 0 == type.CompareTo("int64")) {
                                    int v = 0;
                                    if (!string.IsNullOrEmpty(val)) {
                                        v = int.Parse(val);
                                    }
                                    PropertySet(skillClassType, obj, field, v);
                                } else if (0 == type.CompareTo("float")) {
                                    float v = 0;
                                    if (!string.IsNullOrEmpty(val)) {
                                        v = float.Parse(val);
                                    }
                                    PropertySet(skillClassType, obj, field, v);
                                } else if (0 == type.CompareTo("bool")) {
                                    bool v = false;
                                    if (!string.IsNullOrEmpty(val)) {
                                        v = (val == "true" || val == "1");
                                    }
                                    PropertySet(skillClassType, obj, field, v);
                                } else if (0 == type.CompareTo("string")) {
                                    PropertySet(skillClassType, obj, field, val);
                                } else if (0 == type.CompareTo("int[]") || 0 == type.CompareTo("int32[]") || 0 == type.CompareTo("long[]") || 0 == type.CompareTo("int64[]")) {
                                    if (!string.IsNullOrEmpty(val)) {
                                        string[] v = val.Split(',', ';', '|', ' ');
                                        int[] vals = new int[v.Length];
                                        for (int i = 0; i < v.Length; ++i) {
                                            vals[i] = int.Parse(v[i]);
                                        }
                                        ListPropertySet(skillClassType, obj, field, vals);
                                    }
                                } else if (0 == type.CompareTo("float[]")) {
                                    if (!string.IsNullOrEmpty(val)) {
                                        string[] v = val.Split(',', ';', '|', ' ');
                                        float[] vals = new float[v.Length];
                                        for (int i = 0; i < v.Length; ++i) {
                                            vals[i] = float.Parse(v[i]);
                                        }
                                        ListPropertySet(skillClassType, obj, field, vals);
                                    }
                                } else if (0 == type.CompareTo("bool[]")) {
                                    if (!string.IsNullOrEmpty(val)) {
                                        string[] v = val.Split(',', ';', '|', ' ');
                                        bool[] vals = new bool[v.Length];
                                        for (int i = 0; i < v.Length; ++i) {
                                            vals[i] = (v[i] == "true" || v[i] == "1");
                                        }
                                        ListPropertySet(skillClassType, obj, field, vals);
                                    }
                                } else if (0 == type.CompareTo("string[]")) {
                                    if (!string.IsNullOrEmpty(val)) {
                                        string[] vals = val.Split(',', ';', '|', ' ');
                                        ListPropertySet(skillClassType, obj, field, vals);
                                    }
                                }
                            } catch (Exception ex) {
                                LogSystem.Error("[line:{0} col:{1}] “{2}”, exception:{3}\n{4}", rowIndex + 1, colIndex + 1, lines[rowIndex], ex.Message, ex.StackTrace);
                            }
                            ++colIndex;
                        }
                    }
                    MemoryStream ms=new MemoryStream();
                    s_Serializer.Serialize(ms, list);
                    File.WriteAllBytes(outFile, ms.ToArray());
                }
                return true;
            } catch (Exception ex) {
                LogSystem.Error("exception:{0}\n{1}", ex.Message, ex.StackTrace);
                return false;
            }
        }
        private static void ListPropertyAdd(Type t, Object obj, string name, object val)
        {
            try {
                object listObj = t.InvokeMember(name, System.Reflection.BindingFlags.GetProperty, null, obj, null);
                Type listType = listObj.GetType();
                listType.InvokeMember("Add", System.Reflection.BindingFlags.InvokeMethod, null, listObj, new object[] { val });
            } catch (Exception ex) {
                LogSystem.Error("SetListProperty {0}={1} exception:{2}\n{3}", name, val.ToString(), ex.Message, ex.StackTrace);
            }
        }
        private static void PropertySet(Type t, Object obj, string name, object val)
        {
            try {
                t.InvokeMember(name, System.Reflection.BindingFlags.SetProperty, null, obj, new object[] { val });
            } catch (Exception ex) {
                LogSystem.Error("SetProperty {0}={1} exception:{2}\n{3}", name, val.ToString(), ex.Message, ex.StackTrace);
            }
        }
        private static void ListPropertySet(Type t, Object obj, string name, object val)
        {
            try {
                object listObj = t.InvokeMember(name, System.Reflection.BindingFlags.GetProperty, null, obj, null);
                Type listType = listObj.GetType();
                listType.InvokeMember("AddRange", System.Reflection.BindingFlags.InvokeMethod, null, listObj, new object[] { val });
            } catch (Exception ex) {
                LogSystem.Error("SetListProperty {0}={1} exception:{2}\n{3}", name, val.ToString(), ex.Message, ex.StackTrace);
            }
        }

        private static readonly HashSet<string> s_ListTables = new HashSet<string> { };
        private static TableConfigSerializer s_Serializer = new TableConfigSerializer();
    }
}
