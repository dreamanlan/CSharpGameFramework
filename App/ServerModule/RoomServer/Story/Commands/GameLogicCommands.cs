using System;
using System.Collections;
using System.Collections.Generic;
using StorySystem;
using ScriptRuntime;
using GameFramework;
using GameFrameworkMessage;

namespace GameFramework.Story.Commands
{
    /// <summary>
    /// blackboardclear();
    /// </summary>
    public class BlackboardClearCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            BlackboardClearCommand cmd = new BlackboardClearCommand();
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
        
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                scene.SceneContext.BlackBoard.ClearVariables();
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
        }
    }
    /// <summary>
    /// blackboardset(name,value);
    /// </summary>
    public class BlackboardSetCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            BlackboardSetCommand cmd = new BlackboardSetCommand();
            cmd.m_AttrName = m_AttrName.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_AttrName.Evaluate(instance, iterator, args);
            m_Value.Evaluate(instance, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                string name = m_AttrName.Value;
                object value = m_Value.Value;
                scene.SceneContext.BlackBoard.SetVariable(name, value);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_AttrName.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<string> m_AttrName = new StoryValue<string>();
        private IStoryValue<object> m_Value = new StoryValue();
    }
    /// <summary>
    /// camerafollow(npc_unit_id1,npc_unit_id2,...)[touser(userid)];
    /// </summary>
    public class CameraFollowCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            CameraFollowCommand cmd = new CameraFollowCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            for (int i = 0; i < m_UnitIds.Count; i++) {
                cmd.m_UnitIds.Add(m_UnitIds[i].Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance, iterator, args);
            for (int i = 0; i < m_UnitIds.Count; i++) {
                m_UnitIds[i].Evaluate(instance, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                for (int i = 0; i < m_UnitIds.Count; i++) {
                    int unitId = m_UnitIds[i].Value;
                    EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
                    if (null != entity && (!entity.IsDead() || entity.IsBorning)) {
                        Msg_RC_SendGfxMessage msg = new GameFrameworkMessage.Msg_RC_SendGfxMessage();
                        msg.name = "GameRoot";
                        msg.msg = "CameraFollow";
                        msg.is_with_tag = false;
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.INT;
                        arg.str_val = entity.GetId().ToString();
                        msg.args.Add(arg);

                        if (m_HaveUserId) {
                            int userId = m_UserId.Value;
                            EntityInfo user = scene.GetEntityById(userId);
                            if (null != user) {
                                User us = user.CustomData as User;
                                if (null != us) {
                                    us.SendMessage(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                                }
                            }
                        } else {
                            scene.NotifyAllUser(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                        }
                        break;
                    }
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<int> val = new StoryValue<int>();
                val.InitFromDsl(callData.GetParam(i));
                m_UnitIds.Add(val);
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryValue<int> m_UserId = new StoryValue<int>();
        private List<IStoryValue<int>> m_UnitIds = new List<IStoryValue<int>>();
    }
    /// <summary>
    /// camerafollowrange(npc_unit_id_begin,npc_unit_id_end)[touser(userid)];
    /// </summary>
    public class CameraFollowRangeCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            CameraFollowRangeCommand cmd = new CameraFollowRangeCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            cmd.m_BeginUnitId = m_BeginUnitId.Clone();
            cmd.m_EndUnitId = m_EndUnitId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance, iterator, args);
            m_BeginUnitId.Evaluate(instance, iterator, args);
            m_EndUnitId.Evaluate(instance, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int beginUnitId = m_BeginUnitId.Value;
                int endUnitId = m_EndUnitId.Value;
                for (int unitId = beginUnitId; unitId <= endUnitId; ++unitId) {
                    EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
                    if (null != entity && (!entity.IsDead() || entity.IsBorning)) {
                        Msg_RC_SendGfxMessage msg = new GameFrameworkMessage.Msg_RC_SendGfxMessage();
                        msg.name = "GameRoot";
                        msg.msg = "CameraFollow";
                        msg.is_with_tag = false;
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.INT;
                        arg.str_val = entity.GetId().ToString();
                        msg.args.Add(arg);

                        if (m_HaveUserId) {
                            int userId = m_UserId.Value;
                            EntityInfo user = scene.GetEntityById(userId);
                            if (null != user) {
                                User us = user.CustomData as User;
                                if (null != us) {
                                    us.SendMessage(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                                }
                            }
                        } else {
                            scene.NotifyAllUser(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                        }
                        break;
                    }
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_BeginUnitId.InitFromDsl(callData.GetParam(0));
                m_EndUnitId.InitFromDsl(callData.GetParam(1));
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryValue<int> m_UserId = new StoryValue<int>();
        private IStoryValue<int> m_BeginUnitId = new StoryValue<int>();
        private IStoryValue<int> m_EndUnitId = new StoryValue<int>();
    }
    /// <summary>
    /// cameralookat(npc_unit_id)[touser(userid)];
    /// or
    /// cameralookat(vector3(x,y,z))[touser(userid)];
    /// </summary>
    public class CameraLookCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            CameraLookCommand cmd = new CameraLookCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            cmd.m_Arg = m_Arg.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance, iterator, args);
            m_Arg.Evaluate(instance, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                object obj = m_Arg.Value;
                if (obj is int) {
                    int unitId = (int)obj;
                    EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
                    if (null != entity) {
                        Vector3 pos = entity.GetMovementStateInfo().GetPosition3D();
                        Msg_RC_SendGfxMessage msg = new GameFrameworkMessage.Msg_RC_SendGfxMessage();
                        msg.name = "GameRoot";
                        msg.msg = "CameraLook";
                        msg.is_with_tag = false;
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.FLOAT;
                        arg.str_val = pos.X.ToString();
                        msg.args.Add(arg);
                        arg.val_type = ArgType.FLOAT;
                        arg.str_val = (pos.Y + entity.GetRadius()).ToString();
                        msg.args.Add(arg);
                        arg.val_type = ArgType.FLOAT;
                        arg.str_val = pos.Z.ToString();
                        msg.args.Add(arg);

                        if (m_HaveUserId) {
                            int userId = m_UserId.Value;
                            EntityInfo user = scene.GetEntityById(userId);
                            if (null != user) {
                                User us = user.CustomData as User;
                                if (null != us) {
                                    us.SendMessage(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                                }
                            }
                        } else {
                            scene.NotifyAllUser(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                        }
                    }
                } else {
                    Vector3 pos = (Vector3)obj;
                    Msg_RC_SendGfxMessage msg = new GameFrameworkMessage.Msg_RC_SendGfxMessage();
                    msg.name = "GameRoot";
                    msg.msg = "CameraLook";
                    msg.is_with_tag = false;
                    Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                    arg.val_type = ArgType.FLOAT;
                    arg.str_val = pos.X.ToString();
                    msg.args.Add(arg);
                    arg.val_type = ArgType.FLOAT;
                    arg.str_val = pos.Y.ToString();
                    msg.args.Add(arg);
                    arg.val_type = ArgType.FLOAT;
                    arg.str_val = pos.Z.ToString();
                    msg.args.Add(arg);

                    if (m_HaveUserId) {
                        int userId = m_UserId.Value;
                        EntityInfo user = scene.GetEntityById(userId);
                        if (null != user) {
                            User us = user.CustomData as User;
                            if (null != us) {
                                us.SendMessage(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                            }
                        }
                    } else {
                        scene.NotifyAllUser(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                    }
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Arg.InitFromDsl(callData.GetParam(0));
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryValue<int> m_UserId = new StoryValue<int>();
        private IStoryValue<object> m_Arg = new StoryValue();
    }
    /// <summary>
    /// camerafollowpath()[touser(userid)];
    /// </summary>
    public class CameraFollowPathCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            CameraFollowPathCommand cmd = new CameraFollowPathCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                Msg_RC_SendGfxMessage msg = new GameFrameworkMessage.Msg_RC_SendGfxMessage();
                msg.name = "GameRoot";
                msg.msg = "CameraFollowPath";
                msg.is_with_tag = false;

                if (m_HaveUserId) {
                    int userId = m_UserId.Value;
                    EntityInfo user = scene.GetEntityById(userId);
                    if (null != user) {
                        User us = user.CustomData as User;
                        if (null != us) {
                            us.SendMessage(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                        }
                    }
                } else {
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryValue<int> m_UserId = new StoryValue<int>();
    }
    /// <summary>
    /// lockframe(scale)[touser(userid)];
    /// </summary>
    public class LockFrameCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            LockFrameCommand cmd = new LockFrameCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            cmd.m_Scale = m_Scale.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance, iterator, args);
            m_Scale.Evaluate(instance, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                float scale = m_Scale.Value;
                Msg_RC_LockFrame msg = new Msg_RC_LockFrame();
                msg.scale = scale;

                if (m_HaveUserId) {
                    int userId = m_UserId.Value;
                    EntityInfo user = scene.GetEntityById(userId);
                    if (null != user) {
                        User us = user.CustomData as User;
                        if (null != us) {
                            us.SendMessage(RoomMessageDefine.Msg_RC_LockFrame, msg);
                        }
                    }
                } else {
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_LockFrame, msg);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Scale.InitFromDsl(callData.GetParam(0));
            }
        }


        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryValue<int> m_UserId = new StoryValue<int>();
        private IStoryValue<float> m_Scale = new StoryValue<float>();
    }
    /// <summary>
    /// setleaderid([objid,]leaderid);
    /// </summary>
    public class SetLeaderIdCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SetLeaderIdCommand cmd = new SetLeaderIdCommand();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_LeaderId = m_LeaderId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            if (m_ParamNum > 1) {
                m_ObjId.Evaluate(instance, iterator, args);
            }
            m_LeaderId.Evaluate(instance, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int leaderId = m_LeaderId.Value;
                if (m_ParamNum > 1) {
                    int objId = m_ObjId.Value;
                    EntityInfo npc = scene.GetEntityById(objId);
                    if (null != npc) {
                        npc.GetAiStateInfo().LeaderID = leaderId;
                    }
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            m_ParamNum = callData.GetParamNum();
            if (m_ParamNum > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_LeaderId.InitFromDsl(callData.GetParam(1));
            } else if (m_ParamNum > 0) {
                m_LeaderId.InitFromDsl(callData.GetParam(0));
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_LeaderId = new StoryValue<int>();
    }
    /// <summary>
    /// showdlg(storyDlgId)[touser(userid)];
    /// </summary>
    public class ShowDlgCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ShowDlgCommand cmd = new ShowDlgCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            cmd.m_StoryDlgId = m_StoryDlgId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance, iterator, args);
            m_StoryDlgId.Evaluate(instance, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                Msg_RC_ShowDlg msg = new Msg_RC_ShowDlg();
                msg.dialog_id = m_StoryDlgId.Value;
                if (m_HaveUserId) {
                    int userId = m_UserId.Value;
                    EntityInfo user = scene.GetEntityById(userId);
                    if (null != user) {
                        User us = user.CustomData as User;
                        if (null != us) {
                            us.SendMessage(RoomMessageDefine.Msg_RC_ShowDlg, msg);
                        }
                    }
                } else {
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_ShowDlg, msg);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_StoryDlgId.InitFromDsl(callData.GetParam(0));
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryValue<int> m_UserId = new StoryValue<int>();
        private IStoryValue<int> m_StoryDlgId = new StoryValue<int>();
    }
    /// <summary>
    /// areadetect(pos,radius,type,callback)[set(var,val)];
    /// </summary>
    public class AreaDetectCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            AreaDetectCommand cmd = new AreaDetectCommand();
            cmd.m_Pos = m_Pos.Clone();
            cmd.m_Radius = m_Radius.Clone();
            cmd.m_Type = m_Type.Clone();
            cmd.m_EventName = m_EventName.Clone();
            cmd.m_SetVar = m_SetVar.Clone();
            cmd.m_SetVal = m_SetVal.Clone();
            cmd.m_ElseSetVal = m_ElseSetVal.Clone();
            cmd.m_HaveSet = m_HaveSet;
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Pos.Evaluate(instance, iterator, args);
            m_Radius.Evaluate(instance, iterator, args);
            m_Type.Evaluate(instance, iterator, args);
            m_EventName.Evaluate(instance, iterator, args);
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance, iterator, args);
                m_SetVal.Evaluate(instance, iterator, args);
                m_ElseSetVal.Evaluate(instance, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                bool triggered = false;
                Vector3 pos = m_Pos.Value;
                float radius = m_Radius.Value;
                string type = m_Type.Value;
                string eventName = m_EventName.Value;
                if (type == "user") {
                    scene.KdTree.QueryWithFunc(pos, radius, (float distSqr, KdTreeObject kdObj) => {
                        if (kdObj.Object.EntityType != (int)EntityTypeEnum.Hero) {
                            scene.StorySystem.SendMessage(eventName, pos, radius, type);
                            triggered = true;
                            return false;
                        }
                        return true;
                    });
                } else if (type == "npc") {
                    scene.KdTree.QueryWithFunc(pos, radius, (float distSqr, KdTreeObject kdObj) => {
                        if (kdObj.Object.EntityType != (int)EntityTypeEnum.Hero) {
                            scene.StorySystem.SendMessage(eventName, pos, radius, type);
                            triggered = true;
                            return false;
                        }
                        return true;
                    });
                }
                string varName = m_SetVar.Value;
                object varVal = m_SetVal.Value;
                object elseVal = m_ElseSetVal.Value;
                if (triggered) {
                    instance.SetVariable(varName, varVal);
                } else {
                    instance.SetVariable(varName, elseVal);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 3) {
                m_Pos.InitFromDsl(callData.GetParam(0));
                m_Radius.InitFromDsl(callData.GetParam(1));
                m_Type.InitFromDsl(callData.GetParam(2));
                m_EventName.InitFromDsl(callData.GetParam(3));
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 2) {
                Dsl.CallData first = statementData.Functions[0].Call;
                Dsl.CallData second = statementData.Functions[1].Call;
                if (null != first && null != second) {
                    m_HaveSet = true;

                    Load(first);
                    LoadSet(second);
                }
            }
        }

        private void LoadSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_SetVar.InitFromDsl(callData.GetParam(0));
                m_SetVal.InitFromDsl(callData.GetParam(1));
                m_ElseSetVal.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<Vector3> m_Pos = new StoryValue<Vector3>();
        private IStoryValue<float> m_Radius = new StoryValue<float>();
        private IStoryValue<string> m_Type = new StoryValue<string>();
        private IStoryValue<string> m_EventName = new StoryValue<string>();
        private IStoryValue<string> m_SetVar = new StoryValue<string>();
        private IStoryValue<object> m_SetVal = new StoryValue();
        private IStoryValue<object> m_ElseSetVal = new StoryValue();
        private bool m_HaveSet = false;
    }
    /// <summary>
    /// setstorystate(0_or_1);
    /// </summary>
    public class SetStoryStateCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SetStoryStateCommand cmd = new SetStoryStateCommand();
            cmd.m_StoryState = m_StoryState.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_StoryState.Evaluate(instance, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int state = m_StoryState.Value;
                scene.IsStoryState = state != 0;
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_StoryState.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<int> m_StoryState = new StoryValue<int>();
    }
}
