using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;
using GameFramework;
using GameFrameworkMessage;
using GameFramework.Network;
using GameFramework.Skill;
using GameFramework.Story;
using ScriptRuntime;
using System.Collections;

internal class MsgPongHandler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_Pong pong_msg = msg as Msg_Pong;
        if (pong_msg == null) {
            return;
        }
        long time = TimeUtility.GetLocalMilliseconds();
        NetworkSystem.Instance.OnPong(time, pong_msg.send_ping_time, pong_msg.send_pong_time);
    }
}
internal class MsgShakeHandsRetHandler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_ShakeHands_Ret ret_msg = msg as Msg_RC_ShakeHands_Ret;
        if (msg == null) {
            return;
        }
        NetworkSystem.Instance.WaitShakeHands = false;
        if (ret_msg.auth_result == Msg_RC_ShakeHands_Ret.RetType.SUCCESS) {
            LogSystem.Info("auth ok !!!");
            NetworkSystem.Instance.CanSendMessage = true;
            ClientModule.Instance.QueueAction(ClientModule.Instance.OnRoomServerConnected);
        } else {
            LogSystem.Info("auth failed !!!");
        }
    }
}

internal class Msg_RC_CreateNpc_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_CreateNpc targetmsg = msg as Msg_RC_CreateNpc;
        if (null == targetmsg) {
            return;
        }
        EntityInfo cb = ClientModule.Instance.GetEntityById(targetmsg.npc_id);
        if (cb != null) {
            LogSystem.Info("NpcCreate obj already exist:" + targetmsg.npc_id + " unit:" + targetmsg.unit_id);
            return;
        }
        LogSystem.Info("NpcCreate:" + targetmsg.npc_id + " unit:" + targetmsg.unit_id + " ownerid=" + targetmsg.owner_id);
        EntityInfo npc = ClientModule.Instance.CreateEntity(targetmsg.npc_id, targetmsg.unit_id, targetmsg.cur_pos.x, 0, targetmsg.cur_pos.z, targetmsg.face_direction, targetmsg.camp_id, targetmsg.link_id);
        if (null != npc) {
            npc.SetLevel(targetmsg.level);
            npc.SetAIEnable(false);
            npc.GetMovementStateInfo().IsMoving = false;
            if (targetmsg.owner_id > 0) {
                npc.OwnerId = targetmsg.owner_id;
            }
            if (targetmsg.leader_id > 0) {
                npc.GetAiStateInfo().LeaderID = targetmsg.leader_id;
            }
            if (targetmsg.key > 0 && targetmsg.key == NetworkSystem.Instance.Key) {
                ClientModule.Instance.LeaderID = targetmsg.npc_id;
            }
        }
    }
}
internal class Msg_RC_NpcDead_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_NpcDead targetmsg = msg as Msg_RC_NpcDead;
        if (null == targetmsg) {
            return;
        }

        EntityInfo npc = ClientModule.Instance.GetEntityById(targetmsg.npc_id);
        if (null == npc) {
            LogSystem.Info("NpcDead can't find obj:" + targetmsg.npc_id);
            return;
        }
        LogSystem.Info("NpcDead:" + targetmsg.npc_id);

        npc.SetHp(Operate_Type.OT_Absolute, 0);
        npc.GetMovementStateInfo().IsMoving = false;
    }
}
internal class Msg_RC_DestroyNpc_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_DestroyNpc destroyMsg = msg as Msg_RC_DestroyNpc;
        if (destroyMsg == null) {
            return;
        }
        EntityInfo info = ClientModule.Instance.GetEntityById(destroyMsg.npc_id);
        if (null == info) {
            LogSystem.Info("NpcDestroy can't find obj:" + destroyMsg.npc_id);
            return;
        }
        LogSystem.Info("NpcDestroy:" + destroyMsg.npc_id);

        EntityInfo npc = info;
        if (null != npc) {
            EntityViewModelManager.Instance.DestroyEntityView(npc.GetId());
            ClientModule.Instance.DestroyEntityById(npc.GetId());
        }
    }
}

