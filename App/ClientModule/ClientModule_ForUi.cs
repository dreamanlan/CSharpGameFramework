using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GameFramework.GmCommands;
using GameFramework.Story;
using GameFramework.Skill;

namespace GameFramework
{
    public partial class ClientModule
    {
        public UnityEngine.GameObject GetGameObject(int actorId)
        {
            return EntityController.Instance.GetGameObject(actorId);
        }
        public int GetCampId(int actorId)
        {
            return EntityController.Instance.GetCampId(actorId);
        }
        public void MoveTo(float x, float y, float z)
        {
            if (IsRoomScene) {
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
                    KdTree.Query(obj, skillInfo.distance, (float distSqr, KdTreeObject _obj) => {
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
                    if (obj.Energy >= obj.GetActualProperty().EnergyMax) {
                        if (!skillInfo.IsInCd(TimeUtility.GetLocalMilliseconds())) {
                            if (IsRoomScene) {
                                Network.NetworkSystem.Instance.SyncPlayerSkill(obj, skillId);
                            } else {
                                AiStateInfo aiInfo = obj.GetAiStateInfo();
                                if (aiInfo.AiLogic == (int)AiStateLogicId.Entity_Leader) {
                                    AiData_Leader data = aiInfo.AiDatas.GetData<AiData_Leader>();
                                    if (null == data) {
                                        data = new AiData_Leader();
                                        aiInfo.AiDatas.AddData(data);
                                    }
                                    data.ManualSkillId = skillId;
                                } else {
                                    AiData_General data = aiInfo.AiDatas.GetData<AiData_General>();
                                    if (null == data) {
                                        data = new AiData_General();
                                        aiInfo.AiDatas.AddData(data);
                                    }
                                    data.ManualSkillId = skillId;
                                }
                                aiInfo.ChangeToState((int)AiStateId.SkillCommand);
                            }
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }
        public void SetOperateType(bool bAuto)
        {
            int leaderID = ClientModule.Instance.LeaderID;
            EntityInfo obj = GetEntityById(leaderID);
            if (null != obj) {
                if (IsRoomScene) {
                    Network.NetworkSystem.Instance.SyncOperateMode(bAuto);
                } else {
                    AiStateInfo aiInfo = obj.GetAiStateInfo();
                    AiData_Leader data = aiInfo.AiDatas.GetData<AiData_Leader>();
                    if (null == data) {
                        data = new AiData_Leader();
                        aiInfo.AiDatas.AddData(data);
                    }
                    data.IsAutoOperate = bAuto;
                }
            }
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
                float mp = (float)entity.Energy / entity.GetActualProperty().EnergyMax;
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
            KdTree.Query(entity, radius, (float sqrDist, KdTreeObject obj) => {
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
