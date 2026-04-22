using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptRuntime;
using ScriptableFramework;
using ScriptableFrameworkMessage;

namespace ScriptableFramework.Story.Commands
{
    /// <summary>
    /// blackboardclear();
    /// </summary>
    public class BlackboardClearCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                scene.SceneContext.BlackBoard.ClearVariables();
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// blackboardset(name,value);
    /// </summary>
    public class BlackboardSetCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                string name = operands[0].ToString();
                object value = operands[1].GetObject();
                scene.SceneContext.BlackBoard.SetVariable(name, value);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// camerafollow(npc_unit_id1,npc_unit_id2,...)[touser(userid)];
    /// </summary>
    public class CameraFollowCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                for (int i = 0; i < m_UnitIds.Count; i++) {
                    int unitId = m_UnitIds[i].Calc().GetInt();
                    EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
                    if (null != entity && (!entity.IsDead() || entity.IsBorning)) {
                        Msg_RC_SendGfxMessage msg = new ScriptableFrameworkMessage.Msg_RC_SendGfxMessage();
                        msg.name = "GameRoot";
                        msg.msg = "CameraFollow";
                        msg.is_with_tag = false;
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.INT;
                        arg.str_val = entity.GetId().ToString();
                        msg.args.Add(arg);

                        if (m_HaveUserId) {
                            int userId = m_UserId.Calc().GetInt();
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
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                m_UnitIds.Add(Calculator.Load(callData.GetParam(i)));
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    LoadUserId(second);
                }
            }
            return true;
        }

        private void LoadUserId(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId = Calculator.Load(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IExpression m_UserId;
        private List<IExpression> m_UnitIds = new List<IExpression>();
    }
    /// <summary>
    /// camerafollowrange(npc_unit_id_begin,npc_unit_id_end)[touser(userid)];
    /// </summary>
    public class CameraFollowRangeCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int beginUnitId = m_BeginUnitId.Calc().GetInt();
                int endUnitId = m_EndUnitId.Calc().GetInt();
                for (int unitId = beginUnitId; unitId <= endUnitId; ++unitId) {
                    EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
                    if (null != entity && (!entity.IsDead() || entity.IsBorning)) {
                        Msg_RC_SendGfxMessage msg = new ScriptableFrameworkMessage.Msg_RC_SendGfxMessage();
                        msg.name = "GameRoot";
                        msg.msg = "CameraFollow";
                        msg.is_with_tag = false;
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.INT;
                        arg.str_val = entity.GetId().ToString();
                        msg.args.Add(arg);

                        if (m_HaveUserId) {
                            int userId = m_UserId.Calc().GetInt();
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
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_BeginUnitId = Calculator.Load(callData.GetParam(0));
                m_EndUnitId = Calculator.Load(callData.GetParam(1));
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    LoadUserId(second);
                }
            }
            return true;
        }

        private void LoadUserId(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId = Calculator.Load(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IExpression m_UserId;
        private IExpression m_BeginUnitId;
        private IExpression m_EndUnitId;
    }
    /// <summary>
    /// cameralookat(npc_unit_id)[touser(userid)];
    /// or
    /// cameralookat(vector3(x,y,z))[touser(userid)];
    /// </summary>
    public class CameraLookCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                var obj = m_Arg.Calc();
                if (obj.IsInteger) {
                    int unitId = obj.GetInt();
                    EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
                    if (null != entity) {
                        Vector3 pos = entity.GetMovementStateInfo().GetPosition3D();
                        Msg_RC_SendGfxMessage msg = new ScriptableFrameworkMessage.Msg_RC_SendGfxMessage();
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
                            int userId = m_UserId.Calc().GetInt();
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
                    Vector3 pos = obj.As<Vector3Obj>();
                    Msg_RC_SendGfxMessage msg = new ScriptableFrameworkMessage.Msg_RC_SendGfxMessage();
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
                        int userId = m_UserId.Calc().GetInt();
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
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Arg = Calculator.Load(callData.GetParam(0));
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    LoadUserId(second);
                }
            }
            return true;
        }

        private void LoadUserId(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId = Calculator.Load(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IExpression m_UserId;
        private IExpression m_Arg;
    }
    /// <summary>
    /// camerafollowpath()[touser(userid)];
    /// </summary>
    public class CameraFollowPathCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                Msg_RC_SendGfxMessage msg = new ScriptableFrameworkMessage.Msg_RC_SendGfxMessage();
                msg.name = "GameRoot";
                msg.msg = "CameraFollowPath";
                msg.is_with_tag = false;

                if (m_HaveUserId) {
                    int userId = m_UserId.Calc().GetInt();
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
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    LoadUserId(second);
                }
            }
            return true;
        }

