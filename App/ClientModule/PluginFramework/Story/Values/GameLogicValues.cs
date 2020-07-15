using System;
using System.Collections;
using System.Collections.Generic;
using ScriptRuntime;
using StorySystem;
using GameFramework;

namespace GameFramework.Story.Values
{
    internal sealed class BlackboardGetValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_AttrName.InitFromDsl(callData.GetParam(0));
                }
                if (m_ParamNum > 1) {
                    m_DefaultValue.InitFromDsl(callData.GetParam(1));
                }
            }
        }
        public IStoryValue Clone()
        {
            BlackboardGetValue val = new BlackboardGetValue();
            val.m_ParamNum = m_ParamNum;
            val.m_AttrName = m_AttrName.Clone();
            val.m_DefaultValue = m_DefaultValue.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0) {
                m_AttrName.Evaluate(instance, handler, iterator, args);
            }
            if (m_ParamNum > 1) {
                m_DefaultValue.Evaluate(instance, handler, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_AttrName.HaveValue) {
                string name = m_AttrName.Value;
                m_HaveValue = true;
                if (!PluginFramework.Instance.BlackBoard.TryGetVariable(name, out m_Value)) {
                    if (m_ParamNum > 1) {
                        m_Value = m_DefaultValue.Value;
                    }
                }
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<string> m_AttrName = new StoryValue<string>();
        private IStoryValue m_DefaultValue = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
	    internal sealed class OffsetSplineValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 2) {
                m_Spline.InitFromDsl(callData.GetParam(0));
                m_Offset.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryValue Clone()
        {
            OffsetSplineValue val = new OffsetSplineValue();
            val.m_Spline = m_Spline.Clone();
            val.m_Offset = m_Offset.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;

            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            {
                m_Spline.Evaluate(instance, handler, iterator, args);
                m_Offset.Evaluate(instance, handler, iterator, args);
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
        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_Spline.HaveValue && m_Offset.HaveValue) {
                var list = m_Spline.Value as IList<object>;
                Vector3 offset = m_Offset.Value;
                m_HaveValue = true;
                if (null != list) {
                    List<object> npts = new List<object>();
                    int ct = list.Count;
                    float dir = 0;
                    Vector3 curPt = Vector3.Zero;
                    for (int i = 0; i < ct; ++i) {
                        if (i == 0) {
                            curPt = (Vector3)list[i];
                        }
                        Vector3 pt = Vector3.Zero;
                        if (i + 1 < ct) {
                            pt = (Vector3)list[i + 1];
                            dir = Geometry.GetYRadian(curPt, pt);
                        }
                        npts.Add(curPt + Geometry.GetRotate(offset, dir));
                        curPt = pt;
                    }
                    m_Value = npts;
                } else {
                    m_Value = null;
                }
            }
        }
        private IStoryValue<List<object>> m_Spline = new StoryValue<List<object>>();
        private IStoryValue<Vector3> m_Offset = new StoryValue<Vector3>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class OffsetVector3Value : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && (callData.GetParamNum() == 2 || callData.GetParamNum() == 3)) {
                m_ParamNum = callData.GetParamNum();
                m_Pt.InitFromDsl(callData.GetParam(0));
                if (m_ParamNum == 3) {
                    m_Pt2.InitFromDsl(callData.GetParam(1));
                    m_Offset.InitFromDsl(callData.GetParam(2));
                } else {
                    m_Offset.InitFromDsl(callData.GetParam(1));
                }
            }
        }
        public IStoryValue Clone()
        {
            OffsetVector3Value val = new OffsetVector3Value();
            val.m_ParamNum = m_ParamNum;
            val.m_Pt = m_Pt.Clone();
            val.m_Pt2 = m_Pt2.Clone();
            val.m_Offset = m_Offset.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Pt.Evaluate(instance, handler, iterator, args);
            m_Pt2.Evaluate(instance, handler, iterator, args);
            m_Offset.Evaluate(instance, handler, iterator, args);
            TryUpdateValue();
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

        private void TryUpdateValue()
        {
            if (m_Pt.HaveValue) {
                m_HaveValue = true;
                Vector3 offset = m_Offset.Value;                
                Vector3 pt = m_Pt.Value;
                if (m_ParamNum == 3) {
                    Vector3 pt2 = m_Pt2.Value;
                    float dir = Geometry.GetYRadian(pt, pt2);
                    m_Value = pt + Geometry.GetRotate(offset, dir);
                } else {
                    m_Value = pt + offset;
                }
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<Vector3> m_Pt = new StoryValue<Vector3>();
        private IStoryValue<Vector3> m_Pt2 = new StoryValue<Vector3>();
        private IStoryValue<Vector3> m_Offset = new StoryValue<Vector3>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetDialogItemValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_DlgId.InitFromDsl(callData.GetParam(0));
                    m_Index.InitFromDsl(callData.GetParam(1));
                }
            }
        }
        public IStoryValue Clone()
        {
            GetDialogItemValue val = new GetDialogItemValue();
            val.m_DlgId = m_DlgId.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_DlgId.Evaluate(instance, handler, iterator, args);
            m_Index.Evaluate(instance, handler, iterator, args);
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
    }
    internal sealed class GetMonsterInfoValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_CampId.InitFromDsl(callData.GetParam(0));
                    m_Index.InitFromDsl(callData.GetParam(1));
                }
            }
        }
        public IStoryValue Clone()
        {
            GetMonsterInfoValue val = new GetMonsterInfoValue();
            val.m_CampId = m_CampId.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_CampId.Evaluate(instance, handler, iterator, args);
            m_Index.Evaluate(instance, handler, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_CampId.HaveValue && m_Index.HaveValue) {
                m_HaveValue = true;
                int sceneId = PluginFramework.Instance.SceneId;
                int campId = m_CampId.Value;
                int index = m_Index.Value;
                int monstersId = TableConfigUtility.GenLevelMonstersId(sceneId, campId);
                List<TableConfig.LevelMonster> monsterList;
                if (TableConfig.LevelMonsterProvider.Instance.TryGetValue(monstersId, out monsterList)) {
                    if (index >= 0 && index < monsterList.Count) {
                        m_Value = monsterList[index];
                    } else {
                        m_Value = null;
                    }
                } else {
                    m_Value = null;
                }
            }
        }

        private IStoryValue<int> m_CampId = new StoryValue<int>();
        private IStoryValue<int> m_Index = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetAiDataValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 1) {
                    m_ObjId.InitFromDsl(callData.GetParam(0));
                    m_DataType.InitFromDsl(callData.GetParam(1));
                }
            }
        }
        public IStoryValue Clone()
        {
            GetAiDataValue val = new GetAiDataValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_DataType = m_DataType.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_DataType.Evaluate(instance, handler, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_ObjId.HaveValue && m_DataType.HaveValue) {
                m_HaveValue = true;
                m_Value = null;
                int objId = m_ObjId.Value;
                string typeName = m_DataType.Value;
                EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
                if (null != npc) {
                }
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<string> m_DataType = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetActorIconValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_Index.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue Clone()
        {
            GetActorIconValue val = new GetActorIconValue();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Index.Evaluate(instance, handler, iterator, args);
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
    }
    internal sealed class GetActorValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_ObjId.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue Clone()
        {
            GetActorValue val = new GetActorValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjId.Evaluate(instance, handler, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_ObjId.HaveValue) {
                m_HaveValue = true;
                m_Value = null;
                int objId = m_ObjId.Value;
                m_Value = EntityController.Instance.GetGameObject(objId);
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetPlayerIdValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryValue Clone()
        {
            GetPlayerIdValue val = new GetPlayerIdValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
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

        private void TryUpdateValue(StoryInstance instance)
        {
            m_HaveValue = true;
            m_Value = PluginFramework.Instance.RoomObjId;
        }

        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetPlayerUnitIdValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryValue Clone()
        {
            GetPlayerUnitIdValue val = new GetPlayerUnitIdValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
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

        private void TryUpdateValue(StoryInstance instance)
        {
            m_HaveValue = true;
            m_Value = PluginFramework.Instance.RoomUnitId;
        }

        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetLeaderIdValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_ObjId.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue Clone()
        {
            GetLeaderIdValue val = new GetLeaderIdValue();
            val.m_ParamNum = m_ParamNum;
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0)
                m_ObjId.Evaluate(instance, handler, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            m_HaveValue = true;
            if (m_ParamNum > 0) {
                int objId = m_ObjId.Value;
                EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
                if(null!=npc){
                    m_Value = npc.GetAiStateInfo().LeaderId;
                } else {
                    m_Value = 0;
                }
            } else {
                m_Value = PluginFramework.Instance.LeaderId;
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetLeaderTableIdValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryValue Clone()
        {
            GetLeaderTableIdValue val = new GetLeaderTableIdValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
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

        private void TryUpdateValue(StoryInstance instance)
        {
            m_HaveValue = true;
            m_Value = ClientInfo.Instance.RoleData.HeroId;
        }

        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetMemberCountValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryValue Clone()
        {
            GetMemberCountValue val = new GetMemberCountValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
        
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

        private void TryUpdateValue(StoryInstance instance)
        {
            m_HaveValue = true;
            m_Value = ClientInfo.Instance.RoleData.Members.Count;
        }

        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class IsClientValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryValue Clone()
        {
            IsClientValue val = new IsClientValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
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

        private void TryUpdateValue(StoryInstance instance)
        {
            m_HaveValue = true;
            m_Value = 1;
        }

        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetSceneIdValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryValue Clone()
        {
            GetSceneIdValue val = new GetSceneIdValue();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
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

        private void TryUpdateValue(StoryInstance instance)
        {
            m_HaveValue = true;
            m_Value = GfxStorySystem.Instance.SceneId;
        }

        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetMemberTableIdValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_Index.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue Clone()
        {
            GetMemberTableIdValue val = new GetMemberTableIdValue();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Index.Evaluate(instance, handler, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_Index.HaveValue) {
                m_HaveValue = true;
                int index = m_Index.Value;
                if (index >= 0 && index < ClientInfo.Instance.RoleData.Members.Count) {
                    m_Value = ClientInfo.Instance.RoleData.Members[index].Hero;
                } else {
                    m_Value = 0;
                }
            }
        }

        private IStoryValue<int> m_Index = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class GetMemberLevelValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_Index.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue Clone()
        {
            GetMemberLevelValue val = new GetMemberLevelValue();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_Index.Evaluate(instance, handler, iterator, args);
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

        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_Index.HaveValue) {
                m_HaveValue = true;
                int index = m_Index.Value;
                if (index >= 0 && index < ClientInfo.Instance.RoleData.Members.Count) {
                    m_Value = ClientInfo.Instance.RoleData.Members[index].Level;
                } else {
                    m_Value = 0;
                }
            }
        }

        private IStoryValue<int> m_Index = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class DictGetValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_DictId.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryValue Clone()
        {
            DictGetValue val = new DictGetValue();
            val.m_DictId = m_DictId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_DictId.Evaluate(instance, handler, iterator, args);
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
        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_DictId.HaveValue) {
                m_HaveValue = true;
                m_Value = null;
                string dictId = m_DictId.Value;
                m_Value = Dict.Get(dictId);
            }
        }
        private IStoryValue<string> m_DictId = new StoryValue<string>();
        private bool m_HaveValue;
        private object m_Value;
    }
    internal sealed class DictFormatValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                int num = callData.GetParamNum();
                if (num > 0) {
                    m_DictId.InitFromDsl(callData.GetParam(0));
                }
                for (int i = 1; i < num; ++i) {
                    StoryValue val = new StoryValue();
                    val.InitFromDsl(callData.GetParam(i));
                    m_FormatArgs.Add(val);
                }
            }
        }
        public IStoryValue Clone()
        {
            DictFormatValue val = new DictFormatValue();
            val.m_DictId = m_DictId.Clone();
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                val.m_FormatArgs.Add(m_FormatArgs[i].Clone());
            }
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_DictId.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_FormatArgs.Count; i++) {
                m_FormatArgs[i].Evaluate(instance, handler, iterator, args);
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
        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_DictId.HaveValue) {
                m_HaveValue = true;
                m_Value = null;
                string dictId = m_DictId.Value;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_FormatArgs.Count; i++) {
                    arglist.Add(m_FormatArgs[i].Value);
                }
                object[] args = arglist.ToArray();
                m_Value = Dict.Format(dictId, args);
            }
        }
        private IStoryValue<string> m_DictId = new StoryValue<string>();
        private List<IStoryValue> m_FormatArgs = new List<IStoryValue>();
        private bool m_HaveValue;
        private object m_Value;
    }
}