internal class Msg_RC_NpcMove_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_NpcMove targetmsg = msg as Msg_RC_NpcMove;
        if (null == targetmsg) {
            return;
        }
        EntityInfo npc = ClientModule.Instance.GetEntityById(targetmsg.npc_id);
        if (null == npc) {
            return;
        }
        if (targetmsg.velocity > 0) {
            float x, y;
            float tx, ty;
            ProtoHelper.DecodePosition2D(targetmsg.cur_pos, out x, out y);
            ProtoHelper.DecodePosition2D(targetmsg.target_pos, out tx, out ty);
            Vector2 curPos = new Vector2(x, y);
            Vector2 targetPos = new Vector2(tx, ty);

            MovementStateInfo msi = npc.GetMovementStateInfo();
            float movedir = Geometry.GetYRadian(msi.GetPosition2D(), targetPos);
            float velocity = ProtoHelper.DecodeFloat(targetmsg.velocity);
            msi.SetMoveDir(movedir);
            msi.SetFaceDir(movedir);
            msi.IsMoving = true;
            npc.GetActualProperty().SetMoveSpeed(Operate_Type.OT_Absolute, velocity);
            msi.TargetPosition = new Vector3(targetPos.X, 0, targetPos.Y);
            
            msi.SetPosition2D(curPos);

            EntityViewModel viewModel = EntityViewModelManager.Instance.GetEntityViewById(targetmsg.npc_id);
            if (null != viewModel) {
                viewModel.MoveTo(targetPos.X, 0, targetPos.Y);
            }
        } else {
            float x, y;
            ProtoHelper.DecodePosition2D(targetmsg.cur_pos, out x, out y);
            Vector2 curPos = new Vector2(x, y);
            MovementStateInfo msi = npc.GetMovementStateInfo();
            msi.SetPosition2D(curPos);
            /*
            msi.IsMoving = false;
            EntityViewModel viewModel = EntityViewModelManager.Instance.GetEntityViewById(targetmsg.npc_id);
            if (null != viewModel) {
                viewModel.StopMove();
            }
            */
        }
    }
}
internal class Msg_RC_NpcFace_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_NpcFace face_msg = msg as Msg_RC_NpcFace;
        if (null == face_msg) {
            return;
        }
        EntityInfo npc = ClientModule.Instance.GetEntityById(face_msg.npc_id);
        if (npc == null)
            return;
        if (npc.IsHaveStateFlag(CharacterState_Type.CST_Sleep)) {
            return;
        }
        float dir = ProtoHelper.DecodeFloat(face_msg.face_direction);
        npc.GetMovementStateInfo().SetFaceDir(dir);

        UnityEngine.GameObject actor = EntityController.Instance.GetGameObject(npc.GetId());
        actor.transform.localRotation = UnityEngine.Quaternion.Euler(0, Utility.RadianToDegree(dir), 0);
        //LogSystem.Info("NpcFace, npc:{0} face:{1}", other.GetId(), dir);
    }
}
internal class Msg_RC_NpcSkill_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_NpcSkill targetmsg = msg as Msg_RC_NpcSkill;
        if (null == targetmsg) {
            return;
        }
        EntityInfo npc = ClientModule.Instance.GetEntityById(targetmsg.npc_id);
        if (null == npc) {
            return;
        }
        float x = 0.0f;
        float z = 0.0f;
        ProtoHelper.DecodePosition2D(targetmsg.stand_pos, out x, out z);
        float faceDir = ProtoHelper.DecodeFloat(targetmsg.face_direction);
        int skillId = targetmsg.skill_id;
        LogSystem.Info("Receive Msg_RC_NpcSkill, EntityId={0}, SkillId={1}", targetmsg.npc_id, skillId);

        MovementStateInfo msi = npc.GetMovementStateInfo();
        msi.SetPosition2D(x, z);
        msi.SetFaceDir(faceDir);

        UnityEngine.GameObject actor = EntityController.Instance.GetGameObject(npc.GetId());
        GameFramework.Skill.Trigers.TriggerUtil.MoveObjTo(actor, new UnityEngine.Vector3(x, 0, z));
        actor.transform.localRotation = UnityEngine.Quaternion.Euler(0, Utility.RadianToDegree(faceDir), 0);
        SkillInfo skillInfo = npc.GetSkillStateInfo().GetSkillInfoById(skillId);
        if (null != skillInfo) {
            if (GfxSkillSystem.Instance.StartSkill(npc.GetId(), skillInfo.ConfigData, 0)) {
                Utility.EventSystem.Publish("ui_skill_cooldown", "ui", npc.GetId(), skillId, skillInfo.ConfigData.cooldown);
            }
        }
    }
}
internal class Msg_RC_NpcStopSkill_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_NpcStopSkill targetmsg = msg as Msg_RC_NpcStopSkill;
        if (null == targetmsg) {
            return;
        }
        EntityInfo npc = ClientModule.Instance.GetEntityById(targetmsg.npc_id);
        if (null == npc) {
            return;
        }

        GfxSkillSystem.Instance.StopAllSkill(npc.GetId(), true);
    }
}
internal class Msg_RC_AddImpact_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_AddImpact message = msg as Msg_RC_AddImpact;
        if (null == message) return;
        int skillId = message.skill_id;
        int impactId = message.impact_id;
        EntityInfo target = ClientModule.Instance.SceneContext.GetEntityById(message.target_id);
        if (null == target) {
            LogSystem.Info("Receive Msg_RC_AddImpact, message.target_id={0} is not available", message.target_id);
            return;
        } else {
            LogSystem.Info("Receive Msg_RC_AddImpact, TargetId={0}, ImpactId={1}, SenderId={2}, SkillId={3}",
              message.target_id, impactId, message.sender_id, skillId);
        }
        Vector3 senderPos = Vector3.Zero;
        EntityInfo sender = ClientModule.Instance.GetEntityById(message.sender_id);
        senderPos = sender.GetMovementStateInfo().GetPosition3D();

        ImpactInfo impactInfo = new ImpactInfo(impactId);
        impactInfo.StartTime = TimeUtility.GetLocalMilliseconds();
        impactInfo.SkillId = skillId;
        impactInfo.DurationTime = message.duration;
        impactInfo.SenderPosition = senderPos;
        impactInfo.ImpactSenderId = message.sender_id;
        if (null != impactInfo.ConfigData) {
            target.GetSkillStateInfo().AddImpact(impactInfo);
            int seq = impactInfo.Seq;
            if (GfxSkillSystem.Instance.StartSkill(target.GetId(), impactInfo.ConfigData, seq)) {
            }
        }                
    }
}
internal class Msg_RC_RemoveImpact_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_RemoveImpact _msg = msg as Msg_RC_RemoveImpact;
        if (null == _msg)
            return;
        int impactId = _msg.impact_id;
        EntityInfo obj = ClientModule.Instance.GetEntityById(_msg.obj_id);
        if (null != obj) {
            ImpactInfo impactInfo = obj.GetSkillStateInfo().FindImpactInfoById(impactId);
            if (null != impactInfo) {
                GfxSkillSystem.Instance.StopSkill(obj.GetId(), impactId, impactInfo.Seq, false);
            }
        }
    }
}

