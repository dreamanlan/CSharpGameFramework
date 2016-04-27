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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_AttrName.HaveValue) {
                    string name = m_AttrName.Value;
                    m_HaveValue = true;
                    if (!scene.SceneContext.BlackBoard.TryGetVariable(name, out m_Value)) {
                        if (m_ParamNum > 1) {
                            m_Value = m_DefaultValue.Value;
                        }
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_Index.HaveValue) {
                    m_HaveValue = true;
                    int index = m_Index.Value;
                    m_Value = null;
                }
            }
        }

        private IStoryValue<int> m_Index = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetAiDataValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getaidata") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_ObjId.InitFromDsl(callData.GetParam(0));
                    m_DataType.InitFromDsl(callData.GetParam(1));
                    flag |= m_DataType.Flag;
                    flag |= m_ObjId.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetAiDataValue val = new GetAiDataValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_DataType = m_DataType.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_ObjId.Substitute(iterator, args);
                m_DataType.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_DataType.Evaluate(instance);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue && m_DataType.HaveValue) {
                    m_HaveValue = true;
                    m_Value = null;
                    int objId = m_ObjId.Value;
                    string typeName = m_DataType.Value;
                    EntityInfo npc = scene.SceneContext.GetEntityById(objId);
                    if (null != npc) {
                        if (typeName == "AiData_General") {
                            AiData_General data = npc.GetAiStateInfo().AiDatas.GetData<AiData_General>();
                            if (null == data) {
                                data = new AiData_General();
                                npc.GetAiStateInfo().AiDatas.AddData(data);
                            }
                            m_Value = data;
                        }
                    }
                }
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<string> m_DataType = new StoryValue<string>();
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
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_ObjId.InitFromDsl(callData.GetParam(0));
                    m_Flag |= m_ObjId.Flag;
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            GetLeaderIdValue val = new GetLeaderIdValue();
            val.m_ParamNum = m_ParamNum;
            val.m_ObjId = m_ObjId.Clone();
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
                if (m_ParamNum > 0)
                    m_ObjId.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            if (m_ParamNum > 0)
                m_ObjId.Evaluate(instance);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                m_HaveValue = true;
                if (m_ParamNum > 0) {
                    int objId = m_ObjId.Value;
                    EntityInfo npc = scene.GetEntityById(objId);
                    if (null != npc) {
                        m_Value = npc.GetAiStateInfo().LeaderID;
                    } else {
                        m_Value = 0;
                    }
                } else {
                    m_Value = 0;
                }
            }
        }

        private object m_Iterator = null;
        private object[] m_Args = null;

        private int m_ParamNum = 0;
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
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
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjId.InitFromDsl(callData.GetParam(0));
                    flag |= m_ObjId.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetLeaderLinkIdValue val = new GetLeaderLinkIdValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_ObjId.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    m_HaveValue = true;
                    int objId = m_ObjId.Value;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.GetLinkId();
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetMemberCountValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getmembercount") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjId.InitFromDsl(callData.GetParam(0));
                    flag |= m_ObjId.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetMemberCountValue val = new GetMemberCountValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            val.m_Flag = m_Flag;
            return val;
        }
        public void Substitute(object iterator, object[] args)
        {
            m_HaveValue = false;
            if (StoryValueHelper.HaveArg(Flag)) {
                m_ObjId.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    m_HaveValue = true;
                    int objId = m_ObjId.Value;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        User user = obj.CustomData as User;
                        if (null != user && null != user.LobbyUserData) {
                            m_Value = user.LobbyUserData.Members.Count;
                        } else {
                            m_Value = 0;
                        }
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetMemberLinkIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getmemberlinkid") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_ObjId.InitFromDsl(callData.GetParam(0));
                    m_Index.InitFromDsl(callData.GetParam(1));
                    flag |= m_ObjId.Flag;
                    flag |= m_Index.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetMemberLinkIdValue val = new GetMemberLinkIdValue();
            val.m_ObjId = m_ObjId.Clone();
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
                m_ObjId.Substitute(iterator, args);
                m_Index.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue && m_Index.HaveValue) {
                    m_HaveValue = true;
                    int objId = m_ObjId.Value;
                    int index = m_Index.Value;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        User user = obj.CustomData as User;
                        if (null != user && null != user.LobbyUserData && index >= 0 && index < user.LobbyUserData.Members.Count) {
                            m_Value = user.LobbyUserData.Members[index].Hero;
                        } else {
                            m_Value = 0;
                        }
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_Index = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetMemberLevelValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getmemberlevel") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_ObjId.InitFromDsl(callData.GetParam(0));
                    m_Index.InitFromDsl(callData.GetParam(1));
                    flag |= m_ObjId.Flag;
                    flag |= m_Index.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetMemberLevelValue val = new GetMemberLevelValue();
            val.m_ObjId = m_ObjId.Clone();
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
                m_ObjId.Substitute(iterator, args);
                m_Index.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue && m_Index.HaveValue) {
                    m_HaveValue = true;
                    int objId = m_ObjId.Value;
                    int index = m_Index.Value;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        User user = obj.CustomData as User;
                        if (null != user && null != user.LobbyUserData && index >= 0 && index < user.LobbyUserData.Members.Count) {
                            m_Value = user.LobbyUserData.Members[index].Level;
                        } else {
                            m_Value = 0;
                        }
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_Index = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
}
