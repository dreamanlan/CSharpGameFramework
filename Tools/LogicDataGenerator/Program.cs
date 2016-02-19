using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using Dsl;

namespace LogicDataGenerator
{
  internal class TypeConverterDef
  {
    internal string m_LogicType;
    internal string m_MessageType;
    internal string m_Message2LogicCode;
    internal string m_Logic2MessageCode;
    internal bool m_IsSpecial = false;
    internal string m_CrudCode = string.Empty;
  }
  internal class EnumTypeDef
  {
    internal string m_EnumName;
    internal Dictionary<string, int> m_EnumMembers = new Dictionary<string, int>();
    internal string m_GroupName = string.Empty;
  }
  internal class MemberDef
  {
    internal string m_MemberName;
    internal string m_TypeName;
    internal string m_Modifier;
    internal int m_Order;
    internal string m_Default;
    internal int m_MaxSize;
    internal bool m_IsPrimaryKey;
    internal bool m_IsForeignKey;
  }
  internal class MessageDef
  {
    internal string m_TypeName;
    internal string m_WrapName;
    internal List<MemberDef> m_Members = new List<MemberDef>();
    internal List<string> m_PrimaryKeys = new List<string>();
    internal List<string> m_ForeignKeys = new List<string>();
    internal List<string> m_ColumnNames = new List<string>();
    internal string m_GroupName = string.Empty;
    internal string m_EnumValue = string.Empty;
    internal bool m_DontGenEnum = false;
    internal bool m_DontGenDB = false;
    internal MemberDef GetMemberDef(string memberName)
    {
      MemberDef ret = null;
      foreach (var memberDef in m_Members) {
        if (memberDef.m_MemberName.Equals(memberName)) {
          ret = memberDef;
        }
      }
      return ret;
    }