internal class Msg_RC_AddSkill_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_AddSkill _msg = msg as Msg_RC_AddSkill;
        if (null == _msg)
            return;
        EntityInfo obj = ClientModule.Instance.GetEntityById(_msg.obj_id);
        if (null != obj) {
            if (obj.GetSkillStateInfo().GetSkillInfoById(_msg.skill_id) == null) {
                obj.GetSkillStateInfo().AddSkill(new SkillInfo(_msg.skill_id));
            }
        }
    }
}
internal class Msg_RC_RemoveSkill_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_RemoveSkill _msg = msg as Msg_RC_RemoveSkill;
        if (null == _msg)
            return;
        EntityInfo obj = ClientModule.Instance.GetEntityById(_msg.obj_id);
        if (null != obj) {
            obj.GetSkillStateInfo().RemoveSkill(_msg.skill_id);
        }
    }
}

internal class Msg_RC_AdjustPosition_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_AdjustPosition _msg = msg as Msg_RC_AdjustPosition;
        if (null == _msg)
            return;
        EntityInfo npc = ClientModule.Instance.GetEntityById(_msg.role_id);
        if (null != npc) {
            float x = ProtoHelper.DecodeFloat(_msg.x);
            float z = ProtoHelper.DecodeFloat(_msg.z);
            float faceDir = ProtoHelper.DecodeFloat(_msg.face_dir);

            MovementStateInfo msi = npc.GetMovementStateInfo();
            msi.SetPosition2D(x, z);
            msi.SetFaceDir(faceDir);

            UnityEngine.GameObject actor = EntityController.Instance.GetGameObject(npc.GetId());
            GameFramework.Skill.Trigers.TriggerUtil.MoveObjTo(actor, new UnityEngine.Vector3(x, 0, z));
            actor.transform.localRotation = UnityEngine.Quaternion.Euler(0, Utility.RadianToDegree(faceDir), 0);
        }
    }
}
internal class Msg_RC_SyncProperty_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_SyncProperty targetmsg = msg as Msg_RC_SyncProperty;
        if (null == targetmsg) {
            return;
        }

        EntityInfo cb = ClientModule.Instance.GetEntityById(targetmsg.role_id);
        if (null == cb) {
            return;
        }
        LogSystem.Info("---set {2} hp to {0}/{1}", targetmsg.hp, cb.GetActualProperty().HpMax, cb.GetId());
        cb.SetHp(Operate_Type.OT_Absolute, targetmsg.hp);
        cb.SetEnergy(Operate_Type.OT_Absolute, targetmsg.np);
        cb.SetShield(Operate_Type.OT_Absolute, targetmsg.shield);
        cb.StateFlag = targetmsg.state;
    }
}
internal class Msg_RC_ImpactDamage_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_ImpactDamage damage_msg = msg as Msg_RC_ImpactDamage;
        if (null != damage_msg) {
            EntityInfo entity = ClientModule.Instance.SceneContext.GetEntityById(damage_msg.role_id);
            if (null != entity) {
                int hpDamage = entity.Hp - damage_msg.hp;
                int energyDamage = entity.Energy - damage_msg.energy;
                entity.SetHp(Operate_Type.OT_Absolute, damage_msg.hp);
                //LogSystem.Info("---set {2} hp to {0}/{1}", entity.Hp, entity.GetActualProperty().HpMax, entity.GetId());
                bool isKiller = false;
                bool isOrdinary = false;
                bool isCritical = false;
                if ((damage_msg.damage_status & (int)Msg_RC_ImpactDamage.Flag.IS_KILLER) != 0) {
                    isKiller = true;
                }
                if ((damage_msg.damage_status & (int)Msg_RC_ImpactDamage.Flag.IS_CRITICAL) != 0) {
                    isCritical = true;
                }
                if ((damage_msg.damage_status & (int)Msg_RC_ImpactDamage.Flag.IS_ORDINARY) != 0) {
                    isOrdinary = true;
                }
                entity.SetAttackerInfo(damage_msg.attacker_id, isKiller, isOrdinary, isCritical, hpDamage, energyDamage);
            }
        }
    }
}

