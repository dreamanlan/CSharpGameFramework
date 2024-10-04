using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ScriptableFramework;
namespace DotnetStoryScript
{
    public static class CustomCommandFunctionParser
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
                        dataFile.LoadBinaryFile(file, StoryConfigManager.ReuseKeyBuffer, StoryConfigManager.ReuseIdBuffer);
                        return dataFile;
                    }
                    catch (Exception ex) {
                        var sb = new System.Text.StringBuilder();
                        sb.AppendFormat("[LoadStory] LoadStory file:{0} Exception:{1}\n{2}", file, ex.Message, ex.StackTrace);
                        sb.AppendLine();
                        Helper.LogInnerException(ex, sb);
                        LogSystem.Error("{0}", sb.ToString());
                    }
                }
                else {
                    try {
                        if (dataFile.Load(file, LogSystem.Log)) {
                            return dataFile;
                        }
                        else {
                            LogSystem.Error("LoadStory file:{0} failed", file);
                        }
                    }
                    catch (Exception ex) {
                        var sb = new System.Text.StringBuilder();
                        sb.AppendFormat("[LoadStory] LoadStory file:{0} Exception:{1}\n{2}", file, ex.Message, ex.StackTrace);
                        sb.AppendLine();
                        Helper.LogInnerException(ex, sb);
                        LogSystem.Error("{0}", sb.ToString());
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
                    dataFile.LoadBinaryCode(bytes, StoryConfigManager.ReuseKeyBuffer, StoryConfigManager.ReuseIdBuffer);
                    return dataFile;
                }
                catch (Exception ex) {
                    var sb = new System.Text.StringBuilder();
                    sb.AppendFormat("[LoadStory] LoadStoryText file:{0} Exception:{1}\n{2}", file, ex.Message, ex.StackTrace);
                    sb.AppendLine();
                    Helper.LogInnerException(ex, sb);
                    LogSystem.Error("{0}", sb.ToString());
                }
            }
            else {
                string text = Converter.FileContent2Utf8String(bytes);
                try {
                    Dsl.DslFile dataFile = new Dsl.DslFile();
                    if (dataFile.LoadFromString(text, LogSystem.Log)) {
                        return dataFile;
                    }
                    else {
                        LogSystem.Error("LoadStoryText file:{0} failed", file);
                    }
                }
                catch (Exception ex) {
                    var sb = new System.Text.StringBuilder();
                    sb.AppendFormat("[LoadStory] LoadStoryText file:{0} Exception:{1}\n{2}", file, ex.Message, ex.StackTrace);
                    sb.AppendLine();
                    Helper.LogInnerException(ex, sb);
                    LogSystem.Error("{0}", sb.ToString());
                }
            }
            return null;
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
        public static void FirstParse(IList<Dsl.ISyntaxComponent> dslInfos)
        {
            for (int i = 0; i < dslInfos.Count; i++) {
                Dsl.ISyntaxComponent dslInfo = dslInfos[i];
                FirstParse(dslInfo);
            }
        }
        public static void FinalParse(IList<Dsl.ISyntaxComponent> dslInfos)
        {
            for (int i = 0; i < dslInfos.Count; i++) {
                Dsl.ISyntaxComponent dslInfo = dslInfos[i];
                FinalParse(dslInfo);
            }
        }
        public static void FirstParse(Dsl.ISyntaxComponent dslInfo)
        {
            string id = dslInfo.GetId();
            if (id == "command") {
                string doc = string.Empty;
                CommonCommands.CompositeCommand cmd = new CommonCommands.CompositeCommand();
                cmd.InitSharedData();
                var first = dslInfo as Dsl.FunctionData;
                if (null != first) {
                    if (first.IsHighOrder) {
                        cmd.Name = first.LowerOrderFunction.GetParamId(0);
                    }
                    else {
                        cmd.Name = first.GetParamId(0);
                    }
                }
                else {
                    var statement = dslInfo as Dsl.StatementData;
                    if (null != statement) {
                        first = statement.First.AsFunction;
                        cmd.Name = first.GetParamId(0);
                        for (int i = 1; i < statement.GetFunctionNum(); ++i) {
                            var funcData = statement.GetFunction(i).AsFunction;
                            var cd = funcData;
                            if (funcData.IsHighOrder)
                                cd = funcData.LowerOrderFunction;
                            var fid = funcData.GetId();
                            if (fid == "args") {
                                for (int ix = 0; ix < cd.GetParamNum(); ++ix) {
                                    cmd.ArgNames.Add(cd.GetParamId(ix));
                                }
                            }
                            else if (fid == "opts") {
                                for (int ix = 0; ix < cd.GetParamNum(); ++ix) {
                                    var fcomp = cd.GetParam(ix);
                                    var fcd = fcomp as Dsl.FunctionData;
                                    if (null != fcd) {
                                        cmd.OptArgs.Add(fcd.GetId(), fcd.GetParam(0));
                                    }
                                }
                            }
                            else if (fid == "doc") {
                                doc = cd.GetParamId(0);
                            }
                            else if (fid == "body") {
                            }
                            else {
                                LogSystem.Error("Command {0} unknown part '{1}'", cmd.Name, fid);
                            }
                        }
                    }
                }
                //register
                StoryCommandManager.Instance.RegisterCommandFactory(cmd.Name, doc, new CommonCommands.CompositeCommandFactory(cmd), true);
            }
            else if (id == "function") {
                string doc = string.Empty;
                CommonFunctions.CompositeFunction val = new CommonFunctions.CompositeFunction();
                val.InitSharedData();
                var first = dslInfo as Dsl.FunctionData;
                if (null != first) {
                    if (first.IsHighOrder) {
                        val.Name = first.LowerOrderFunction.GetParamId(0);
                    }
                    else {
                        val.Name = first.GetParamId(0);
                    }
                }
                else {
                    var statement = dslInfo as Dsl.StatementData;
                    if (null != statement) {
                        first = statement.First.AsFunction;
                        val.Name = first.GetParamId(0);
                        for (int i = 1; i < statement.GetFunctionNum(); ++i) {
                            var funcData = statement.GetFunction(i).AsFunction;
                            var cd = funcData;
                            if (funcData.IsHighOrder) {
                                cd = funcData.LowerOrderFunction;
                            }
                            var fid = funcData.GetId();
                            if (fid == "args") {
                                for (int ix = 0; ix < cd.GetParamNum(); ++ix) {
                                    val.ArgNames.Add(cd.GetParamId(ix));
                                }
                            }
                            else if (fid == "ret") {
                                val.ReturnName = cd.GetParamId(0);
                            }
                            else if (fid == "opts") {
                                for (int ix = 0; ix < cd.GetParamNum(); ++ix) {
                                    var fcomp = cd.GetParam(ix);
                                    var fcd = fcomp as Dsl.FunctionData;
                                    if (null != fcd) {
                                        val.OptArgs.Add(fcd.GetId(), fcd.GetParam(0));
                                    }
                                }
                            }
                            else if (fid == "doc") {
                                doc = cd.GetParamId(0);
                            }
                            else if (fid == "body") {
                            }
                            else {
                                LogSystem.Error("Value {0} unknown part '{1}'", val.Name, fid);
                            }
                        }
                    }
                }
                //register
                StoryFunctionManager.Instance.RegisterFunctionFactory(val.Name, doc, new CommonFunctions.CompositeValueFactory(val), true);
            }
        }
        public static void FinalParse(Dsl.ISyntaxComponent dslInfo)
        {
            string id = dslInfo.GetId();
            if (id == "command") {
                string name = string.Empty;
                var first = dslInfo as Dsl.FunctionData;
                var statement = dslInfo as Dsl.StatementData;
                if (null != first) {
                    if (first.IsHighOrder) {
                        name = first.LowerOrderFunction.GetParamId(0);
                    }
                    else {
                        name = first.GetParamId(0);
                    }
                }
                else {
                    if (null != statement) {
                        first = statement.First.AsFunction;
                        name = first.GetParamId(0);
                    }
                }

                IStoryCommandFactory factory = StoryCommandManager.Instance.FindFactory(name);
                if (null != factory) {
                    CommonCommands.CompositeCommand cmd = factory.Create() as CommonCommands.CompositeCommand;
                    cmd.InitialCommands.Clear();

                    Dsl.FunctionData bodyFunc = null;
                    if (null != statement) {
                        for (int i = 0; i < statement.GetFunctionNum(); ++i) {
                            var funcData = statement.GetFunction(i).AsFunction;
                            var fid = funcData.GetId();
                            if (funcData.HaveStatement() && fid != "opts") {
                                bodyFunc = funcData;
                            }
                        }
                    }
                    else {
                        bodyFunc = first;
                    }
                    if (null != bodyFunc) {
                        for (int ix = 0; ix < bodyFunc.GetParamNum(); ++ix) {
                            Dsl.ISyntaxComponent syntaxComp = bodyFunc.GetParam(ix);
                            IStoryCommand sub = StoryCommandManager.Instance.CreateCommand(syntaxComp);
                            cmd.InitialCommands.Add(sub);
                        }
                    }
                    else {
                        LogSystem.Error("Can't find command {0}'s body", name);
                    }
                }
                else {
                    LogSystem.Error("Can't find command {0}'s factory", name);
                }
            }
            else if (id == "function") {
                string name = string.Empty;
                var first = dslInfo as Dsl.FunctionData;
                var statement = dslInfo as Dsl.StatementData;
                if (null != first) {
                    if (first.IsHighOrder) {
                        name = first.LowerOrderFunction.GetParamId(0);
                    }
                    else {
                        name = first.GetParamId(0);
                    }
                }
                else {
                    if (null != statement) {
                        first = statement.First.AsFunction;
                        name = first.GetParamId(0);
                    }
                }

                IStoryFunctionFactory factory = StoryFunctionManager.Instance.FindFactory(name);
                if (null != factory) {
                    CommonFunctions.CompositeFunction val = factory.Build() as CommonFunctions.CompositeFunction;
                    val.InitialCommands.Clear();

                    Dsl.FunctionData bodyFunc = null;
                    if (null != statement) {
                        for (int i = 0; i < statement.GetFunctionNum(); ++i) {
                            var funcData = statement.GetFunction(i).AsFunction;
                            var fid = funcData.GetId();
                            if (funcData.HaveStatement() && fid != "opts") {
                                bodyFunc = funcData;
                            }
                        }
                    }
                    else {
                        bodyFunc = first;
                    }
                    if (null != bodyFunc) {
                        for (int ix = 0; ix < bodyFunc.GetParamNum(); ++ix) {
                            Dsl.ISyntaxComponent syntaxComp = bodyFunc.GetParam(ix);
                            IStoryCommand sub = StoryCommandManager.Instance.CreateCommand(syntaxComp);
                            val.InitialCommands.Add(sub);
                        }
                    }
                    else {
                        LogSystem.Error("Can't find value {0}'s body", name);
                    }
                }
                else {
                    LogSystem.Error("Can't find value {0}'s factory", name);
                }
            }
        }
    }
}
