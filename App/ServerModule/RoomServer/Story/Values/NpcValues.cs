using System;
using System.Collections.Generic;
using StorySystem;
using GameFramework;
using ScriptRuntime;

namespace GameFramework.Story.Values
{
    internal sealed class NpcIdListValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "npcidlist") {
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR;
            }
        }
        public IStoryValue<object> Clone()
        {
            NpcIdListValue val = new NpcIdListValue();
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
        }
        public void Evaluate(StoryInstance instance)
        {
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
                List<object> npcs = new List<object>();
                scene.EntityManager.Entities.VisitValues((EntityInfo npcInfo) => {
                    npcs.Add(npcInfo.GetId());
                });
                m_HaveValue = true;
                m_Value = npcs;
            }
        }

        private object m_Iterator = null;
        private object[] m_Args = null;

        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class CombatNpcCountValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "combatnpccount") {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_CampId.InitFromDsl(callData.GetParam(0));
                    flag |= m_CampId.Flag;
                }
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            CombatNpcCountValue val = new CombatNpcCountValue();
            val.m_ParamNum = m_ParamNum;
            val.m_CampId = m_CampId;
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
                    m_CampId.Substitute(iterator, args);
                }
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            if (m_ParamNum > 0) {
                m_CampId.Evaluate(instance);
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
                m_HaveValue = true;
                if (m_ParamNum > 0) {
                    m_Value = scene.GetBattleNpcCount(m_CampId.Value);
                } else {
                    m_Value = scene.GetBattleNpcCount();
                }
            }
        }

        private object m_Iterator = null;
        private object[] m_Args = null;

        private int m_ParamNum = 0;
        private IStoryValue<int> m_CampId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetFormationValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getformation" && 1 == callData.GetParamNum()) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_UnitId.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetFormationValue val = new GetFormationValue();
            val.m_UnitId = m_UnitId.Clone();
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
                m_UnitId.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
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
                int unitId = m_UnitId.Value;
                m_HaveValue = true;
                EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != entity) {
                    m_Value = entity.GetMovementStateInfo().FormationIndex;
                }
            }
        }

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetNpcTypeValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getnpctype" && callData.GetParamNum() == 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_UnitId.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetNpcTypeValue val = new GetNpcTypeValue();
            val.m_UnitId = m_UnitId.Clone();
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
                m_UnitId.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetSummonerIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getsummonerid" && callData.GetParamNum() == 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_UnitId.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetSummonerIdValue val = new GetSummonerIdValue();
            val.m_UnitId = m_UnitId.Clone();
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
                m_UnitId.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class GetSummonSkillIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getsummonskillid" && callData.GetParamNum() == 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_UnitId.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            GetSummonSkillIdValue val = new GetSummonSkillIdValue();
            val.m_UnitId = m_UnitId.Clone();
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
                m_UnitId.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class NpcFindImpactSeqByIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "npcfindimpactseqbyid" && 2 == callData.GetParamNum()) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_ImpactId.InitFromDsl(callData.GetParam(1));
                m_Flag = (int)StoryValueFlagMask.HAVE_VAR | m_UnitId.Flag | m_ImpactId.Flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            NpcFindImpactSeqByIdValue val = new NpcFindImpactSeqByIdValue();
            val.m_UnitId = m_UnitId.Clone();
            val.m_ImpactId = m_ImpactId.Clone();
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
                m_UnitId.Substitute(iterator, args);
                m_ImpactId.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_ImpactId.Evaluate(instance);
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

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_ImpactId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
    internal sealed class NpcCountValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "npccount" && 2 == callData.GetParamNum()) {
                int flag = (int)StoryValueFlagMask.HAVE_VAR;
                m_StartUnitId.InitFromDsl(callData.GetParam(0));
                m_EndUnitId.InitFromDsl(callData.GetParam(1));
                flag |= m_StartUnitId.Flag;
                flag |= m_EndUnitId.Flag;
                m_Flag = flag;
            }
        }
        public IStoryValue<object> Clone()
        {
            NpcCountValue val = new NpcCountValue();
            val.m_StartUnitId = m_StartUnitId.Clone();
            val.m_EndUnitId = m_EndUnitId.Clone();
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
                m_StartUnitId.Substitute(iterator, args);
                m_EndUnitId.Substitute(iterator, args);
            }
        }
        public void Evaluate(StoryInstance instance)
        {
            m_StartUnitId.Evaluate(instance);
            m_EndUnitId.Evaluate(instance);
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
                int startUnitId = m_StartUnitId.Value;
                int endUnitId = m_EndUnitId.Value;
                m_Value = scene.GetNpcCount(startUnitId, endUnitId);
            }
        }

        private object m_Iterator = null;
        private object[] m_Args = null;

        private IStoryValue<int> m_StartUnitId = new StoryValue<int>();
        private IStoryValue<int> m_EndUnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
        private int m_Flag = (int)StoryValueFlagMask.HAVE_ARG_AND_VAR;
    }
}