internal class Msg_RC_ChangeScene_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_ChangeScene _msg = msg as Msg_RC_ChangeScene;
        if (null == _msg)
            return;
        int targetSceneId = _msg.target_scene_id;
        TableConfig.Level cfg = TableConfig.LevelProvider.Instance.GetLevel(targetSceneId);
        if (null != cfg && cfg.type == (int)SceneTypeEnum.Room) {
            NetworkSystem.Instance.QuitBattle(false);
        }
        ClientModule.Instance.ChangeScene(targetSceneId);
    }
}
internal class Msg_RC_SyncNpcOwnerId_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_SyncNpcOwnerId target_msg = msg as Msg_RC_SyncNpcOwnerId;
        if (null == target_msg) {
            return;
        }
        EntityInfo obj = ClientModule.Instance.GetEntityById(target_msg.npc_id);
        if (null != obj) {
            EntityInfo npc = obj as EntityInfo;
            if (null != npc)
                npc.OwnerId = target_msg.owner_id;
        }
    }
}
internal class Msg_RC_CampChanged_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_CampChanged _msg = msg as Msg_RC_CampChanged;
        if (null == _msg)
            return;
        if (_msg.obj_id <= 0) {
            ClientModule.Instance.CampId = _msg.camp_id;
        } else {
            EntityInfo obj = ClientModule.Instance.GetEntityById(_msg.obj_id);
            if (null != obj) {
                obj.SetCampId(_msg.camp_id);
                Utility.EventSystem.Publish("ui_actor_color", "ui", _msg.obj_id, CharacterRelation.RELATION_FRIEND == EntityInfo.GetRelation(ClientModule.Instance.CampId, _msg.camp_id));
            }
        }
    }
}

internal class Msg_RC_DebugSpaceInfo_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_DebugSpaceInfo targetmsg = msg as Msg_RC_DebugSpaceInfo;
        if (null == targetmsg)
            return;
    }
}

