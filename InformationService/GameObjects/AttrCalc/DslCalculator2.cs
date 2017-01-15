using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;

namespace GameFramework.AttrCalc
{
    public sealed class DslCalculator2
    {
        public void Load(string dslFile)
        {
            Dsl.DslFile file = new Dsl.DslFile();
            string path = HomePath.GetAbsolutePath(dslFile);
            if (file.Load(path, LogSystem.Log)) {
                foreach (Dsl.DslInfo info in file.DslInfos) {
                    Load(info);
                }
            }
        }
        public long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, string proc, params long[] args)
        {
            if (GlobalVariables.s_EnableCalculatorLog) {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < args.Length; ++i) {
                    sb.Append(args[i].ToString());
                    if (i + 1 < args.Length) {
                        sb.Append(",");
                    }
                }
                LogSystem.Info("[calculator] begin calc proc {0} ({1})", proc, sb.ToString());
            }
            long ret = 0;
            m_Variables.Clear();
            List<Interpreter> exps;
            if (m_Procs.TryGetValue(proc, out exps)) {
                for (int i = 0; i < exps.Count; ++i) {
                    ret = exps[i].Calc(context, source, target, args, m_Variables);
                    if (GlobalVariables.s_EnableCalculatorLog) {
                        LogSystem.Info("[calculator] calc proc {0} index {1} result {2}", proc, i, ret);
                    }
                }
            }
            if (GlobalVariables.s_EnableCalculatorLog) {
                LogSystem.Info("[calculator] end calc proc {0}", proc);
            }
            return ret;
        }

        private void Load(Dsl.DslInfo info)
        {
            Dsl.FunctionData func = info.First;
            string id = func.Call.GetParamId(0);
            List<Interpreter> list;
            if (!m_Procs.TryGetValue(id, out list)) {
                list = new List<Interpreter>();
                m_Procs.Add(id, list);
            }
            foreach (Dsl.ISyntaxComponent comp in func.Statements) {
                Load(list, comp);
            }
        }
        private void Load(List<Interpreter> list, Dsl.ISyntaxComponent comp)
        {
            Interpreter ip = new Interpreter();
            ip.Load(comp);
            list.Add(ip);
        }

        private enum InsEnum : int
        {
            ATTRSET = 0,
            VARSET,
            VAR,
            CONST,
            ATTR,
            ATTR2,
            VALUE,
            ARG,
            ADD,
            SUB,
            MUL,
            DIV,
            MOD,
            MAX,
            MIN,
            ABS,
            CLAMP,
        }
        private class Instruction
        {
            internal InsEnum Opcode;
            internal long Operand;

            internal Instruction(InsEnum op)
            {
                Opcode = op;
                Operand = 0;
            }
            internal Instruction(InsEnum op, long v)
            {
                Opcode = op;
                Operand = v;
            }
        }
        private class Interpreter
        {
            internal void Load(Dsl.ISyntaxComponent comp)
            {
                Dsl.CallData callData = comp as Dsl.CallData;
                if (null != callData) {
                    if (!callData.HaveId()) {
                        Dsl.ISyntaxComponent param = callData.GetParam(0);
                        Load(param);
                    } else {
                        string op = callData.GetId();
                        if (op == "=") {//赋值
                            Dsl.CallData param1 = callData.GetParam(0) as Dsl.CallData;
                            Dsl.ISyntaxComponent param2 = callData.GetParam(1);
                            string name = param1.GetId();
                            int id = int.Parse(param1.GetParamId(0));
                            Load(param2);
                            if (name == "attr") {
                                m_Codes.Add(new Instruction(InsEnum.ATTRSET, id));
                            } else if (name == "var") {
                                m_Codes.Add(new Instruction(InsEnum.VARSET, id));
                            }
                        } else if (op == "var") {//读属性
                            int id = int.Parse(callData.GetParamId(0));
                            m_Codes.Add(new Instruction(InsEnum.VAR, id));
                        } else if (op == "attr") {//读属性
                            int id = int.Parse(callData.GetParamId(0));
                            m_Codes.Add(new Instruction(InsEnum.ATTR, id));
                        } else if (op == "attr2") {//读属性
                            int id = int.Parse(callData.GetParamId(0));
                            m_Codes.Add(new Instruction(InsEnum.ATTR2, id));
                        } else if (op == "value") {//读常量表
                            int id = int.Parse(callData.GetParamId(0));
                            m_Codes.Add(new Instruction(InsEnum.VALUE, id));
                        } else if (op == "arg") {//读参数值
                            int index = int.Parse(callData.GetParamId(0));
                            m_Codes.Add(new Instruction(InsEnum.ARG, index));
                        } else if(op=="const") {//明确标明的常量值
                            //普通常量
                            long val = long.Parse(callData.GetParamId(0));
                            m_Codes.Add(new Instruction(InsEnum.CONST, val));
                        } else {//二元及以上运算
                            Dsl.ISyntaxComponent param1 = callData.GetParam(0);
                            Dsl.ISyntaxComponent param2 = callData.GetParam(1);
                            Load(param1);
                            Load(param2);
                            if (op == "max") {
                                m_Codes.Add(new Instruction(InsEnum.MAX));
                            } else if (op == "min") {
                                m_Codes.Add(new Instruction(InsEnum.MIN));
                            } else if (op == "abs") {
                                m_Codes.Add(new Instruction(InsEnum.ABS));
                            } else if (op == "+") {
                                m_Codes.Add(new Instruction(InsEnum.ADD));
                            } else if (op == "-") {
                                m_Codes.Add(new Instruction(InsEnum.SUB));
                            } else if (op == "*") {
                                m_Codes.Add(new Instruction(InsEnum.MUL));
                            } else if (op == "/") {
                                m_Codes.Add(new Instruction(InsEnum.DIV));
                            } else if (op == "%") {
                                m_Codes.Add(new Instruction(InsEnum.MOD));
                            } else {//三元及以上运算
                                Dsl.ISyntaxComponent param3 = callData.GetParam(2);
                                Load(param3);
                                if (op == "clamp") {
                                    m_Codes.Add(new Instruction(InsEnum.CLAMP));
                                }
                            }
                        }
                    }
                } else {
                    Dsl.ValueData valueData = comp as Dsl.ValueData;
                    if (null != valueData && valueData.GetIdType() == Dsl.ValueData.NUM_TOKEN) {
                        //普通常量
                        long val = long.Parse(valueData.GetId());
                        m_Codes.Add(new Instruction(InsEnum.CONST, val));
                    }
                }
            }
            internal long Calc(SceneContextInfo context, CharacterProperty source, CharacterProperty target, long[] args, Dictionary<int, long> variables)
            {
                m_Stack.Clear();
                for (int i = 0; i < m_Codes.Count; ++i) {
                    Instruction ins = m_Codes[i];
                    switch (ins.Opcode) {
                        case InsEnum.CONST:
                            m_Stack.Push(ins.Operand);
                            break;
                        case InsEnum.ATTR: {
                                int id = (int)ins.Operand;
                                long val = source.GetLong((CharacterPropertyEnum)id);
                                m_Stack.Push(val);
                            }
                            break;
                        case InsEnum.ATTR2: {
                                int id = (int)ins.Operand;
                                long val = target.GetLong((CharacterPropertyEnum)id);
                                m_Stack.Push(val);
                            }
                            break;
                        case InsEnum.VALUE: {
                                int id = (int)ins.Operand;
                                TableConfig.Const cfg = TableConfig.ConstProvider.Instance.GetConst(id);
                                if (null != cfg) {
                                    m_Stack.Push(cfg.value);
                                } else {
                                    m_Stack.Push(0);
                                }
                            }
                            break;
                        case InsEnum.ARG: {
                                int index = (int)ins.Operand;
                                if (index >= 0 && index < args.Length) {
                                    m_Stack.Push(args[index]);
                                } else {
                                    m_Stack.Push(0);
                                }
                            }
                            break;
                        case InsEnum.VAR: {
                                int id = (int)ins.Operand;
                                long ret;
                                if (!variables.TryGetValue(id, out ret)) {
                                    ret = 0;
                                }
                                m_Stack.Push(ret);
                            }
                            break;
                        case InsEnum.ATTRSET: {
                                long op2 = m_Stack.Pop();
                                int id = (int)ins.Operand;
                                TableConfig.AttrDefine cfg = TableConfig.AttrDefineProvider.Instance.GetAttrDefine(id);
                                if (null != cfg) {
                                    if (op2 < cfg.minValue)
                                        op2 = cfg.minValue;
                                    if (op2 > cfg.maxValue)
                                        op2 = cfg.maxValue;
                                }
                                source.SetLong((CharacterPropertyEnum)id, op2);
                                m_Stack.Push(op2);
                            }
                            break;
                        case InsEnum.VARSET: {
                                long op2 = m_Stack.Pop();
                                int id = (int)ins.Operand;
                                variables[id] = op2;
                                m_Stack.Push(op2);
                            }
                            break;
                        case InsEnum.ADD: {
                                long op2 = m_Stack.Pop();
                                long op1 = m_Stack.Pop();
                                m_Stack.Push(op1 + op2);
                            }
                            break;
                        case InsEnum.SUB: {
                                long op2 = m_Stack.Pop();
                                long op1 = m_Stack.Pop();
                                m_Stack.Push(op1 - op2);
                            }
                            break;
                        case InsEnum.MUL: {
                                long op2 = m_Stack.Pop();
                                long op1 = m_Stack.Pop();
                                m_Stack.Push(op1 * op2);
                            }
                            break;
                        case InsEnum.DIV: {
                                long op2 = m_Stack.Pop();
                                long op1 = m_Stack.Pop();
                                m_Stack.Push(op1 / op2);
                            }
                            break;
                        case InsEnum.MOD: {
                                long op2 = m_Stack.Pop();
                                long op1 = m_Stack.Pop();
                                m_Stack.Push(op1 % op2);
                            }
                            break;
                        case InsEnum.MAX: {
                                long op2 = m_Stack.Pop();
                                long op1 = m_Stack.Pop();
                                m_Stack.Push(op1 >= op2 ? op1 : op2);
                            }
                            break;
                        case InsEnum.MIN: {
                                long op2 = m_Stack.Pop();
                                long op1 = m_Stack.Pop();
                                m_Stack.Push(op1 <= op2 ? op1 : op2);
                            }
                            break;
                        case InsEnum.ABS: {
                                long op2 = m_Stack.Pop();
                                m_Stack.Push(op2 >= 0 ? op2 : -op2);
                            }
                            break;
                        case InsEnum.CLAMP: {
                                long op3 = m_Stack.Pop();
                                long op2 = m_Stack.Pop();
                                long op1 = m_Stack.Pop();
                                if (op3 < op1)
                                    m_Stack.Push(op1);
                                else if (op3 > op2)
                                    m_Stack.Push(op2);
                                else
                                    m_Stack.Push(op3);
                            }
                            break;
                    }
                }
                return m_Stack.Pop();
            }

            private Stack<long> m_Stack = new Stack<long>();
            private List<Instruction> m_Codes = new List<Instruction>();
        }

        private Dictionary<string, List<Interpreter>> m_Procs = new Dictionary<string, List<Interpreter>>();
        private Dictionary<int, long> m_Variables = new Dictionary<int, long>();
    }
}
