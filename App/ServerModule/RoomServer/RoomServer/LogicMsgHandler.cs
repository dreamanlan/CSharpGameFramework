using System;
using System.Collections;
using System.Collections.Generic;
using GameFrameworkMessage;
using ScriptRuntime;

namespace GameFramework
{
    public class DefaultMsgHandler
    {
        public static void Execute(object msg, User user)
        {
            LogSys.Log(ServerLogType.DEBUG, "Unhandled msg {0} user {1}({2},{3},{4})!!!", msg.GetType(), user.RoleId, user.GetKey(), user.Guid, user.Name);
        }
    }

    public class EnterHandler
    {
        public static void Execute(object msg, User user)
        {
            Msg_CR_Enter enter_msg = msg as Msg_CR_Enter;
            if (enter_msg == null) {
                return;
            }
            LogSys.Log(ServerLogType.DEBUG, "user {0}({1},{2},{3}) enter.", user.RoleId, user.GetKey(), user.Guid, user.Name);
            user.UserControlState = (int)UserControlState.User;
            user.IsEntered = true;

            RoomUserManager roomUserMgr = user.OwnRoomUserManager;
            if (null != roomUserMgr) {
                Scene scene = roomUserMgr.ActiveScene;
                if (null != scene) {
                    EntityInfo userInfo = user.Info;
                    if (null != userInfo) {
                        if (scene.SceneState == SceneState.Running) {
                            scene.SyncForNewUser(user);
                            var args = scene.StorySystem.NewBoxedValueList();
                            args.Add(userInfo.GetId());
                            args.Add(userInfo.GetUnitId());
                            args.Add(userInfo.GetCampId());
                            args.Add(userInfo.GetMovementStateInfo().PositionX);
                            args.Add(userInfo.GetMovementStateInfo().PositionZ);
                            scene.StorySystem.SendMessage("user_enter_scene", args);
                        }
                    }
                }
            }
        }
    }

    public class Msg_CR_QuitHandler
    {
        public static void Execute(object msg, User user)
        {
            Msg_CR_Quit quitClient = msg as Msg_CR_Quit;
            if (quitClient == null)
                return;
            if (null != user.OwnRoomUserManager) {
                if (quitClient.is_force) {
                    user.OwnRoomUserManager.DeleteUser(user);
                } else {
                    user.OwnRoomUserManager.DropUser(user);
                }
            }
        }
    }

