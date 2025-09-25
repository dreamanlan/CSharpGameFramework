using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetStoryScript
{
    public sealed class ScriptableDslHelper
    {
        public enum PairTagEnum
        {
            PAIR_TAG_IF = 1,
            PAIR_TAG_ELSEIF,
            PAIR_TAG_ELIF,
            PAIR_TAG_WHILE,
            PAIR_TAG_FOREACH,
            PAIR_TAG_LOOP,
            PAIR_TAG_LOOPI,
            PAIR_TAG_LOOPD,
            PAIR_TAG_LOOPLIST,
            PAIR_TAG_LAST = PAIR_TAG_LOOPLIST
        }

        public Dictionary<string, uint> NameTags { get => m_NameTags; set => m_NameTags = value; }
        public Dictionary<string, string> FirstLastKeyOfCompoundStatements { get => m_FirstLastKeyOfCompoundStatements; set => m_FirstLastKeyOfCompoundStatements = value; }
        public Dictionary<string, HashSet<string>> SuccessorsOfCompoundStatements { get => m_SuccessorsOfCompoundStatements; set => m_SuccessorsOfCompoundStatements = value; }
        public HashSet<string> CompoundStatements { get => m_CompoundStatements; set => m_CompoundStatements = value; }

        public void SetCallbacks(Dsl.DslFile file)
        {
            file.SetNameTags(NameTags);
            file.onGetToken = (ref Dsl.Common.DslAction dslAction, ref Dsl.Common.DslToken dslToken, ref string tok, ref short val, ref int line) => {
                if (tok == "return") {
                    var oldCurTok = dslToken.getCurToken();
                    var oldLastTok = dslToken.getLastToken();
                    if (dslToken.PeekNextValidChar(0) == ';')
                        return false;
                    //insert backtick char
                    dslToken.setCurToken("`");
                    dslToken.setLastToken(oldCurTok);
                    dslToken.enqueueToken(dslToken.getCurToken(), dslToken.getOperatorTokenValue(), line);
                    dslToken.setCurToken(oldCurTok);
                    dslToken.setLastToken(oldLastTok);
                    return true;
                }
                else if (tok == ")") {
                    if (dslAction.PeekPairTypeStack(out var tag) == Dsl.Common.PairTypeEnum.PAIR_TYPE_PARENTHESES) {
                        if (tag > 0) {
                            var oldCurTok = dslToken.getCurToken();
                            var oldLastTok = dslToken.getLastToken();
                            char nextChar = dslToken.PeekNextValidChar(0);
                            if (nextChar == '{' || nextChar == ',' || nextChar == ';')
                                return false;
                            //insert backtick char
                            dslToken.setCurToken("`");
                            dslToken.setLastToken(oldCurTok);
                            dslToken.enqueueToken(dslToken.getCurToken(), dslToken.getOperatorTokenValue(), line);
                            dslToken.setCurToken(oldCurTok);
                            dslToken.setLastToken(oldLastTok);
                            return true;
                        }
                    }
                }
                return false;
            };
            file.onBeforeAddFunction = (ref Dsl.Common.DslAction dslAction, Dsl.StatementData statement) => {
                //You can end the current statement here and start a new empty statement.
                string fid = statement.GetId();
                var func = statement.Last.AsFunction;
                if (null != func) {
                    string lid = func.GetId();
                    if (func.HaveStatement()) {
                        if (string.IsNullOrEmpty(fid) || FirstLastKeyOfCompoundStatements.TryGetValue(fid, out var lastKey) && lastKey == lid) {
                            //End the current statement and start a new empty statement.
                            dslAction.endStatement();
                            dslAction.beginStatement();
                            return true;
                        }
                    }
                }

                return false;
            };
            file.onAddFunction = (ref Dsl.Common.DslAction dslAction, Dsl.StatementData statement, Dsl.FunctionData function) => {
                //Do not change the program structure here. At this point, the "function" is still an empty function, and the real function information has not been filled in.
                //The difference between this and "onBeforeAddFunction" is that the "function" is constructed and added to the function table of the current statement.
                return false;
            };
            file.onBeforeEndStatement = (ref Dsl.Common.DslAction dslAction, Dsl.StatementData statement) => {
                //The statement can be split here.
                return false;
            };
            file.onEndStatement = (ref Dsl.Common.DslAction dslAction, ref Dsl.StatementData statement) => {
                //The entire statement can be replaced here, but do not modify other parts of the program structure. The difference between this and "onBeforeEndStatement" is that
                //the statement has been popped from the stack at this point, and the simplified statement will be added to the upper-level syntax unit in the future.
                return false;
            };
            file.onBeforeBuildOperator = (ref Dsl.Common.DslAction dslAction, Dsl.Common.OperatorCategoryEnum category, string op, Dsl.StatementData statement) => {
                //The statement can be split here.
                return false;
            };
            file.onBuildOperator = (ref Dsl.Common.DslAction dslAction, Dsl.Common.OperatorCategoryEnum category, string op, ref Dsl.StatementData statement) => {
                //The statement can be replaced here, but do not modify other syntax structures.
                return false;
            };
            file.onSetFunctionId = (ref Dsl.Common.DslAction dslAction, string name, Dsl.StatementData statement, Dsl.FunctionData function) => {
                //The statement can be split here.
                string sid = statement.GetId();
                var func = statement.Last.AsFunction;
                if (null != func && statement.GetFunctionNum() > 1) {
                    if (SuccessorsOfCompoundStatements.TryGetValue(sid, out var successors) && !successors.Contains(name)) {
                        statement.Functions.Remove(func);
                        dslAction.endStatement();
                        dslAction.beginStatement();
                        var stm = dslAction.getCurStatement();
                        stm.AddFunction(func);
                        return true;
                    }
                    else if (CompoundStatements.Contains(name)) {
                        statement.Functions.Remove(func);
                        dslAction.endStatement();
                        dslAction.beginStatement();
                        var stm = dslAction.getCurStatement();
                        stm.AddFunction(func);
                        return true;
                    }
                }
                return false;
            };
            file.onBeforeBuildHighOrder = (ref Dsl.Common.DslAction dslAction, Dsl.StatementData statement, Dsl.FunctionData function) => {
                //The statement can be split here.
                return false;
            };
            file.onBuildHighOrder = (ref Dsl.Common.DslAction dslAction, Dsl.StatementData statement, Dsl.FunctionData function) => {
                //The statement can be split here.
                return false;
            };
        }

        private Dictionary<string, uint> m_NameTags = new Dictionary<string, uint> {
                { "if", (int)PairTagEnum.PAIR_TAG_IF },
                { "elseif", (int)PairTagEnum.PAIR_TAG_ELSEIF },
                { "elif", (int)PairTagEnum.PAIR_TAG_ELIF },
                { "while", (int)PairTagEnum.PAIR_TAG_WHILE },
                { "foreach", (int)PairTagEnum.PAIR_TAG_FOREACH },
                { "loop", (int)PairTagEnum.PAIR_TAG_LOOP },
                { "loopi", (int)PairTagEnum.PAIR_TAG_LOOPI },
                { "loopd", (int)PairTagEnum.PAIR_TAG_LOOPD },
                { "looplist", (int)PairTagEnum.PAIR_TAG_LOOPLIST }
            };
        private Dictionary<string, string> m_FirstLastKeyOfCompoundStatements = new Dictionary<string, string> {
                { "if", "else" },
                { "while", "while" },
                { "foreach", "foreach" },
                { "loop", "loop" },
                { "loopi", "loopi" },
                { "loopd", "loopd" },
                { "looplist", "looplist" },
                { "struct", "struct" }
            };
        private Dictionary<string, HashSet<string>> m_SuccessorsOfCompoundStatements = new Dictionary<string, HashSet<string>> {
                { "if", new HashSet<string> { "elseif", "elif", "else" } },
                { "elseif", new HashSet<string> { "elseif", "elif", "else" } },
                { "elif", new HashSet<string> { "elseif", "elif", "else" } },
                { "else", new HashSet<string> { } },
                { "while", new HashSet<string> { } },
                { "foreach", new HashSet<string> { } },
                { "loop", new HashSet<string> { } },
                { "loopi", new HashSet<string> { } },
                { "loopd", new HashSet<string> { } },
                { "looplist", new HashSet<string> { } },
                { "struct", new HashSet<string> { } },
                { "script", new HashSet<string> { "args" } },
                { "story", new HashSet<string> { "args" } }
            };
        private HashSet<string> m_CompoundStatements = new HashSet<string> {
                "if",
                "while",
                "foreach",
                "loop",
                "loopi",
                "loopd",
                "looplist",
                "struct",
                "script",
                "story"
            };

        public static ScriptableDslHelper ForDslCalculator
        {
            get {
                if (null == s_ForDslCalculator) {
                    s_ForDslCalculator = new ScriptableDslHelper();
                }
                return s_ForDslCalculator;
            }
            set => s_ForDslCalculator = value;
        }
        public static ScriptableDslHelper ForStoryInstance
        {
            get {
                if (null == s_ForStoryInstance) {
                    s_ForStoryInstance = new ScriptableDslHelper();
                    var obj = s_ForStoryInstance;
                    obj.FirstLastKeyOfCompoundStatements.Add("local", "local");
                    obj.SuccessorsOfCompoundStatements.Add("command", new HashSet<string> { "args", "opts", "doc", "body" });
                    obj.SuccessorsOfCompoundStatements.Add("function", new HashSet<string> { "args", "ret", "opts", "doc", "body" });
                    obj.SuccessorsOfCompoundStatements.Add("onmessage", new HashSet<string> { "args", "comment", "comments", "body" });
                    obj.SuccessorsOfCompoundStatements.Add("onnamespacedmessage ", new HashSet<string> { "args", "comment", "comments", "body" });
                    obj.CompoundStatements.Add("command");
                    obj.CompoundStatements.Add("function");
                    obj.CompoundStatements.Add("onmessage");
                    obj.CompoundStatements.Add("onnamespacedmessage");
                    obj.CompoundStatements.Add("local");
                }
                return s_ForStoryInstance;
            }
            set => s_ForStoryInstance = value;
        }

        private static ScriptableDslHelper s_ForDslCalculator = null;
        private static ScriptableDslHelper s_ForStoryInstance = null;
    }
}
