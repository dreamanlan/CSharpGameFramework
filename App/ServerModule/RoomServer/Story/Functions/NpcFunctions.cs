using System;
using System.Collections.Generic;
using DotnetStoryScript;
using ScriptableFramework;
using ScriptRuntime;

namespace ScriptableFramework.Story.Functions
{
    public sealed class NpcIdListFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryFunction Clone()
        {
            NpcIdListFunction val = new NpcIdListFunction();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
        
            TryUpdateValue(instance);
        }
        public void Analyze(StoryInstance instance)
        {
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
                List<object> npcs = new List<object>();
                scene.EntityManager.Entities.VisitValues((EntityInfo npcInfo) => {
                    npcs.Add(npcInfo.GetId());
                });
                m_HaveValue = true;
                m_Value = BoxedValue.FromObject(npcs);
            }
        }
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class CombatNpcCountFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_CampId.InitFromDsl(callData.GetParam(0));
                }
            }
        }
        public IStoryFunction Clone()
        {
            CombatNpcCountFunction val = new CombatNpcCountFunction();
            val.m_ParamNum = m_ParamNum;
            val.m_CampId = m_CampId;
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 0) {
                m_CampId.Evaluate(instance, handler, iterator, args);
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
                m_HaveValue = true;
                if (m_ParamNum > 0) {
                    m_Value = scene.GetBattleNpcCount(m_CampId.Value);
                } else {
                    m_Value = scene.GetBattleNpcCount();
                }
            }
        }
        private int m_ParamNum = 0;
        private IStoryFunction<int> m_CampId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class NpcGetFormationFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && 1 == callData.GetParamNum()) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            NpcGetFormationFunction val = new NpcGetFormationFunction();
            val.m_UnitId = m_UnitId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_UnitId.Evaluate(instance, handler, iterator, args);
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
                int unitId = m_UnitId.Value;
                m_HaveValue = true;
                EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != entity) {
                    m_Value = entity.GetMovementStateInfo().FormationIndex;
                }
            }
        }
        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class NpcGetNpcTypeFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            NpcGetNpcTypeFunction val = new NpcGetNpcTypeFunction();
            val.m_UnitId = m_UnitId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_UnitId.Evaluate(instance, handler, iterator, args);
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
                if (m_UnitId.HaveValue) {
                    int unitId = m_UnitId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                    if (null != obj) {
                        m_Value = obj.EntityType;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class NpcGetSummonerIdFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            NpcGetSummonerIdFunction val = new NpcGetSummonerIdFunction();
            val.m_UnitId = m_UnitId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_UnitId.Evaluate(instance, handler, iterator, args);
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
                if (m_UnitId.HaveValue) {
                    int unitId = m_UnitId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                    if (null != obj) {
                        m_Value = obj.SummonerId;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class NpcGetSummonSkillIdFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryFunction Clone()
        {
            NpcGetSummonSkillIdFunction val = new NpcGetSummonSkillIdFunction();
            val.m_UnitId = m_UnitId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_UnitId.Evaluate(instance, handler, iterator, args);
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
                if (m_UnitId.HaveValue) {
                    int unitId = m_UnitId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                    if (null != obj) {
                        m_Value = obj.SummonSkillId;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class NpcFindImpactSeqByIdFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && 2 == callData.GetParamNum()) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_ImpactId.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryFunction Clone()
        {
            NpcFindImpactSeqByIdFunction val = new NpcFindImpactSeqByIdFunction();
            val.m_UnitId = m_UnitId.Clone();
            val.m_ImpactId = m_ImpactId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_ImpactId.Evaluate(instance, handler, iterator, args);
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
                int unitId = m_UnitId.Value;
                int impactId = m_ImpactId.Value;
                m_HaveValue = true;
                EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != entity) {
                    ImpactInfo impactInfo = entity.GetSkillStateInfo().FindImpactInfoById(impactId);
                    if (null != impactInfo) {
                        m_Value = impactInfo.Seq;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<int> m_ImpactId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    public sealed class NpcCountFunction : IStoryFunction
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && 2 == callData.GetParamNum()) {
                m_StartUnitId.InitFromDsl(callData.GetParam(0));
                m_EndUnitId.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryFunction Clone()
        {
            NpcCountFunction val = new NpcCountFunction();
            val.m_StartUnitId = m_StartUnitId.Clone();
            val.m_EndUnitId = m_EndUnitId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_StartUnitId.Evaluate(instance, handler, iterator, args);
            m_EndUnitId.Evaluate(instance, handler, iterator, args);
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
                int startUnitId = m_StartUnitId.Value;
                int endUnitId = m_EndUnitId.Value;
                m_Value = scene.GetNpcCount(startUnitId, endUnitId);
            }
        }
        private IStoryFunction<int> m_StartUnitId = new StoryValue<int>();
        private IStoryFunction<int> m_EndUnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