    public class Msg_CR_UserMoveToPosHandler
    {
        public static void Execute(object msg, User user)
        {
            Msg_CR_UserMoveToPos move_msg = msg as Msg_CR_UserMoveToPos;
            if (move_msg == null)
                return;
            EntityInfo charactor = user.Info;
            if (charactor == null) {
                LogSys.Log(ServerLogType.DEBUG, "charactor {0}({1},{2},{3}) not exist", user.RoleId, user.GetKey(), user.Guid, user.Name);
                return;
            }
            ///
            if (charactor.GetAIEnable()) {
                float tx = move_msg.target_pos.x;
                float tz = move_msg.target_pos.z;
                ScriptRuntime.Vector3 pos = new ScriptRuntime.Vector3(tx, 0, tz);

                MovementStateInfo msi = charactor.GetMovementStateInfo();
                msi.IsMoving = true;
                msi.TargetPosition = pos;
                float dir = Geometry.GetYRadian(msi.GetPosition3D(), pos);
                msi.SetFaceDir(dir);

                Msg_RC_NpcMove npcMoveBuilder = DataSyncUtility.BuildNpcMoveMessage(charactor);
                if (null != npcMoveBuilder) {
                    Scene scene = user.OwnRoomUserManager.ActiveScene;
                    if (null != scene) {
                        scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcMove, npcMoveBuilder);
                    }
                }
            }
        }
    }

    public class UseSkillHandler
    {
        public static void Execute(object msg, User user)
        {
            Msg_CR_Skill use_skill = msg as Msg_CR_Skill;
            if (null == use_skill)
                return;
            EntityInfo userObj = user.Info;
            if (null == userObj) {
                LogSys.Log(ServerLogType.DEBUG, "UseSkillHandler, charactor {0}({1},{2},{3}) not exist", user.RoleId, user.GetKey(), user.Guid, user.Name);
                return;
            }
            Scene scene = user.OwnRoomUserManager.ActiveScene;
            if (null != scene) {
                EntityInfo obj = scene.GetEntityById(use_skill.role_id);
                if (null != obj) {
                    AiStateInfo aiInfo = obj.GetAiStateInfo();
                    if (use_skill.target_id > 0) {
                        aiInfo.Target = use_skill.target_id;
                    } else if (use_skill.target_dir > 0) {
                        float dir = use_skill.target_dir;
                        obj.GetMovementStateInfo().SetFaceDir(dir);
                        aiInfo.Target = 0;
                    }
                    Msg_RC_NpcSkill retMsg = DataSyncUtility.BuildNpcSkillMessage(obj, use_skill.skill_id);
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcSkill, retMsg);
                }
            }
        }
    }

    public class Msg_CR_StopSkillHandler
    {
        public static void Execute(object msg, User user)
        {
            Msg_CR_StopSkill stopMsg = msg as Msg_CR_StopSkill;
            if (null == stopMsg) return;
            EntityInfo userInfo = user.Info;
            if (null == userInfo) return;
            Scene scene = user.OwnRoomUserManager.ActiveScene;
            if (null != scene) {
                Msg_RC_NpcStopSkill retMsg = DataSyncUtility.BuildNpcStopSkillMessage(userInfo);
                scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcStopSkill, retMsg);
            }
        }
    }

    public class SwitchDebugHandler
    {
        public static void Execute(object msg, User user)
        {
            Msg_CR_SwitchDebug switchDebug = msg as Msg_CR_SwitchDebug;
            if (switchDebug == null)
                return;
            user.IsDebug = switchDebug.is_debug;
        }
    }

    public class Msg_CR_DlgClosedHandler
    {
        public static void Execute(object msg, User user)
        {
            Msg_CR_DlgClosed dialog_msg = msg as Msg_CR_DlgClosed;
            if (dialog_msg == null)
                return;
            Scene scene = user.OwnRoomUserManager.ActiveScene;
            if (null != scene) {
                scene.StorySystem.SendMessage("dialog_over:" + dialog_msg.dialog_id, user.RoleId);
            }
        }
    }
    
    public class Msg_CR_GmCommandHandler
    {
        public static void Execute(object msg, User user)
        {
            Msg_CR_GmCommand cmdMsg = msg as Msg_CR_GmCommand;
            if (cmdMsg == null) {
                return;
            }
            if (!GlobalVariables.Instance.IsDebug)
                return;
            Scene scene = user.OwnRoomUserManager.ActiveScene;
            if (scene != null) {
                switch (cmdMsg.type) {
                    case 0:
                        //resetdsl
                        scene.GmStorySystem.Reset();
                        if (scene.GmStorySystem.GlobalVariables.ContainsKey("EntityInfo")) {
                            scene.GmStorySystem.GlobalVariables["EntityInfo"] = BoxedValue.FromObject(user.Info);
                        } else {
                            scene.GmStorySystem.GlobalVariables.Add("EntityInfo", BoxedValue.FromObject(user.Info));
                        }
                        StorySystem.StoryConfigManager.Instance.Clear();
                        scene.StorySystem.ClearStoryInstancePool();
                        scene.StorySystem.LoadSceneStories();
                        scene.StorySystem.StartStory("local_main");
                        scene.StorySystem.StartStory("story_main");
                        break;
                    case 1:
                        //script
                        if (null != cmdMsg.content) {
                            scene.GmStorySystem.Reset();
                            if (scene.GmStorySystem.GlobalVariables.ContainsKey("EntityInfo")) {
                                scene.GmStorySystem.GlobalVariables["EntityInfo"] = BoxedValue.FromObject(user.Info);
                            } else {
                                scene.GmStorySystem.GlobalVariables.Add("EntityInfo", BoxedValue.FromObject(user.Info));
                            }
                            scene.GmStorySystem.LoadStory(cmdMsg.content);
                            scene.GmStorySystem.StartStory("main");
                        }
                        break;
                    case 2:
                        //command
                        if (null != cmdMsg.content) {
                            string cmd = cmdMsg.content;
                            scene.GmStorySystem.Reset();
                            if (scene.GmStorySystem.GlobalVariables.ContainsKey("EntityInfo")) {
                                scene.GmStorySystem.GlobalVariables["EntityInfo"] = BoxedValue.FromObject(user.Info);
                            }
                            else {
                                scene.GmStorySystem.GlobalVariables.Add("EntityInfo", BoxedValue.FromObject(user.Info));
                            }
                            scene.GmStorySystem.LoadStoryText(System.Text.Encoding.UTF8.GetBytes("script(main){onmessage(\"start\"){" + cmd + "}}"));
                            scene.GmStorySystem.StartStory("main");
                        }
                        break;
                }
            }
        }
    }

    public class MsgStoryMessageHandler
    {
        public static void Execute(object msg, User user)
        {
            Msg_CRC_StoryMessage target_msg = msg as Msg_CRC_StoryMessage;
            if (target_msg == null) {
                return;
            }
            Scene scene = user.OwnRoomUserManager.ActiveScene;
            if (scene != null) {
                try {
                    //客户端发来的消息都加上前缀client，防止直接调用服务器端逻辑（服务器消息不能用client前缀！）
                    string msgId = string.Format("client:{0}", target_msg.m_MsgId);
                    ArrayList args = new ArrayList();
                    args.Add(user.RoleId);
                    for (int i = 0; i < target_msg.m_Args.Count; i++) {
                        switch (target_msg.m_Args[i].val_type) {
                            case ArgType.NULL://null
                                args.Add(null);
                                break;
                            case ArgType.INT://int
                                args.Add(int.Parse(target_msg.m_Args[i].str_val));
                                break;
                            case ArgType.FLOAT://float
                                args.Add(float.Parse(target_msg.m_Args[i].str_val));
                                break;
                            default://string
                                args.Add(target_msg.m_Args[i].str_val);
                                break;
                        }
                    }
                    object[] objArgs = args.ToArray();
                    scene.StorySystem.SendMessage(msgId, objArgs);
                } catch (Exception ex) {
                    LogSys.Log(ServerLogType.ERROR, "Msg_CRC_StoryMessage throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
        }
    }
}
