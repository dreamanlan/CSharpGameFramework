using System;
using System.Collections.Generic;
using ScriptRuntime;
using StorySystem;
using GameFramework;

namespace GameFramework.Story.Functions
{
    public sealed class BlackboardGetFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            BlackboardGetFunction val = new BlackboardGetFunction();
            val.m_ParamNum = m_ParamNum;
            val.m_AttrName = m_AttrName.Clone();
            val.m_DefaultValue = m_DefaultValue.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_AttrName.HaveValue) {
                    string name = m_AttrName.Value;
                    m_HaveValue = true;
                    object v;
                    if (scene.SceneContext.BlackBoard.TryGetVariable(name, out v)) {
                        m_Value = BoxedValue.FromObject(v);
                    }
                    else {
                        if (m_ParamNum > 1) {
                            m_Value = m_DefaultValue.Value;
                        }
                    }
                }
            }
        }
        private int m_ParamNum = 0;
        private IStoryFunction<string> m_AttrName = new StoryValue<string>();
        private IStoryFunction m_DefaultValue = new StoryValue();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class GetDialogItemFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            GetDialogItemFunction val = new GetDialogItemFunction();
            val.m_DlgId = m_DlgId.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
        public BoxedValue Value
        {
            get
            {
                return m_Value;
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
                        m_Value = BoxedValue.FromObject(cfg);
                    } else {
                        m_Value = BoxedValue.NullObject;
                    }
                }
            }
        }

        private IStoryFunction<int> m_DlgId = new StoryValue<int>();
        private IStoryFunction<int> m_Index = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class GetActorIconValue : IStoryFunction
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
        public IStoryFunction Clone()
        {
            GetActorIconValue val = new GetActorIconValue();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_Index.HaveValue) {
                    m_HaveValue = true;
                    int index = m_Index.Value;
                    m_Value = BoxedValue.NullObject;
                }
            }
        }

        private IStoryFunction<int> m_Index = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class GetMonsterInfoFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            GetMonsterInfoFunction val = new GetMonsterInfoFunction();
            val.m_CampId = m_CampId.Clone();
            val.m_Index = m_Index.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_CampId.HaveValue && m_Index.HaveValue) {
                    m_HaveValue = true;
                    int sceneId = scene.SceneResId;
                    int campId = m_CampId.Value;
                    int index = m_Index.Value;
                    int monstersId = TableConfigUtility.GenLevelMonstersId(sceneId, campId);
                    List<TableConfig.LevelMonster> monsterList;
                    if (TableConfig.LevelMonsterProvider.Instance.TryGetValue(monstersId, out monsterList)) {
                        if (index >= 0 && index < monsterList.Count) {
                            m_Value = BoxedValue.FromObject(monsterList[index]);
                        } else {
                            m_Value = BoxedValue.NullObject;
                        }
                    } else {
                        m_Value = BoxedValue.NullObject;
                    }
                }
            }
        }

        private IStoryFunction<int> m_CampId = new StoryValue<int>();
        private IStoryFunction<int> m_Index = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class GetAiDataFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            GetAiDataFunction val = new GetAiDataFunction();
            val.m_ObjId = m_ObjId.Clone();
            val.m_DataType = m_DataType.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue && m_DataType.HaveValue) {
                    m_HaveValue = true;
                    m_Value = BoxedValue.NullObject;
                    int objId = m_ObjId.Value;
                    string typeName = m_DataType.Value;
                    EntityInfo npc = scene.SceneContext.GetEntityById(objId);
                    if (null != npc) {
                    }
                }
            }
        }

        private IStoryFunction<int> m_ObjId = new StoryValue<int>();
        private IStoryFunction<string> m_DataType = new StoryValue<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class GetLeaderIdFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            GetLeaderIdFunction val = new GetLeaderIdFunction();
            val.m_ParamNum = m_ParamNum;
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
        public BoxedValue Value
        {
            get
            {
                return m_Value;
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
                        m_Value = npc.GetAiStateInfo().LeaderId;
                    } else {
                        m_Value = 0;
                    }
                } else {
                    m_Value = 0;
                }
            }
        }
        private int m_ParamNum = 0;
        private IStoryFunction<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class GetLeaderTableIdFunction : IStoryFunction
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
        public IStoryFunction Clone()
        {
            GetLeaderTableIdFunction val = new GetLeaderTableIdFunction();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
        public BoxedValue Value
        {
            get
            {
                return m_Value;
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
                        int leaderId = obj.GetAiStateInfo().LeaderId;
                        EntityInfo leader = scene.SceneContext.GetEntityById(leaderId);
                        if (null != leader) {
                            m_Value = leader.GetTableId();
                        } else {
                            m_Value = 0;
                        }
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }

        private IStoryFunction<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class IsClientFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryFunction Clone()
        {
            IsClientFunction val = new IsClientFunction();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            m_HaveValue = true;
            m_Value = 0;
        }
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetRoomIdFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryFunction Clone()
        {
            GetRoomIdFunction val = new GetRoomIdFunction();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                m_HaveValue = true;
                m_Value = scene.GetRoomUserManager().RoomId;
            }
        }

        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetSceneIdFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryFunction Clone()
        {
            GetSceneIdFunction val = new GetSceneIdFunction();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
        public BoxedValue Value
        {
            get
            {
                return m_Value;
            }
        }

        private void TryUpdateValue(StoryInstance instance)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                m_HaveValue = true;
                m_Value = scene.SceneResId;
            }
        }

        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
