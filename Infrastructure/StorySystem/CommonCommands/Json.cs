using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
namespace StorySystem.CommonCommands
{
    /// <summary>
    /// jsonadd(json,val);
    /// </summary>
    internal sealed class JsonAddCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            JsonAddCommand cmd = new JsonAddCommand();
            cmd.m_Var = m_Var.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Var.Evaluate(instance, iterator, args);
            m_Value.Evaluate(instance, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_Var.HaveValue && m_Value.HaveValue) {
                object obj = m_Var.Value;
                JsonData json = obj as JsonData;
                if (null != json) {
                    json.Add(m_Value.Value);
                }
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Var.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
        }
        private IStoryValue<object> m_Var = new StoryValue();
        private IStoryValue<object> m_Value = new StoryValue();
    }
    /// <summary>
    /// jsonset(json,key/index,val);
    /// </summary>
    internal sealed class JsonSetCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            JsonSetCommand cmd = new JsonSetCommand();
            cmd.m_Var = m_Var.Clone();
            cmd.m_Key = m_Key.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Var.Evaluate(instance, iterator, args);
            m_Key.Evaluate(instance, iterator, args);
            m_Value.Evaluate(instance, iterator, args);
        
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_Var.HaveValue && m_Key.HaveValue && m_Value.HaveValue) {
                object obj = m_Var.Value;
                JsonData json = obj as JsonData;
                object key = m_Key.Value;
                if (null != json && null != key) {
                    if (key is int) {
                        json[(int)key] = ToJsonData(m_Value.Value);
                    } else if (key is float) {
                        json[(int)(float)key] = ToJsonData(m_Value.Value);
                    } else if (key is string) {
                        json[(string)key] = ToJsonData(m_Value.Value);
                    } else {
                        //error
                    }
                }
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_Var.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
                m_Value.InitFromDsl(callData.GetParam(2));
            }
        }
        private JsonData ToJsonData(object obj)
        {
            if (obj == null)
                return null;
            if (obj is JsonData)
                return (JsonData)obj;
            return new JsonData(obj);
        }
        private IStoryValue<object> m_Var = new StoryValue();
        private IStoryValue<object> m_Key = new StoryValue();
        private IStoryValue<object> m_Value = new StoryValue();
    }
    /// <summary>
    /// jsonremove(json,key/index);
    /// </summary>
    internal sealed class JsonRemoveCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            JsonRemoveCommand cmd = new JsonRemoveCommand();
            cmd.m_Var = m_Var.Clone();
            cmd.m_Key = m_Key.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Var.Evaluate(instance, iterator, args);
            m_Key.Evaluate(instance, iterator, args);
        
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_Var.HaveValue && m_Key.HaveValue) {
                object obj = m_Var.Value;
                JsonData json = obj as JsonData;
                object key = m_Key.Value;
                if (null != json && null != key) {
                    if (key is int && json.IsArray) {
                        IList list = json as IList;
                        if (null != list) {
                            list.RemoveAt((int)key);
                        }
                    } else if (key is float && json.IsArray) {
                        IList list = json as IList;
                        if (null != list) {
                            list.RemoveAt((int)(float)key);
                        }
                    } else if (key is string && json.IsObject) {
                        IDictionary dict = json as IDictionary;
                        dict.Remove((string)key);
                    } else {
                        //error
                    }
                }
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Var.InitFromDsl(callData.GetParam(0));
                m_Key.InitFromDsl(callData.GetParam(1));
            }
        }
        private IStoryValue<object> m_Var = new StoryValue();
        private IStoryValue<object> m_Key = new StoryValue();
    }
}
