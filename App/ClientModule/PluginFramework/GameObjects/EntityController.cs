using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework.Skill;
using SkillSystem;

namespace GameFramework
{
    public class EntityController
    {
        public void Init()
        {
        }

        public void Release()
        {
        }

        public void Tick()
        {
        }

        public EntityViewModel GetEntityViewById(int objId)
        {
            return EntityViewModelManager.Instance.GetEntityViewById(objId);
        }

        public EntityViewModel GetEntityViewByUnitId(int unitId)
        {
            return EntityViewModelManager.Instance.GetEntityViewByUnitId(unitId);
        }

        public bool IsVisible(int objId)
        {
            bool ret = false;
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view) {
                ret = view.Visible;
            }
            return ret;
        }

        public UnityEngine.GameObject GetGameObject(int objId)
        {
            return EntityViewModelManager.Instance.GetGameObject(objId);
        }
        public UnityEngine.GameObject GetGameObjectByUnitId(int unitId)
        {
            return EntityViewModelManager.Instance.GetGameObjectByUnitId(unitId);
        }
        public EntityViewModel GetEntityView(UnityEngine.GameObject obj)
        {
            return EntityViewModelManager.Instance.GetEntityView(obj);
        }
        public int GetGameObjectUnitId(UnityEngine.GameObject obj)
        {
            return EntityViewModelManager.Instance.GetGameObjectUnitId(obj);
        }
        public int GetGameObjectId(UnityEngine.GameObject obj)
        {
            return EntityViewModelManager.Instance.GetGameObjectId(obj);
        }
        public bool ExistGameObject(UnityEngine.GameObject obj)
        {
            return EntityViewModelManager.Instance.ExistGameObject(obj);
        }
        public bool ExistGameObject(int objId)
        {
            return EntityViewModelManager.Instance.ExistGameObject(objId);
        }
        public int GetEntityType(int objId)
        {
            int type = 0;
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                type = view.Entity.EntityType;
            }
            return type;
        }
        public int GetEntityType(UnityEngine.GameObject obj)
        {
            int type = 0;
            EntityViewModel view = GetEntityView(obj);
            if (null != view && null != view.Entity) {
                type = view.Entity.EntityType;
            }
            return type;
        }
        public int GetCampId(int objId)
        {
            int campId = 0;
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                campId = entity.GetCampId();
            }
            return campId;
        }
        public int GetCampId(UnityEngine.GameObject obj)
        {
            int campId = 0;
            EntityViewModel view = GetEntityView(obj);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                campId = entity.GetCampId();
            }
            return campId;
        }
        public bool IsMovableEntity(int objId)
        {
            bool ret = true;
            int type = GetEntityType(objId);
            if (type == (int)EntityTypeEnum.Tower) {
                ret = false;
            }
            return ret;
        }
        public bool IsMovableEntity(UnityEngine.GameObject obj)
        {
            bool ret = true;
            int type = GetEntityType(obj);
            if (type == (int)EntityTypeEnum.Tower) {
                ret = false;
            }
            return ret;
        }
        public bool IsRotatableEntity(int objId)
        {
            bool ret = true;
            int type = GetEntityType(objId);
            if (type == (int)EntityTypeEnum.Tower) {
                ret = false;
            }
            return ret;
        }
        public bool IsRotatableEntity(UnityEngine.GameObject obj)
        {
            bool ret = true;
            int type = GetEntityType(obj);
            if (type == (int)EntityTypeEnum.Tower) {
                ret = false;
            }
            return ret;
        }
        public void SyncFaceDir(UnityEngine.GameObject obj)
        {
            EntityViewModel viewModel = GetEntityView(obj);
            if (null != viewModel) {
                viewModel.SyncSpatial();
            }
        }
        public bool CalcPosAndDir(int targetId, out ScriptRuntime.Vector3 pos, out float dir)
        {
            EntityViewModel view = GetEntityViewById(targetId);
            if (null != view) {
                MovementStateInfo msi = view.Entity.GetMovementStateInfo();
                pos = msi.GetPosition3D();
                dir = msi.GetFaceDir();
                return true;
            } else {
                pos = ScriptRuntime.Vector3.Zero;
                dir = 0;
                return false;
            }
        }
        public float CalcSkillDistance(float dist, int srcId, int targetId)
        {
            EntityViewModel view1 = GetEntityViewById(srcId);
            EntityViewModel view2 = GetEntityViewById(targetId);
            if (null != view1 && null != view2) {
                return dist + view1.Entity.GetRadius() + view2.Entity.GetRadius();
            } else {
                return dist;
            }
        }
        public bool CanCastSkill(UnityEngine.GameObject obj, TableConfig.Skill configData, int seq)
        {
            bool ret = true;
            if (configData.type == (int)SkillOrImpactType.Skill) {
                EntityViewModel view = GetEntityView(obj);
                if (null != view && null != view.Entity) {
                    EntityInfo entity = view.Entity;
                    if (entity.GetSkillStateInfo().IsSkillActivated()) {
                        SkillInfo skillInfo = entity.GetSkillStateInfo().GetCurSkillInfo();
                        if (null != skillInfo && skillInfo.ConfigData.skillData.interruptPriority >= configData.skillData.interruptPriority) {
                            ret = false;
                        }
                    }
                }
            }
            return ret;
        }
        public void CancelCastSkill(UnityEngine.GameObject obj)
        {
            EntityViewModel view = GetEntityView(obj);
            if (null != view && null != view.Entity) {
                view.Entity.IsControlByManual = false;
            }
        }
        public GfxSkillSenderInfo BuildSkillInfo(UnityEngine.GameObject obj, TableConfig.Skill configData, int seq)
        {
            GfxSkillSenderInfo ret = null;
            EntityViewModel view = GetEntityView(obj);
            if (null != view && null != view.Actor && null != view.Entity && null != configData) {
                EntityInfo entity = view.Entity;
                int objId = view.Entity.GetId();
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
            } else {
                ret = new GfxSkillSenderInfo(configData, seq, 0, obj);
            }
            return ret;
        }
        public void ActivateSkill(GfxSkillSenderInfo sender, SkillInstance instance)
        {
            int objId = sender.ObjId;
            int skillId = sender.SkillId;
            int seq = sender.Seq;

            if (objId <= 0)
                return;
            //LogSystem.Warn("{0} ActivateSkill {1} {2}", objId, skillId, seq);

            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                SkillInfo skillInfo = entity.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null != skillInfo) {
                    SkillInfo curSkillInfo = entity.GetSkillStateInfo().GetCurSkillInfo();
                    if (null != curSkillInfo && (curSkillInfo.ConfigData.skillData.interruptPriority < skillInfo.ConfigData.skillData.interruptPriority)) {
                        GfxSkillSystem.Instance.StopSkill(objId, curSkillInfo.SkillId, 0, true);
                        if (skillId == view.Entity.ManualSkillId) {
                            LogSystem.Warn("ManualSkill {0} interrupt {1}.", skillId, curSkillInfo.SkillId);
                        }
                    }
                    if (skillId == view.Entity.ManualSkillId) {
                        LogSystem.Warn("ManualSkill {0} activate.", skillId);
                    }
                    entity.GetSkillStateInfo().SetCurSkillInfo(skillId);
                    skillInfo.IsSkillActivated = true;
                    skillInfo.CdEndTime = TimeUtility.GetLocalMilliseconds() + (long)skillInfo.ConfigData.skillData.cooldown;
                    if (skillInfo.ConfigData.skillData.addsc > 0 && PluginFramework.Instance.IsBattleState) {
                        //回蓝
                        entity.Energy += skillInfo.ConfigData.skillData.addsc;
                        entity.EntityManager.FireDamageEvent(objId, 0, false, false, 0, -skillInfo.ConfigData.skillData.addsc);
                    }
                }
            }
        }
        public void DeactivateSkill(GfxSkillSenderInfo sender, SkillInstance instance)
        {
            int objId = sender.ObjId;
            int skillId = sender.SkillId;
            int seq = sender.Seq;

            if (objId <= 0)
                return;

            //LogSystem.Warn("{0} DeactivateSkill {1} {2}", objId, skillId, seq);

            EntityViewModel view = GetEntityViewById(objId);
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
        public void CancelIfImpact(UnityEngine.GameObject obj, TableConfig.Skill cfg, int seq)
        {
            EntityViewModel view = GetEntityView(obj);
            if (null != view && null != view.Entity) {
                if (cfg.type != (int)SkillOrImpactType.Skill) {
                    view.Entity.GetSkillStateInfo().RemoveImpact(seq);
                }
            }
        }
        public void StopSkillAnimation(UnityEngine.GameObject obj)
        {
            EntityViewModel view = GetEntityView(obj);
            if (null != view && null != view.Actor && null != view.Entity && !view.Entity.IsDead()) {
            
            }
        }
        public void PauseSkillAnimation(UnityEngine.GameObject obj, bool pause)
        {
            if (null != obj) {
                UnityEngine.Animator animator = obj.GetComponentInChildren<UnityEngine.Animator>();
                if (null != animator) {
                    if (pause)
                        animator.speed = 0;
                    else
                        animator.speed = 1;
                }
            }
        }
        public int SelectTargetForSkill(string type, int actorId, TableConfig.Skill cfg, int seq, HashSet<int> history)
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
                    PluginFramework.Instance.KdTree.QueryWithAction(pos, cfg.skillData.distance, (float distSqr, KdTreeObject kdObj) => {
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
                    PluginFramework.Instance.KdTree.QueryWithAction(pos, cfg.skillData.distance, (float distSqr, KdTreeObject kdObj) => {
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
        public int GetRandEnemyId(int campId, HashSet<int> history)
        {
            int id = 0;
            List<int> ids = new List<int>();
            for (LinkedListNode<EntityInfo> linkNode = PluginFramework.Instance.EntityManager.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
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
        public int GetRandFriendId(int campId, HashSet<int> history)
        {
            int id = 0;
            List<int> ids = new List<int>();
            for (LinkedListNode<EntityInfo> linkNode = PluginFramework.Instance.EntityManager.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
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
        public int GetRandEnemyId(int campId)
        {
            int id = 0;
            List<int> ids = new List<int>();
            for (LinkedListNode<EntityInfo> linkNode = PluginFramework.Instance.EntityManager.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
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
        public int GetRandFriendId(int campId)
        {
            int id = 0;
            List<int> ids = new List<int>();
            for (LinkedListNode<EntityInfo> linkNode = PluginFramework.Instance.EntityManager.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
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
        public bool HaveShield(int actorId)
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
        public int GetTargetType(int actorId, TableConfig.Skill cfg, int seq)
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
        public int GetImpactDuration(int actorId, int impactId, int seq)
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
        public UnityEngine.Vector3 GetImpactSenderPosition(int actorId, int impactId, int seq)
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
        public int GetImpactSkillId(int actorId, int impactId, int seq)
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
        public void CalcSenderAndTarget(GfxSkillSenderInfo senderObj, out int senderId, out int targetId)
        {
            senderId = 0;
            targetId = 0;

            int targetType = GetTargetType(senderObj.ObjId, senderObj.ConfigData, senderObj.Seq);            
            if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                senderId = senderObj.ObjId;
                targetId = senderObj.TargetObjId;
                if (targetType == (int)SkillTargetType.RandEnemy) {
                    targetId = GetRandEnemyId(GetCampId(senderId));
                } else if (targetType == (int)SkillTargetType.RandFriend) {
                    targetId = GetRandFriendId(GetCampId(senderId));
                } else if (targetType == (int)SkillTargetType.Friend) {
                    targetId = senderObj.ObjId;
                } else if (targetType == (int)SkillTargetType.Self) {
                    targetId = senderObj.ObjId;
                }
            } else {
                senderId = senderObj.TargetObjId;
                targetId = senderObj.ObjId;
                if (targetType == (int)SkillTargetType.RandEnemy) {
                    targetId = GetRandEnemyId(GetCampId(senderId));
                } else if (targetType == (int)SkillTargetType.RandFriend) {
                    targetId = GetRandFriendId(GetCampId(senderId));
                } else if (targetType == (int)SkillTargetType.Friend) {
                    targetId = senderObj.TargetObjId;
                } else if (targetType == (int)SkillTargetType.Self) {
                    targetId = senderObj.TargetObjId;
                }
            }
        }
        public CharacterRelation GetRelation(int one, int other)
        {
            EntityViewModel view1 = GetEntityViewById(one);
            EntityViewModel view2 = GetEntityViewById(other);
            if (null== view1 || null == view1.Entity || null==view2 || null == view2.Entity)
                return CharacterRelation.RELATION_INVALID;
            else
                return EntityInfo.GetRelation(view1.Entity, view2.Entity);
        }
        public CharacterRelation GetRelation(UnityEngine.GameObject one, UnityEngine.GameObject other)
        {
            EntityViewModel view1 = GetEntityView(one);
            EntityViewModel view2 = GetEntityView(other);
            if (null == view1 || null == view1.Entity || null == view2 || null == view2.Entity)
                return CharacterRelation.RELATION_INVALID;
            else
                return EntityInfo.GetRelation(view1.Entity, view2.Entity);
        }
        public bool HaveState(int objId, string state)
        {
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                EntityInfo npc = view.Entity;
                return npc.HaveState((CharacterPropertyEnum)CharacterStateUtility.NameToState(state));
            }
            return false;
        }
        public void DisableState(int objId, string state)
        {
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                EntityInfo npc = view.Entity;
                if (!string.IsNullOrEmpty(state)) {
                    npc.DisableState((CharacterPropertyEnum)CharacterStateUtility.NameToState(state));
                }
            }
        }
        public void AddState(int objId, string state)
        {
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                entity.AddState((CharacterPropertyEnum)CharacterStateUtility.NameToState(state));
            }
        }
        public void RemoveState(int objId, string state)
        {
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                if (string.IsNullOrEmpty(state)) {
                    //entity.StateFlag = 0;
                } else {
                    entity.RemoveState((CharacterPropertyEnum)CharacterStateUtility.NameToState(state));
                }
            }
        }
        public void AddShield(int objId, TableConfig.Skill cfg, int seq)
        {
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                int v;
                if (cfg.attrValues.TryGetValue((int)CharacterPropertyEnum.x2012_护盾值, out v)) {
                    entity.Shield += v;
                }
            }
        }
        public void RemoveShield(int objId, TableConfig.Skill cfg, int seq)
        {
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view && null != view.Entity) {
                EntityInfo entity = view.Entity;
                entity.Shield = 0;
            }
        }
        public ImpactInfo SendImpact(int srcObjId, int targetId, int impactId, int skillId)
        {
            EntityViewModel view = GetEntityViewById(srcObjId);
            if(null!=view && null!=view.Entity){
                SkillInfo skillInfo = view.Entity.GetSkillStateInfo().GetSkillInfoById(skillId);
                GfxSkillSenderInfo senderInfo;
                SkillInstance skillInst = GfxSkillSystem.Instance.FindActiveSkillInstance(srcObjId,skillId,0,out senderInfo);
                if(null!=skillInst){
                    Dictionary<string, object> args;
                    Skill.Trigers.TriggerUtil.CalcImpactConfig(0, impactId, skillInst, senderInfo.ConfigData, out args);
                    return EntityController.Instance.SendImpact(senderInfo.ConfigData, senderInfo.Seq, senderInfo.ObjId, srcObjId, targetId, impactId, true, args);
                }
            }
            return null;
        }
        public ImpactInfo SendImpact(TableConfig.Skill cfg, int seq, int curObjId, int srcObjId, int targetId, int impactId, bool isFinal, Dictionary<string, object> args)
        {
            EntityViewModel view = GetEntityViewById(targetId);
            if (null != view && null != view.Entity && null != view.Actor && !view.Entity.IsDeadSkillCasting()) {
                EntityInfo npc = view.Entity;
                if (null != cfg) {
                    UnityEngine.Quaternion hitEffectRotation = UnityEngine.Quaternion.identity;
                    UnityEngine.GameObject srcObj = GetGameObject(srcObjId);
                    UnityEngine.GameObject targetObj = view.Actor;
                    if (null != srcObj && null != targetObj) {
                        hitEffectRotation = srcObj.transform.localRotation;
                    }
                    var addArgs = new Dictionary<string, object> { { "hitEffectRotation", hitEffectRotation } };
                    ImpactInfo impactInfo = null;
                    if (impactId <= 0 || impactId >= SkillInstance.c_FirstInnerHitSkillId) {
                        impactInfo = new ImpactInfo(PredefinedSkill.Instance.HitSkillCfg);
                        impactId = PredefinedSkill.c_HitSkillId;
                    } else {
                        impactInfo = new ImpactInfo(impactId);
                    }
                    if (null != impactInfo.ConfigData) {
                        if (TryInitImpactInfo(impactInfo, cfg, seq, curObjId, srcObjId, args)) {
                            if (impactInfo.ConfigData.type == (int)SkillOrImpactType.Buff) {
                                ImpactInfo oldImpactInfo = npc.GetSkillStateInfo().FindImpactInfoById(impactInfo.ImpactId);
                                if (null != oldImpactInfo) {
                                    oldImpactInfo.DurationTime += impactInfo.DurationTime;
                                    return oldImpactInfo;
                                }
                            }
                            impactInfo.DamageData.IsFinal = isFinal;
                            npc.GetSkillStateInfo().AddImpact(impactInfo);
                            SkillInfo skillInfo = npc.GetSkillStateInfo().GetCurSkillInfo();
                            if (null != skillInfo && cfg.skillData.isInterrupt) {
                                GfxSkillSystem.Instance.StopSkill(targetId, skillInfo.SkillId, 0, true);
                            }
                            GfxSkillSystem.Instance.StartSkill(targetId, impactInfo.ConfigData, impactInfo.Seq, args, addArgs);
                            return impactInfo;
                        }
                    } else {
                        LogSystem.Error("impact {0} config can't found !", impactInfo.ImpactId);
                    }
                }
            }
            return null;
        }
        public ImpactInfo TrackImpact(TableConfig.Skill cfg, int seq, int curObjId, int srcObjId, int targetId, string emitBone, int emitImpact, UnityEngine.Vector3 offset, bool isFinal, Dictionary<string, object> args)
        {
            EntityViewModel view = GetEntityViewById(targetId);
            EntityViewModel srcView = GetEntityViewById(srcObjId);
            if (null != view && null != view.Entity && null != view.Actor && !view.Entity.IsDeadSkillCasting()) {
                EntityInfo npc = view.Entity;
                if (null != cfg) {
                    ImpactInfo impactInfo = null;
                    if (emitImpact <= 0 || emitImpact >= SkillInstance.c_FirstInnerEmitSkillId) {
                        impactInfo = new ImpactInfo(PredefinedSkill.Instance.EmitSkillCfg);
                    } else {
                        impactInfo = new ImpactInfo(emitImpact);
                    }
                    if (TryInitImpactInfo(impactInfo, cfg, seq, curObjId, srcObjId, args)) {
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
                        impactInfo.DamageData.IsFinal = isFinal;
                        npc.GetSkillStateInfo().AddImpact(impactInfo);
                        GfxSkillSystem.Instance.StartSkill(targetId, impactInfo.ConfigData, impactInfo.Seq, args);
                        return impactInfo;
                    }
                }
            }
            return null;
        }
        public int GetTrackSendImpact(int targetId, int seq, Dictionary<string, object> args)
        {
            int impactId = 0;
            EntityViewModel view = GetEntityViewById(targetId);
            if (null != view && null != view.Entity && null != view.Actor) {
                EntityInfo npc = view.Entity;
                ImpactInfo trackImpactInfo = npc.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                if (null != trackImpactInfo) {
                    int targetImpactId = trackImpactInfo.ImpactToTarget;
                    if (targetImpactId <= 0) {
                        targetImpactId = trackImpactInfo.ConfigData.impact;
                    }
                    if (targetImpactId <= 0) {
                        object v;
                        if (args.TryGetValue("impact", out v)) {
                            targetImpactId = (int)v;
                        }
                    }
                    impactId = targetImpactId;
                }
            }
            return impactId;
        }
        public ImpactInfo TrackSendImpact(int targetId, int impactId, int seq, int impactToTarget, Dictionary<string, object> args)
        {
            EntityViewModel view = GetEntityViewById(targetId);
            if (null != view && null != view.Entity && null != view.Actor && !view.Entity.IsDeadSkillCasting()) {
                EntityInfo npc = view.Entity;
                ImpactInfo trackImpactInfo = npc.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                if (null != trackImpactInfo && impactId == trackImpactInfo.ImpactId) {
                    ImpactInfo impactInfo = null;
                    if (impactToTarget <= 0 || impactToTarget >= SkillInstance.c_FirstInnerHitSkillId) {
                        impactToTarget = PredefinedSkill.c_HitSkillId;
                        impactInfo = new ImpactInfo(PredefinedSkill.Instance.HitSkillCfg);
                    }
                    var addArgs = new Dictionary<string, object>();
                    if (null == impactInfo) {
                        impactInfo = new ImpactInfo(impactId);
                    }
                    impactInfo.StartTime = TimeUtility.GetLocalMilliseconds();
                    impactInfo.ImpactSenderId = trackImpactInfo.ImpactSenderId;
                    impactInfo.SenderPosition = trackImpactInfo.SenderPosition;
                    impactInfo.SkillId = trackImpactInfo.SkillId;
                    impactInfo.DurationTime = trackImpactInfo.DurationTime > 0 ? trackImpactInfo.DurationTime : impactInfo.ConfigData.impactData.duration;
                    impactInfo.TargetType = trackImpactInfo.TargetType;
                    impactInfo.DamageData.CopyFrom(trackImpactInfo.DamageData);
                    impactInfo.DamageData.Merge(impactInfo.ConfigData.damageData);
                    if (impactInfo.ConfigData.type == (int)SkillOrImpactType.Buff) {
                        ImpactInfo oldImpactInfo = npc.GetSkillStateInfo().FindImpactInfoById(impactInfo.ImpactId);
                        if (null != oldImpactInfo) {
                            oldImpactInfo.DurationTime += impactInfo.DurationTime;
                            return oldImpactInfo;
                        }
                    }
                    npc.GetSkillStateInfo().AddImpact(impactInfo);
                    GfxSkillSystem.Instance.StartSkill(targetId, impactInfo.ConfigData, impactInfo.Seq, args, addArgs);
                    return impactInfo;
                }
            }
            return null;
        }
        public void ImpactDamage(int srcObjId, int targetId, int impactId, int seq, bool isFinal)
        {
            if (!PluginFramework.Instance.IsBattleState)
                return;
            EntityViewModel view = GetEntityViewById(targetId);
            EntityViewModel srcView = GetEntityViewById(srcObjId);
            if (null != view && null != view.Entity && null != view.Actor) {
                EntityInfo targetObj = view.Entity;
                EntityInfo srcObj = null;
                if (null != srcView && null != srcView.Entity) {
                    srcObj = srcView.Entity;
                }
                if (null != targetObj && !view.Entity.IsDeadSkillCasting()) {
                    ImpactInfo impactInfo = targetObj.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                    if (null != impactInfo && impactId == impactInfo.ImpactId) {
                        EntityInfo ownerObj = GetRootSummoner(srcObj);
                        int ownerId = 0;
                        if (null != ownerObj) {
                            ownerId = ownerObj.GetId();
                        }
                        int addsc = impactInfo.DamageData.AddSc;
                        int adduc = impactInfo.DamageData.AddUc;
                        
                        int index = impactInfo.CurDamageCount;
                        ++impactInfo.CurDamageCount;
                        int multiple = impactInfo.DamageData.GetMultiple(index);
                        int damage = impactInfo.DamageData.GetDamage(index);
                        long hitrate = 0;
                        long critrate = 0;
                        long blockrate = 0;
                        long phyDamage = 0;
                        long magDamage = 0;

                        hitrate = AttrCalculator.Calc(targetObj.SceneContext, impactInfo.SenderProperty, targetObj.ActualProperty, "hitrate");
                        critrate = AttrCalculator.Calc(targetObj.SceneContext, impactInfo.SenderProperty, targetObj.ActualProperty, "critrate");
                        blockrate = AttrCalculator.Calc(targetObj.SceneContext, impactInfo.SenderProperty, targetObj.ActualProperty, "blockrate");
                        long rnd = Helper.Random.Next();
                        long critonoff = 0;
                        long blockonoff = 0;
                        if (rnd <= critrate) {
                            critonoff = 1;
                        } else if (rnd > critrate && rnd <= critrate + blockrate) {
                            blockonoff = 1;
                        }
                        impactInfo.DamageData.IsCritical = critonoff > 0;
                        impactInfo.DamageData.IsBlock = blockonoff > 0;
                        phyDamage = AttrCalculator.Calc(targetObj.SceneContext, impactInfo.SenderProperty, targetObj.ActualProperty, "phydamage", multiple, damage, critonoff, blockonoff);
                        magDamage = AttrCalculator.Calc(targetObj.SceneContext, impactInfo.SenderProperty, targetObj.ActualProperty, "magdamage", multiple, damage, critonoff, blockonoff);

                        damage = (int)(phyDamage + magDamage);
                        if (damage < 0)
                            damage = 0;
                        int vampire = impactInfo.DamageData.GetVampire(index);
                        bool isKiller = false;
                        if (targetObj.Shield >= damage) {
                            targetObj.Shield -= (int)damage;
                        } else if (targetObj.Shield > 0) {
                            int leftDamage = (int)damage - targetObj.Shield;
                            targetObj.Shield = 0;
                            targetObj.Hp -= (int)leftDamage;
                            if (targetObj.Hp <= 0) {
                                isKiller = true;
                            }
                        } else {                            
                            targetObj.Hp -= (int)damage;
                            if (targetObj.Hp <= 0) {
                                isKiller = true;
                            }
                        }
                        if (isKiller) {
                            targetObj.GetCombatStatisticInfo().AddDeadCount(1);
                            if (null != srcObj) {
                                EntityInfo killer = srcObj;
                                if (killer.SummonerId > 0) {
                                    EntityViewModel npcViewModel = GetEntityViewById(killer.SummonerId);
                                    if (null != npcViewModel) {
                                        killer = npcViewModel.Entity;
                                    }
                                }
                                if (targetObj.EntityType == (int)EntityTypeEnum.Tower) {
                                    killer.GetCombatStatisticInfo().AddKillTowerCount(1);
                                } else if (targetObj.EntityType == (int)EntityTypeEnum.Hero) {
                                    killer.GetCombatStatisticInfo().AddKillHeroCount(1);
                                    killer.GetCombatStatisticInfo().AddMultiKillCount(1);
                                } else {
                                    killer.GetCombatStatisticInfo().AddKillNpcCount(1);
                                }
                            }
                        }
                        targetObj.SetAttackerInfo(srcObjId, isKiller, true, false, (int)damage, 0);
                    }
                }
            }
        }
        public void KeepTarget(int targetId)
        {
            EntityViewModel view = GetEntityViewById(targetId);
            if (null != view && null != view.Entity) {
                view.Entity.CanDead = false;
            }
        }
        public void BornFinish(int objId)
        {
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view) {
                EntityInfo entity = view.Entity;
                if (null != entity) {
                    entity.IsBorning = false;
                    entity.SetAIEnable(true);
                    entity.RemoveState(CharacterPropertyEnum.x3009_无敌);
                }
            }
        }
        public void DeadFinish(int objId)
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
        public EntityInfo GetRootSummoner(EntityInfo obj)
        {
            while (null != obj && obj.SummonerId > 0) {
                obj = PluginFramework.Instance.GetEntityById(obj.SummonerId);
            }
            return obj;
        }
        public int GetRootSummonerId(EntityInfo obj)
        {
            int ret = 0;
            obj = GetRootSummoner(obj);
            if (null != obj) {
                ret = obj.GetId();
            }
            return ret;
        }
        public EntityInfo GetRootSummonerInfo(EntityInfo obj, int curSkillId, out int skillId)
        {
            skillId = curSkillId;
            while (null != obj && obj.SummonerId > 0) {
                skillId = obj.SummonSkillId;
                obj = PluginFramework.Instance.GetEntityById(obj.SummonerId);
            }
            return obj;
        }
        private bool TryInitImpactInfo(ImpactInfo impactInfo, TableConfig.Skill cfg, int seq, int curObjId, int srcObjId, Dictionary<string, object> args)
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
                impactInfo.DurationTime = impactInfo.ConfigData.impactData.duration;
                impactInfo.TargetType = cfg.targetType;
                impactInfo.DamageData.CopyFrom(cfg.damageData);
                impactInfo.DamageData.Merge(impactInfo.ConfigData.damageData);
                impactInfo.ImpactToTarget = cfg.impact;
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
                    impactInfo.DurationTime = srcImpactInfo.DurationTime > 0 ? srcImpactInfo.DurationTime : impactInfo.ConfigData.impactData.duration;
                    impactInfo.TargetType = srcImpactInfo.TargetType;
                    impactInfo.DamageData.CopyFrom(srcImpactInfo.DamageData);
                    impactInfo.DamageData.Merge(impactInfo.ConfigData.damageData);
                    impactInfo.ImpactToTarget = cfg.impact != 0 ? cfg.impact : srcImpactInfo.ImpactToTarget;
                    ret = true;
                }
            }
            return ret;
        }

        private EntityController()
        {
        }

        public static EntityController Instance
        {
            get { return s_instance_; }
        }

        private static EntityController s_instance_ = new EntityController();
    }
}