internal class Msg_CRC_StoryMessage_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_CRC_StoryMessage _msg = msg as Msg_CRC_StoryMessage;
        if (null == _msg)
            return;
        try {
            string msgId = _msg.m_MsgId;
            ArrayList args = new ArrayList();
            for (int i = 0; i < _msg.m_Args.Count; i++) {
                switch (_msg.m_Args[i].val_type) {
                    case ArgType.NULL://null
                        args.Add(null);
                        break;
                    case ArgType.INT://int
                        args.Add(int.Parse(_msg.m_Args[i].str_val));
                        break;
                    case ArgType.FLOAT://float
                        args.Add(float.Parse(_msg.m_Args[i].str_val));
                        break;
                    default://string
                        args.Add(_msg.m_Args[i].str_val);
                        break;
                }
            }
            object[] objArgs = args.ToArray();
            GfxStorySystem.Instance.SendMessage(msgId, objArgs);
        } catch (Exception ex) {
            LogSystem.Error("Msg_CRC_StoryMessage throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
internal class Msg_RC_PublishEvent_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_PublishEvent _msg = msg as Msg_RC_PublishEvent;
        if (null == _msg)
            return;
        try {
            bool isLogic = _msg.is_logic_event;
            string name = _msg.ev_name;
            string group = _msg.group;
            ArrayList args = new ArrayList();
            for (int i = 0; i < _msg.args.Count; i++) {
                switch (_msg.args[i].val_type) {
                    case ArgType.NULL://null
                        args.Add(null);
                        break;
                    case ArgType.INT://int
                        args.Add(int.Parse(_msg.args[i].str_val));
                        break;
                    case ArgType.FLOAT://float
                        args.Add(float.Parse(_msg.args[i].str_val));
                        break;
                    default://string
                        args.Add(_msg.args[i].str_val);
                        break;
                }
            }
            object[] objArgs = args.ToArray();
            Utility.EventSystem.Publish(name, group, objArgs);
        } catch (Exception ex) {
            LogSystem.Error("Msg_RC_PublishEvent_Handler throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}
internal class Msg_RC_SendGfxMessage_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_SendGfxMessage _msg = msg as Msg_RC_SendGfxMessage;
        if (null == _msg)
            return;
        try {
            bool isWithTag = _msg.is_with_tag;
            string name = _msg.name;
            string message = _msg.msg;
            ArrayList args = new ArrayList();
            for (int i = 0; i < _msg.args.Count; i++) {
                switch (_msg.args[i].val_type) {
                    case ArgType.NULL://null
                        args.Add(null);
                        break;
                    case ArgType.INT://int
                        args.Add(int.Parse(_msg.args[i].str_val));
                        break;
                    case ArgType.FLOAT://float
                        args.Add(float.Parse(_msg.args[i].str_val));
                        break;
                    default://string
                        args.Add(_msg.args[i].str_val);
                        break;
                }
            }
            object[] objArgs = args.ToArray();
            if (isWithTag) {
                if (objArgs.Length == 0)
                    Utility.SendMessageWithTag(name, message, null);
                else if (objArgs.Length == 1)
                    Utility.SendMessageWithTag(name, message, objArgs[0]);
                else
                    Utility.SendMessageWithTag(name, message, objArgs);
            } else {
                if (objArgs.Length == 0)
                    Utility.SendMessage(name, message, null);
                else if (objArgs.Length == 1)
                    Utility.SendMessage(name, message, objArgs[0]);
                else
                    Utility.SendMessage(name, message, objArgs);
            }
        } catch (Exception ex) {
            LogSystem.Error("Msg_RC_SendGfxMessage throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
}

internal class Msg_RC_HighlightPrompt_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_HighlightPrompt _msg = msg as Msg_RC_HighlightPrompt;
        if (null == _msg)
            return;
        ClientModule.Instance.HighlightPrompt(_msg.dict_id, _msg.argument.ToArray());
    }
}
internal class Msg_RC_ShowDlg_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_ShowDlg _msg = msg as Msg_RC_ShowDlg;
        if (null == _msg)
            return;
        GfxStorySystem.Instance.SendMessage("show_dlg", _msg.dialog_id);
    }
}
internal class Msg_RC_LockFrame_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_LockFrame _msg = msg as Msg_RC_LockFrame;
        if (null == _msg)
            return;
        UnityEngine.Time.timeScale = _msg.scale;
    }
}
internal class Msg_RC_PlayAnimation_Handler
{
    internal static void Execute(object msg, NetConnection conn)
    {
        Msg_RC_PlayAnimation _msg = msg as Msg_RC_PlayAnimation;
        if (null == _msg)
            return;
        EntityViewModel view = EntityViewModelManager.Instance.GetEntityViewById(_msg.obj_id);
        if (null != view) {
            view.PlayAnimation(_msg.anim_name);
        }
    }
}