        private void LoadUserId(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId = Calculator.Load(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IExpression m_UserId;
    }
    /// <summary>
    /// lockframe(scale)[touser(userid)];
    /// </summary>
    public class LockFrameCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                float scale = m_Scale.Calc().GetFloat();
                Msg_RC_LockFrame msg = new Msg_RC_LockFrame();
                msg.scale = scale;

                if (m_HaveUserId) {
                    int userId = m_UserId.Calc().GetInt();
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
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Scale = Calculator.Load(callData.GetParam(0));
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    LoadUserId(second);
                }
            }
            return true;
        }

        private void LoadUserId(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId = Calculator.Load(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IExpression m_UserId;
        private IExpression m_Scale;
    }
    /// <summary>
    /// setleaderid([objid,]leaderid);
    /// </summary>
    public class SetLeaderIdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count > 1) {
                    int objId = operands[0].GetInt();
                    int leaderId = operands[1].GetInt();
                    EntityInfo npc = scene.GetEntityById(objId);
                    if (null != npc) {
                        npc.GetAiStateInfo().LeaderId = leaderId;
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// showdlg(storyDlgId)[touser(userid)];
    /// </summary>
    public class ShowDlgCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                Msg_RC_ShowDlg msg = new Msg_RC_ShowDlg();
                msg.dialog_id = m_StoryDlgId.Calc().GetInt();
                if (m_HaveUserId) {
                    int userId = m_UserId.Calc().GetInt();
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
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_StoryDlgId = Calculator.Load(callData.GetParam(0));
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    LoadUserId(second);
                }
            }
            return true;
        }

        private void LoadUserId(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserId = Calculator.Load(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IExpression m_UserId;
        private IExpression m_StoryDlgId;
    }
    /// <summary>
    /// areadetect(pos,radius,type,callback)[set(var,val)];
    /// </summary>
    public class AreaDetectCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                bool triggered = false;
                Vector3Obj posObj = m_Pos.Calc();
                Vector3 pos = posObj.Value;
                float radius = m_Radius.Calc().GetFloat();
                string type = m_Type.Calc().ToString();
                string eventName = m_EventName.Calc().ToString();
                if (type == "user") {
                    scene.KdTree.QueryWithFunc(pos, radius, (float distSqr, KdTreeObject kdObj) => {
                        if (kdObj.Object.EntityType != (int)EntityTypeEnum.Hero) {
                            scene.StorySystem.SendMessage(eventName, (Vector3Obj)pos, radius, type);
                            triggered = true;
                            return false;
                        }
                        return true;
                    });
                } else if (type == "npc") {
                    scene.KdTree.QueryWithFunc(pos, radius, (float distSqr, KdTreeObject kdObj) => {
                        if (kdObj.Object.EntityType != (int)EntityTypeEnum.Hero) {
                            scene.StorySystem.SendMessage(eventName, (Vector3Obj)pos, radius, type);
                            triggered = true;
                            return false;
                        }
                        return true;
                    });
                }
                if (m_HaveSet) {
                    string varName = m_SetVar.Calc().ToString();
                    var varVal = m_SetVal.Calc();
                    var elseVal = m_ElseSetVal.Calc();
                    if (triggered) {
                        instance.SetVariable(varName, varVal);
                    } else {
                        instance.SetVariable(varName, elseVal);
                    }
                }
            }
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 3) {
                m_Pos = Calculator.Load(callData.GetParam(0));
                m_Radius = Calculator.Load(callData.GetParam(1));
                m_Type = Calculator.Load(callData.GetParam(2));
                m_EventName = Calculator.Load(callData.GetParam(3));
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    m_HaveSet = true;
                    Load(first);
                    LoadSet(second);
                }
            }
            return true;
        }

        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_SetVar = Calculator.Load(callData.GetParam(0));
                m_SetVal = Calculator.Load(callData.GetParam(1));
                m_ElseSetVal = Calculator.Load(callData.GetParam(2));
            }
        }

        private IExpression m_Pos;
        private IExpression m_Radius;
        private IExpression m_Type;
        private IExpression m_EventName;
        private IExpression m_SetVar;
        private IExpression m_SetVal;
        private IExpression m_ElseSetVal;
        private bool m_HaveSet = false;
    }
}
