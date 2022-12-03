﻿using System;
using System.Collections.Generic;
using StorySystem;
using GameFramework;
using ScriptRuntime;

namespace GameFramework.Story.Values
{
    internal sealed class NpcIdListValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData) {
            }
        }
        public IStoryValue Clone()
        {
            NpcIdListValue val = new NpcIdListValue();
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
            List<object> npcs = new List<object>();
            PluginFramework.Instance.EntityManager.Entities.VisitValues((EntityInfo npcInfo) => {
                npcs.Add(npcInfo.GetId());
            });
            m_HaveValue = true;
            m_Value = BoxedValue.FromObject(npcs);
        }

        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class CombatNpcCountValue : IStoryValue
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
        public IStoryValue Clone()
        {
            CombatNpcCountValue val = new CombatNpcCountValue();
            val.m_ParamNum = m_ParamNum;
            val.m_CampId = m_CampId.Clone();
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
            m_HaveValue = true;
            if (m_ParamNum > 0) {
                m_Value = PluginFramework.Instance.GetBattleNpcCount(m_CampId.Value);
            } else {
                m_Value = PluginFramework.Instance.GetBattleNpcCount();
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<int> m_CampId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class NpcGetFormationValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && 1 == callData.GetParamNum()) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue Clone()
        {
            NpcGetFormationValue val = new NpcGetFormationValue();
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
            int unitId = m_UnitId.Value;
            m_HaveValue = true;
            EntityInfo entity = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != entity) {
                m_Value = entity.GetMovementStateInfo().FormationIndex;
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class NpcGetNpcTypeValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue Clone()
        {
            NpcGetNpcTypeValue val = new NpcGetNpcTypeValue();
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
            if (m_UnitId.HaveValue) {
                int unitId = m_UnitId.Value;
                m_HaveValue = true;
                EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
                if (null != obj) {
                    m_Value = obj.EntityType;
                } else {
                    m_Value = 0;
                }
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class NpcGetSummonerIdValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue Clone()
        {
            NpcGetSummonerIdValue val = new NpcGetSummonerIdValue();
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
            if (m_UnitId.HaveValue) {
                int unitId = m_UnitId.Value;
                m_HaveValue = true;
                EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
                if (null != obj) {
                    m_Value = obj.SummonerId;
                } else {
                    m_Value = 0;
                }
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class NpcGetSummonSkillIdValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() == 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue Clone()
        {
            NpcGetSummonSkillIdValue val = new NpcGetSummonSkillIdValue();
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
            if (m_UnitId.HaveValue) {
                int unitId = m_UnitId.Value;
                m_HaveValue = true;
                EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
                if (null != obj) {
                    m_Value = obj.SummonSkillId;
                } else {
                    m_Value = 0;
                }
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class NpcFindImpactSeqByIdValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && 2 == callData.GetParamNum()) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_ImpactId.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryValue Clone()
        {
            NpcFindImpactSeqByIdValue val = new NpcFindImpactSeqByIdValue();
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
            int unitId = m_UnitId.Value;
            int impactId = m_ImpactId.Value;
            m_HaveValue = true;
            EntityInfo entity = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != entity) {
                ImpactInfo impactInfo = entity.GetSkillStateInfo().FindImpactInfoById(impactId);
                if (null != impactInfo) {
                    m_Value = impactInfo.Seq;
                } else {
                    m_Value = 0;
                }
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_ImpactId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class NpcCountValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && 2 == callData.GetParamNum()) {
                m_StartUnitId.InitFromDsl(callData.GetParam(0));
                m_EndUnitId.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryValue Clone()
        {
            NpcCountValue val = new NpcCountValue();
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
            m_HaveValue = true;
            int startUnitId = m_StartUnitId.Value;
            int endUnitId = m_EndUnitId.Value;
            m_Value = PluginFramework.Instance.GetNpcCount(startUnitId, endUnitId);
        }

        private IStoryValue<int> m_StartUnitId = new StoryValue<int>();
        private IStoryValue<int> m_EndUnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
