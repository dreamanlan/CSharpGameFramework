using System;
using System.Collections.Generic;
using ScriptRuntime;
using StorySystem;
using GameFramework;

namespace GameFramework.Story.Values
{
    internal sealed class BlackboardGetValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "blackboardget") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_AttrName.InitFromDsl(callData.GetParam(0));
                    flag |= m_AttrName.Flag;
                }
                if (m_ParamNum > 1) {
                    m_DefaultValue.InitFromDsl(callData.GetParam(1));
                    flag |= m_DefaultValue.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            BlackboardGetValue val = new BlackboardGetValue();
            val.m_ParamNum = m_ParamNum;
            val.m_AttrName = m_AttrName.Clone();
            val.m_DefaultValue = m_DefaultValue.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
                if (m_ParamNum > 0) {
                    m_AttrName.Substitute(iterator, args);
                }
                if (m_ParamNum > 1) {
                    m_DefaultValue.Substitute(iterator, args);
                }
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            if (m_ParamNum > 0) {
                m_AttrName.Evaluate(instance);
            }
            if (m_ParamNum > 1) {
                m_DefaultValue.Evaluate(instance);
            }
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_AttrName.HaveValue) {
                string name = m_AttrName.Value;
                m_HaveValue = true;
                if (!ClientModule.Instance.BlackBoard.TryGetVariable(name, out m_Value)) {
                    if (m_ParamNum > 1) {
                        m_Value = m_DefaultValue.Value;
                    }
                }
            }
        }

        private object m_Iterator = null;
        private object[] m_Args = null;

        private int m_ParamNum = 0;
        private IStoryValue<string> m_AttrName = new StoryValue<string>();
        private IStoryValue<object> m_DefaultValue = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetDialogItemValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getdialogitem") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_DlgId.InitFromDsl(callData.GetParam(0));
                    m_Index.InitFromDsl(callData.GetParam(1));
                    flag |= m_DlgId.Flag;
                    flag |= m_Index.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetDialogItemValue val = new GetDialogItemValue();
            val.m_DlgId = m_DlgId.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_DlgId.Substitute(iterator, args);
                m_Index.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_DlgId.Evaluate(instance);
            m_Index.Evaluate(instance);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_DlgId.HaveValue && m_Index.HaveValue) {
                m_HaveValue = true;
                int dlgId = m_DlgId.Value;
                int index = m_Index.Value;
                int dlgItemId = TableConfigUtility.GenStoryDlgItemId(dlgId, index);
                TableConfig.StoryDlg cfg = TableConfig.StoryDlgProvider.Instance.GetStoryDlg(dlgItemId);
                if (null != cfg) {
                    m_Value = cfg;
                } else {
                    m_Value = null;
                }
            }
        }

        private IStoryValue<int> m_DlgId = new StoryValue<int>();
        private IStoryValue<int> m_Index = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetActorIconValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getactoricon") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_Index.InitFromDsl(callData.GetParam(0));
                    flag |= m_Index.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetActorIconValue val = new GetActorIconValue();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_Index.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_Index.Evaluate(instance);
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_Index.HaveValue) {
                m_HaveValue = true;
                int index = m_Index.Value;
                UnityEngine.Sprite obj = SpriteManager.GetActorIcon(index);
                if (null != obj) {
                    m_Value = obj;
                } else {
                    m_Value = null;
                }
            }
        }

        private IStoryValue<int> m_Index = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetLeaderLinkIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getleaderlinkid") {
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetLeaderLinkIdValue val = new GetLeaderLinkIdValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            m_HaveValue = true;
            m_Value = ClientModule.Instance.LeaderLinkID;
        }

        private object m_Iterator = null;
        private object[] m_Args = null;

        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetLeaderIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getleaderid") {
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetLeaderIdValue val = new GetLeaderIdValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            m_HaveValue = true;
            m_Value = ClientModule.Instance.LeaderID;
        }

        private object m_Iterator = null;
        private object[] m_Args = null;

        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetSummonerSkillIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getsummonerskillid") {
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetSummonerSkillIdValue val = new GetSummonerSkillIdValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Iterator = iterator;
            m_Args = args;
            if (StoryValueHelper.HaveArg(Flag)) {
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Value;
            }
        }
        public int Flag
        {
            get
            {
                return m_Flag;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            m_HaveValue = true;
            m_Value = ClientModule.Instance.SummonerSkillId;
        }

        private object m_Iterator = null;
        private object[] m_Args = null;

        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
}