    internal SortedDictionary<string, EnumTypeDef> m_EnumTypes = new SortedDictionary<string, EnumTypeDef>();
    internal SortedDictionary<string, MessageDef> m_Messages = new SortedDictionary<string, MessageDef>();
  }
  internal class MessageDslParser
  {
    internal string DslFile
    {
      get { return m_DslFile; }
    }
    public List<string> Explanation
    {
      get { return m_Explanation; }
    }
    public string Version
    {
      get { return m_Version; }
    }
    public string Package
    {
      get { return m_Package; }
    }
    public List<string> SortedMessageEnums
    {
      get { return m_SortedMessageEnums; }
    }
    internal SortedDictionary<string, EnumTypeDef> TopmostEnumTypes
    {
      get { return m_EnumTypes; }
    }
    internal SortedDictionary<string, MessageDef> TopmostMessages
    {
      get { return m_Messages; }
    }
    internal bool Init(string dslFile)
    {
      m_DslFile = dslFile;
      try {
        DslFile file = new DslFile();
        if (!file.Load(dslFile, LogSystem.Log)) {
          LogSystem.Error("DSL {0} load failed !", dslFile);
          return false;
        }
        bool haveError = false;
        foreach (DslInfo info in file.DslInfos) {
          if (info.GetId() == "explanationfile") {
            if (info.Functions.Count == 1) {
              FunctionData funcData = info.First;
              CallData call = funcData.Call;
              if (null != call && call.GetParamNum() >= 1) {
                string commentFile = call.GetParamId(0);
                string path = Path.Combine(Path.GetDirectoryName(m_DslFile), commentFile);
                try {
                  string[] lines = File.ReadAllLines(path);
                  m_Explanation.AddRange(lines);
                } catch(Exception fileEx) {
                  LogSystem.Error("File.ReadAllLines({0}) throw exception {1}, line {2} file {3}", fileEx.Message, path, info.GetLine(), dslFile);
                  haveError = true;
                }
              } else {
                LogSystem.Error("explanationfile must have param file , line {0} file {1}", info.GetLine(), dslFile);
                haveError = true;
              }
            } else {
              LogSystem.Error("explanationfile must end with ';' , line {0} file {1}", info.GetLine(), dslFile);
              haveError = true;
            }
          } else if (info.GetId() == "explanation") {
            if (info.Functions.Count == 1) {
              FunctionData funcData = info.First;
              CallData call = funcData.Call;
              if (null != call && call.GetParamNum() >= 1) {
                string line = call.GetParamId(0);
                m_Explanation.Add(line);
              } else {
                LogSystem.Error("explanation must have param explanation , line {0} file {1}", info.GetLine(), dslFile);
                haveError = true;
              }
            } else {
              LogSystem.Error("explanation must end with ';' , line {0} file {1}", info.GetLine(), dslFile);
              haveError = true;
            }
          } else if (info.GetId() == "version") {
            if (info.Functions.Count == 1) {
              FunctionData funcData = info.First;
              CallData call = funcData.Call;
              if (null != call && call.GetParamNum() >= 1) {
                m_Version = call.GetParamId(0);
              } else {
                LogSystem.Error("version must have param version , line {0} file {1}", info.GetLine(), dslFile);
                haveError = true;
              }
            } else {
              LogSystem.Error("version must end with ';' , line {0} file {1}", info.GetLine(), dslFile);
              haveError = true;
            }
          } else if (info.GetId() == "package") {
            if (info.Functions.Count == 1) {
              FunctionData funcData = info.First;
              CallData call = funcData.Call;
              if (null != call && call.GetParamNum() >= 1) {
                m_Package = call.GetParamId(0);
              } else {
                LogSystem.Error("package must have param package , line {0} file {1}", info.GetLine(), dslFile);
                haveError = true;
              }
            } else {
              LogSystem.Error("package must end with ';' , line {0} file {1}", info.GetLine(), dslFile);
              haveError = true;
            }
          } else if (info.GetId() == "defaultsize") {
            if (info.Functions.Count == 1) {
              FunctionData funcData = info.First;
              CallData call = funcData.Call;
              if (null != call && call.GetParamNum() >= 1) {
                m_DefVarcharSize = int.Parse(call.GetParamId(0));
              } else {
                LogSystem.Error("defaultsize must have param size , line {0} file {1}", info.GetLine(), dslFile);
                haveError = true;
              }
            } else {
              LogSystem.Error("defaultsize must end with ';' , line {0} file {1}", info.GetLine(), dslFile);
              haveError = true;
            }
          } else if (info.GetId() == "import") {
            if (info.Functions.Count == 1) {
              FunctionData funcData = info.First;
              CallData call = funcData.Call;
              if (null != call && call.GetParamNum() >= 1) {
                string importDslFile = call.GetParamId(0);
                if (string.IsNullOrEmpty(importDslFile)) {
                  LogSystem.Error("import must have param dslfile, line {0} file {1}", info.GetLine(), dslFile);
                  haveError = true;
                } else {
                  string filePath = Path.Combine(Path.GetDirectoryName(dslFile), importDslFile);
                  MessageDslParser parser = new MessageDslParser();
                  if (parser.Init(filePath)) {
                    if (m_Explanation.Count <= 0 && parser.m_Explanation.Count > 0) {
                      m_Explanation.AddRange(parser.m_Explanation);
                    }
                    if (string.IsNullOrEmpty(m_Version) && !string.IsNullOrEmpty(parser.m_Version)) {
                      m_Version = parser.m_Version;
                    }
                    if (string.IsNullOrEmpty(m_Package) && !string.IsNullOrEmpty(parser.m_Package)) {
                      m_Package = parser.m_Package;
                    }
                    if (m_DefVarcharSize == c_DefVarcharSize && parser.m_DefVarcharSize != c_DefVarcharSize) {
                      m_DefVarcharSize = parser.m_DefVarcharSize;
                    }
                    foreach (var pair in parser.m_TypeConverters) {
                      m_TypeConverters.Add(pair.Key, pair.Value);
                    }
                    foreach (var pair in parser.m_EnumTypes) {
                      m_EnumTypes.Add(pair.Key, pair.Value);
                    }
                    foreach (var pair in parser.m_Messages) {
                      m_Messages.Add(pair.Key, pair.Value);
                    }
                    foreach (string name in parser.m_SortedMessageEnums) {
                      m_SortedMessageEnums.Add(name);
                    }
                  }
                }
              }
            } else {
              LogSystem.Error("import must end with ';' , line {0} file {1}", info.GetLine(), dslFile);
              haveError = true;
            }
          } else if (info.GetId() == "typeconverter") {
            if (info.Functions.Count == 1) {
              FunctionData funcData = info.First;
              if (null != funcData) {
                CallData callData = funcData.Call;
                if (null != callData && callData.GetParamNum() >= 1) {
                  string logicTypeName = callData.GetParamId(0);

                  TypeConverterDef typeConverter = new TypeConverterDef();
                  typeConverter.m_LogicType = logicTypeName;
                  typeConverter.m_IsSpecial = false;
                  if (m_TypeConverters.ContainsKey(logicTypeName)) {
                    m_TypeConverters[logicTypeName] = typeConverter;
                  } else {
                    m_TypeConverters.Add(logicTypeName, typeConverter);
                  }

                  foreach (ISyntaxComponent comp in funcData.Statements) {
                    FunctionData item = comp as FunctionData;
                    if (null != item) {
                      if (item.HaveExternScript()) {
                        if (item.GetId() == "message2logic") {
                          typeConverter.m_Message2LogicCode = item.GetExternScript();
                        } else if (item.GetId() == "logic2message") {
                          typeConverter.m_Logic2MessageCode = item.GetExternScript();
                        } else if (item.GetId() == "crudcode") {
                          typeConverter.m_IsSpecial = true;
                          typeConverter.m_CrudCode = item.GetExternScript();
                        }
                      } else {
                        LogSystem.Error("typeconverter {0} member must contains code ! line {1} file {2}", comp.ToScriptString(), comp.GetLine(), dslFile);
                        haveError = true;
                      }
                    } else {
                      CallData converterData = comp as CallData;
                      if (null != converterData && converterData.GetParamNum() == 1) {
                        string id = converterData.GetId();
                        string param = converterData.GetParamId(0);
                        if (id == "messagetype") {
                          typeConverter.m_MessageType = param;
                        }
                      } else {
                        if (comp is ValueData || null != converterData) {
                          LogSystem.Error("typeconverter {0} member must have params ! line {1} file {2}", comp.ToScriptString(), comp.GetLine(), dslFile);
                          haveError = true;
                        } else {
                          LogSystem.Error("typeconverter {0} member must end with ';' ! line {1} file {2}", comp.ToScriptString(), comp.GetLine(), dslFile);
                          haveError = true;
                        }
                      }
                    }
                  }
                  if (string.IsNullOrEmpty(typeConverter.m_LogicType) ||
                    string.IsNullOrEmpty(typeConverter.m_MessageType) ||
                    string.IsNullOrEmpty(typeConverter.m_Message2LogicCode) ||
                    string.IsNullOrEmpty(typeConverter.m_Logic2MessageCode)) {
                    LogSystem.Error("typeconverter {0} error, logictype or messagetype or message2logic or logic2message is null or empty ! line {1} file {2}", funcData.ToScriptString(), funcData.GetLine(), dslFile);
                    haveError = true;
                  }
                  if (typeConverter.m_IsSpecial && string.IsNullOrEmpty(typeConverter.m_CrudCode)) {
                    LogSystem.Error("typeconverter {0} error, special type must have crudcode ! line {1} file {2}", funcData.ToScriptString(), funcData.GetLine(), dslFile);
                    haveError = true;
                  }
                } else {
                  LogSystem.Error("typeconverter {0} must have 1 params ! line {1} file {2}", funcData.ToScriptString(), funcData.GetLine(), dslFile);
                  haveError = true;
                }
              } else {
                LogSystem.Error("typeconverter {0} must have function body ! line {1} file {2}", info.ToScriptString(), info.GetLine(), dslFile);
                haveError = true;
              }
            } else {
              LogSystem.Error("typeconverter {0} must end with ';' ! line {1} file {2}", info.ToScriptString(), info.GetLine(), dslFile);
              haveError = true;
            }
          } else if (info.GetId() == "enum") {
            if (info.Functions.Count == 1) {
              FunctionData funcData = info.First;
              ParseEnum(dslFile, funcData, string.Empty, m_EnumTypes, ref haveError);
            } else {
              LogSystem.Error("enum must end with ';' , line {0} file {1}", info.GetLine(), dslFile);
              haveError = true;
            }
          } else if (info.GetId() == "message") {
            if (info.Functions.Count == 1) {
              FunctionData funcData = info.First;
              string typeName = ParseMessage(dslFile, funcData, string.Empty, false, m_Messages, ref haveError);
              if (!string.IsNullOrEmpty(typeName) && !haveError) {
                m_SortedMessageEnums.Add(typeName);
              }
            } else {
              LogSystem.Error("message must end with ';' , line {0} file {1}", info.GetLine(), dslFile);
              haveError = true;
            }
          } else {
            LogSystem.Error("Unknown part {0}, line {1} file {2}", info.GetId(), info.GetLine(), dslFile);
            haveError = true;
          }
        }
        return !haveError;
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }
      return false;
    }
    internal EnumTypeDef FindTopmostEnumTypeDef(string enumName)
    {
      EnumTypeDef ret;
      m_EnumTypes.TryGetValue(enumName, out ret);
      return ret;
    }
    internal MessageDef FindTopmostMessageDef(string messageName)
    {
      MessageDef ret;
      m_Messages.TryGetValue(messageName, out ret);
      return ret;
    }
    internal EnumTypeDef FindEnumTypeDef(string enumName)
    {
      EnumTypeDef ret;
      if (!m_EnumTypes.TryGetValue(enumName, out ret)) {
        foreach (var pair in m_Messages) {
          if (FindEnumTypeDef(pair.Value, enumName, out ret))
            break;
        }
      }
      return ret;
    }
    internal MessageDef FindMessageDef(string messageName)
    {
      MessageDef ret;
      if (!m_Messages.TryGetValue(messageName, out ret)) {
        foreach (var pair in m_Messages) {
          if (FindMessageDef(pair.Value, messageName, out ret))
            break;
        }
      }
      return ret;
    }
    internal void GenAllMessageWraps(string file, params string[] groups)
    {
      HashSet<string> expectGroups = new HashSet<string>(groups);
      if (expectGroups.Count <= 0) {
        expectGroups.Add(string.Empty);
      }
      try {
        using (StreamWriter sw = new StreamWriter(file, true)) {
          sw.WriteLine("//----------------------------------------------------------------------------");
          sw.WriteLine("//！！！不要手动修改此文件，此文件由LogicDataGenerator按{0}生成！！！", m_DslFile);
          sw.WriteLine("//----------------------------------------------------------------------------");
          sw.WriteLine("using System;");
          sw.WriteLine("using System.Collections.Generic;");
          sw.WriteLine("using System.Runtime.InteropServices;");
          sw.WriteLine("using System.IO;");
          sw.WriteLine("using System.Text;");
          if (!string.IsNullOrEmpty(m_Package)) {
            sw.WriteLine("using {0};", m_Package);
          }
          sw.WriteLine("using GameFramework;");

          foreach (var messagePair in m_Messages) {
            string message = messagePair.Key;
            MessageDef messageDef = messagePair.Value;
            if (!messageDef.m_DontGenEnum && !messageDef.m_DontGenDB && expectGroups.Contains(messageDef.m_GroupName)) {
              GenMessageWrap(messageDef, sw);
            }
          }

          sw.Close();
        }
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }
    }
    private void GenMessageWrap(MessageDef messageDef, TextWriter sw)
    {
      if (messageDef.m_EnumTypes.Count > 0 || messageDef.m_Messages.Count > 0) {
        Console.WriteLine("[*** Warning ***] message {0} have inner enum or message, only simple message can wrap !!!", messageDef.m_TypeName);
      }
      try {
        sw.WriteLine();
        sw.WriteLine("namespace GameFramework");
        sw.WriteLine("{");
        sw.WriteLine("\tpublic sealed partial class {0}", messageDef.m_WrapName);
        sw.WriteLine("\t{");
        sw.WriteLine();
        sw.WriteLine("\t\tpublic bool Modified");
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tget{ return m_Modified;}");
        sw.WriteLine("\t\t\tset{ m_Modified = value;}");
        sw.WriteLine("\t\t}");
        sw.WriteLine("\t\tpublic List<string> PrimaryKeys");
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tget{ return m_PrimaryKeys;}");
        sw.WriteLine("\t\t}");
        sw.WriteLine("\t\tpublic List<string> ForeignKeys");
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tget{ return m_ForeignKeys;}");
        sw.WriteLine("\t\t}");
        sw.WriteLine();
        foreach (MemberDef memberDef in messageDef.m_Members) {
          if (null != FindMessageDef(memberDef.m_TypeName)) {
            Console.WriteLine("[*** Warning ***] message {0} use other message, only simple message can wrap !!!", messageDef.m_TypeName);
          }
          string callOnUpdated = "OnFieldUpdated()";
          bool isSpecial = false;
          TypeConverterDef converter;
          if (m_TypeConverters.TryGetValue(memberDef.m_TypeName, out converter)) {
            if (memberDef.m_IsPrimaryKey || memberDef.m_IsForeignKey) {
              callOnUpdated = string.Format("On{0}Updated()", memberDef.m_MemberName);
            }
            isSpecial = converter.m_IsSpecial;
            if (isSpecial) {
              sw.WriteLine(IndentCode("\t\t", converter.m_CrudCode), memberDef.m_MemberName, callOnUpdated);
            } else {
              sw.WriteLine("\t\tpublic {0} {1}", memberDef.m_TypeName, memberDef.m_MemberName);
              sw.WriteLine("\t\t{");
              sw.WriteLine("\t\t\tget{{return m_{0};}}", memberDef.m_MemberName);
              sw.WriteLine("\t\t\tset");
              sw.WriteLine("\t\t\t{");
              sw.WriteLine("\t\t\t\tm_{0} = value;", memberDef.m_MemberName);
              sw.WriteLine("\t\t\t\t{0};", callOnUpdated);
              sw.WriteLine("\t\t\t}");
              sw.WriteLine("\t\t}");
            }
          } else {                   
            sw.WriteLine("\t\tpublic {0} {1}", memberDef.m_TypeName, memberDef.m_MemberName);
            sw.WriteLine("\t\t{");
            sw.WriteLine("\t\t\tget{{return m_{0}.{1};}}", messageDef.m_TypeName, memberDef.m_MemberName);
            sw.WriteLine("\t\t\tset");
            sw.WriteLine("\t\t\t{");
            sw.WriteLine("\t\t\t\tm_{0}.{1} = value;", messageDef.m_TypeName, memberDef.m_MemberName);
            if (!(memberDef.m_IsPrimaryKey || memberDef.m_IsForeignKey)) {
              sw.WriteLine("\t\t\t\t{0};", callOnUpdated);
            } else {
              if (memberDef.m_IsPrimaryKey) {
                sw.WriteLine("\t\t\t\t{0};", "OnPrimaryKeyUpdated()");
              }
              if (memberDef.m_IsForeignKey) {
                sw.WriteLine("\t\t\t\t{0};", "OnForeignKeyUpdated()");
              }
            }           
            sw.WriteLine("\t\t\t}");
            sw.WriteLine("\t\t}");
          }
        }
        sw.WriteLine();
        sw.WriteLine("\t\tpublic {0} ToProto()", messageDef.m_TypeName);
        sw.WriteLine("\t\t{");

        foreach (MemberDef memberDef in messageDef.m_Members) {
          TypeConverterDef converter;
          if (m_TypeConverters.TryGetValue(memberDef.m_TypeName, out converter)) {
            sw.WriteLine(IndentCode("\t\t\t", converter.m_Logic2MessageCode), messageDef.m_TypeName, memberDef.m_MemberName);
          }
        }

        sw.WriteLine("\t\t\treturn m_{0};", messageDef.m_TypeName);
        sw.WriteLine("\t\t}");
        sw.WriteLine("\t\tpublic void FromProto({0} proto)", messageDef.m_TypeName);
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tm_{0} = proto;", messageDef.m_TypeName);
        sw.WriteLine("\t\t\tUpdatePrimaryKeys();");
        sw.WriteLine("\t\t\tUpdateForeignKeys();");

        foreach (MemberDef memberDef in messageDef.m_Members) {
          TypeConverterDef converter;
          if (m_TypeConverters.TryGetValue(memberDef.m_TypeName, out converter)) {
            sw.WriteLine(IndentCode("\t\t\t", converter.m_Message2LogicCode), messageDef.m_TypeName, memberDef.m_MemberName);
          }
        }

        sw.WriteLine("\t\t}");

        foreach (MemberDef memberDef in messageDef.m_Members) {
          TypeConverterDef converter;
          if (m_TypeConverters.TryGetValue(memberDef.m_TypeName, out converter)) {
            if (memberDef.m_IsPrimaryKey || memberDef.m_IsForeignKey) {
              sw.WriteLine();
              sw.WriteLine("\t\tprivate void On{0}Updated()", memberDef.m_MemberName);
              sw.WriteLine("\t\t{");
              sw.WriteLine("\t\t\tm_Modified = true;");
              if (memberDef.m_IsPrimaryKey) {
                sw.WriteLine(IndentCode("\t\t\t", converter.m_Logic2MessageCode), messageDef.m_TypeName, memberDef.m_MemberName);
                sw.WriteLine("\t\t\tUpdatePrimaryKeys();");
              } else if (memberDef.m_IsForeignKey) {
                sw.WriteLine(IndentCode("\t\t\t", converter.m_Logic2MessageCode), messageDef.m_TypeName, memberDef.m_MemberName);
                sw.WriteLine("\t\t\tUpdateForeignKeys();");
              }
              sw.WriteLine("\t\t}");
            }
          }
        }

        sw.WriteLine();
        sw.WriteLine("\t\tprivate void OnFieldUpdated()");
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tm_Modified = true;");
        sw.WriteLine("\t\t}");

        sw.WriteLine();
        sw.WriteLine("\t\tprivate void OnPrimaryKeyUpdated()");
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tm_Modified = true;");
        sw.WriteLine("\t\t\tUpdatePrimaryKeys();");
        sw.WriteLine("\t\t}");

        sw.WriteLine();
        sw.WriteLine("\t\tprivate void OnForeignKeyUpdated()");
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tm_Modified = true;");
        sw.WriteLine("\t\t\tUpdateForeignKeys();");
        sw.WriteLine("\t\t}");

        sw.WriteLine();
        sw.WriteLine("\t\tprivate void UpdatePrimaryKeys()");
        sw.WriteLine("\t\t{");
        if (messageDef.m_PrimaryKeys.Count > 0) {
          sw.WriteLine("\t\t\tm_PrimaryKeys.Clear();");
          foreach (string key in messageDef.m_PrimaryKeys) {
            MemberDef memberDef = messageDef.GetMemberDef(key);
            if (memberDef.m_TypeName.Equals("string")) {
              sw.WriteLine("\t\t\tif (m_{0}.{1} != null) {2}", messageDef.m_TypeName, key, '{');
              sw.WriteLine("\t\t\t\tm_PrimaryKeys.Add(m_{0}.{1}.ToString());", messageDef.m_TypeName, key);
              sw.WriteLine("\t\t\t}");
            } else {
              sw.WriteLine("\t\t\tm_PrimaryKeys.Add(m_{0}.{1}.ToString());", messageDef.m_TypeName, key);
            }
          }
        }
        sw.WriteLine("\t\t}");
        sw.WriteLine();
        sw.WriteLine("\t\tprivate void UpdateForeignKeys()");
        sw.WriteLine("\t\t{");
        if (messageDef.m_ForeignKeys.Count > 0) {
          sw.WriteLine("\t\t\tm_ForeignKeys.Clear();");
          foreach (string key in messageDef.m_ForeignKeys) {
            MemberDef memberDef = messageDef.GetMemberDef(key);
            if (memberDef.m_TypeName.Equals("string")) {
              sw.WriteLine("\t\t\tif (m_{0}.{1} != null) {2}", messageDef.m_TypeName, key, '{');
              sw.WriteLine("\t\t\t\tm_ForeignKeys.Add(m_{0}.{1}.ToString());", messageDef.m_TypeName, key);
              sw.WriteLine("\t\t\t}");
            } else {
              sw.WriteLine("\t\t\tm_ForeignKeys.Add(m_{0}.{1}.ToString());", messageDef.m_TypeName, key);
            }
          }
        }
        sw.WriteLine("\t\t}");

        sw.WriteLine();
        sw.WriteLine("\t\tprivate bool m_Modified = false;");
        sw.WriteLine("\t\tprivate List<string> m_PrimaryKeys = new List<string>();");
        sw.WriteLine("\t\tprivate List<string> m_ForeignKeys = new List<string>();");
        sw.WriteLine("\t\tprivate {0} m_{1} = new {2}();", messageDef.m_TypeName, messageDef.m_TypeName, messageDef.m_TypeName);

        foreach (MemberDef memberDef in messageDef.m_Members) {
          TypeConverterDef converter;
          if (m_TypeConverters.TryGetValue(memberDef.m_TypeName, out converter)) {
            sw.WriteLine("\t\tprivate {0} m_{1} = new {0}();", memberDef.m_TypeName, memberDef.m_MemberName);
          }
        }

        sw.WriteLine();
        sw.WriteLine("\t}");
        sw.WriteLine("}");
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }
    }
    internal void GenAllMessagesEnum(string file, string enumName, params string[] groups)
    {
      HashSet<string> expectGroups = new HashSet<string>(groups);
      if (expectGroups.Count <= 0) {
        expectGroups.Add(string.Empty);
      }
      try {
        string package;
        if (!string.IsNullOrEmpty(m_Package)) {
          package = m_Package;
        } else {
          package = "GameFrameworkMessage";
        }
        using (StreamWriter sw = new StreamWriter(file, true)) {
          sw.WriteLine("//----------------------------------------------------------------------------");
          sw.WriteLine("//！！！不要手动修改此文件，此文件由LogicDataGenerator按{0}生成！！！", m_DslFile);
          sw.WriteLine("//----------------------------------------------------------------------------");
          sw.WriteLine("using System;");
          sw.WriteLine("using System.Collections.Generic;");
          sw.WriteLine("");
          sw.WriteLine("namespace {0}", package);
          sw.WriteLine("{");
          sw.WriteLine("\tpublic enum {0}", enumName);
          sw.WriteLine("\t{");
          bool first = true;
          foreach (string message in m_SortedMessageEnums) {
            MessageDef messageDef;
            if (m_Messages.TryGetValue(message, out messageDef)) {
              if (!messageDef.m_DontGenEnum) {
                if (!string.IsNullOrEmpty(messageDef.m_EnumValue)) {
                  sw.WriteLine("\t\t{0} = {1},", message, messageDef.m_EnumValue);
                } else {
                  if (first) {
                    sw.WriteLine("\t\t{0} = 1,", message);
                  } else {
                    sw.WriteLine("\t\t{0},", message);
                  }
                }
                first = false;
              }
            }
          }
          sw.WriteLine("\t\tMaxNum");
          sw.WriteLine("\t}");
          sw.WriteLine("");
          sw.WriteLine("\tpublic static class {0}2Type", enumName);
          sw.WriteLine("\t{");
          sw.WriteLine("\t\tpublic static Type Query(int id)");
          sw.WriteLine("\t\t{");
          sw.WriteLine("\t\t\tType t;");
          sw.WriteLine("\t\t\ts_{0}2Type.TryGetValue(id, out t);", enumName);
          sw.WriteLine("\t\t\treturn t;");
          sw.WriteLine("\t\t}");
          sw.WriteLine("");
          sw.WriteLine("\t\tpublic static int Query(Type t)");
          sw.WriteLine("\t\t{");
          sw.WriteLine("\t\t\tint id;");
          sw.WriteLine("\t\t\ts_Type2{0}.TryGetValue(t, out id);", enumName);
          sw.WriteLine("\t\t\treturn id;");
          sw.WriteLine("\t\t}");
          sw.WriteLine("");
          sw.WriteLine("\t\tstatic {0}2Type()", enumName);
          sw.WriteLine("\t\t{");
          foreach (var messagePair in m_Messages) {
            string message = messagePair.Key;
            MessageDef messageDef = messagePair.Value;
            if (!messageDef.m_DontGenEnum && expectGroups.Contains(messageDef.m_GroupName)) {
              sw.WriteLine("\t\t\ts_{0}2Type.Add((int){1}.{2}, typeof({3}));", enumName, enumName, message, message);
              sw.WriteLine("\t\t\ts_Type2{0}.Add(typeof({1}), (int){2}.{3});", enumName, message, enumName, message);
            }
          }
          sw.WriteLine("\t\t}");
          sw.WriteLine("");
          sw.WriteLine("\t\tprivate static Dictionary<int, Type> s_{0}2Type = new Dictionary<int, Type>();", enumName);
          sw.WriteLine("\t\tprivate static Dictionary<Type, int> s_Type2{0} = new Dictionary<Type, int>();", enumName);
          sw.WriteLine("\t}");
          sw.WriteLine("}");
          sw.WriteLine("");
          sw.Close();
        }
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }
    }
    internal void GenAllJsMessagesEnum(string jsFile, string enumName, params string[] groups)
    {
      HashSet<string> expectGroups = new HashSet<string>(groups);
      if (expectGroups.Count <= 0) {
        expectGroups.Add(string.Empty);
      }
      using (StreamWriter sw = new StreamWriter(jsFile, true)) {
        sw.WriteLine("//----------------------------------------------------------------------------");
        sw.WriteLine("//！！！不要手动修改此文件，此文件由LogicDataGenerator按{0}生成！！！", m_DslFile);
        sw.WriteLine("//----------------------------------------------------------------------------");
        
        sw.WriteLine("var {0} = {{", enumName);
        bool first = true;
        string specValue = string.Empty;
        int index = 1;
        foreach (string message in m_SortedMessageEnums) {
          MessageDef messageDef;
          if (m_Messages.TryGetValue(message, out messageDef)) {
            if (!messageDef.m_DontGenEnum) {
              if (!string.IsNullOrEmpty(messageDef.m_EnumValue)) {
                sw.WriteLine("\t{0} : {1},", message, messageDef.m_EnumValue);
                specValue = messageDef.m_EnumValue;
                index = 1;
              } else {
                if (first || string.IsNullOrEmpty(specValue)) {
                  sw.WriteLine("\t{0} : {1},", message, index);
                  ++index;
                } else {
                  sw.WriteLine("\t{0} : {1} + {2},", message, specValue, index);
                  ++index;
                }
              }
              first = false;
            }
          }
        }
        sw.WriteLine("};");
        sw.WriteLine();

        if (!string.IsNullOrEmpty(m_Package)) {
          sw.WriteLine("exports.{0} = {{", m_Package);
        } else {
          sw.WriteLine("exports = {{", m_Package);
        }
        sw.WriteLine();
        sw.WriteLine("\t{0} : {0},", enumName);
        sw.WriteLine();
        
        sw.WriteLine("};");
        sw.Close();
      }
    }
    internal void GenAllMessageProtos(string file, params string[] groups)
    {
      HashSet<string> expectGroups = new HashSet<string>(groups);
      if (expectGroups.Count <= 0) {
        expectGroups.Add(string.Empty);
      }
      try {
        using (StreamWriter sw = new StreamWriter(file, true)) {
          sw.WriteLine("//----------------------------------------------------------------------------");
          sw.WriteLine("//！！！不要手动修改此文件，此文件由LogicDataGenerator按{0}生成！！！", m_DslFile);
          sw.WriteLine("//----------------------------------------------------------------------------");
          if (!string.IsNullOrEmpty(m_Package)) {
            sw.WriteLine("package {0};", m_Package);
          }

          foreach (var enumTypePair in m_EnumTypes) {
            string enumType = enumTypePair.Key;
            EnumTypeDef enumTypeDef = enumTypePair.Value;
            if (expectGroups.Contains(enumTypeDef.m_GroupName)) {
              GenEnumTypeProto(enumTypeDef, 0, sw);
            }
          }

          foreach (var messagePair in m_Messages) {
            string message = messagePair.Key;
            MessageDef messageDef = messagePair.Value;
            if (expectGroups.Contains(messageDef.m_GroupName)) {
              GenMessageProto(messageDef, 0, sw);
            }
          }

          sw.Close();
        }
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }
    }
    private void GenEnumTypeProto(EnumTypeDef enumTypeDef, int indent, TextWriter sw)
    {
      const string c_Indents = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t";
      string indentStr = c_Indents.Substring(0, indent);
      try {
        sw.WriteLine();
        sw.WriteLine("{0}enum {1}", indentStr, enumTypeDef.m_EnumName);
        sw.WriteLine("{0}{{", indentStr);
        foreach (var pair in enumTypeDef.m_EnumMembers) {
          sw.WriteLine("{0}\t{1} = {2};", indentStr, pair.Key, pair.Value);
        }
        sw.WriteLine("{0}}}", indentStr);
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }
    }
    private void GenMessageProto(MessageDef messageDef, int indent, TextWriter sw)
    {
      const string c_Indents = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t";
      string indentStr = c_Indents.Substring(0, indent);
      try {
        sw.WriteLine();
        sw.WriteLine("{0}message {1}", indentStr, messageDef.m_TypeName);
        sw.WriteLine("{0}{{", indentStr);
        foreach (var enumDefPair in messageDef.m_EnumTypes) {
          GenEnumTypeProto(enumDefPair.Value, indent + 1, sw);
        }
        foreach (var messageDefPair in messageDef.m_Messages) {
          GenMessageProto(messageDefPair.Value, indent + 1, sw);
        }
        foreach (MemberDef memberDef in messageDef.m_Members) {
          string type = GetProtoType(memberDef.m_TypeName);
          if (null != memberDef.m_Default) {
            sw.WriteLine("{0}\t{1} {2} {3} = {4}[default = {5}];", indentStr, memberDef.m_Modifier, type, memberDef.m_MemberName, memberDef.m_Order, memberDef.m_Default);
          } else {
            sw.WriteLine("{0}\t{1} {2} {3} = {4};", indentStr, memberDef.m_Modifier, type, memberDef.m_MemberName, memberDef.m_Order);
          }
        }
        sw.WriteLine("{0}}}", indentStr);
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }
    }
    internal void GenAllMessageDDL(string file, params string[] groups)
    {
      HashSet<string> expectGroups = new HashSet<string>(groups);
      if (expectGroups.Count <= 0) {
        expectGroups.Add(string.Empty);
      }
      using (StreamWriter sw = new StreamWriter(file, true)) {
        sw.WriteLine("#----------------------------------------------------------------------------");
        sw.WriteLine("#！！！不要手动修改此文件，此文件由LogicDataGenerator按{0}生成！！！", m_DslFile);
        sw.WriteLine("#----------------------------------------------------------------------------");
        foreach (string line in m_Explanation) {
          sw.WriteLine("#{0}", line);
        }
        sw.WriteLine();
        sw.WriteLine("call SetDSNodeVersion('{0}');", m_Version);
        sw.WriteLine(); 
        foreach (var messagePair in m_Messages) {
          string message = messagePair.Key;
          MessageDef messageDef = messagePair.Value;
          if (!messageDef.m_DontGenDB && expectGroups.Contains(messageDef.m_GroupName)) {
            string primaryKeyClause = string.Join(",", messageDef.m_PrimaryKeys.ToArray());
            sw.WriteLine("create table {0}", messageDef.m_TypeName);
            sw.WriteLine("(");
            sw.WriteLine("\tAutoKey int not null auto_increment,");
            sw.WriteLine("\tIsValid boolean not null,");
            sw.WriteLine("\tDataVersion int not null,");
            foreach (MemberDef memberDef in messageDef.m_Members) {
              string sqlType = GetSqlType(memberDef.m_TypeName);
              if (0 == sqlType.CompareTo("varchar")) {
                int size;
                if (memberDef.m_MaxSize > 0) {
                  size = memberDef.m_MaxSize;
                } else {
                  size = m_DefVarcharSize;
                }
                if (memberDef.m_IsPrimaryKey || memberDef.m_IsForeignKey) {
                  sw.WriteLine("\t{0} varchar({1}) binary not null,", memberDef.m_MemberName, size);
                } else {
                  sw.WriteLine("\t{0} varchar({1}) not null,", memberDef.m_MemberName, size);
                }
              } else {
                sw.WriteLine("\t{0} {1} not null,", memberDef.m_MemberName, sqlType);
              }
            }
            sw.WriteLine("\tprimary key (AutoKey)");
            sw.WriteLine(") ENGINE=InnoDB;");
            sw.WriteLine("create unique index {0}PrimaryIndex on {1} ({2});", messageDef.m_TypeName, messageDef.m_TypeName, primaryKeyClause);
            if (messageDef.m_ForeignKeys.Count > 0) {
              sw.WriteLine("create index {0}Index on  {1} ({2});", messageDef.m_TypeName, messageDef.m_TypeName, string.Join(",", messageDef.m_ForeignKeys.ToArray()));
            }
            sw.WriteLine();
          }
        }
        sw.WriteLine();
        sw.WriteLine("#----------------------------------------------------------------------------------------------------------------------");
        sw.WriteLine();
        foreach (var messagePair in m_Messages) {
          string message = messagePair.Key;
          MessageDef messageDef = messagePair.Value;
          if (!messageDef.m_DontGenDB && expectGroups.Contains(messageDef.m_GroupName)) {
            string primaryKeyClause = string.Join(",", messageDef.m_PrimaryKeys.ToArray());
            sw.WriteLine("drop procedure if exists Save{0};", messageDef.m_TypeName);
            sw.WriteLine("delimiter $$");           
            sw.WriteLine("create procedure Save{0}(", messageDef.m_TypeName);
            sw.WriteLine("\tin _IsValid boolean");
            sw.WriteLine("\t,in _DataVersion int");
            foreach (MemberDef memberDef in messageDef.m_Members) {
              string sqlType = GetSqlType(memberDef.m_TypeName);
              if (0 == sqlType.CompareTo("varchar")) {
                int size;
                if (memberDef.m_MaxSize > 0) {
                  size = memberDef.m_MaxSize;
                } else {
                  size = m_DefVarcharSize;
                }
                if (memberDef.m_IsPrimaryKey || memberDef.m_IsForeignKey) {
                  sw.WriteLine("\t,in _{0} varchar({1})", memberDef.m_MemberName, size);
                } else {
                  sw.WriteLine("\t,in _{0} varchar({1})", memberDef.m_MemberName, size);
                }
              } else {
                sw.WriteLine("\t,in _{0} {1}", memberDef.m_MemberName, sqlType);
              }
            }
            sw.WriteLine(")");
            sw.WriteLine("begin");
            sw.WriteLine("\tinsert into {0} (AutoKey,IsValid,DataVersion,{1})", messageDef.m_TypeName, string.Join(",", messageDef.m_ColumnNames.ToArray()));
            sw.WriteLine("\t\tvalues ");
            sw.WriteLine("\t\t\t(null,_IsValid,_DataVersion");
            foreach (string name in messageDef.m_ColumnNames) {
              sw.WriteLine("\t\t\t,_{0}", name);
            }
            sw.WriteLine("\t\t\t)");
            sw.WriteLine("\t\ton duplicate key update ");
            sw.WriteLine("\t\t\tIsValid =  if(DataVersion < _DataVersion, _IsValid, IsValid),");
            foreach (string name in messageDef.m_ColumnNames) {
              sw.WriteLine("\t\t\t{0} =  if(DataVersion < _DataVersion, _{1}, {2}),", name, name, name);
            }
            sw.WriteLine("\t\t\tDataVersion =  if(DataVersion < _DataVersion, _DataVersion, DataVersion);");
            sw.WriteLine("end $$");
            sw.WriteLine("delimiter ;");
            sw.WriteLine();
          }
        }
        sw.WriteLine();
        sw.WriteLine("#----------------------------------------------------------------------------------------------------------------------");
        sw.WriteLine();
        foreach (var messagePair in m_Messages) {
          string message = messagePair.Key;
          MessageDef messageDef = messagePair.Value;
          if (!messageDef.m_DontGenDB && expectGroups.Contains(messageDef.m_GroupName)) {
            sw.WriteLine("drop procedure if exists LoadAll{0};", messageDef.m_TypeName);
            sw.WriteLine("delimiter $$");
            sw.WriteLine("create procedure LoadAll{0}(in _Start int, in _Count int)", messageDef.m_TypeName);
            sw.WriteLine("begin");
            sw.WriteLine("\tselect * from {0} where IsValid = 1 limit _Start, _Count;", messageDef.m_TypeName);
            sw.WriteLine("end $$");
            sw.WriteLine("delimiter ;");
            sw.WriteLine();
            if (messageDef.m_PrimaryKeys.Count > 0) {
              sw.WriteLine("drop procedure if exists LoadSingle{0};", messageDef.m_TypeName);
              sw.WriteLine("delimiter $$");              
              sw.WriteLine("create procedure LoadSingle{0}(", messageDef.m_TypeName);
              string prefix = string.Empty;
              foreach (MemberDef memberDef in messageDef.m_Members) {
                if (memberDef.m_IsPrimaryKey) {
                  string sqlType = GetSqlType(memberDef.m_TypeName);
                  if (0 == sqlType.CompareTo("varchar")) {
                    int size;
                    if (memberDef.m_MaxSize > 0) {
                      size = memberDef.m_MaxSize;
                    } else {
                      size = m_DefVarcharSize;
                    }
                    if (memberDef.m_IsPrimaryKey || memberDef.m_IsForeignKey) {
                      sw.WriteLine("\t{0}in _{1} varchar({2})", prefix, memberDef.m_MemberName, size);
                    } else {
                      sw.WriteLine("\t{0}in _{1} varchar({2})", prefix, memberDef.m_MemberName, size);
                    }
                  } else {
                    sw.WriteLine("\t{0}in _{1} {2}", prefix, memberDef.m_MemberName, sqlType);
                  }
                  prefix = ",";
                }
              }
              sw.WriteLine(")");
              sw.WriteLine("begin");
              sw.WriteLine("\tselect * from {0} where IsValid = 1 ", messageDef.m_TypeName);
              foreach (string name in messageDef.m_PrimaryKeys) {
                sw.WriteLine("\t\tand {0} = _{1} ", name, name);
              }
              sw.WriteLine("\t\t;");
              sw.WriteLine("end $$");
              sw.WriteLine("delimiter ;");
              sw.WriteLine();
            }
            if (messageDef.m_ForeignKeys.Count > 0) {
              sw.WriteLine("drop procedure if exists LoadMulti{0};", messageDef.m_TypeName);
              sw.WriteLine("delimiter $$");             
              sw.WriteLine("create procedure LoadMulti{0}(", messageDef.m_TypeName);
              string prefix = string.Empty;
              foreach (MemberDef memberDef in messageDef.m_Members) {
                if (memberDef.m_IsForeignKey) {
                  string sqlType = GetSqlType(memberDef.m_TypeName);
                  if (0 == sqlType.CompareTo("varchar")) {
                    int size;
                    if (memberDef.m_MaxSize > 0) {
                      size = memberDef.m_MaxSize;
                    } else {
                      size = m_DefVarcharSize;
                    }
                    if (memberDef.m_IsPrimaryKey || memberDef.m_IsForeignKey) {
                      sw.WriteLine("\t{0}in _{1} varchar({2})", prefix, memberDef.m_MemberName, size);
                    } else {
                      sw.WriteLine("\t{0}in _{1} varchar({2})", prefix, memberDef.m_MemberName, size);
                    }
                  } else {
                    sw.WriteLine("\t{0}in _{1} {2}", prefix, memberDef.m_MemberName, sqlType);
                  }
                  prefix = ",";
                }
              }
              sw.WriteLine(")");
              sw.WriteLine("begin");
              sw.WriteLine("\tselect * from {0} where IsValid = 1 ", messageDef.m_TypeName);
              foreach (string name in messageDef.m_ForeignKeys) {
                sw.WriteLine("\t\tand {0} = _{1} ", name, name);
              }
              sw.WriteLine("\t\t;");
              sw.WriteLine("end $$");
              sw.WriteLine("delimiter ;");
              sw.WriteLine();
            }
          }
        }
        sw.Close();
      }
    }
    internal void GenAllMessageDML(string file, string enumName, params string[] groups)
    {
      HashSet<string> expectGroups = new HashSet<string>(groups);
      if (expectGroups.Count <= 0) {
        expectGroups.Add(string.Empty);
      }
      using (StreamWriter sw = new StreamWriter(file, true)) {
        sw.WriteLine("//----------------------------------------------------------------------------");
        sw.WriteLine("//！！！不要手动修改此文件，此文件由LogicDataGenerator按{0}生成！！！", m_DslFile);
        sw.WriteLine("//----------------------------------------------------------------------------");
        sw.WriteLine("using System;");
        sw.WriteLine("using System.Collections.Generic;");
        sw.WriteLine("using System.Runtime.InteropServices;");
        sw.WriteLine("using System.IO;");
        sw.WriteLine("using System.Text;");
        sw.WriteLine("using System.Data;");
        sw.WriteLine("using System.Data.Common;");
        sw.WriteLine("using MySql.Data.MySqlClient;");
        if (!string.IsNullOrEmpty(m_Package)) {
          sw.WriteLine("using {0};", m_Package);
        }
        sw.WriteLine("using GameFramework;");
        sw.WriteLine();
        sw.WriteLine("namespace GameFramework");
        sw.WriteLine("{");
        sw.WriteLine("\tinternal static class DataDML");
        sw.WriteLine("\t{");
        sw.WriteLine();

        sw.WriteLine("\t\tinternal static void Save(int msgId, bool isValid, int dataVersion, byte[] data)");
        sw.WriteLine("\t\t{"); 
        sw.WriteLine("\t\t\tswitch(msgId){");
        foreach (var messagePair in m_Messages) {
          string message = messagePair.Key;
          MessageDef messageDef = messagePair.Value;
          if (!messageDef.m_DontGenEnum && !messageDef.m_DontGenDB && expectGroups.Contains(messageDef.m_GroupName)) {
            sw.WriteLine("\t\t\t\tcase (int){0}.{1}:", enumName, messageDef.m_TypeName);
            sw.WriteLine("\t\t\t\t\tSave{0}(isValid, dataVersion, data);", messageDef.m_TypeName);
            sw.WriteLine("\t\t\t\t\tbreak;");
          }
        }
        sw.WriteLine("\t\t\t}");
        sw.WriteLine("\t\t}");

        sw.WriteLine("\t\tinternal static int BatchSave(int msgId, List<bool> validList, List<byte[]> dataList, int dataVersion)");
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tint count = 0;");
        sw.WriteLine("\t\t\tswitch(msgId){");
        foreach (var messagePair in m_Messages) {
          string message = messagePair.Key;
          MessageDef messageDef = messagePair.Value;
          if (!messageDef.m_DontGenEnum && !messageDef.m_DontGenDB && expectGroups.Contains(messageDef.m_GroupName)) {
            sw.WriteLine("\t\t\t\tcase (int){0}.{1}:", enumName, messageDef.m_TypeName);
            sw.WriteLine("\t\t\t\t\tcount = BatchSave{0}(validList, dataList, dataVersion);", messageDef.m_TypeName);
            sw.WriteLine("\t\t\t\t\tbreak;");
          }
        }
        sw.WriteLine("\t\t\t}");
        sw.WriteLine("\t\t\treturn count;");
        sw.WriteLine("\t\t}");

        sw.WriteLine("\t\tinternal static List<GeneralRecordData> LoadAll(int msgId, int start, int count)");
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tif(start<0)");
        sw.WriteLine("\t\t\t\tstart=0;");
        sw.WriteLine("\t\t\tif(count<=0)");
        sw.WriteLine("\t\t\t\tcount=int.MaxValue;");
        sw.WriteLine("\t\t\tswitch(msgId){");
        foreach (var messagePair in m_Messages) {
          string message = messagePair.Key;
          MessageDef messageDef = messagePair.Value;
          if (!messageDef.m_DontGenEnum && !messageDef.m_DontGenDB && expectGroups.Contains(messageDef.m_GroupName)) {
            sw.WriteLine("\t\t\t\tcase (int){0}.{1}:", enumName, messageDef.m_TypeName);
            sw.WriteLine("\t\t\t\t\treturn LoadAll{0}(start, count);", messageDef.m_TypeName);
          }
        }
        sw.WriteLine("\t\t\t}");
        sw.WriteLine("\t\t\treturn null;");
        sw.WriteLine("\t\t}");

        sw.WriteLine("\t\tinternal static GeneralRecordData LoadSingle(int msgId, List<string> primaryKeys)");
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tswitch(msgId){");
        foreach (var messagePair in m_Messages) {
          string message = messagePair.Key;
          MessageDef messageDef = messagePair.Value;
          if (!messageDef.m_DontGenEnum && !messageDef.m_DontGenDB && expectGroups.Contains(messageDef.m_GroupName)) {
            sw.WriteLine("\t\t\t\tcase (int){0}.{1}:", enumName, messageDef.m_TypeName);
            sw.WriteLine("\t\t\t\t\treturn LoadSingle{0}(primaryKeys);", messageDef.m_TypeName);
          }
        }
        sw.WriteLine("\t\t\t}");
        sw.WriteLine("\t\t\treturn null;");
        sw.WriteLine("\t\t}");

        sw.WriteLine("\t\tinternal static List<GeneralRecordData> LoadMulti(int msgId, List<string> foreignKeys)");
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tswitch(msgId){");
        foreach (var messagePair in m_Messages) {
          string message = messagePair.Key;
          MessageDef messageDef = messagePair.Value;
          if (!messageDef.m_DontGenEnum && !messageDef.m_DontGenDB && expectGroups.Contains(messageDef.m_GroupName)) {
            sw.WriteLine("\t\t\t\tcase (int){0}.{1}:", enumName, messageDef.m_TypeName);
            sw.WriteLine("\t\t\t\t\treturn LoadMulti{0}(foreignKeys);", messageDef.m_TypeName);
          }
        }
        sw.WriteLine("\t\t\t}");
        sw.WriteLine("\t\t\treturn null;");
        sw.WriteLine("\t\t}");

        foreach (var messagePair in m_Messages) {
          string message = messagePair.Key;
          MessageDef messageDef = messagePair.Value;
          if (!messageDef.m_DontGenEnum && !messageDef.m_DontGenDB && expectGroups.Contains(messageDef.m_GroupName)) {
            GenMessageLoadAll(messageDef, sw);
            GenMessageLoadSingle(messageDef, sw);
            GenMessageLoadMulti(messageDef, sw);
            GenMessageSave(messageDef, sw);
            GenMessageBatchSave(messageDef, sw);
          }
        }

        sw.WriteLine();
        sw.WriteLine("\t}");
        sw.WriteLine("}");

        sw.Close();
      }
    }
    private void GenMessageLoadAll(MessageDef messageDef, TextWriter sw)
    {
      try {
        sw.WriteLine();
        sw.WriteLine("\t\tprivate static List<GeneralRecordData> LoadAll{0}(int start, int count)", messageDef.m_TypeName);
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tList<GeneralRecordData> ret = new List<GeneralRecordData>();");
        sw.WriteLine("\t\t\ttry {");
        sw.WriteLine("\t\t\t  using (MySqlCommand cmd = new MySqlCommand()) {");
        sw.WriteLine("\t\t\t    cmd.Connection = DBConn.MySqlConn;");
        sw.WriteLine("\t\t\t    cmd.CommandType = CommandType.StoredProcedure;");
        sw.WriteLine("\t\t\t    cmd.CommandText = \"LoadAll{0}\";", messageDef.m_TypeName);

        sw.WriteLine("\t\t\t    MySqlParameter inputParam;");
        sw.WriteLine("\t\t\t    inputParam = new MySqlParameter(\"@_Start\", MySqlDbType.Int32);");
        sw.WriteLine("\t\t\t    inputParam.Direction = ParameterDirection.Input;");
        sw.WriteLine("\t\t\t    inputParam.Value = start;");
        sw.WriteLine("\t\t\t    cmd.Parameters.Add(inputParam);");
        sw.WriteLine("\t\t\t    inputParam = new MySqlParameter(\"@_Count\", MySqlDbType.Int32);");
        sw.WriteLine("\t\t\t    inputParam.Direction = ParameterDirection.Input;");
        sw.WriteLine("\t\t\t    inputParam.Value = count;");
        sw.WriteLine("\t\t\t    cmd.Parameters.Add(inputParam);");

        sw.WriteLine("\t\t\t    using (DbDataReader reader = cmd.ExecuteReader()) {");
        sw.WriteLine("\t\t\t      while (reader.Read()) {");
        sw.WriteLine("\t\t\t        GeneralRecordData record = new GeneralRecordData();");
        sw.WriteLine("\t\t\t        object val;");
        sw.WriteLine("\t\t\t        {0} msg = new {1}();", messageDef.m_TypeName, messageDef.m_TypeName);
        foreach (MemberDef memberDef in messageDef.m_Members) {
          sw.WriteLine("\t\t\t        val = reader[\"{0}\"];", memberDef.m_MemberName);
          sw.WriteLine("\t\t\t        msg.{0} = ({1})val;", memberDef.m_MemberName, GetMessageType(memberDef.m_TypeName));
          if (memberDef.m_IsPrimaryKey) {
            sw.WriteLine("\t\t\t        record.PrimaryKeys.Add(val.ToString());");
          } else if (memberDef.m_IsForeignKey) {
            sw.WriteLine("\t\t\t        record.ForeignKeys.Add(val.ToString());");
          }
        }
        sw.WriteLine("\t\t\t        record.DataVersion = (int)reader[\"DataVersion\"];");
        sw.WriteLine("\t\t\t        record.Data = DbDataSerializer.Encode(msg);");
        sw.WriteLine("\t\t\t        ret.Add(record);");
        sw.WriteLine("\t\t\t      }");
        sw.WriteLine("\t\t\t    }");
        sw.WriteLine("\t\t\t  }");
        sw.WriteLine("\t\t\t} catch (Exception ex) {");
        sw.WriteLine("\t\t\t  DBConn.Close();");
        sw.WriteLine("\t\t\t  throw ex;");
        sw.WriteLine("\t\t\t}");
        sw.WriteLine("\t\t\treturn ret;");

        sw.WriteLine("\t\t}");
        sw.WriteLine();
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }
    }
    private void GenMessageLoadSingle(MessageDef messageDef, TextWriter sw)
    {
      try {
        sw.WriteLine();
        sw.WriteLine("\t\tprivate static GeneralRecordData LoadSingle{0}(List<string> primaryKeys)", messageDef.m_TypeName);
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tGeneralRecordData ret = null;");
        if (messageDef.m_PrimaryKeys.Count > 0) {
          sw.WriteLine("\t\t\ttry {");
          sw.WriteLine("\t\t\t  using (MySqlCommand cmd = new MySqlCommand()) {");
          sw.WriteLine("\t\t\t    cmd.Connection = DBConn.MySqlConn;");
          sw.WriteLine("\t\t\t    cmd.CommandType = CommandType.StoredProcedure;");
          sw.WriteLine("\t\t\t    cmd.CommandText = \"LoadSingle{0}\";", messageDef.m_TypeName);
          sw.WriteLine("\t\t\t    if(primaryKeys.Count != {0})", messageDef.m_PrimaryKeys.Count);
          sw.WriteLine("\t\t\t\t    throw new Exception(\"primary key number don't match !!!\");");
          sw.WriteLine("\t\t\t    MySqlParameter inputParam;");
          int ix = 0;
          foreach (MemberDef memberDef in messageDef.m_Members) {
            if (memberDef.m_IsPrimaryKey) {
              string mysqlClientType = GetMySqlClientType(memberDef.m_TypeName);
              sw.WriteLine("\t\t\t    inputParam = new MySqlParameter(\"@_{0}\", {1});", memberDef.m_MemberName, mysqlClientType);
              sw.WriteLine("\t\t\t    inputParam.Direction = ParameterDirection.Input;");
              string sqlType = GetSqlType(memberDef.m_TypeName);
              if (0 == sqlType.CompareTo("varchar")) {
                int size;
                if (memberDef.m_MaxSize > 0) {
                  size = memberDef.m_MaxSize;
                } else {
                  size = m_DefVarcharSize;
                }
                sw.WriteLine("\t\t\t    inputParam.Value = primaryKeys[{0}];", ix);
                sw.WriteLine("\t\t\t    inputParam.Size = {0};", size);
              } else {
                sw.WriteLine("\t\t\t    inputParam.Value = ({0})Convert.ChangeType(primaryKeys[{1}],typeof({2}));", memberDef.m_TypeName, ix, memberDef.m_TypeName);
              }
              sw.WriteLine("\t\t\t    cmd.Parameters.Add(inputParam);");
              ix++;
            }
          }
          sw.WriteLine("\t\t\t    using (DbDataReader reader = cmd.ExecuteReader()) {");
          sw.WriteLine("\t\t\t      if (reader.Read()) {");
          sw.WriteLine("\t\t\t        ret = new GeneralRecordData();");
          sw.WriteLine("\t\t\t        object val;");
          sw.WriteLine("\t\t\t        {0} msg = new {1}();", messageDef.m_TypeName, messageDef.m_TypeName);
          foreach (MemberDef memberDef in messageDef.m_Members) {
            sw.WriteLine("\t\t\t        val = reader[\"{0}\"];", memberDef.m_MemberName);
            sw.WriteLine("\t\t\t        msg.{0} = ({1})val;", memberDef.m_MemberName, GetMessageType(memberDef.m_TypeName));
            if (memberDef.m_IsPrimaryKey) {
              sw.WriteLine("\t\t\t        ret.PrimaryKeys.Add(val.ToString());");
            } else if (memberDef.m_IsForeignKey) {
              sw.WriteLine("\t\t\t        ret.ForeignKeys.Add(val.ToString());");
            }
          }
          sw.WriteLine("\t\t\t        ret.DataVersion = (int)reader[\"DataVersion\"];");
          sw.WriteLine("\t\t\t        ret.Data = DbDataSerializer.Encode(msg);");
          sw.WriteLine("\t\t\t      }");
          sw.WriteLine("\t\t\t    }");
          sw.WriteLine("\t\t\t  }");
          sw.WriteLine("\t\t\t} catch (Exception ex) {");
          sw.WriteLine("\t\t\t  DBConn.Close();");
          sw.WriteLine("\t\t\t  throw ex;");
          sw.WriteLine("\t\t\t}");
        }
        sw.WriteLine("\t\t\treturn ret;");

        sw.WriteLine("\t\t}");
        sw.WriteLine();
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }
    }
    private void GenMessageLoadMulti(MessageDef messageDef, TextWriter sw)
    {
      try {
        sw.WriteLine();
        sw.WriteLine("\t\tprivate static List<GeneralRecordData> LoadMulti{0}(List<string> foreignKeys)", messageDef.m_TypeName);
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tList<GeneralRecordData> ret = new List<GeneralRecordData>();");
        if (messageDef.m_ForeignKeys.Count > 0) {
          sw.WriteLine("\t\t\ttry {");
          sw.WriteLine("\t\t\t  using (MySqlCommand cmd = new MySqlCommand()) {");
          sw.WriteLine("\t\t\t    cmd.Connection = DBConn.MySqlConn;");
          sw.WriteLine("\t\t\t    cmd.CommandType = CommandType.StoredProcedure;");
          sw.WriteLine("\t\t\t    cmd.CommandText = \"LoadMulti{0}\";", messageDef.m_TypeName);
          sw.WriteLine("\t\t\t    if(foreignKeys.Count != {0})", messageDef.m_ForeignKeys.Count);
          sw.WriteLine("\t\t\t\t    throw new Exception(\"foreign key number don't match !!!\");");
          sw.WriteLine("\t\t\t    MySqlParameter inputParam;");
          int ix = 0;
          foreach (MemberDef memberDef in messageDef.m_Members) {
            if (memberDef.m_IsForeignKey) {
              string mysqlClientType = GetMySqlClientType(memberDef.m_TypeName);
              sw.WriteLine("\t\t\t    inputParam = new MySqlParameter(\"@_{0}\", {1});", memberDef.m_MemberName, mysqlClientType);
              sw.WriteLine("\t\t\t    inputParam.Direction = ParameterDirection.Input;");
              string sqlType = GetSqlType(memberDef.m_TypeName);
              if (0 == sqlType.CompareTo("varchar")) {
                int size;
                if (memberDef.m_MaxSize > 0) {
                  size = memberDef.m_MaxSize;
                } else {
                  size = m_DefVarcharSize;
                }
                sw.WriteLine("\t\t\t    inputParam.Value = foreignKeys[{0}];", ix);
                sw.WriteLine("\t\t\t    inputParam.Size = {0};", size);
              } else {
                sw.WriteLine("\t\t\t    inputParam.Value = ({0})Convert.ChangeType(foreignKeys[{1}],typeof({2}));", memberDef.m_TypeName, ix, GetMessageType(memberDef.m_TypeName));
              }
              sw.WriteLine("\t\t\t    cmd.Parameters.Add(inputParam);");
              ix++;
            }
          }
          sw.WriteLine("\t\t\t    using (DbDataReader reader = cmd.ExecuteReader()) {");
          sw.WriteLine("\t\t\t      while (reader.Read()) {");
          sw.WriteLine("\t\t\t        GeneralRecordData record = new GeneralRecordData();");
          sw.WriteLine("\t\t\t        object val;");
          sw.WriteLine("\t\t\t        {0} msg = new {1}();", messageDef.m_TypeName, messageDef.m_TypeName);
          foreach (MemberDef memberDef in messageDef.m_Members) {
            sw.WriteLine("\t\t\t        val = reader[\"{0}\"];", memberDef.m_MemberName);
            sw.WriteLine("\t\t\t        msg.{0} = ({1})val;", memberDef.m_MemberName, GetMessageType(memberDef.m_TypeName));
            if (memberDef.m_IsPrimaryKey) {
              sw.WriteLine("\t\t\t        record.PrimaryKeys.Add(val.ToString());");
            } else if (memberDef.m_IsForeignKey) {
              sw.WriteLine("\t\t\t        record.ForeignKeys.Add(val.ToString());");
            }
          }
          sw.WriteLine("\t\t\t        record.DataVersion = (int)reader[\"DataVersion\"];");
          sw.WriteLine("\t\t\t        record.Data = DbDataSerializer.Encode(msg);");
          sw.WriteLine("\t\t\t        ret.Add(record);");
          sw.WriteLine("\t\t\t      }");
          sw.WriteLine("\t\t\t    }");
          sw.WriteLine("\t\t\t  }");
          sw.WriteLine("\t\t\t} catch (Exception ex) {");
          sw.WriteLine("\t\t\t  DBConn.Close();");
          sw.WriteLine("\t\t\t  throw ex;");
          sw.WriteLine("\t\t\t}");
        }
        sw.WriteLine("\t\t\treturn ret;");

        sw.WriteLine("\t\t}");
        sw.WriteLine();
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }
    }
    private void GenMessageSave(MessageDef messageDef, TextWriter sw)
    {
      try {
        sw.WriteLine();
        sw.WriteLine("\t\tprivate static void Save{0}(bool isValid, int dataVersion, byte[] data)", messageDef.m_TypeName);
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tobject _msg;");
        sw.WriteLine("\t\t\tif(DbDataSerializer.Decode(data, typeof({0}), out _msg)){{", messageDef.m_TypeName);
        sw.WriteLine("\t\t\t\t{0} msg = _msg as {1};", messageDef.m_TypeName, messageDef.m_TypeName);

        sw.WriteLine("\t\t\t\ttry {");
        sw.WriteLine("\t\t\t\t  using (MySqlCommand cmd = new MySqlCommand()) {");
        sw.WriteLine("\t\t\t\t    cmd.Connection = DBConn.MySqlConn;");
        sw.WriteLine("\t\t\t\t    cmd.CommandType = CommandType.StoredProcedure;");
        sw.WriteLine("\t\t\t\t    cmd.CommandText = \"Save{0}\";",messageDef.m_TypeName);
        sw.WriteLine("\t\t\t\t    MySqlParameter inputParam;");
        sw.WriteLine("\t\t\t\t    inputParam = new MySqlParameter(\"@_IsValid\", MySqlDbType.Bit);");
        sw.WriteLine("\t\t\t\t    inputParam.Direction = ParameterDirection.Input;");
        sw.WriteLine("\t\t\t\t    inputParam.Value = isValid;");
        sw.WriteLine("\t\t\t\t    cmd.Parameters.Add(inputParam);");
        sw.WriteLine("\t\t\t\t    inputParam = new MySqlParameter(\"@_DataVersion\", MySqlDbType.Int32);");
        sw.WriteLine("\t\t\t\t    inputParam.Direction = ParameterDirection.Input;");
        sw.WriteLine("\t\t\t\t    inputParam.Value = dataVersion;");
        sw.WriteLine("\t\t\t\t    cmd.Parameters.Add(inputParam);");
        foreach (MemberDef memberDef in messageDef.m_Members) {
          string mysqlClientType = GetMySqlClientType(memberDef.m_TypeName);
          sw.WriteLine("\t\t\t\t    inputParam = new MySqlParameter(\"@_{0}\", {1});", memberDef.m_MemberName, mysqlClientType);
          sw.WriteLine("\t\t\t\t    inputParam.Direction = ParameterDirection.Input;");
          sw.WriteLine("\t\t\t\t    inputParam.Value = msg.{0};", memberDef.m_MemberName);
          string sqlType = GetSqlType(memberDef.m_TypeName);
          if (0 == sqlType.CompareTo("varchar")) {
            int size;
            if (memberDef.m_MaxSize > 0) {
              size = memberDef.m_MaxSize;
            } else {
              size = m_DefVarcharSize;
            }
            sw.WriteLine("\t\t\t\t    inputParam.Size = {0};", size);
          }
          sw.WriteLine("\t\t\t\t    cmd.Parameters.Add(inputParam);");
        }
        sw.WriteLine("\t\t\t\t    cmd.ExecuteNonQuery();");
        sw.WriteLine("\t\t\t\t  }");
        sw.WriteLine("\t\t\t\t} catch (Exception ex) {");
        sw.WriteLine("\t\t\t\t  DBConn.Close();");
        sw.WriteLine("\t\t\t\t  throw ex;");
        sw.WriteLine("\t\t\t\t}");

        sw.WriteLine("\t\t\t}");
        sw.WriteLine("\t\t}");
        sw.WriteLine();
      } catch (Exception ex) {
        Console.WriteLine(ex);
      }
    }
    private void GenMessageBatchSave(MessageDef messageDef, TextWriter sw)
    {
      try {
        sw.WriteLine();
        sw.WriteLine("\t\tprivate static int BatchSave{0}(List<bool> validList, List<byte[]> dataList, int dataVersion)", messageDef.m_TypeName);
        sw.WriteLine("\t\t{");
        sw.WriteLine("\t\t\tif (dataList.Count <= 0) {");
        sw.WriteLine("\t\t\t  return 0;");
        sw.WriteLine("\t\t\t}");
        sw.WriteLine("\t\t\tStringBuilder sbSql = new StringBuilder(\"insert into {0} \", 4096); ", messageDef.m_TypeName);
        StringBuilder sbMember = new StringBuilder();
        foreach (MemberDef memberDef in messageDef.m_Members) {
          sbMember.Append(memberDef.m_MemberName);
          sbMember.Append(',');
        }
        sbMember.Remove(sbMember.Length - 1, 1);
        sw.WriteLine("\t\t\tsbSql.Append(\"(IsValid,DataVersion,{0})\");", sbMember.ToString());
        sw.WriteLine("\t\t\tsbSql.Append(\" values \");", sbMember.ToString());
        sw.WriteLine("\t\t\tfor (int i = 0; i < validList.Count; ++i) {");       
        sw.WriteLine("\t\t\t  Byte valid = 1;");
        sw.WriteLine("\t\t\t  if (validList[i] == false) {");
        sw.WriteLine("\t\t\t    valid = 0;");
        sw.WriteLine("\t\t\t  }");
        sw.WriteLine("\t\t\t  StringBuilder sbValue = new StringBuilder();");
        sw.WriteLine("\t\t\t  sbValue.AppendFormat(\"({0},{1}\", valid, dataVersion);");
        sw.WriteLine("\t\t\t  object _msg;");
        sw.WriteLine("\t\t\t  if (DbDataSerializer.Decode(dataList[i], typeof({0}), out _msg)) {{", messageDef.m_TypeName);
        sw.WriteLine("\t\t\t    {0} msg = _msg as {0};", messageDef.m_TypeName);
        foreach (MemberDef memberDef in messageDef.m_Members) {
          sw.WriteLine("\t\t\t    sbValue.Append(',');");          
          if (memberDef.m_TypeName.Equals("bool")) {
             sw.WriteLine("\t\t\t    sbValue.Append(msg.{0});", memberDef.m_MemberName);
          } else {
             sw.WriteLine("\t\t\t    sbValue.AppendFormat(\"\'{{0}}\'\", msg.{0});", memberDef.m_MemberName);          
          }          
        }
        sw.WriteLine("\t\t\t    sbValue.Append(')');");
        sw.WriteLine("\t\t\t    sbSql.Append(sbValue.ToString());");
        sw.WriteLine("\t\t\t    sbSql.Append(',');");
        sw.WriteLine("\t\t\t  }");
        sw.WriteLine("\t\t\t}");
        sw.WriteLine("\t\t\tsbSql.Remove(sbSql.Length - 1, 1);");
        sw.WriteLine("\t\t\tsbSql.Append(\" on duplicate key update \");");
        sw.WriteLine("\t\t\tsbSql.AppendFormat(\" IsValid = if(DataVersion < {0}, values(IsValid), IsValid),\", dataVersion);");
        foreach (MemberDef memberDef in messageDef.m_Members) {
          sw.WriteLine("\t\t\tsbSql.AppendFormat(\" {0} = if(DataVersion < {{0}}, values({0}), {0}),\", dataVersion);", memberDef.m_MemberName);
        }
        sw.WriteLine("\t\t\tsbSql.AppendFormat(\" DataVersion = if(DataVersion < {0}, {0}, DataVersion),\", dataVersion);");
        sw.WriteLine("\t\t\tsbSql.Remove(sbSql.Length - 1, 1);");
      
        sw.WriteLine("\t\t\tstring statement = sbSql.ToString();");
        sw.WriteLine("\t\t\tint count = 0;");
        sw.WriteLine("\t\t\ttry {");
        sw.WriteLine("\t\t\t  using (MySqlCommand cmd = new MySqlCommand()) {");
        sw.WriteLine("\t\t\t    cmd.Connection = DBConn.MySqlConn;");
        sw.WriteLine("\t\t\t    cmd.CommandType = CommandType.Text;");
        sw.WriteLine("\t\t\t    cmd.CommandText = statement;");       
        sw.WriteLine("\t\t\t    count = cmd.ExecuteNonQuery();");
        sw.WriteLine("\t\t\t  }");
        sw.WriteLine("\t\t\t} catch (Exception ex) {");
        sw.WriteLine("\t\t\t  if (dataList.Count < 200) {");
        sw.WriteLine("\t\t\t    LogSys.Log(LOG_TYPE.ERROR, \"Error Sql statement:{0}\", statement);");
        sw.WriteLine("\t\t\t  }");
        sw.WriteLine("\t\t\t  DBConn.Close();");
        sw.WriteLine("\t\t\t  throw ex;");
        sw.WriteLine("\t\t\t}");
        sw.WriteLine("\t\t\treturn count;");
        sw.WriteLine("\t\t}");
        sw.WriteLine();
      } catch (Exception ex) {
        Console.WriteLine(ex);
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
    private string GetMessageType(string type)
    {
      string val;
      TypeConverterDef converter;
      if (m_TypeConverters.TryGetValue(type, out converter)) {
        val = converter.m_MessageType;
      } else {
        val = type;
      }
      return val;
    }
    private string GetProtoType(string type)
    {
      if (s_Type2ProtoTypes.Count <= 0) {
        s_Type2ProtoTypes.Add("int", "int32");
        s_Type2ProtoTypes.Add("uint", "uint32");
        s_Type2ProtoTypes.Add("long", "int64");
        s_Type2ProtoTypes.Add("ulong", "uint64");
        s_Type2ProtoTypes.Add("byte[]", "bytes");
      }
      string val;
      type = GetMessageType(type);
      if (!s_Type2ProtoTypes.TryGetValue(type, out val)) {
        val = type;
      }
      return val;
    }
    private string GetSqlType(string type)
    {
      if (s_Type2SqlTypes.Count <= 0) {
        s_Type2SqlTypes.Add("int", "int");
        s_Type2SqlTypes.Add("uint", "int unsigned");
        s_Type2SqlTypes.Add("long", "bigint");
        s_Type2SqlTypes.Add("ulong", "bigint unsigned");
        s_Type2SqlTypes.Add("byte[]", "blob");
        s_Type2SqlTypes.Add("string", "varchar");
        s_Type2SqlTypes.Add("bool", "boolean");
      }
      string val;
      type = GetMessageType(type);
      if (!s_Type2SqlTypes.TryGetValue(type, out val)) {
        val = type;
      }
      return val;
    }
    private string GetMySqlClientType(string type)
    {
      if (s_Type2MySqlClientTypes.Count <= 0) {
        s_Type2MySqlClientTypes.Add("int", "MySqlDbType.Int32");
        s_Type2MySqlClientTypes.Add("uint", "MySqlDbType.UInt32");
        s_Type2MySqlClientTypes.Add("long", "MySqlDbType.Int64");
        s_Type2MySqlClientTypes.Add("ulong", "MySqlDbType.UInt64");
        s_Type2MySqlClientTypes.Add("byte[]", "MySqlDbType.Blob");
        s_Type2MySqlClientTypes.Add("string", "MySqlDbType.VarChar");
        s_Type2MySqlClientTypes.Add("bool", "MySqlDbType.Bit");
        s_Type2MySqlClientTypes.Add("float", "MySqlDbType.Float");
        s_Type2MySqlClientTypes.Add("double", "MySqlDbType.Double");
      }
      string val;
      type = GetMessageType(type);
      if (!s_Type2MySqlClientTypes.TryGetValue(type, out val)) {
        val = type;
      }
      return val;
    }
    
    private string m_DslFile = string.Empty;
    private List<string> m_Explanation = new List<string>();
    private string m_Version = string.Empty;
    private string m_Package = string.Empty;
    private Dictionary<string, TypeConverterDef> m_TypeConverters = new Dictionary<string, TypeConverterDef>();
    private SortedDictionary<string, EnumTypeDef> m_EnumTypes = new SortedDictionary<string, EnumTypeDef>();
    private SortedDictionary<string, MessageDef> m_Messages = new SortedDictionary<string, MessageDef>();
    private List<string> m_SortedMessageEnums = new List<string>();
    private int m_DefVarcharSize = c_DefVarcharSize;
    private const int c_DefVarcharSize = 32;

    private static void ParseEnum(string dslFile, FunctionData funcData, string defGroupName, SortedDictionary<string, EnumTypeDef> enumTypes, ref bool haveError)
    {
      if (null != funcData) {
        CallData callData = funcData.Call;
        if (null != callData && callData.GetParamNum() >= 1) {
          string enumName, groupName;
          enumName = callData.GetParamId(0);
          if (callData.GetParamNum() > 1) {
            groupName = callData.GetParamId(1);
          } else {
            groupName = defGroupName;
          }

          EnumTypeDef enumTypeDef = new EnumTypeDef();
          enumTypeDef.m_EnumName = enumName;
          enumTypeDef.m_GroupName = groupName;
          if (enumTypes.ContainsKey(enumName)) {
            enumTypes[enumName] = enumTypeDef;
          } else {
            enumTypes.Add(enumName, enumTypeDef);
          }

          int nextValue = 0;
          foreach (ISyntaxComponent comp in funcData.Statements) {
            ValueData val = comp as ValueData;
            if (null != val) {
              if (val.HaveId()) {
                enumTypeDef.m_EnumMembers.Add(val.GetId(), nextValue);
                ++nextValue;
              } else {
                LogSystem.Error("enum member {0} must have name ! line {1} file {2}", comp.ToScriptString(), comp.GetLine(), dslFile);
                haveError = true;
              }
            } else {
              CallData field = comp as CallData;
              if (null != field) {
                if (field.HaveId()) {
                  if (field.GetParamNum() >= 1) {
                    ISyntaxComponent param = field.GetParam(0);
                    string id = param.GetId();
                    int idType = param.GetIdType();
                    if (idType == ValueData.NUM_TOKEN) {
                      nextValue = int.Parse(id);
                    } else {
                      LogSystem.Error("enum member {0} value must be number ! line {1} file {2}", comp.ToScriptString(), comp.GetLine(), dslFile);
                      haveError = true;
                    }
                    enumTypeDef.m_EnumMembers.Add(field.GetId(), nextValue);
                  } else {
                    enumTypeDef.m_EnumMembers.Add(field.GetId(), nextValue);
                  }
                  ++nextValue;
                } else {
                  LogSystem.Error("enum member {0} must have name ! line {1} file {2}", comp.ToScriptString(), comp.GetLine(), dslFile);
                  haveError = true;
                }
              } else {
                LogSystem.Error("enum member {0} must end with ';' ! line {1} file {2}", comp.ToScriptString(), comp.GetLine(), dslFile);
                haveError = true;
              }
            }
          }
        } else {
          LogSystem.Error("enum {0} must have 1 or 2 params (name and group) ! line {1} file {2}", funcData.ToScriptString(), funcData.GetLine(), dslFile);
          haveError = true;
        }
      }
    }
    private static string ParseMessage(string dslFile, FunctionData funcData, string defGroupName, bool isInnerMessage, SortedDictionary<string, MessageDef> messages, ref bool haveError)
    {
      string typeName = null;
      if (null != funcData) {
        CallData callData = funcData.Call;
        if (null != callData && callData.GetParamNum() >= 1) {
          string groupName;
          typeName = callData.GetParamId(0);
          if (callData.GetParamNum() > 1) {
            groupName = callData.GetParamId(1);
          } else {
            groupName = defGroupName;
          }

          MessageDef messageDef = new MessageDef();
          messageDef.m_TypeName = typeName;
          messageDef.m_WrapName = typeName + "Wrap";
          messageDef.m_GroupName = groupName;
          messageDef.m_DontGenEnum = isInnerMessage;
          messageDef.m_DontGenDB = isInnerMessage;
          if (messages.ContainsKey(typeName)) {
            messages[typeName] = messageDef;
          } else {
            messages.Add(typeName, messageDef);
          }

          int nextOrder = 1;
          foreach (ISyntaxComponent comp in funcData.Statements) {
            CallData field = comp as CallData;
            if (null != field) {
              if (field.GetId() == "option") {
                if (field.GetParamId(0) == "dontgenenum")
                  messageDef.m_DontGenEnum = true;
                else if (field.GetParamId(0) == "dontgendb")
                  messageDef.m_DontGenDB = true;
              } else if (field.GetId() == "enumvalue") {
                messageDef.m_EnumValue = field.GetParamId(0);
              } else if (field.GetId() == "wrapname") {
                messageDef.m_WrapName = field.GetParamId(0);
              } else if (field.GetId() == "member") {
                if (field.GetParamNum() >= 3) {
                  MemberDef memberDef = new MemberDef();
                  memberDef.m_MemberName = field.GetParamId(0);
                  memberDef.m_TypeName = field.GetParamId(1);
                  memberDef.m_Modifier = field.GetParamId(2);
                  memberDef.m_Order = nextOrder++;
                  messageDef.m_Members.Add(memberDef);
                  messageDef.m_ColumnNames.Add(memberDef.m_MemberName);
                } else {
                  LogSystem.Error("member {0} must have name、type and modifier ! line {1} file {2}", comp.ToScriptString(), comp.GetLine(), dslFile);
                  haveError = true;
                }
              }
            } else {
              FunctionData customCode = comp as FunctionData;
              if (null != customCode) {
                if (customCode.GetId() == "enum") {
                  ParseEnum(dslFile, customCode, messageDef.m_GroupName, messageDef.m_EnumTypes, ref haveError);
                } if (customCode.GetId() == "message") {
                  ParseMessage(dslFile, customCode, messageDef.m_GroupName, true, messageDef.m_Messages, ref haveError);
                } if (customCode.GetId() == "member") {
                  CallData customField = customCode.Call;
                  if (null != customField && customField.GetParamNum() >= 3) {
                    MemberDef memberDef = new MemberDef();
                    memberDef.m_MemberName = customField.GetParamId(0);
                    memberDef.m_TypeName = customField.GetParamId(1);
                    memberDef.m_Modifier = customField.GetParamId(2);
                    memberDef.m_Order = nextOrder++;
                    messageDef.m_Members.Add(memberDef);
                    messageDef.m_ColumnNames.Add(memberDef.m_MemberName);

                    foreach (ISyntaxComponent comp2 in customCode.Statements) {
                      ValueData val = comp2 as ValueData;
                      if (null != val) {
                        if (val.GetId() == "primarykey") {
                          memberDef.m_IsPrimaryKey = true;
                          messageDef.m_PrimaryKeys.Add(memberDef.m_MemberName);
                        } else if (val.GetId() == "foreignkey") {
                          memberDef.m_IsForeignKey = true;
                          messageDef.m_ForeignKeys.Add(memberDef.m_MemberName);
                        }
                      } else {
                        CallData item = comp2 as CallData;
                        if (null != item) {
                          if (item.GetId() == "default") {
                            memberDef.m_Default = item.GetParamId(0);
                          } else if (item.GetId() == "maxsize") {
                            memberDef.m_MaxSize = int.Parse(item.GetParamId(0));
                          } else if (item.GetId() == "primarykey") {
                            memberDef.m_IsPrimaryKey = true;
                            messageDef.m_PrimaryKeys.Add(memberDef.m_MemberName);
                          } else if (item.GetId() == "foreignkey") {
                            memberDef.m_IsForeignKey = true;
                            messageDef.m_ForeignKeys.Add(memberDef.m_MemberName);
                          }
                        } else {
                          LogSystem.Error("message member options {0} must end with ';' ! line {1} file {2}", comp2.ToScriptString(), comp2.GetLine(), dslFile);
                          haveError = true;
                        }
                      }
                    }
                  }
                }
              } else {
                LogSystem.Error("message member {0} must have params and end with ';' ! line {1} file {2}", comp.ToScriptString(), comp.GetLine(), dslFile);
                haveError = true;
              }
            }
          }
        } else {
          LogSystem.Error("message {0} must have 1 or 2 params (name and group) ! line {1} file {2}", funcData.ToScriptString(), funcData.GetLine(), dslFile);
          haveError = true;
        }
      }
      return typeName;
    }
    private static bool FindEnumTypeDef(MessageDef messageDef, string enumName, out EnumTypeDef ret)
    {
      bool find = messageDef.m_EnumTypes.TryGetValue(enumName, out ret);
      if (!find) {
        foreach (var pair in messageDef.m_Messages) {
          find = FindEnumTypeDef(pair.Value, enumName, out ret);
          if(find) {
            break;
          }
        }
      }
      return find;
    }
    private static bool FindMessageDef(MessageDef messageDef, string messageName, out MessageDef ret)
    {
      bool find = messageDef.m_Messages.TryGetValue(messageName, out ret);
      if (!find) {
        foreach (var pair in messageDef.m_Messages) {
          find = FindMessageDef(pair.Value, messageName, out ret);
          if (find) {
            break;
          }
        }
      }
      return find;
    }

    private static Dictionary<string, string> s_Type2ProtoTypes = new Dictionary<string, string>();
    private static Dictionary<string, string> s_Type2SqlTypes = new Dictionary<string, string>();
    private static Dictionary<string, string> s_Type2MySqlClientTypes = new Dictionary<string, string>();
  }
  internal class Program
  {
    static void Main(string[] args)
    {
      LogSystem.OnOutput = (Log_Type type, string msg) => {
        Console.WriteLine("{0}", msg);
      };
      ResourceReadProxy.OnReadAsArray = (string path) => {
        byte[] buffer = null;
        try {
          buffer = File.ReadAllBytes(path);
        } catch {
        }
        return buffer;
      };
      if (args.Length == 1 && args[0] == "genmsg") {
        LogSystem.Info("GenAllMessage");
        GenMessageData();
      } else {
        LogSystem.Info("GenMessageAndProto");
        GenDataAndProto();
      }
    }

    private static void GenMessageData()
    {
      GenDataMessage();
      GenBigwordMessage();
      //GenBillingMessage();
      GenClientLobbyMessage();
      GenClientLobbyGmMessage();
      GenClientRoomMessage();
    }
    private static void GenDataAndProto()
    {
      GenServerData();
    }

    private static void GenServerData()
    {
      MessageDslParser parser = new MessageDslParser();
      parser.Init("DataProto/Data.dsl");
      parser.GenAllMessageWraps("DataProto/Data.cs", string.Empty);
      parser.GenAllMessagesEnum("DataProto/DataEnum.cs", "DataEnum", string.Empty);
      parser.GenAllMessageProtos("DataProto/Data.proto", string.Empty);
      parser.GenAllMessageDDL("DataProto/CreateDataTables.sql", string.Empty);
      parser.GenAllMessageDML("DataProto/DataDML.cs", "DataEnum", string.Empty);
    }

    private static void GenDataMessage()
    {
      MessageDslParser parser = new MessageDslParser();
      parser.Init("ProtoFiles/DataMessageDefine.dsl");
      parser.GenAllMessagesEnum("ProtoFiles/DataMessageDefineEnum.cs", "DataMessageEnum", string.Empty);
      parser.GenAllMessageProtos("ProtoFiles/DataMessageDefine.proto", string.Empty);
    }
    private static void GenBigwordMessage()
    {
      MessageDslParser parser = new MessageDslParser();
      parser.Init("ProtoFiles/BigworldAndRoomServer.dsl");
      parser.GenAllMessagesEnum("ProtoFiles/BigworldAndRoomServerMessageEnum.cs", "BigworldAndRoomServerMessageEnum", string.Empty);
      parser.GenAllMessageProtos("ProtoFiles/BigworldAndRoomServer.proto", string.Empty);
    }
    private static void GenBillingMessage()
    {
      MessageDslParser parser = new MessageDslParser();
      parser.Init("ProtoFiles/Billing.dsl");
      parser.GenAllMessagesEnum("ProtoFiles/BillingEnum.cs", "BillingMessageEnum", string.Empty);
      parser.GenAllMessageProtos("ProtoFiles/Billing.proto", string.Empty);
    }
    private static void GenClientLobbyMessage()
    {
      MessageDslParser parser = new MessageDslParser();
      parser.Init("ProtoFiles/LobbyMsg.dsl");
      parser.GenAllMessagesEnum("ProtoFiles/LobbyMsgEnum.cs", "LobbyMessageDefine", string.Empty);
      parser.GenAllJsMessagesEnum("ProtoFiles/LobbyMsgEnum.js", "LobbyMessageDefine", string.Empty);
      parser.GenAllMessageProtos("ProtoFiles/LobbyMsg.proto", string.Empty);
    }
    private static void GenClientLobbyGmMessage()
    {
        MessageDslParser parser = new MessageDslParser();
        parser.Init("ProtoFiles/LobbyGmMsg.dsl");
        parser.GenAllMessagesEnum("ProtoFiles/LobbyGmMsgEnum.cs", "LobbyGmMessageDefine", string.Empty);
        parser.GenAllJsMessagesEnum("ProtoFiles/LobbyGmMsgEnum.js", "LobbyGmMessageDefine", string.Empty);
        parser.GenAllMessageProtos("ProtoFiles/LobbyGmMsg.proto", string.Empty);
    }
    private static void GenClientRoomMessage()
    {
      MessageDslParser parser = new MessageDslParser();
      parser.Init("ProtoFiles/RoomMsg.dsl");
      parser.GenAllMessagesEnum("ProtoFiles/RoomMsgEnum.cs", "RoomMessageDefine", string.Empty);
      parser.GenAllMessageProtos("ProtoFiles/RoomMsg.proto", string.Empty);
    }
    
    private static void ConvertToUtf8(string file, string destfile)
    {
      Encoding ansi = Encoding.GetEncoding(936);
      File.WriteAllText(destfile, File.ReadAllText(file, ansi), Encoding.UTF8);
    }

  }
}
