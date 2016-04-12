using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework.Skill;
using SkillSystem;

namespace GameFramework
{
    internal class EntityController
    {
        internal void Init()
        {
        }

        internal void Release()
        {
        }

        internal void Tick()
        {
        }

        internal EntityViewModel GetEntityViewById(int objId)
        {
            return EntityViewModelManager.Instance.GetEntityViewById(objId);
        }

        internal EntityViewModel GetEntityViewByUnitId(int unitId)
        {
            return EntityViewModelManager.Instance.GetEntityViewByUnitId(unitId);
        }

        internal bool IsVisible(int objId)
        {
            bool ret = false;
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view) {
                ret = view.Visible;
            }
            return ret;
        }

        internal UnityEngine.GameObject GetGameObject(int objId)
        {
            return EntityViewModelManager.Instance.GetGameObject(objId);
        }
        internal EntityViewModel GetEntityView(UnityEngine.GameObject obj)
        {
            return EntityViewModelManager.Instance.GetEntityView(obj);
        }
        internal int GetGameObjectId(UnityEngine.GameObject obj)
        {
            return EntityViewModelManager.Instance.GetGameObjectId(obj);
        }
        internal bool ExistGameObject(UnityEngine.GameObject obj)
        {
            return EntityViewModelManager.Instance.ExistGameObject(obj);
        }
        internal bool ExistGameObject(int objId)
        {
            return EntityViewModelManager.Instance.ExistGameObject(objId);
        }
        internal int GetEntityType(int objId)
        {
            int type = 0;
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                type = view.Entity.EntityType;
            }
            return type;
        }
        internal int GetEntityType(UnityEngine.GameObject obj)
        {
            int type = 0;
            EntityViewModel view = GetEntityView(obj);
            if (null != view && null != view.Entity) {
                type = view.Entity.EntityType;
            }
            return type;
        }
        internal int GetCampId(int objId)
        {
            int campId = 0;
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                campId = entity.GetCampId();
            }
            return campId;
        }
        internal int GetCampId(UnityEngine.GameObject obj)
        {
            int campId = 0;
            EntityViewModel view = GetEntityView(obj);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                campId = entity.GetCampId();
            }
            return campId;
        }
        internal bool IsMovableEntity(int objId)
        {
            bool ret = true;
            int type = GetEntityType(objId);
            if (type == (int)EntityTypeEnum.Tower) {
                ret = false;
            }
            return ret;
        }
        internal bool IsMovableEntity(UnityEngine.GameObject obj)
        {
            bool ret = true;
            int type = GetEntityType(obj);
            if (type == (int)EntityTypeEnum.Tower) {
                ret = false;
            }
            return ret;
        }
        internal bool IsRotatableEntity(int objId)
        {
            bool ret = true;
            int type = GetEntityType(objId);
            if (type == (int)EntityTypeEnum.Tower) {
                ret = false;
            }
            return ret;
        }
        internal bool IsRotatableEntity(UnityEngine.GameObject obj)
        {
            bool ret = true;
            int type = GetEntityType(obj);
            if (type == (int)EntityTypeEnum.Tower) {
                ret = false;
            }
            return ret;
        }
        internal void SyncFaceDir(UnityEngine.GameObject obj)
        {
            EntityViewModel viewModel = GetEntityView(obj);
            if (null != viewModel) {
                viewModel.SyncSpatial();
            }
        }
        internal bool CanCastSkill(int objId, TableConfig.Skill configData, int seq)
        {
            bool ret=true;
            if (configData.type == (int)SkillOrImpactType.Skill) {
                EntityViewModel view = GetEntityViewById(objId);
                if (null != view && null != view.Entity) {
                    EntityInfo entity = view.Entity;
                    if (entity.GetSkillStateInfo().IsSkillActivated()) {
                        SkillInfo skillInfo = entity.GetSkillStateInfo().GetCurSkillInfo();
                        if (null != skillInfo && skillInfo.ConfigData.interruptPriority >= configData.interruptPriority) {
                            ret = false;
                        }
                    }
                }
            }
            return ret;
        }
        internal void CancelCastSkill(int actorId)
        {
            EntityViewModel view = GetEntityViewById(actorId);
            if (null != view && null != view.Entity) {
                view.Entity.IsControlByManual = false;
            }
        }
        internal GfxSkillSenderInfo BuildSkillInfo(int objId, TableConfig.Skill configData, int seq)
        {
            GfxSkillSenderInfo ret = null;
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Actor && null != view.Entity && null != configData) {
                EntityInfo entity = view.Entity;
                int targetId = 0;
                if (configData.type == (int)SkillOrImpactType.Skill) {
                    targetId = entity.GetAiStateInfo().Target;
                } else {
                    ImpactInfo impactInfo = entity.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                    if (null != impactInfo) {
                        targetId = impactInfo.ImpactSenderId;
                    }
                }
                UnityEngine.GameObject targetObj = GetGameObject(targetId);
                if (null != targetObj) {
                    ret = new GfxSkillSenderInfo(configData, seq, objId, view.Actor, targetId, targetObj);
                } else {
                    ret = new GfxSkillSenderInfo(configData, seq, objId, view.Actor);
                }
            }
            return ret;
        }
        internal void ActivateSkill(int actorId, int skillId, int seq)
        {
            EntityViewModel view = GetEntityViewById(actorId);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                SkillInfo skillInfo = entity.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null != skillInfo) {
                    SkillInfo curSkillInfo = entity.GetSkillStateInfo().GetCurSkillInfo();
                    if (null != curSkillInfo && (curSkillInfo.ConfigData.interruptPriority < skillInfo.ConfigData.interruptPriority)) {
                        GfxSkillSystem.Instance.StopSkill(actorId, curSkillInfo.SkillId, 0, true);
                        if (skillId == view.Entity.ManualSkillId) {
                            LogSystem.Warn("ManualSkill {0} interrupt {1}.", skillId, curSkillInfo.SkillId);
                        }
                    }
                    if (skillId == view.Entity.ManualSkillId) {
                        LogSystem.Warn("ManualSkill {0} activate.", skillId);
                    }
                    entity.GetSkillStateInfo().SetCurSkillInfo(skillId);
                    skillInfo.IsSkillActivated = true;
                    skillInfo.CdEndTime = TimeUtility.GetLocalMilliseconds() + (long)(skillInfo.ConfigData.cooldown * 1000);
                    if (skillInfo.ConfigData.mpRecover > 0 && !ClientModule.Instance.IsRoomScene) {
                        //回蓝
                        entity.SetEnergy(Operate_Type.OT_Relative, skillInfo.ConfigData.mpRecover);
                        entity.EntityManager.FireDamageEvent(actorId, 0, false, false, 0, -skillInfo.ConfigData.mpRecover);
                    }
                }
            }
        }
        internal void DeactivateSkill(int actorId, int skillId, int seq)
        {
            EntityViewModel view = GetEntityViewById(actorId);
            if (null != view && null != view.Entity) {
                SkillInfo skillInfo = view.Entity.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null != skillInfo) {
                    if (skillId == view.Entity.ManualSkillId) {
                        LogSystem.Warn("ManualSkill {0} deactivate.", skillId);
                    }
                    skillInfo.IsSkillActivated = false;
                    view.Entity.IsControlByManual = false;
                } else {
                    view.Entity.GetSkillStateInfo().RemoveImpact(seq);
                }
            }
        }
        internal void StopSkillAnimation(int actorId)
        {
            UnityEngine.GameObject obj = GetGameObject(actorId);
            if (null != obj) {
                UnityEngine.Animator animator = obj.GetComponent<UnityEngine.Animator>();
                if (null != animator) {
                    animator.Play("stand");
                }
            }
        }
        internal int SelectTargetForSkill(string type, int actorId, TableConfig.Skill cfg, int seq, HashSet<int> history)
        {
            if (string.IsNullOrEmpty(type))
                return 0;
            EntityViewModel view = GetEntityViewById(actorId);
            if(null!=view && null!=view.Entity){
                EntityInfo entity = view.Entity;
                int campId = entity.GetCampId();
                if (type.CompareTo("minhp") == 0) {
                    int targetId = 0;
                    float minhp = float.MaxValue;
                    ScriptRuntime.Vector3 pos = entity.GetMovementStateInfo().GetPosition3D();
                    ClientModule.Instance.KdTree.Query(pos, cfg.distance, (float distSqr, KdTreeObject kdObj) => {
                        if (minhp > kdObj.Object.Hp && EntityInfo.GetRelation(campId, kdObj.Object.GetCampId()) == CharacterRelation.RELATION_ENEMY) {
                            int objId = kdObj.Object.GetId();
                            if (!history.Contains(objId)) {
                                minhp = kdObj.Object.Hp;
                                targetId = objId;
                            }
                        }
                    });
                    return targetId;
                } else if (type.CompareTo("maxdist") == 0) {
                    int targetId = 0;
                    float maxDistSqr = 0;
                    ScriptRuntime.Vector3 pos = entity.GetMovementStateInfo().GetPosition3D();
                    ClientModule.Instance.KdTree.Query(pos, cfg.distance, (float distSqr, KdTreeObject kdObj) => {
                        if (maxDistSqr < distSqr && EntityInfo.GetRelation(campId, kdObj.Object.GetCampId()) == CharacterRelation.RELATION_ENEMY) {
                            int objId = kdObj.Object.GetId();
                            if (!history.Contains(objId)) {
                                maxDistSqr = distSqr;
                                targetId = objId;
                            }
                        }
                    });
                    return targetId;
                } else if (type.CompareTo("randenemy") == 0) {
                    return GetRandEnemyId(campId, history);
                } else if (type.CompareTo("randfriend") == 0) {
                    return GetRandFriendId(campId, history);
                }
            }
            return 0;
        }
        internal int GetRandEnemyId(int campId, HashSet<int> history)
        {
            int id = 0;
            List<int> ids = new List<int>();
            for (LinkedListNode<EntityInfo> linkNode = ClientModule.Instance.EntityManager.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (EntityInfo.GetRelation(campId, info.GetCampId()) == CharacterRelation.RELATION_ENEMY && info.EntityType != (int)EntityTypeEnum.Tower) {
                    ids.Add(info.GetId());
                }
            }
            for (int ct = 0; ct < ids.Count; ++ct) {
                int index = Helper.Random.Next(ids.Count);
                if (index >= 0 && index < ids.Count) {
                    int _id = ids[index];
                    if (!history.Contains(id)) {
                        id = _id;
                        break;
                    }
                }
            }
            return id;
        }
        internal int GetRandFriendId(int campId, HashSet<int> history)
        {
            int id = 0;
            List<int> ids = new List<int>();
            for (LinkedListNode<EntityInfo> linkNode = ClientModule.Instance.EntityManager.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (EntityInfo.GetRelation(campId, info.GetCampId()) == CharacterRelation.RELATION_FRIEND && info.EntityType != (int)EntityTypeEnum.Tower) {
                    ids.Add(info.GetId());
                }
            }
            for (int ct = 0; ct < ids.Count; ++ct) {
                int index = Helper.Random.Next(ids.Count);
                if (index >= 0 && index < ids.Count) {
                    int _id = ids[index];
                    if (!history.Contains(id)) {
                        id = _id;
                        break;
                    }
                }
            }
            return id;
        }
        internal int GetRandEnemyId(int campId)
        {
            int id = 0;
            List<int> ids = new List<int>();
            for (LinkedListNode<EntityInfo> linkNode = ClientModule.Instance.EntityManager.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (EntityInfo.GetRelation(campId, info.GetCampId()) == CharacterRelation.RELATION_ENEMY && info.EntityType != (int)EntityTypeEnum.Tower) {
                    ids.Add(info.GetId());
                }
            }
            int index = Helper.Random.Next(ids.Count);
            if (index >= 0 && index < ids.Count) {
                id = ids[index];
            }
            return id;
        }
        internal int GetRandFriendId(int campId)
        {
            int id = 0;
            List<int> ids = new List<int>();
            for (LinkedListNode<EntityInfo> linkNode = ClientModule.Instance.EntityManager.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (EntityInfo.GetRelation(campId, info.GetCampId()) == CharacterRelation.RELATION_FRIEND && info.EntityType != (int)EntityTypeEnum.Tower) {
                    ids.Add(info.GetId());
                }
            }
            int index = Helper.Random.Next(ids.Count);
            if (index >= 0 && index < ids.Count) {
                id = ids[index];
            }
            return id;
        }
        internal bool HaveShield(int actorId)
        {
            EntityViewModel view = GetEntityViewById(actorId);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                if (entity.Shield > 0) {
                    return true;
                }
            }
            return false;
        }
        internal int GetTargetType(int actorId, TableConfig.Skill cfg, int seq)
        {
            if (cfg.type == (int)SkillOrImpactType.Skill) {
                return cfg.targetType;
            } else {
                EntityViewModel view = GetEntityViewById(actorId);
                if (null != view && null != view.Entity) {
                    EntityInfo entity = view.Entity;
                    ImpactInfo impactInfo = entity.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                    if (null != impactInfo) {
                        return impactInfo.TargetType;
                    }
                }
            }
            return 0;
        }
        internal int GetImpactDuration(int actorId, int impactId, int seq)
        {
            EntityViewModel view = GetEntityViewById(actorId);
            if (null != view && null != view.Entity) {
                ImpactInfo impactInfo = view.Entity.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                if (null != impactInfo && impactId == impactInfo.ImpactId) {
                    return impactInfo.DurationTime;
                }
            }
            return 0;
        }
        internal UnityEngine.Vector3 GetImpactSenderPosition(int actorId, int impactId, int seq)
        {
            EntityViewModel view = GetEntityViewById(actorId);
            if (null != view && null != view.Entity) {
                ImpactInfo impactInfo = view.Entity.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                if (null != impactInfo && impactId == impactInfo.ImpactId) {
                    return new UnityEngine.Vector3(impactInfo.SenderPosition.X, impactInfo.SenderPosition.Y, impactInfo.SenderPosition.Z);
                }
            }
            return UnityEngine.Vector3.zero;
        }
        internal int GetImpactSkillId(int actorId, int impactId, int seq)
        {
            EntityViewModel view = GetEntityViewById(actorId);
            if (null != view && null != view.Entity) {
                ImpactInfo impactInfo = view.Entity.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                if (null != impactInfo && impactId == impactInfo.ImpactId) {
                    return impactInfo.SkillId;
                }
            }
            return 0;
        }
        internal void CalcSenderAndTarget(GfxSkillSenderInfo senderObj, out int senderId, out int targetId)
        {
            senderId = 0;
            targetId = 0;

            int targetType = GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);            
            if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                senderId = senderObj.ActorId;
                targetId = senderObj.TargetActorId;
                if (targetType == (int)SkillTargetType.RandEnemy) {
                    targetId = GetRandEnemyId(GetCampId(senderId));
                } else if (targetType == (int)SkillTargetType.RandFriend) {
                    targetId = GetRandFriendId(GetCampId(senderId));
                } else if (targetType == (int)SkillTargetType.Friend) {
                    targetId = senderObj.ActorId;
                } else if (targetType == (int)SkillTargetType.Self) {
                    targetId = senderObj.ActorId;
                }
            } else {
                senderId = senderObj.TargetActorId;
                targetId = senderObj.ActorId;
                if (targetType == (int)SkillTargetType.RandEnemy) {
                    targetId = GetRandEnemyId(GetCampId(senderId));
                } else if (targetType == (int)SkillTargetType.RandFriend) {
                    targetId = GetRandFriendId(GetCampId(senderId));
                } else if (targetType == (int)SkillTargetType.Friend) {
                    targetId = senderObj.TargetActorId;
                } else if (targetType == (int)SkillTargetType.Self) {
                    targetId = senderObj.TargetActorId;
                }
            }
        }
        internal CharacterRelation GetRelation(int one, int other)
        {
            EntityViewModel view1 = GetEntityViewById(one);
            EntityViewModel view2 = GetEntityViewById(other);
            if (null== view1 || null == view1.Entity || null==view2 || null == view2.Entity)
                return CharacterRelation.RELATION_INVALID;
            else
                return EntityInfo.GetRelation(view1.Entity, view2.Entity);
        }
        internal CharacterRelation GetRelation(UnityEngine.GameObject one, UnityEngine.GameObject other)
        {
            EntityViewModel view1 = GetEntityView(one);
            EntityViewModel view2 = GetEntityView(other);
            if (null == view1 || null == view1.Entity || null == view2 || null == view2.Entity)
                return CharacterRelation.RELATION_INVALID;
            else
                return EntityInfo.GetRelation(view1.Entity, view2.Entity);
        }
        internal void AddState(int objId, string state)
        {
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                entity.SetStateFlag(Operate_Type.OT_AddBit, CharacterStateUtility.FromString(state));
            }
        }
        internal void RemoveState(int objId, string state)
        {
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                if (string.IsNullOrEmpty(state)) {
                    entity.StateFlag = 0;
                } else {
                    entity.SetStateFlag(Operate_Type.OT_RemoveBit, CharacterStateUtility.FromString(state));
                }
            }
        }
        internal void AddShield(int objId, TableConfig.Skill cfg, int seq)
        {
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                if (cfg.type == (int)SkillOrImpactType.Skill) {
                    entity.SetShield(Operate_Type.OT_Relative, cfg.addShield);
                } else {
                    ImpactInfo impactInfo = entity.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                    if (null != impactInfo) {
                        entity.SetShield(Operate_Type.OT_Relative, impactInfo.AddShield);
                    }
                }
            }
        }
        internal void RemoveShield(int objId, TableConfig.Skill cfg, int seq)
        {
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                if (cfg.type == (int)SkillOrImpactType.Skill) {
                    entity.SetShield(Operate_Type.OT_Absolute, 0);
                } else {
                    ImpactInfo impactInfo = entity.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                    if (null != impactInfo) {
                        entity.SetShield(Operate_Type.OT_Absolute, 0);
                    }
                }
            }
        }
        internal ImpactInfo SendImpact(int srcObjId, int targetId, int impactId, int skillId)
        {
            EntityViewModel view = GetEntityViewById(srcObjId);
            if(null!=view && null!=view.Entity){
                SkillInfo skillInfo = view.Entity.GetSkillStateInfo().GetSkillInfoById(skillId);
                GfxSkillSenderInfo senderInfo;
                SkillInstance skillInst = GfxSkillSystem.Instance.GetSkillInstance(srcObjId,skillId,0,out senderInfo);
                if(null!=skillInst){
                    Dictionary<string, object> args;
                    Skill.Trigers.TriggerUtil.CalcHitConfig(skillInst.LocalVariables, senderInfo.ConfigData, out args);
                    return EntityController.Instance.SendImpact(senderInfo.ConfigData, senderInfo.Seq, senderInfo.ActorId, srcObjId, targetId, impactId, args);
                }
            }
            return null;
        }
        internal ImpactInfo SendImpact(TableConfig.Skill cfg, int seq, int curObjId, int srcObjId, int targetId, int impactId, Dictionary<string, object> args)
        {
            EntityViewModel view = GetEntityViewById(targetId);
            if (null != view && null != view.Entity && null != view.Actor && !view.Entity.IsDead()) {
                EntityInfo npc = view.Entity;
                if (null != cfg) {
                    UnityEngine.Quaternion hitEffectRotation = UnityEngine.Quaternion.identity;
                    UnityEngine.GameObject srcObj = GetGameObject(srcObjId);
                    UnityEngine.GameObject targetObj = view.Actor;
                    if (null != srcObj && null != targetObj) {
                        hitEffectRotation = UnityEngine.Quaternion.Inverse(srcObj.transform.localRotation);
                    }

                    ImpactInfo impactInfo = null;
                    if (impactId <= 0) {
                        impactInfo = new ImpactInfo(PredefinedSkill.Instance.HitSkillCfg);
                        impactId = PredefinedSkill.Instance.HitSkillCfg.id;
                    } else {
                        impactInfo = new ImpactInfo(impactId);
                    }
                    if (TryInitImpactInfo(impactInfo, cfg, seq, curObjId, srcObjId)) {
                        npc.GetSkillStateInfo().AddImpact(impactInfo);
                        SkillInfo skillInfo = npc.GetSkillStateInfo().GetCurSkillInfo();
                        if (null != skillInfo && (cfg.isInterrupt || impactInfo.ConfigData.isInterrupt)) {
                            GfxSkillSystem.Instance.StopSkill(targetId, skillInfo.SkillId, 0, true);
                        }
                        GfxSkillSystem.Instance.StartSkill(targetId, impactInfo.ConfigData, impactInfo.Seq, args, new Dictionary<string, object> { { "hitEffectRotation", hitEffectRotation } });
                        return impactInfo;
                    }
                }
            }
            return null;
        }
        internal ImpactInfo TrackImpact(TableConfig.Skill cfg, int seq, int curObjId, int srcObjId, int targetId, string emitBone, int emitImpact, UnityEngine.Vector3 offset, Dictionary<string, object> args)
        {
            EntityViewModel view = GetEntityViewById(targetId);
            EntityViewModel srcView = GetEntityViewById(srcObjId);
            if (null != view && null != view.Entity && null != view.Actor && !view.Entity.IsDead()) {
                EntityInfo npc = view.Entity;
                if (null != cfg) {
                    ImpactInfo impactInfo = null;
                    if (emitImpact <= 0) {
                        impactInfo = new ImpactInfo(PredefinedSkill.Instance.EmitSkillCfg);
                    } else {
                        impactInfo = new ImpactInfo(emitImpact);
                    }
                    if (TryInitImpactInfo(impactInfo, cfg, seq, curObjId, srcObjId)) {
                        UnityEngine.GameObject srcObj = GetGameObject(srcObjId);
                        if (null != srcObj) {
                            UnityEngine.Transform t = Utility.FindChildRecursive(srcObj.transform, emitBone);
                            if (null != t) {
                                UnityEngine.Vector3 pos = t.TransformPoint(offset);
                                impactInfo.SenderPosition = new ScriptRuntime.Vector3(pos.x, pos.y, pos.z);
                            } else {
                                UnityEngine.Vector3 pos = srcObj.transform.TransformPoint(offset);
                                pos.y += npc.GetRadius();
                                impactInfo.SenderPosition = new ScriptRuntime.Vector3(pos.x, pos.y, pos.z);
                            }
                        }
                        npc.GetSkillStateInfo().AddImpact(impactInfo);
                        GfxSkillSystem.Instance.StartSkill(targetId, impactInfo.ConfigData, impactInfo.Seq, args);
                        return impactInfo;
                    }
                }
            }
            return null;
        }
        internal ImpactInfo TrackSendImpact(int targetId, int impactId, int seq, Dictionary<string, object> args)
        {
            EntityViewModel view = GetEntityViewById(targetId);
            if (null != view && null != view.Entity && null != view.Actor && !view.Entity.IsDead()) {
                EntityInfo npc = view.Entity;
                ImpactInfo trackImpactInfo = npc.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                if (null != trackImpactInfo && impactId == trackImpactInfo.ImpactId) {
                    ImpactInfo impactInfo = null;
                    int targetImpactId = trackImpactInfo.ImpactToTarget;
                    if (targetImpactId <= 0) {
                        targetImpactId = trackImpactInfo.ConfigData.impactToTarget;
                        if (targetImpactId <= 0) {
                            targetImpactId = PredefinedSkill.Instance.HitSkillCfg.id;
                            impactInfo = new ImpactInfo(PredefinedSkill.Instance.HitSkillCfg);
                        }
                    }
                    if (null == impactInfo) {
                        impactInfo = new ImpactInfo(targetImpactId);
                    }
                    impactInfo.StartTime = TimeUtility.GetLocalMilliseconds();
                    impactInfo.ImpactSenderId = trackImpactInfo.ImpactSenderId;
                    impactInfo.SenderPosition = trackImpactInfo.SenderPosition;
                    impactInfo.SkillId = trackImpactInfo.SkillId;
                    impactInfo.DurationTime = trackImpactInfo.DurationTime;
                    impactInfo.TargetType = trackImpactInfo.TargetType;
                    impactInfo.Damage = trackImpactInfo.Damage;
                    impactInfo.AddAttack = trackImpactInfo.AddAttack;
                    impactInfo.AddShield = trackImpactInfo.AddShield;
                    impactInfo.AddSpeed = trackImpactInfo.AddSpeed;
                    impactInfo.HpRecover = trackImpactInfo.HpRecover;
                    npc.GetSkillStateInfo().AddImpact(impactInfo);
                    GfxSkillSystem.Instance.StartSkill(targetId, impactInfo.ConfigData, impactInfo.Seq, args);
                    return impactInfo;
                }
            }
            return null;
        }
        internal void ImpactDamage(int srcObjId, int targetId, int impactId, int seq)
        {
            if (ClientModule.Instance.IsRoomScene)
                return;
            EntityViewModel view = GetEntityViewById(targetId);
            EntityViewModel srcView = GetEntityViewById(srcObjId);
            if (null != view && null != view.Entity && null != view.Actor && !view.Entity.IsDead()) {
                EntityInfo entity = view.Entity;
                EntityInfo srcNpc = null;
                if (null != srcView && null != srcView.Entity) {
                    srcNpc = srcView.Entity;
                }
                ImpactInfo impactInfo = entity.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                if (null != impactInfo && impactId == impactInfo.ImpactId) {
                    TableConfig.Skill cfg = impactInfo.ConfigData;
                    int targetType = impactInfo.TargetType;
                    float damage = cfg.damage != 0 && null != srcNpc ? srcNpc.GetActualProperty().AttackBase * cfg.damage / 1000.0f : impactInfo.Damage;
                    int addShield = cfg.addShield != 0 ? cfg.addShield : impactInfo.AddShield;
                    int hpRecover = cfg.hpRecover != 0 ? cfg.hpRecover : impactInfo.HpRecover;

                    if (hpRecover != 0) {
                        entity.SetHp(Operate_Type.OT_Relative, (int)impactInfo.HpRecover);
                        entity.SetAttackerInfo(srcObjId, false, true, false, -impactInfo.HpRecover, 0);
                    }
                    if (addShield != 0) {
                        entity.SetShield(Operate_Type.OT_Relative, impactInfo.AddShield);
                    }
                    if ((targetType == (int)SkillTargetType.Enemy || targetType == (int)SkillTargetType.RandEnemy) && damage != 0) {
                        if (entity.EntityType == (int)EntityTypeEnum.Tower) {
                            if (null != srcNpc && srcNpc.NormalSkillId != impactInfo.SkillId) {
                                //技能打塔不产生伤害
                                return;
                            }
                        }
                        bool isKiller = false;
                        if (entity.Shield >= damage) {
                            entity.SetShield(Operate_Type.OT_Relative, -(int)damage);
                        } else if (entity.Shield > 0) {
                            int leftDamage = (int)damage - entity.Shield;
                            entity.SetShield(Operate_Type.OT_Absolute, 0);
                            if (entity.GetId() == ClientModule.Instance.LeaderID && entity.Hp <= leftDamage) {
                                //队长不死，demo专用代码
                            } else {
                                entity.SetHp(Operate_Type.OT_Relative, -(int)leftDamage);
                                if (entity.Hp <= 0) {
                                    isKiller = true;
                                }
                            }
                        } else {
                            if (entity.GetId() == ClientModule.Instance.LeaderID && entity.Hp <= damage) {
                                //队长不死，demo专用代码
                            } else {
                                entity.SetHp(Operate_Type.OT_Relative, -(int)damage);
                                if (entity.Hp <= 0) {
                                    isKiller = true;
                                }
                            }
                        }
                        if (isKiller) {
                            entity.GetCombatStatisticInfo().AddDeadCount(1);
                            if (null != srcNpc) {
                                EntityInfo killer = srcNpc;
                                if (killer.SummonerId > 0) {
                                    EntityViewModel npcViewModel = GetEntityViewById(killer.SummonerId);
                                    if (null != npcViewModel) {
                                        killer = npcViewModel.Entity;
                                    }
                                }
                                if (entity.EntityType == (int)EntityTypeEnum.Tower) {
                                    killer.GetCombatStatisticInfo().AddKillTowerCount(1);
                                } else if (entity.EntityType == (int)EntityTypeEnum.Hero) {
                                    killer.GetCombatStatisticInfo().AddKillHeroCount(1);
                                    killer.GetCombatStatisticInfo().AddMultiKillCount(1);
                                } else {
                                    killer.GetCombatStatisticInfo().AddKillNpcCount(1);
                                }
                            }
                        }
                        entity.SetAttackerInfo(srcObjId, isKiller, true, false, (int)damage, 0);
                    }
                }
            }
        }
        internal void BornFinish(int objId)
        {
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view) {
                EntityInfo entity = view.Entity;
                if (null != entity) {
                    entity.IsBorning = false;
                    entity.SetAIEnable(true);
                    entity.SetStateFlag(Operate_Type.OT_RemoveBit, CharacterState_Type.CST_Invincible);
                }
            }
        }
        internal void DeadFinish(int objId)
        {
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view) {
                EntityInfo entity = view.Entity;
                if (null != entity) {
                    entity.DeadTime = 0;
                    entity.NeedDelete = true;
                }
            }
        }

        private bool TryInitImpactInfo(ImpactInfo impactInfo, TableConfig.Skill cfg, int seq, int curObjId, int srcObjId)
        {
            bool ret = false;
            EntityInfo srcNpc = null;
            EntityViewModel srcView = GetEntityViewById(srcObjId);
            if (null != srcView && null != srcView.Entity) {
                srcNpc = srcView.Entity;
            }
            impactInfo.StartTime = TimeUtility.GetLocalMilliseconds();
            impactInfo.ImpactSenderId = srcObjId;
            if (cfg.type == (int)SkillOrImpactType.Skill) {
                impactInfo.SenderPosition = null != srcNpc ? srcNpc.GetMovementStateInfo().GetPosition3D() : ScriptRuntime.Vector3.Zero;
                impactInfo.SkillId = cfg.id;
                impactInfo.DurationTime = (int)(cfg.duration * 1000);
                impactInfo.TargetType = cfg.targetType;
                impactInfo.Damage = null != srcNpc ? srcNpc.GetActualProperty().AttackBase * cfg.damage / 1000.0f : 0;
                impactInfo.AddAttack = cfg.addAttack;
                impactInfo.AddShield = cfg.addShield;
                impactInfo.AddSpeed = cfg.addSpeed;
                impactInfo.HpRecover = cfg.hpRecover;
                impactInfo.ImpactToTarget = cfg.impactToTarget;
                ret = true;
            } else {
                ImpactInfo srcImpactInfo = null;
                EntityViewModel myView = GetEntityViewById(curObjId);
                if (null != myView && null != myView.Entity) {
                    srcImpactInfo = myView.Entity.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                }
                if (null != srcImpactInfo) {
                    //如果当前技能配置有数据则继承当前配置数据，否则继承源impact记录的数据。
                    impactInfo.SenderPosition = srcImpactInfo.SenderPosition;
                    impactInfo.SkillId = srcImpactInfo.SkillId;
                    impactInfo.DurationTime = (int)(cfg.duration * 1000);
                    impactInfo.TargetType = srcImpactInfo.TargetType;
                    impactInfo.Damage = cfg.damage != 0 && null != srcNpc ? srcNpc.GetActualProperty().AttackBase * cfg.damage / 1000.0f : srcImpactInfo.Damage;
                    impactInfo.AddAttack = cfg.addAttack != 0 ? cfg.addAttack : srcImpactInfo.AddAttack;
                    impactInfo.AddShield = cfg.addShield != 0 ? cfg.addShield : srcImpactInfo.AddShield;
                    impactInfo.AddSpeed = Math.Abs(cfg.addSpeed) > Geometry.c_FloatPrecision ? cfg.addSpeed : srcImpactInfo.AddSpeed;
                    impactInfo.HpRecover = cfg.hpRecover != 0 ? cfg.hpRecover : srcImpactInfo.HpRecover;
                    impactInfo.ImpactToTarget = cfg.impactToTarget != 0 ? cfg.impactToTarget : srcImpactInfo.ImpactToTarget;
                    ret = true;
                }
            }
            return ret;
        }

        private EntityController()
        {
        }

        internal static EntityController Instance
        {
            get { return s_instance_; }
        }

        private static EntityController s_instance_ = new EntityController();
    }
}