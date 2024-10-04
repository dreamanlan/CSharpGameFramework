using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript;
using ScriptRuntime;
using ScriptableFramework;
using GameFrameworkMessage;

namespace ScriptableFramework.Story.Commands
{
    /// <summary>
    /// objface(obj_id, dir);
    /// </summary>
    public class ObjFaceCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjFaceCommand cmd = new ObjFaceCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Dir = m_Dir.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_Dir.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                float dir = m_Dir.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    MovementStateInfo msi = obj.GetMovementStateInfo();
                    msi.SetFaceDir(dir);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Dir.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<float> m_Dir = new StoryFunction<float>();
    }
    /// <summary>
    /// objmove(obj_id, vector3(x,y,z));
    /// </summary>
    public class ObjMoveCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjMoveCommand cmd = new ObjMoveCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Pos = m_Pos.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_Pos.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                Vector3 pos = m_Pos.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    List<Vector3> waypoints = new List<Vector3>();
                    waypoints.Add(pos);
                    AiStateInfo aiInfo = obj.GetAiStateInfo();
                    AiData_ForMoveCommand data = aiInfo.AiDatas.GetData<AiData_ForMoveCommand>();
                    if (null == data) {
                        data = new AiData_ForMoveCommand(waypoints);
                        aiInfo.AiDatas.AddData(data);
                    }
                    data.WayPoints = waypoints;
                    data.Index = 0;
                    data.IsFinish = false;
                    obj.GetMovementStateInfo().TargetPosition = pos;
                    aiInfo.Time = 1000;//Movement is triggered on the next frame
                    aiInfo.ChangeToState((int)PredefinedAiStateId.MoveCommand);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Pos.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<Vector3> m_Pos = new StoryFunction<Vector3>();
    }
    /// <summary>
    /// objmovewithwaypoints(obj_id, vector3list("1 2 3 4 5 6"));
    /// </summary>
    public class ObjMoveWithWaypointsCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjMoveWithWaypointsCommand cmd = new ObjMoveWithWaypointsCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_WayPoints = m_WayPoints.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_WayPoints.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                List<object> poses = m_WayPoints.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj && null != poses && poses.Count > 0) {
                    List<Vector3> waypoints = new List<Vector3>();
                    waypoints.Add(obj.GetMovementStateInfo().GetPosition3D());
                    for (int i = 0; i < poses.Count; ++i) {
                        Vector3 pt = (Vector3)poses[i];
                        waypoints.Add(pt);
                    }
                    AiStateInfo aiInfo = obj.GetAiStateInfo();
                    AiData_ForMoveCommand data = aiInfo.AiDatas.GetData<AiData_ForMoveCommand>();
                    if (null == data) {
                        data = new AiData_ForMoveCommand(waypoints);
                        aiInfo.AiDatas.AddData(data);
                    }
                    data.WayPoints = waypoints;
                    data.Index = 0;
                    data.IsFinish = false;
                    obj.GetMovementStateInfo().TargetPosition = waypoints[0];
                    aiInfo.Time = 1000;//Movement is triggered on the next frame
                    aiInfo.ChangeToState((int)PredefinedAiStateId.MoveCommand);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_WayPoints.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<List<object>> m_WayPoints = new StoryFunction<List<object>>();
    }
    /// <summary>
    /// objstop(obj_id);
    /// </summary>
    public class ObjStopCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjStopCommand cmd = new ObjStopCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    AiStateInfo aiInfo = obj.GetAiStateInfo();
                    if (aiInfo.CurState == (int)PredefinedAiStateId.MoveCommand) {
                        aiInfo.Time = 0;
                        aiInfo.Target = 0;
                    }
                    obj.GetMovementStateInfo().IsMoving = false;
                    if (aiInfo.CurState > (int)PredefinedAiStateId.Invalid)
                        aiInfo.ChangeToState((int)PredefinedAiStateId.Idle);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
    }
    /// <summary>
    /// objattack(npc_obj_id[,target_obj_id]);
    /// </summary>
    public class ObjAttackCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjAttackCommand cmd = new ObjAttackCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_TargetObjId = m_TargetObjId.Clone();
            cmd.m_ParamNum = m_ParamNum;
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_TargetObjId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                EntityInfo entity = scene.SceneContext.GetEntityById(objId);
                EntityInfo target = null;
                int targetObjId = m_TargetObjId.Value;
                if (null != entity && null != target) {
                    AiStateInfo aiInfo = entity.GetAiStateInfo();
                    aiInfo.Target = targetObjId;
                    aiInfo.LastChangeTargetTime = TimeUtility.GetLocalMilliseconds();
                    aiInfo.ChangeToState((int)PredefinedAiStateId.Idle);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            m_ParamNum = callData.GetParamNum();
            if (m_ParamNum > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_TargetObjId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private int m_ParamNum = 0;
        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_TargetObjId = new StoryFunction<int>();
    }
    /// <summary>
    /// objsetformation(obj_id, index);
    /// </summary>
    public class ObjSetFormationCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjSetFormationCommand cmd = new ObjSetFormationCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_FormationIndex = m_FormationIndex.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_FormationIndex.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    obj.GetMovementStateInfo().FormationIndex = m_FormationIndex.Value;
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_FormationIndex.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_FormationIndex = new StoryFunction<int>();
    }
    /// <summary>
    /// objenableai(obj_id, 1_or_0);
    /// </summary>
    public class ObjEnableAiCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjEnableAiCommand cmd = new ObjEnableAiCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Enable = m_Enable.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_Enable.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    obj.SetAIEnable(m_Enable.Value != 0);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Enable.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_Enable = new StoryFunction<int>();
    }
    /// <summary>
    /// objsetai(objid,ai_logic_id,stringlist("param1 param2 param3 ..."));
    /// </summary>
    public class ObjSetAiCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjSetAiCommand cmd = new ObjSetAiCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_AiLogic = m_AiLogic.Clone();
            cmd.m_AiParams = m_AiParams.Clone();
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_AiLogic.Evaluate(instance, handler, iterator, args);
            m_AiParams.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                string aiLogic = m_AiLogic.Value;
                IEnumerable aiParams = m_AiParams.Value;
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    charObj.GetAiStateInfo().Reset();
                    charObj.GetAiStateInfo().AiLogic = aiLogic;
                    int ix = 0;
                    foreach (string aiParam in aiParams) {
                        if (ix < AiStateInfo.c_MaxAiParamNum) {
                            charObj.GetAiStateInfo().AiParam[ix] = aiParam;
                            ++ix;
                        } else {
                            break;
                        }
                    }
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_AiLogic.InitFromDsl(callData.GetParam(1));
                m_AiParams.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<string> m_AiLogic = new StoryFunction<string>();
        private IStoryFunction<IEnumerable> m_AiParams = new StoryFunction<IEnumerable>();
    }
    /// <summary>
    /// objsetaitarget(objid,targetid);
    /// </summary>
    public class ObjSetAiTargetCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjSetAiTargetCommand cmd = new ObjSetAiTargetCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_TargetId = m_TargetId.Clone();
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_TargetId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int targetId = m_TargetId.Value;
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    charObj.GetAiStateInfo().Target = targetId;
                    charObj.GetAiStateInfo().HateTarget = targetId;
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_TargetId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_TargetId = new StoryFunction<int>();
    }
    /// <summary>
    /// objanimation(obj_id, anim);
    /// </summary>
    public class ObjAnimationCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjAnimationCommand cmd = new ObjAnimationCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Anim = m_Anim.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_Anim.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                string anim = m_Anim.Value;
                EntityInfo obj = scene.EntityController.GetGameObject(objId);
                if (null != obj) {
                    Msg_RC_PlayAnimation msg = new Msg_RC_PlayAnimation();
                    msg.obj_id = objId;
                    msg.anim_name = anim;
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_PlayAnimation, msg);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Anim.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<string> m_Anim = new StoryFunction<string>();
    }
    /// <summary>
    /// objaddimpact(obj_id, impactid, arg1, arg2, ...)[seq("@seq")];
    /// </summary>
    public class ObjAddImpactCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjAddImpactCommand cmd = new ObjAddImpactCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_ImpactId = m_ImpactId.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            cmd.m_HaveSeq = m_HaveSeq;
            cmd.m_SeqVarName = m_SeqVarName.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_ImpactId.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }
            if (m_HaveSeq) {
                m_SeqVarName.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int impactId = m_ImpactId.Value;
                int seq = 0;
                Dictionary<string, object> locals = new Dictionary<string, object>();
                for (int i = 0; i < m_Args.Count - 1; i += 2) {
                    string key = m_Args[i].Value;
                    object val = m_Args[i + 1].Value.GetInt();
                    if (!string.IsNullOrEmpty(key)) {
                        locals.Add(key, val);
                    }
                }
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    ImpactInfo impactInfo = new ImpactInfo(impactId);
                    impactInfo.StartTime = TimeUtility.GetLocalMilliseconds();
                    impactInfo.ImpactSenderId = objId;
                    impactInfo.SkillId = 0;
                    if (null != impactInfo.ConfigData) {
                        obj.GetSkillStateInfo().AddImpact(impactInfo);
                        seq = impactInfo.Seq;
                        Msg_RC_AddImpact addImpactBuilder = new Msg_RC_AddImpact();
                        addImpactBuilder.sender_id = obj.GetId();
                        addImpactBuilder.target_id = obj.GetId();
                        addImpactBuilder.impact_id = impactId;
                        addImpactBuilder.skill_id = -1;
                        addImpactBuilder.duration = impactInfo.DurationTime;
                        scene.NotifyAllUser(RoomMessageDefine.Msg_RC_AddImpact, addImpactBuilder);
                    }
                }
                if (m_HaveSeq) {
                    string varName = m_SeqVarName.Value;
                    instance.SetVariable(varName, seq);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_ImpactId.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryFunction val = new StoryFunction();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
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
                    LoadVarName(second);
                }
            }
            return true;
        }

        private void LoadVarName(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "seq" && callData.GetParamNum() == 1) {
                m_SeqVarName.InitFromDsl(callData.GetParam(0));
                m_HaveSeq = true;
            }
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_ImpactId = new StoryFunction<int>();
        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
        private bool m_HaveSeq = false;
        private IStoryFunction<string> m_SeqVarName = new StoryFunction<string>();
    }
    /// <summary>
    /// objremoveimpact(obj_id, seq);
    /// </summary>
    public class ObjRemoveImpactCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjRemoveImpactCommand cmd = new ObjRemoveImpactCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Seq = m_Seq.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_Seq.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int seq = m_Seq.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    ImpactInfo impactInfo = obj.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                    if (null != impactInfo) {
                        Msg_RC_RemoveImpact removeImpactBuilder = new Msg_RC_RemoveImpact();
                        removeImpactBuilder.obj_id = obj.GetId();
                        removeImpactBuilder.impact_id = impactInfo.ImpactId;
                        scene.NotifyAllUser(RoomMessageDefine.Msg_RC_RemoveImpact, removeImpactBuilder);

                        obj.GetSkillStateInfo().RemoveImpact(seq);
                    }
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Seq.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_Seq = new StoryFunction<int>();
    }
    /// <summary>
    /// objcastskill(obj_id, skillid, arg1, arg2, ...);
    /// </summary>
    public class ObjCastSkillCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjCastSkillCommand cmd = new ObjCastSkillCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_SkillId = m_SkillId.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_SkillId.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int skillId = m_SkillId.Value;
                Dictionary<string, object> locals = new Dictionary<string, object>();
                for (int i = 0; i < m_Args.Count - 1; i += 2) {
                    string key = m_Args[i].Value;
                    object val = m_Args[i + 1].Value.GetInt();
                    if (!string.IsNullOrEmpty(key)) {
                        locals.Add(key, val);
                    }
                }
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    SkillInfo skillInfo = obj.GetSkillStateInfo().GetSkillInfoById(skillId);
                    if (null != skillInfo) {
                        Msg_RC_NpcSkill skillBuilder = new Msg_RC_NpcSkill();
                        skillBuilder.npc_id = obj.GetId();
                        skillBuilder.skill_id = skillId;
                        float x = obj.GetMovementStateInfo().GetPosition3D().X;
                        float z = obj.GetMovementStateInfo().GetPosition3D().Z;
                        skillBuilder.stand_pos = DataSyncUtility.ToPosition(x, z);
                        skillBuilder.face_direction = obj.GetMovementStateInfo().GetFaceDir();

                        scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcSkill, skillBuilder);
                    }
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_SkillId.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryFunction val = new StoryFunction();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_SkillId = new StoryFunction<int>();
        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
    }
    /// <summary>
    /// objstopskill(obj_id);
    /// </summary>
    public class ObjStopSkillCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjStopSkillCommand cmd = new ObjStopSkillCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;

                Msg_RC_NpcStopSkill skillBuilder = new Msg_RC_NpcStopSkill();
                skillBuilder.npc_id = objId;
                scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcStopSkill, skillBuilder);
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_SkillId = new StoryFunction<int>();
    }
    /// <summary>
    /// objaddskill(obj_id, skillid);
    /// </summary>
    public class ObjAddSkillCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjAddSkillCommand cmd = new ObjAddSkillCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_SkillId = m_SkillId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_SkillId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int skillId = m_SkillId.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    if (obj.GetSkillStateInfo().GetSkillInfoById(skillId) == null) {
                        obj.GetSkillStateInfo().AddSkill(new SkillInfo(skillId));

                        Msg_RC_AddSkill msg = new Msg_RC_AddSkill();
                        msg.obj_id = obj.GetId();
                        msg.skill_id = skillId;
                        scene.NotifyAllUser(RoomMessageDefine.Msg_RC_AddSkill, msg);
                    }
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_SkillId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_SkillId = new StoryFunction<int>();
    }
    /// <summary>
    /// objremoveskill(obj_id, skillid);
    /// </summary>
    public class ObjRemoveSkillCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjRemoveSkillCommand cmd = new ObjRemoveSkillCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_SkillId = m_SkillId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_SkillId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int skillId = m_SkillId.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    obj.GetSkillStateInfo().RemoveSkill(skillId);

                    Msg_RC_RemoveSkill msg = new Msg_RC_RemoveSkill();
                    msg.obj_id = obj.GetId();
                    msg.skill_id = skillId;
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_RemoveSkill, msg);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_SkillId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_SkillId = new StoryFunction<int>();
    }
    /// <summary>
    /// objlisten(unit_id, message_type, true_or_false);
    /// </summary>
    public class ObjListenCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjListenCommand cmd = new ObjListenCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Event = m_Event.Clone();
            cmd.m_Enable = m_Enable.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_Event.Evaluate(instance, handler, iterator, args);
            m_Enable.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                string eventName = m_Event.Value;
                string enable = m_Enable.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    if (StoryListenFlagEnum.Damage == StoryListenFlagUtility.FromString(eventName)) {
                        if (0 == string.Compare(enable, "true"))
                            obj.AddStoryFlag(StoryListenFlagEnum.Damage);
                        else
                            obj.RemoveStoryFlag(StoryListenFlagEnum.Damage);
                    }
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Event.InitFromDsl(callData.GetParam(1));
                m_Enable.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<string> m_Event = new StoryFunction<string>();
        private IStoryFunction<string> m_Enable = new StoryFunction<string>();
    }
    /// <summary>
    /// sethp(objid,value);
    /// </summary>
    public class SetHpCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SetHpCommand cmd = new SetHpCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int value = m_Value.Value;
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    charObj.Hp = value;
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_Value = new StoryFunction<int>();
    }
    /// <summary>
    /// setenergy(objid,value);
    /// </summary>
    public class SetEnergyCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SetEnergyCommand cmd = new SetEnergyCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int value = m_Value.Value;
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    charObj.Energy = value;
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_Value = new StoryFunction<int>();
    }
    /// <summary>
    /// objset(uniqueid,localname,value);
    /// </summary>
    public class ObjSetCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjSetCommand cmd = new ObjSetCommand();
            cmd.m_UniqueId = m_UniqueId.Clone();
            cmd.m_AttrName = m_AttrName.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UniqueId.Evaluate(instance, handler, iterator, args);
            m_AttrName.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int uniqueId = m_UniqueId.Value;
                string localName = m_AttrName.Value;
                object value = m_Value.Value.GetInt();
                scene.SceneContext.ObjectSet(uniqueId, localName, value);
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_UniqueId.InitFromDsl(callData.GetParam(0));
                m_AttrName.InitFromDsl(callData.GetParam(1));
                m_Value.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }

        private IStoryFunction<int> m_UniqueId = new StoryFunction<int>();
        private IStoryFunction<string> m_AttrName = new StoryFunction<string>();
        private IStoryFunction m_Value = new StoryFunction();
    }
    /// <summary>
    /// setlevel(objid,value);
    /// </summary>
    public class SetLevelCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SetLevelCommand cmd = new SetLevelCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int value = m_Value.Value;
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    charObj.Level = value;
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_Value = new StoryFunction<int>();
    }
    /// <summary>
    /// setattr(objid,attrname,value);
    /// </summary>
    public class SetAttrCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SetAttrCommand cmd = new SetAttrCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_AttrName = m_AttrName.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_AttrName.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                string attrName = m_AttrName.Value;
                var value = m_Value.Value;
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    try {
                        //Type t = charObj.GetBaseProperty().GetType();
                        //t.InvokeMember("Set" + attrName, System.Reflection.BindingFlags.InvokeMethod, null, charObj.GetBaseProperty(), new object[] { Operate_Type.OT_Absolute, value });
                        charObj.LevelChanged = true;
                    } catch (Exception ex) {
                        LogSystem.Warn("setattr throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_AttrName.InitFromDsl(callData.GetParam(1));
                m_Value.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<string> m_AttrName = new StoryFunction<string>();
        private IStoryFunction m_Value = new StoryFunction();
    }
    /// <summary>
    /// markcontrolbystory(objid,true_or_false);
    /// </summary>
    public class MarkControlByStoryCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            MarkControlByStoryCommand cmd = new MarkControlByStoryCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_Value.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                string value = m_Value.Value;
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    charObj.IsControlByStory = (0 == value.CompareTo("true"));
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<string> m_Value = new StoryFunction<string>();
    }
    /// <summary>
    /// setunitid(obj_id, dir);
    /// </summary>
    public class SetUnitIdCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SetUnitIdCommand cmd = new SetUnitIdCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_UnitId = m_UnitId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_UnitId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int unitId = m_UnitId.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    obj.SetUnitId(unitId);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_UnitId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_UnitId = new StoryFunction<int>();
    }
    /// <summary>
    /// objsetcamp(objid,camp_id);
    /// </summary>
    public class ObjSetCampCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjSetCampCommand cmd = new ObjSetCampCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_CampId = m_CampId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_CampId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                EntityInfo obj = scene.SceneContext.GetEntityById(m_ObjId.Value);
                if (null != obj) {
                    int campId = m_CampId.Value;
                    obj.SetCampId(campId);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_CampId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_CampId = new StoryFunction<int>();
    }
    /// objsetsummonerid(objid, objid);
    /// </summary>
    public class ObjSetSummonerIdCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjSetSummonerIdCommand cmd = new ObjSetSummonerIdCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_SummonerId = m_SummonerId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_SummonerId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int summonerId = m_SummonerId.Value;
                EntityInfo npcInfo = scene.SceneContext.GetEntityById(objId);
                if (null != npcInfo) {
                    npcInfo.SummonerId = summonerId;
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_SummonerId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_SummonerId = new StoryFunction<int>();
    }
    /// objsetsummonskillid(objid, objid);
    /// </summary>
    public class ObjSetSummonSkillIdCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ObjSetSummonSkillIdCommand cmd = new ObjSetSummonSkillIdCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_SummonSkillId = m_SummonSkillId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_SummonSkillId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int summonSkillId = m_SummonSkillId.Value;
                EntityInfo npcInfo = scene.SceneContext.GetEntityById(objId);
                if (null != npcInfo) {
                    npcInfo.SummonSkillId = summonSkillId;
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_SummonSkillId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryFunction<int>();
        private IStoryFunction<int> m_SummonSkillId = new StoryFunction<int>();
    }
}
