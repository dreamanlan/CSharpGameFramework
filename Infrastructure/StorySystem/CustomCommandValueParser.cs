using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GameFramework;
namespace StorySystem
{
    public static class CustomCommandValueParser
    {
        public static Dsl.DslFile LoadStory(string file)
        {
            if (!string.IsNullOrEmpty(file)) {
                Dsl.DslFile dataFile = new Dsl.DslFile();
                var bytes = new byte[Dsl.DslFile.c_BinaryIdentity.Length];
                using (var fs = File.OpenRead(file)) {
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Close();
                }
                var id = System.Text.Encoding.ASCII.GetString(bytes);
                if (id == Dsl.DslFile.c_BinaryIdentity) {
                    try {
                        dataFile.LoadBinaryFile(file);
                        return dataFile;
                    } catch {
                    }
                } else {
                    try {
                        if (dataFile.Load(file, LogSystem.Log)) {
                            return dataFile;
                        } else {
                            LogSystem.Error("LoadStory file:{0} failed", file);
                        }
                    } catch (Exception ex) {
                        LogSystem.Error("LoadStory file:{0} Exception:{1}\n{2}", file, ex.Message, ex.StackTrace);
                    }
                }
            }
            return null;
        }
        public static Dsl.DslFile LoadStoryText(string file, byte[] bytes)
        {
            if (Dsl.DslFile.IsBinaryDsl(bytes, 0)) {
                try {
                    Dsl.DslFile dataFile = new Dsl.DslFile();
                    dataFile.LoadBinaryCode(bytes);
                    return dataFile;
                } catch {
                    return null;
                }
            } else {
                string text = Converter.FileContent2Utf8String(bytes);
                try {
                    Dsl.DslFile dataFile = new Dsl.DslFile();
                    if (dataFile.LoadFromString(text, file, LogSystem.Log)) {
                        return dataFile;
                    } else {
                        LogSystem.Error("LoadStoryText text:{0} failed", file);
                    }
                } catch (Exception ex) {
                    LogSystem.Error("LoadStoryText text:{0} Exception:{1}\n{2}", text, ex.Message, ex.StackTrace);
                }
                return null;
            }
        }
        public static void FirstParse(params Dsl.DslFile[] dataFiles)
        {
            for (int ix = 0; ix < dataFiles.Length; ++ix) {
                Dsl.DslFile dataFile = dataFiles[ix];
                FirstParse(dataFile.DslInfos);
            }
        }
        public static void FinalParse(params Dsl.DslFile[] dataFiles)
        {
            for (int ix = 0; ix < dataFiles.Length; ++ix) {
                Dsl.DslFile dataFile = dataFiles[ix];
                FinalParse(dataFile.DslInfos);
            }
        }
        public static void FirstParse(IList<Dsl.DslInfo> dslInfos)
        {
            for (int i = 0; i < dslInfos.Count; i++) {
                Dsl.DslInfo dslInfo = dslInfos[i];
                FirstParse(dslInfo);
            }
        }
        public static void FinalParse(IList<Dsl.DslInfo> dslInfos)
        {
            for (int i = 0; i < dslInfos.Count; i++) {
                Dsl.DslInfo dslInfo = dslInfos[i];
                FinalParse(dslInfo);
            }
        }
        public static void FirstParse(Dsl.DslInfo dslInfo)
        {            
            string id = dslInfo.GetId();
            if (id == "command") {
                StorySystem.CommonCommands.CompositeCommand cmd = new CommonCommands.CompositeCommand();
                cmd.InitSharedData();
                Dsl.FunctionData first = dslInfo.First;
                cmd.Name = first.Call.GetParamId(0);

                for(int i = 1; i < dslInfo.GetFunctionNum(); ++i) {
                    var funcData = dslInfo.GetFunction(i);
                    var fid = funcData.GetId();
                    if (fid == "args") {
                        for (int ix = 0; ix < funcData.Call.GetParamNum(); ++ix) {
                            cmd.ArgNames.Add(funcData.Call.GetParamId(ix));
                        }
                    } else if(fid == "opts") {
                        for (int ix = 0; ix < funcData.GetStatementNum(); ++ix) {
                            var fcomp = funcData.GetStatement(ix);
                            var fcd = fcomp as Dsl.CallData;
                            if (null != fcd) {
                                cmd.OptArgs.Add(fcd.GetId(), fcd.GetParam(0));
                            }
                        }
                    } else if (fid == "body") {
                    } else {
                        LogSystem.Error("Command {0} unknown part '{1}'", cmd.Name, fid);
                    }
                }
                //注册
                StoryCommandManager.Instance.RegisterCommandFactory(cmd.Name, new CommonCommands.CompositeCommandFactory(cmd), true);
            } else if (id == "value") {
                StorySystem.CommonValues.CompositeValue val = new CommonValues.CompositeValue();
                val.InitSharedData();
                Dsl.FunctionData first = dslInfo.First;
                val.Name = first.Call.GetParamId(0);

                for (int i = 1; i < dslInfo.GetFunctionNum(); ++i) {
                    var funcData = dslInfo.GetFunction(i);
                    var fid = funcData.GetId();
                    if (fid == "args") {
                        for (int ix = 0; ix < funcData.Call.GetParamNum(); ++ix) {
                            val.ArgNames.Add(funcData.Call.GetParamId(ix));
                        }
                    } else if (fid == "ret") {
                        val.ReturnName = funcData.Call.GetParamId(0);
                    } else if (fid == "opts") {
                        for (int ix = 0; ix < funcData.GetStatementNum(); ++ix) {
                            var fcomp = funcData.GetStatement(ix);
                            var fcd = fcomp as Dsl.CallData;
                            if (null != fcd) {
                                val.OptArgs.Add(fcd.GetId(), fcd.GetParam(0));
                            }
                        }
                    } else if (fid == "body") {
                    } else {
                        LogSystem.Error("Value {0} unknown part '{1}'", val.Name, fid);
                    }
                }
                //注册
                StoryValueManager.Instance.RegisterValueFactory(val.Name, new CommonValues.CompositeValueFactory(val), true);
            }
        }
        public static void FinalParse(Dsl.DslInfo dslInfo)
        {
            string id = dslInfo.GetId();
            if (id == "command") {                        
                Dsl.FunctionData first = dslInfo.First;
                string name = first.Call.GetParamId(0);

                IStoryCommandFactory factory = StoryCommandManager.Instance.FindFactory(name);
                if (null != factory) {
                    StorySystem.CommonCommands.CompositeCommand cmd = factory.Create() as StorySystem.CommonCommands.CompositeCommand;
                    cmd.InitialCommands.Clear();

                    Dsl.FunctionData bodyFunc = null;
                    for (int i = 0; i < dslInfo.GetFunctionNum(); ++i) {
                        var funcData = dslInfo.GetFunction(i);
                        var fid = funcData.GetId();
                        if (funcData.HaveStatement() && fid != "opts") {
                            bodyFunc = funcData;
                        }
                    }
                    if (null != bodyFunc) {
                        for (int ix = 0; ix < bodyFunc.GetStatementNum(); ++ix) {
                            Dsl.ISyntaxComponent syntaxComp = bodyFunc.GetStatement(ix);
                            IStoryCommand sub = StoryCommandManager.Instance.CreateCommand(syntaxComp);
                            cmd.InitialCommands.Add(sub);
                        }
                    } else {
                        LogSystem.Error("Can't find command {0}'s body", name);
                    }
                } else {
                    LogSystem.Error("Can't find command {0}'s factory", name);
                }
            } else if (id == "value") {
                Dsl.FunctionData first = dslInfo.First;
                string name = first.Call.GetParamId(0);
                IStoryValueFactory factory = StoryValueManager.Instance.FindFactory(name);
                if (null != factory) {
                    StorySystem.CommonValues.CompositeValue val = factory.Build() as StorySystem.CommonValues.CompositeValue;
                    val.InitialCommands.Clear();

                    Dsl.FunctionData bodyFunc = null;
                    for (int i = 0; i < dslInfo.GetFunctionNum(); ++i) {
                        var funcData = dslInfo.GetFunction(i);
                        var fid = funcData.GetId();
                        if (funcData.HaveStatement() && fid != "opts") {
                            bodyFunc = funcData;
                        }
                    }
                    if (null != bodyFunc) {
                        for (int ix = 0; ix < bodyFunc.GetStatementNum(); ++ix) {
                            Dsl.ISyntaxComponent syntaxComp = bodyFunc.GetStatement(ix);
                            IStoryCommand sub = StoryCommandManager.Instance.CreateCommand(syntaxComp);
                            val.InitialCommands.Add(sub);
                        }
                    } else {
                        LogSystem.Error("Can't find value {0}'s body", name);
                    }
                } else {
                    LogSystem.Error("Can't find value {0}'s factory", name);
                }
            }
        }       
    }
}
