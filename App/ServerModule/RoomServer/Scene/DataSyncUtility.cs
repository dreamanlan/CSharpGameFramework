using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFrameworkMessage;

namespace GameFramework
{
    internal sealed class DataSyncUtility
    {
        internal static void SyncBuffListToObserver(EntityInfo obj, Observer observer)
        {
            List<ImpactInfo> impacts = obj.GetSkillStateInfo().GetAllImpact();
            foreach (ImpactInfo info in impacts) {
            }
        }
        internal static void SyncBuffListToObservers(EntityInfo obj, Scene scene)
        {
            List<ImpactInfo> impacts = obj.GetSkillStateInfo().GetAllImpact();
            foreach (ImpactInfo info in impacts) {
            }
        }
        internal static void SyncBuffListToUser(EntityInfo obj, User user)
        {
            List<ImpactInfo> impacts = obj.GetSkillStateInfo().GetAllImpact();
            foreach (ImpactInfo info in impacts) {
            }
        }
        internal static void SyncBuffListToCaredUsers(EntityInfo obj)
        {
            Scene scene = obj.SceneContext.CustomData as Scene;
            if (null != scene) {
                List<ImpactInfo> impacts = obj.GetSkillStateInfo().GetAllImpact();
                foreach (ImpactInfo info in impacts) {
                }
            }
        }

        internal static void SyncUserPropertyToCaredUsers(EntityInfo user)
        {
            Scene scene = user.SceneContext.CustomData as Scene;
            if (null != scene) {
                Msg_RC_SyncProperty bd = BuildSyncPropertyMessage(user);
                scene.NotifyAllUser(RoomMessageDefine.Msg_RC_SyncProperty, bd);
            }
        }

        internal static Msg_RC_CreateNpc BuildCreateNpcMessage(EntityInfo npc, int rate = -1)
        {
            Msg_RC_CreateNpc bder = new Msg_RC_CreateNpc();
            bder.npc_id = npc.GetId();
            bder.unit_id = npc.GetUnitId();
            ScriptRuntime.Vector3 pos = npc.GetMovementStateInfo().GetPosition3D();
            GameFrameworkMessage.Position pos_bd = new GameFrameworkMessage.Position();
            pos_bd.x = (float)pos.X;
            pos_bd.z = (float)pos.Z;
            bder.cur_pos = pos_bd;
            bder.face_direction = (float)npc.GetMovementStateInfo().GetFaceDir();
            bder.link_id = npc.GetLinkId();
            bder.camp_id = npc.GetCampId();
            if (npc.OwnerId > 0) {
                bder.owner_id = npc.OwnerId;
            }
            if (npc.GetAiStateInfo().LeaderID > 0) {
                bder.leader_id = npc.GetAiStateInfo().LeaderID;
            }
            User user = npc.CustomData as User;
            if (null != user) {
                bder.key = user.GetKey();
            }
            bder.level = npc.GetLevel();

            return bder;
        }

        internal static Msg_RC_SyncProperty BuildSyncPropertyMessage(EntityInfo obj)
        {
            Msg_RC_SyncProperty builder = new Msg_RC_SyncProperty();
            builder.role_id = obj.GetId();
            builder.hp = obj.Hp;
            builder.np = obj.Energy;
            builder.shield = obj.Shield;
            builder.state = obj.StateFlag;
            return builder;
        }

        internal static Msg_RC_SyncNpcOwnerId BuildSyncNpcOwnerIdMessage(EntityInfo npc)
        {
            Msg_RC_SyncNpcOwnerId builder = new Msg_RC_SyncNpcOwnerId();
            builder.npc_id = npc.GetId();
            builder.owner_id = npc.OwnerId;
            return builder;
        }

        internal static Msg_RC_CampChanged BuildCampChangedMessage(EntityInfo obj)
        {
            Msg_RC_CampChanged msg = new Msg_RC_CampChanged();
            msg.obj_id = obj.GetId();
            msg.camp_id = obj.GetCampId();
            return msg;
        }

        internal static Msg_RC_NpcMove BuildNpcMoveMessage(EntityInfo npc)
        {
            ScriptRuntime.Vector3 srcPos = npc.GetMovementStateInfo().GetPosition3D();
            Msg_RC_NpcMove npcMoveBuilder = new Msg_RC_NpcMove();
            if (npc.GetMovementStateInfo().IsMoving) {
                npcMoveBuilder.npc_id = npc.GetId();
                npcMoveBuilder.velocity = ProtoHelper.EncodeFloat(npc.GetActualProperty().MoveSpeed);
                ScriptRuntime.Vector3 targetPos = npc.GetMovementStateInfo().TargetPosition;
                npcMoveBuilder.target_pos = ProtoHelper.EncodePosition2D(targetPos.X, targetPos.Z);
                npcMoveBuilder.cur_pos = ProtoHelper.EncodePosition2D(srcPos.X, srcPos.Z);
            } else {
                npcMoveBuilder.npc_id = npc.GetId();
                npcMoveBuilder.cur_pos = ProtoHelper.EncodePosition2D(srcPos.X, srcPos.Z);
            }
            return npcMoveBuilder;
        }

        internal static Msg_RC_NpcFace BuildNpcFaceMessage(EntityInfo npc)
        {
            Msg_RC_NpcFace npcFaceBuilder = new Msg_RC_NpcFace();
            npcFaceBuilder.npc_id = npc.GetId();
            npcFaceBuilder.face_direction = ProtoHelper.EncodeFloat(npc.GetMovementStateInfo().GetFaceDir());
            return npcFaceBuilder;
        }
    }
}
