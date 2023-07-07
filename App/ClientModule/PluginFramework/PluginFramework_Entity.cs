﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GameFramework.GmCommands;
using GameFramework.Story;
using GameFramework.Skill;
using SkillSystem;

namespace GameFramework
{
    public partial class PluginFramework
    {
        public bool IsLocalSkillEffect(GfxSkillSenderInfo info)
        {
            if (info.ObjId == m_LeaderId || info.TargetObjId == m_LeaderId) {
                return true;
            }
            EntityInfo sender = GetEntityById(info.ObjId);
            if (null != sender && !sender.IsServerEntity) {
                return true;
            }
            EntityInfo target = GetEntityById(info.TargetObjId);
            if (null != target && !target.IsServerEntity) {
                return true;
            }
            return false;
        }
        public int UnitId2ObjId(int unitId)
        {
            int id = 0;
            EntityInfo entity = GetEntityByUnitId(unitId);
            if (null != entity) {
                id = entity.GetId();
            }
            return id;
        }
        public int ObjId2UnitId(int actorId)
        {
            int id = 0;
            EntityInfo entity = GetEntityById(actorId);
            if (null != entity) {
                id = entity.GetUnitId();
            }
            return id;
        }
        public EntityViewModel GetEntityView(UnityEngine.GameObject obj)
        {
            return EntityController.Instance.GetEntityView(obj);
        }
        public EntityViewModel GetEntityViewById(int entityId)
        {
            return EntityController.Instance.GetEntityViewById(entityId);
        }
        public EntityViewModel GetEntityViewByUnitId(int unitId)
        {
            return EntityController.Instance.GetEntityViewByUnitId(unitId);
        }
        public bool ExistGameObject(UnityEngine.GameObject obj)
        {
            return EntityController.Instance.ExistGameObject(obj);
        }
        public bool ExistGameObject(int objId)
        {
            return EntityController.Instance.ExistGameObject(objId);
        }
        public UnityEngine.GameObject GetGameObject(int objId)
        {
            return EntityController.Instance.GetGameObject(objId);
        }
        public UnityEngine.GameObject GetGameObjectByUnitId(int unitId)
        {
            return EntityController.Instance.GetGameObjectByUnitId(unitId);
        }
        public int GetGameObjectId(UnityEngine.GameObject obj)
        {
            return EntityController.Instance.GetGameObjectId(obj);
        }
        public int GetGameObjectUnitId(UnityEngine.GameObject obj)
        {
            return EntityController.Instance.GetGameObjectUnitId(obj);
        }
        public EntityInfo GetEntityByGameObject(UnityEngine.GameObject obj)
        {
            int objId = GetGameObjectId(obj);
            return GetEntityById(objId);
        }
        public UnityEngine.GameObject GetGameObjectByEntity(EntityInfo info)
        {
            return GetGameObject(info.GetId());
        }
        public int GetGameObjectCurSkillId(UnityEngine.GameObject obj)
        {
            int id = 0;
            EntityViewModel view = EntityController.Instance.GetEntityView(obj);
            if (null != view) {
                SkillInfo skillInfo = view.Entity.GetSkillStateInfo().GetCurSkillInfo();
                if (null != skillInfo) {
                    id = skillInfo.SkillId;
                }
            }
            return id;
        }
        public List<InplaceSkillPropertyInfo> GetInplaceSkillPropertyInfos(int skillId)
        {
            List<InplaceSkillPropertyInfo> ret = null;
            SkillInstance inst = GfxSkillSystem.Instance.FindSkillInstanceForSkillViewer(skillId);
            if (null != inst) {
                ret = inst.CollectProperties();
                if (null != inst.EmitSkillInstances) {
                    foreach (var pair in inst.EmitSkillInstances) {
                        SkillInstance iinst = GfxSkillSystem.Instance.FindInnerSkillInstanceForSkillViewer(PredefinedSkill.c_EmitSkillId, pair.Value);
                        if (null != iinst) {
                            ret.Add(new InplaceSkillPropertyInfo { Group = "InnerSkillInstance", Key = "emitskill" + iinst.InnerDslSkillId });
                            ret.AddRange(iinst.CollectProperties());
                        }
                    }
                }
                if (null != inst.HitSkillInstances) {
                    foreach (var pair in inst.HitSkillInstances) {
                        SkillInstance iinst = GfxSkillSystem.Instance.FindInnerSkillInstanceForSkillViewer(PredefinedSkill.c_HitSkillId, pair.Value);
                        if (null != iinst) {
                            ret.Add(new InplaceSkillPropertyInfo { Group = "InnerSkillInstance", Key = "hitskill" + iinst.InnerDslSkillId });
                            ret.AddRange(iinst.CollectProperties());
                        }
                    }
                }
            }
            return ret;
        }
        public int GetGameObjectType(int id)
        {
            int type = -1;
            EntityInfo entity = GetEntityById(id);
            if (null != entity) {
                type = entity.EntityType;
            }
            return type;
        }
        public float GetGameObjectHp(int id)
        {
            float hp = -1;
            EntityInfo entity = GetEntityById(id);
            if (null != entity) {
                hp = entity.Hp;
            }
            return hp;
        }
        public float GetGameObjectEnergy(int id)
        {
            float np = -1;
            EntityInfo entity = GetEntityById(id);
            if (null != entity) {
                np = entity.Energy;
            }
            return np;
        }
        public CharacterProperty GetGameObjectProperty(int id)
        {
            CharacterProperty prop = null;
            EntityInfo entity = GetEntityById(id);
            if (null != entity) {
                prop = entity.ActualProperty;
            }
            return prop;
        }
        public int GetCampId(int actorId)
        {
            return EntityController.Instance.GetCampId(actorId);
        }
        public void ClickNpc(int targetId)
        {
            SetLockTarget(targetId);
        }
        public void SetLockTarget(int targetId)
        {
            int oldTargetId = 0;
            if (null != m_SelectedTarget) {
                oldTargetId = m_SelectedTarget.TargetId;
            }
            OnSelectedTargetChange(oldTargetId, targetId);
            EntityInfo target = GetEntityById(targetId);
            if (null != target) {
                m_SelectedTarget = new LockTargetInfo { Target = target, TargetId = targetId };
                EntityInfo leader = GetEntityById(LeaderId);
                if (null != leader) {
                    AiStateInfo aiInfo = leader.GetAiStateInfo();
                    if (null != SelectedTarget) {
                        aiInfo.Target = SelectedTarget.TargetId;
                    }
                }
            } else {
                m_SelectedTarget = null;
            }
        }
        public void MoveTo(float x, float y, float z)
        {
            if (!IsMainUiScene && !IsBattleState) {
                Network.NetworkSystem.Instance.SyncPlayerMoveToPos(new ScriptRuntime.Vector3(x, y, z));
            } else {
                GfxStorySystem.Instance.SendMessage("move_to", x, y, z);
            }
        }
        public bool SkillCanFindTarget(int objId, int skillId)
        {
            bool ret = false;
            EntityInfo obj = GetEntityById(objId);
            if (null != obj) {
                SkillInfo skillInfo = obj.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null != skillInfo) {
                    bool find = false;
                    KdTree.QueryWithFunc(obj, skillInfo.Distance, (float distSqr, KdTreeObject _obj) => {
                        EntityInfo target = _obj.Object;
                        if (CharacterRelation.RELATION_ENEMY == EntityInfo.GetRelation(obj, target) && !target.IsDead()) {
                            find = true;
                            return false;
                        }
                        return true;
                    });
                    ret = find;
                }
            }
            return ret;
        }
        public bool CastSkill(int objId, int skillId)
        {
            bool ret = false;
            EntityInfo obj = GetEntityById(objId);
            if (null != obj) {
                SkillInfo skillInfo = obj.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null != skillInfo) {
                    if (!skillInfo.IsInCd(TimeUtility.GetLocalMilliseconds())) {
                        int targetId = 0;
                        if (null != SelectedTarget) {
                            targetId = SelectedTarget.TargetId;
                        }
                        if (!IsBattleState) {
                            Network.NetworkSystem.Instance.SyncPlayerSkill(obj, skillId, targetId, obj.GetMovementStateInfo().GetFaceDir());
                        } else {
                            AiStateInfo aiInfo = obj.GetAiStateInfo();
                            aiInfo.Target = targetId;
                            GfxSkillSystem.Instance.StartSkill(objId, skillInfo.ConfigData, 0);
                        }
                        ret = true;
                    }
                }
            }
            return ret;
        }
        public bool GetAIEnable(int objID)
        {
            EntityInfo character = GetEntityById(objID);
            if(character != null){
                return character.GetAIEnable();
            }
            return false;
        }
        public void SetAIEnable(int objID, bool bEnable)
        {
            EntityInfo character = GetEntityById(objID);
            if (character != null) {
                character.SetAIEnable(bEnable);
            }
        }
        public float GetNpcMp(int objID)
        {
            EntityInfo entity = GetEntityById(objID);
            if (entity != null) {
                float mp = (float)entity.Energy / entity.EnergyMax;
                return mp;
            }
            return 0;
        }
        public bool GetNpcCooldown(int objID, out float curValue, out float time)
        {
            curValue = 0;
            time = 1;
            EntityInfo entity = GetEntityById(objID);
            if (entity != null) {
                return true;
            }
            return false;
        }
        private bool HaveNpcInRange(EntityInfo entity, CharacterRelation relation, float radius)
        {
            bool find = false;
            KdTree.QueryWithFunc(entity, radius, (float sqrDist, KdTreeObject obj) => {
                if (EntityInfo.GetRelation(entity, obj.Object) == relation) {
                    find = true;
                    return false;
                }
                return true;
            });
            return find;
        }
    }
}
