using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFrameworkMessage;

namespace GameFramework
{
  public sealed partial class Scene
  {
    public void DestroyEntities(int[] unit_ids)
    {
      Msg_RC_DestroyNpc destroyNpcBuilder = new Msg_RC_DestroyNpc();
      for (int i = 0; i < unit_ids.Length; i++) {
        EntityInfo npc = EntityManager.GetEntityInfoByUnitId(unit_ids[i]);
        if (npc != null) {
          destroyNpcBuilder.npc_id = npc.GetId();
          NotifyAllUser(RoomMessageDefine.Msg_RC_DestroyNpc, destroyNpcBuilder);
          EntityManager.RemoveEntity(npc.GetId());
        }
      }
    }

    public void ReloadObjects()
    {
        if (m_PreparedReloadMonsterCount > 0) {
            for (int i = 0; i < m_PreparedReloadMonsterCount; ++i) {
                TableConfig.LevelMonster monster = m_PreparedReloadMonsters[i];
                if (null != monster) {
                    int campId = monster.camp;
                    int unitId = campId * 10000 + i;
                    int objId = CreateEntity(unitId, monster.x, 0.0f, monster.y, monster.dir, campId, monster.actorID, monster.aiLogic, monster.aiParams.ToArray());
                    EntityInfo npc = GetEntityById(objId);
                    if (null != npc) {
                        npc.IsPassive = monster.passive;
                        npc.LevelMonsterData = monster;
                        npc.Level = monster.level;

                        Msg_RC_CreateNpc msg = DataSyncUtility.BuildCreateNpcMessage(npc);
                        NotifyAllUser(RoomMessageDefine.Msg_RC_CreateNpc, msg);

                        Msg_RC_SyncProperty msg2 = DataSyncUtility.BuildSyncPropertyMessage(npc);
                        NotifyAllUser(RoomMessageDefine.Msg_RC_SyncProperty, msg2);
                    }
                }
            }
            m_PreparedReloadMonsterCount = 0;
        }
        while (m_ReloadMonstersQueue.Count > 0 && m_PreparedReloadMonsterCount < c_MaxReloadMonsterNum) {
            m_PreparedReloadMonsters[m_PreparedReloadMonsterCount++] = m_ReloadMonstersQueue.Dequeue();
        }
    }

    private void OnHightlightPrompt(int userId, string dict, object[] args)
    {
      Msg_RC_HighlightPrompt builder = new Msg_RC_HighlightPrompt();
      builder.dict_id = dict;
      foreach (object arg in args) {
        builder.argument.Add(arg.ToString());
      }
      if (userId > 0) {
        EntityInfo info = EntityManager.GetEntityInfo(userId);
        if (null != info) {
          User user = info.CustomData as User;
          if (null != user) {
            user.SendMessage(RoomMessageDefine.Msg_RC_HighlightPrompt, builder);
          }
        }
      } else {
        NotifyAllUser(RoomMessageDefine.Msg_RC_HighlightPrompt, builder);
      }
    }

