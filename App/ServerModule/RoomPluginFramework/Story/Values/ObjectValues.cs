using System;
using System.Collections.Generic;
using StorySystem;
using GameFramework;
using ScriptRuntime;

namespace GameFramework.Story.Values
{
    public sealed class UnitId2ObjIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "unitid2objid" && callData.GetParamNum() == 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            UnitId2ObjIdValue val = new UnitId2ObjIdValue();
            val.m_UnitId = m_UnitId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_UnitId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_UnitId.HaveValue) {
                    int unitId = m_UnitId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                    if (null != obj) {
                        m_Value = obj.GetId();
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class ObjId2UnitIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "objid2unitid" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            ObjId2UnitIdValue val = new ObjId2UnitIdValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.GetUnitId();
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class UnitId2UniqueIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "unitid2uniqueid" && callData.GetParamNum() == 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            UnitId2UniqueIdValue val = new UnitId2UniqueIdValue();
            val.m_UnitId = m_UnitId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_UnitId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_UnitId.HaveValue) {
                    int unitId = m_UnitId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                    if (null != obj) {
                        if (obj.UniqueId <= 0) {
                            obj.UniqueId = scene.SceneContext.GenUniqueId();
                        }
                        m_Value = obj.UniqueId;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class ObjId2UniqueIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "objid2uniqueid" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            ObjId2UniqueIdValue val = new ObjId2UniqueIdValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        if (obj.UniqueId <= 0) {
                            obj.UniqueId = scene.SceneContext.GenUniqueId();
                        }
                        m_Value = obj.UniqueId;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class GetPositionValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getposition" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            GetPositionValue val = new GetPositionValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.GetMovementStateInfo().GetPosition3D();
                    } else {
                        m_Value = Vector3.Zero;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class GetPositionXValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getpositionx" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            GetPositionXValue val = new GetPositionXValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.GetMovementStateInfo().PositionX;
                    } else {
                        m_Value = 0.0f;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class GetPositionYValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getpositiony" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            GetPositionYValue val = new GetPositionYValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.GetMovementStateInfo().PositionY;
                    } else {
                        m_Value = 0.0f;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class GetPositionZValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getpositionz" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            GetPositionZValue val = new GetPositionZValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.GetMovementStateInfo().PositionZ;
                    } else {
                        m_Value = 0.0f;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class GetCampValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getcamp" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            GetCampValue val = new GetCampValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.GetCampId();
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class IsEnemyValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "isenemy" && callData.GetParamNum() == 2) {
                m_Camp1.InitFromDsl(callData.GetParam(0));
                m_Camp2.InitFromDsl(callData.GetParam(1));
                TryUpdateValue(null);
            }
        }
        public IStoryValue<object> Clone()
        {
            IsEnemyValue val = new IsEnemyValue();
            val.m_Camp1 = m_Camp1.Clone();
            val.m_Camp2 = m_Camp2.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_Camp1.Evaluate(instance, iterator, args);
            m_Camp2.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_Camp1.HaveValue && m_Camp2.HaveValue) {
                    int camp1 = m_Camp1.Value;
                    int camp2 = m_Camp2.Value;
                    m_HaveValue = true;
                    m_Value = (EntityInfo.GetRelation(camp1, camp2) == CharacterRelation.RELATION_ENEMY ? 1 : 0);
                }
            }
        }
        private IStoryValue<int> m_Camp1 = new StoryValue<int>();
        private IStoryValue<int> m_Camp2 = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class IsFriendValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "isfriend" && callData.GetParamNum() == 2) {
                m_Camp1.InitFromDsl(callData.GetParam(0));
                m_Camp2.InitFromDsl(callData.GetParam(1));
                TryUpdateValue(null);
            }
        }
        public IStoryValue<object> Clone()
        {
            IsFriendValue val = new IsFriendValue();
            val.m_Camp1 = m_Camp1.Clone();
            val.m_Camp2 = m_Camp2.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_Camp1.Evaluate(instance, iterator, args);
            m_Camp2.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_Camp1.HaveValue && m_Camp2.HaveValue) {
                    int camp1 = m_Camp1.Value;
                    int camp2 = m_Camp2.Value;
                    m_HaveValue = true;
                    m_Value = (EntityInfo.GetRelation(camp1, camp2) == CharacterRelation.RELATION_FRIEND ? 1 : 0);
                }
            }
        }
        private IStoryValue<int> m_Camp1 = new StoryValue<int>();
        private IStoryValue<int> m_Camp2 = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class GetHpValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "gethp" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            GetHpValue val = new GetHpValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.Hp;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class GetEnergyValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getarmor" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            GetEnergyValue val = new GetEnergyValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.Energy;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class GetMaxHpValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getmaxhp" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            GetMaxHpValue val = new GetMaxHpValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.HpMax;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class GetMaxEnergyValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getmaxarmor" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            GetMaxEnergyValue val = new GetMaxEnergyValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.EnergyMax;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class CalcOffsetValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "calcoffset" && callData.GetParamNum() == 3) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_TargetId.InitFromDsl(callData.GetParam(1));
                m_Offset.InitFromDsl(callData.GetParam(2));
            }
        }
        public IStoryValue<object> Clone()
        {
            CalcOffsetValue val = new CalcOffsetValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_TargetId = m_TargetId.Clone();
            val.m_Offset = m_Offset.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
            m_TargetId.Evaluate(instance, iterator, args);
            m_Offset.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue && m_TargetId.HaveValue) {
                    int objId = m_ObjId.Value;
                    int targetId = m_TargetId.Value;
                    Vector3 offset = m_Offset.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    EntityInfo target = scene.SceneContext.GetEntityById(targetId);
                    if (null != obj && null != target) {
                        Vector2 srcPos = obj.GetMovementStateInfo().GetPosition2D();
                        float y = obj.GetMovementStateInfo().PositionY;
                        Vector2 targetPos = target.GetMovementStateInfo().GetPosition2D();
                        float radian = Geometry.GetYRadian(srcPos, targetPos);
                        Vector2 newPos = srcPos + Geometry.GetRotate(new Vector2(offset.X, offset.Z), radian);
                        m_Value = new Vector3(newPos.X, y + offset.Y, newPos.Y);
                    } else if (null != obj) {
                        Vector2 srcPos = obj.GetMovementStateInfo().GetPosition2D();
                        float y = obj.GetMovementStateInfo().PositionY;
                        float radian = obj.GetMovementStateInfo().GetFaceDir();
                        Vector2 newPos = srcPos + Geometry.GetRotate(new Vector2(offset.X, offset.Z), radian);
                        m_Value = new Vector3(newPos.X, y + offset.Y, newPos.Y);
                    } else {
                        m_Value = Vector3.Zero;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_TargetId = new StoryValue<int>();
        private IStoryValue<Vector3> m_Offset = new StoryValue<Vector3>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class CalcDirValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "calcdir" && callData.GetParamNum() == 2) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_TargetId.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryValue<object> Clone()
        {
            CalcDirValue val = new CalcDirValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_TargetId = m_TargetId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
            m_TargetId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue && m_TargetId.HaveValue) {
                    int objId = m_ObjId.Value;
                    int targetId = m_TargetId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    EntityInfo target = scene.SceneContext.GetEntityById(targetId);
                    if (null != obj && null != target) {
                        Vector2 srcPos = obj.GetMovementStateInfo().GetPosition2D();
                        Vector2 targetPos = target.GetMovementStateInfo().GetPosition2D();
                        m_Value = Geometry.GetYRadian(srcPos, targetPos);
                    } else if (null != obj) {
                        m_Value = obj.GetMovementStateInfo().GetFaceDir();
                    } else {
                        m_Value = 0.0f;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_TargetId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class ObjGetValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "objget") {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 1) {
                    m_UniqueId.InitFromDsl(callData.GetParam(0));
                    m_LocalName.InitFromDsl(callData.GetParam(1));
                }
                if (m_ParamNum > 2) {
                    m_DefaultValue.InitFromDsl(callData.GetParam(2));
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            ObjGetValue val = new ObjGetValue();
            val.m_ParamNum = m_ParamNum;
            val.m_UniqueId = m_UniqueId.Clone();
            val.m_LocalName = m_LocalName.Clone();
            val.m_DefaultValue = m_DefaultValue.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            if (m_ParamNum > 1) {
                m_UniqueId.Evaluate(instance, iterator, args);
                m_LocalName.Evaluate(instance, iterator, args);
            }
            if (m_ParamNum > 2) {
                m_DefaultValue.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_UniqueId.HaveValue && m_LocalName.HaveValue) {
                    int uniqueId = m_UniqueId.Value;
                    string localName = m_LocalName.Value;
                    m_HaveValue = true;
                    if (!scene.SceneContext.ObjectTryGet(uniqueId, localName, out m_Value) && m_ParamNum > 2) {
                        m_Value = m_DefaultValue.Value;
                    }
                }
            }
        }
        private int m_ParamNum = 0;
        private IStoryValue<int> m_UniqueId = new StoryValue<int>();
        private IStoryValue<string> m_LocalName = new StoryValue<string>();
        private IStoryValue<object> m_DefaultValue = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class GetLinkIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getlinkid" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            GetLinkIdValue val = new GetLinkIdValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
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
    }
    public sealed class GetLevelValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getlevel" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            GetLevelValue val = new GetLevelValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.Level;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class GetAttrValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "getattr") {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 1) {
                    m_ObjId.InitFromDsl(callData.GetParam(0));
                    m_AttrName.InitFromDsl(callData.GetParam(1));
                }
                if (m_ParamNum > 2) {
                    m_DefaultValue.InitFromDsl(callData.GetParam(2));
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            GetAttrValue val = new GetAttrValue();
            val.m_ParamNum = m_ParamNum;
            val.m_ObjId = m_ObjId.Clone();
            val.m_AttrName = m_AttrName.Clone();
            val.m_DefaultValue = m_DefaultValue.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            if (m_ParamNum > 1) {
                m_ObjId.Evaluate(instance, iterator, args);
                m_AttrName.Evaluate(instance, iterator, args);
            }
            if (m_ParamNum > 2) {
                m_DefaultValue.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue && m_AttrName.HaveValue) {
                    int objId = m_ObjId.Value;
                    string attrName = m_AttrName.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        try {
                            Type t = obj.GetType();
                            m_Value = t.InvokeMember(attrName, System.Reflection.BindingFlags.GetProperty, null, obj, null);
                        } catch (Exception ex) {
                            LogSystem.Warn("setattr throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
                            m_Value = null;
                        }
                    } else if (m_ParamNum > 2) {
                        m_Value = m_DefaultValue.Value;
                    } else {
                        m_Value = null;
                    }
                }
            }
        }
        private int m_ParamNum = 0;
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<string> m_AttrName = new StoryValue<string>();
        private IStoryValue<object> m_DefaultValue = new StoryValue();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class IsCombatNpcValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "iscombatnpc" && 1 == callData.GetParamNum()) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            IsCombatNpcValue val = new IsCombatNpcValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                m_HaveValue = true;
                EntityInfo entity = scene.SceneContext.GetEntityById(objId);
                if (null != entity) {
                    m_Value = (entity.IsCombatNpc() ? 1 : 0);
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class IsControlByStoryValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "iscontrolbystory" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            IsControlByStoryValue val = new IsControlByStoryValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.IsControlByStory;
                    } else {
                        m_Value = false;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class CanCastSkillValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "cancastskill") {
                m_ParamNum = callData.GetParamNum();
                if (m_ParamNum > 0) {
                    m_ObjId.InitFromDsl(callData.GetParam(0));
                }
                if (m_ParamNum > 1) {
                    m_SkillId.InitFromDsl(callData.GetParam(1));
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            CanCastSkillValue val = new CanCastSkillValue();
            val.m_ParamNum = m_ParamNum;
            val.m_ObjId = m_ObjId.Clone();
            val.m_SkillId = m_SkillId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            if (m_ParamNum > 0)
                m_ObjId.Evaluate(instance, iterator, args);
            if (m_ParamNum > 1)
                m_SkillId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    int skillId = 0;
                    if (m_ParamNum > 1 && m_SkillId.HaveValue)
                        skillId = m_SkillId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = false;
                        if (!obj.IsDead() && !obj.IsUnderControl() && !obj.GetSkillStateInfo().IsSkillActivated()) {
                            if (m_ParamNum <= 1) {
                                m_Value = true;
                            } else {
                                SkillInfo curSkillInfo = obj.GetSkillStateInfo().GetSkillInfoById(skillId);
                                if (null != curSkillInfo) {
                                    long curTime = TimeUtility.GetLocalMilliseconds();
                                    if (!curSkillInfo.IsInCd(curTime)) {
                                        m_Value = true;
                                    } else {
                                        LogSystem.Warn("obj {0} objcancastskill {1} is in CD {2}", obj.GetId(), skillId, curSkillInfo.GetCD(curTime));
                                    }
                                }
                            }
                        } else {
                            SkillInfo oldSkillInfo = obj.GetSkillStateInfo().GetCurSkillInfo();
                            int oldSkillId = 0;
                            if (null != oldSkillInfo) {
                                oldSkillId = oldSkillInfo.SkillId;
                            }
                            LogSystem.Warn("obj {0} objcancastskill {1} return false because isdead {2} or isundercontrol {3} or isskillactivated {4} (old skill:{5})", obj.GetId(), skillId, obj.IsDead(), obj.IsUnderControl(), obj.GetSkillStateInfo().IsSkillActivated(), oldSkillId);
                        }
                    } else {
                        m_Value = false;
                    }
                }
            }
        }
        private int m_ParamNum = 0;
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_SkillId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class IsUnderControlValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "isundercontrol" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            IsUnderControlValue val = new IsUnderControlValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.IsUnderControl();
                    } else {
                        m_Value = false;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class ObjGetFormationValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "objgetformation" && 1 == callData.GetParamNum()) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            ObjGetFormationValue val = new ObjGetFormationValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                m_HaveValue = true;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    m_Value = obj.GetMovementStateInfo().FormationIndex;
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class ObjFindImpactSeqByIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "objfindimpactseqbyid" && 2 == callData.GetParamNum()) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_ImpactId.InitFromDsl(callData.GetParam(1));
            }
        }
        public IStoryValue<object> Clone()
        {
            ObjFindImpactSeqByIdValue val = new ObjFindImpactSeqByIdValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_ImpactId = m_ImpactId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
            m_ImpactId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int impactId = m_ImpactId.Value;
                m_HaveValue = true;
                EntityInfo entity = scene.SceneContext.GetEntityById(objId);
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
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_ImpactId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class ObjGetNpcTypeValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "objgetnpctype" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            ObjGetNpcTypeValue val = new ObjGetNpcTypeValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.EntityType;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class ObjGetSummonerIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "objgetsummonerid" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            ObjGetSummonerIdValue val = new ObjGetSummonerIdValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.SummonerId;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
    public sealed class ObjGetSummonSkillIdValue : IStoryValue<object>
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData && callData.GetId() == "objgetsummonskillid" && callData.GetParamNum() == 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }
        public IStoryValue<object> Clone()
        {
            ObjGetSummonSkillIdValue val = new ObjGetSummonSkillIdValue();
            val.m_ObjId = m_ObjId.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_HaveValue = false;        
            m_ObjId.Evaluate(instance, iterator, args);
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_ObjId.HaveValue) {
                    int objId = m_ObjId.Value;
                    m_HaveValue = true;
                    EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                    if (null != obj) {
                        m_Value = obj.SummonSkillId;
                    } else {
                        m_Value = 0;
                    }
                }
            }
        }
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private bool m_HaveValue;
        private object m_Value;
    }
}