    private void OnCreateEntity(EntityInfo entity)
    {
        if (null != entity) {
            OnAiInitDslLogic(entity);
            StorySystem.SendMessage("obj_created", entity.GetId());
            StorySystem.SendMessage(string.Format("npc_created:{0}", entity.GetUnitId()), entity.GetId());
        }
    }
    private void OnDestroyEntity(EntityInfo entity)
    {
        if (null != entity) {
            OnAiDestroy(entity);
            TableConfig.LevelMonster monster = entity.LevelMonsterData;
            if (null != monster) {
                ReloadMonstersQueue.Enqueue(monster);
            }
        }
    }
    private void OnAiInitDslLogic(EntityInfo npc)
    {
        AiStateInfo aiInfo = npc.GetAiStateInfo();
        if (aiInfo.AiParam.Length >= 2) {
            string storyId = aiInfo.AiLogic;
            string storyFile = aiInfo.AiParam[0];
            if (!string.IsNullOrEmpty(storyId) && !string.IsNullOrEmpty(storyFile)) {
                aiInfo.HomePos = npc.GetMovementStateInfo().GetPosition3D();
                aiInfo.ChangeToState((int)PredefinedAiStateId.Idle);
                aiInfo.AiStoryInstanceInfo = StorySystem.NewAiStoryInstance(storyId, string.Empty, storyFile);
                if (null != aiInfo.AiStoryInstanceInfo) {
                    aiInfo.AiStoryInstanceInfo.m_StoryInstance.Context = this;
                    aiInfo.AiStoryInstanceInfo.m_StoryInstance.SetVariable("@objid", npc.GetId());
                    aiInfo.AiStoryInstanceInfo.m_StoryInstance.Start();
                }
            }
        }
        m_EntitiesForAi.Add(npc);
    }
    private void OnAiDestroy(EntityInfo npc)
    {
        m_EntitiesForAi.Remove(npc);
        var aiStateInfo = npc.GetAiStateInfo();
        var aiStoryInfo = aiStateInfo.AiStoryInstanceInfo;
        if (null != aiStoryInfo) {
            aiStoryInfo.Recycle();
            aiStateInfo.AiStoryInstanceInfo = null;
        }
    }
    private void OnAiMoveCommand(EntityInfo npc, long deltaTime)
    {
        DoMoveCommandState(npc, deltaTime);
    }
    private void OnAiWaitCommand(EntityInfo npc, long deltaTime)
    {
        if (npc.GetMovementStateInfo().IsMoving) {
            AiStopPursue(npc);
        }
    }

    private static void DoMoveCommandState(EntityInfo npc, long deltaTime)
    {
        //执行状态处理
        AiData_ForMoveCommand data = GetAiDataForMoveCommand(npc);
        if (null == data) return;

        if (!data.IsFinish) {
            if (WayPointArrived(npc, data)) {
                ScriptRuntime.Vector3 targetPos = new ScriptRuntime.Vector3();
                MoveToNext(npc, data, ref targetPos);
                if (!data.IsFinish) {
                    AiPursue(npc, targetPos);
                }
            } else {
                ScriptRuntime.Vector3 targetPos = data.WayPoints[data.Index];
                AiPursue(npc, targetPos);
            }
        }

        //判断是否状态结束并执行相应处理
        if (data.IsFinish) {
            Scene scene = npc.SceneContext.CustomData as Scene;
            if (null != scene) {
                scene.StorySystem.SendMessage("npcarrived:" + npc.GetUnitId(), npc.GetId());
                scene.StorySystem.SendMessage("objarrived", npc.GetId());
            }
            AiStopPursue(npc);
            npc.GetAiStateInfo().ChangeToState((int)PredefinedAiStateId.Idle);
        }
    }
    private static void MoveToNext(EntityInfo charObj, AiData_ForMoveCommand data, ref ScriptRuntime.Vector3 targetPos)
    {
        if (++data.Index >= data.WayPoints.Count) {
            data.IsFinish = true;
            return;
        }

        var move_info = charObj.GetMovementStateInfo();
        targetPos = data.WayPoints[data.Index];
        move_info.TargetPosition = targetPos;
    }
    private static bool WayPointArrived(EntityInfo charObj, AiData_ForMoveCommand data)
    {
        bool ret = false;
        var move_info = charObj.GetMovementStateInfo();
        float powDistDest = move_info.CalcDistancSquareToTarget();
        if (powDistDest <= 1f) {
            ret = true;
        }
        return ret;
    }
    private static AiData_ForMoveCommand GetAiDataForMoveCommand(EntityInfo npc)
    {
        AiData_ForMoveCommand data = npc.GetAiStateInfo().AiDatas.GetData<AiData_ForMoveCommand>();
        return data;
    }
    private static void AiPursue(EntityInfo npc, ScriptRuntime.Vector3 target)
    {
        MovementStateInfo msi = npc.GetMovementStateInfo();
        msi.IsMoving = true;
        msi.TargetPosition = target;
        float dir = Geometry.GetYRadian(msi.GetPosition3D(), target);
        msi.SetFaceDir(dir);
    }
    private static void AiStopPursue(EntityInfo npc)
    {
        npc.GetMovementStateInfo().IsMoving = false;
    }
  }
}
