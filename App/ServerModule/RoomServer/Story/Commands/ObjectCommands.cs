using System;
using System.Collections;
using System.Collections.Generic;
using StorySystem;
using ScriptRuntime;
using GameFramework;
using GameFramework.Skill;
using GameFrameworkMessage;

namespace GameFramework.Story.Commands
{
    /// <summary>
    /// objface(obj_id, dir);
    /// </summary>
    internal class ObjFaceCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjFaceCommand cmd = new ObjFaceCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Dir = m_Dir.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_Dir.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_Dir.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Dir.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<float> m_Dir = new StoryValue<float>();
    }
    /// <summary>
    /// objmove(obj_id, vector3(x,y,z));
    /// </summary>
    internal class ObjMoveCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjMoveCommand cmd = new ObjMoveCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Pos = m_Pos.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_Pos.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_Pos.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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
                    aiInfo.Time = 1000;//下一帧即触发移动
                    aiInfo.ChangeToState((int)AiStateId.MoveCommand);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Pos.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<Vector3> m_Pos = new StoryValue<Vector3>();
    }
    /// <summary>
    /// objmovewithwaypoints(obj_id, vector3list("1 2 3 4 5 6"));
    /// </summary>
    internal class ObjMoveWithWaypointsCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjMoveWithWaypointsCommand cmd = new ObjMoveWithWaypointsCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_WayPoints = m_WayPoints.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_WayPoints.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_WayPoints.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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
                    aiInfo.Time = 1000;//下一帧即触发移动
                    aiInfo.ChangeToState((int)AiStateId.MoveCommand);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_WayPoints.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<List<object>> m_WayPoints = new StoryValue<List<object>>();
    }
    /// <summary>
    /// objpatrol(npc_obj_id,vector3list("1 2 3 4 5 6")[,isloop]);
    /// </summary>
    internal class ObjPatrolCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjPatrolCommand cmd = new ObjPatrolCommand();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_WayPoints = m_WayPoints.Clone();
            cmd.m_IsLoop = m_IsLoop.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_WayPoints.Substitute(iterator, args);
            if (m_ParamNum > 2)
                m_IsLoop.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_WayPoints.Evaluate(instance);
            if (m_ParamNum > 2)
                m_IsLoop.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                List<object> poses = m_WayPoints.Value;
                bool isLoop = m_ParamNum > 2 ? 0 == m_IsLoop.Value.CompareTo("isloop") : false;
                EntityInfo entity = scene.SceneContext.GetEntityById(objId);
                if (null != entity && null != poses) {
                    AiStateInfo aiInfo = entity.GetAiStateInfo();
                    AiData_ForPatrolCommand data = aiInfo.AiDatas.GetData<AiData_ForPatrolCommand>();
                    if (null == data) {
                        data = new AiData_ForPatrolCommand();
                        aiInfo.AiDatas.AddData(data);
                    }
                    List<Vector3> wayPts = new List<Vector3>();
                    for (int i = 0; i < poses.Count; ++i) {
                        Vector3 pt = (Vector3)poses[i];
                        wayPts.Add(pt);
                    }
                    data.PatrolPath.SetPathPoints(entity.GetMovementStateInfo().GetPosition3D(), wayPts);
                    data.IsLoopPatrol = isLoop;
                    aiInfo.Time = 1000;//下一帧即触发移动
                    aiInfo.ChangeToState((int)AiStateId.PatrolCommand);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int m_ParamNum = callData.GetParamNum();
            if (m_ParamNum > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_WayPoints.InitFromDsl(callData.GetParam(1));
            }
            if (m_ParamNum > 2) {
                m_IsLoop.InitFromDsl(callData.GetParam(2));
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<List<object>> m_WayPoints = new StoryValue<List<object>>();
        private IStoryValue<string> m_IsLoop = new StoryValue<string>();
    }
    /// <summary>
    /// objstop(obj_id);
    /// </summary>
    internal class ObjStopCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjStopCommand cmd = new ObjStopCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    AiStateInfo aiInfo = obj.GetAiStateInfo();
                    if (aiInfo.CurState == (int)AiStateId.MoveCommand || aiInfo.CurState == (int)AiStateId.PursuitCommand || aiInfo.CurState == (int)AiStateId.PatrolCommand) {
                        aiInfo.Time = 0;
                        aiInfo.Target = 0;
                    }
                    obj.GetMovementStateInfo().IsMoving = false;
                    if (aiInfo.CurState > (int)AiStateId.Invalid && aiInfo.CurState < (int)AiStateId.MaxNum)
                        aiInfo.ChangeToState((int)AiStateId.Idle);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
    }
    /// <summary>
    /// objpursuit(obj_id, target_obj_id);
    /// </summary>
    internal class ObjPursuitCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjPursuitCommand cmd = new ObjPursuitCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_TargetId = m_TargetId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_TargetId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_TargetId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int targetId = m_TargetId.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    AiStateInfo aiInfo = obj.GetAiStateInfo();
                    aiInfo.Target = targetId;
                    aiInfo.Time = 1000;//下一帧即触发移动
                    aiInfo.ChangeToState((int)AiStateId.PursuitCommand);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_TargetId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_TargetId = new StoryValue<int>();
    }
    /// <summary>
    /// objattack(npc_obj_id[,target_obj_id]);
    /// </summary>
    internal class ObjAttackCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
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

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_TargetObjId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_TargetObjId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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
                    aiInfo.ChangeToState((int)AiStateId.Pursuit);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            m_ParamNum = callData.GetParamNum();
            if (m_ParamNum > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_TargetObjId.InitFromDsl(callData.GetParam(1));
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_TargetObjId = new StoryValue<int>();
    }
    /// <summary>
    /// objsetformation(obj_id, index);
    /// </summary>
    internal class ObjSetFormationCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjSetFormationCommand cmd = new ObjSetFormationCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_FormationIndex = m_FormationIndex.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_FormationIndex.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_FormationIndex.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_FormationIndex.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_FormationIndex = new StoryValue<int>();
    }
    /// <summary>
    /// objenableai(obj_id, true_or_false);
    /// </summary>
    internal class ObjEnableAiCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjEnableAiCommand cmd = new ObjEnableAiCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Enable = m_Enable.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_Enable.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_Enable.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                string enable = m_Enable.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    obj.SetAIEnable(m_Enable.Value != "false");
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Enable.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<string> m_Enable = new StoryValue<string>();
    }
    /// <summary>
    /// objsetai(objid,ai_logic_id,stringlist("param1 param2 param3 ..."));
    /// </summary>
    internal class ObjSetAiCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjSetAiCommand cmd = new ObjSetAiCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_AiLogic = m_AiLogic.Clone();
            cmd.m_AiParams = m_AiParams.Clone();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_AiLogic.Substitute(iterator, args);
            m_AiParams.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_AiLogic.Evaluate(instance);
            m_AiParams.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int aiLogic = m_AiLogic.Value;
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

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_AiLogic.InitFromDsl(callData.GetParam(1));
                m_AiParams.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_AiLogic = new StoryValue<int>();
        private IStoryValue<IEnumerable> m_AiParams = new StoryValue<IEnumerable>();
    }
    /// <summary>
    /// objsetaitarget(objid,targetid);
    /// </summary>
    internal class ObjSetAiTargetCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjSetAiTargetCommand cmd = new ObjSetAiTargetCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_TargetId = m_TargetId.Clone();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_TargetId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_TargetId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_TargetId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_TargetId = new StoryValue<int>();
    }
    /// <summary>
    /// objanimation(obj_id, anim);
    /// </summary>
    internal class ObjAnimationCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjAnimationCommand cmd = new ObjAnimationCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Anim = m_Anim.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_Anim.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_Anim.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Anim.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<string> m_Anim = new StoryValue<string>();
    }
    /// <summary>
    /// objaddimpact(obj_id, impactid, arg1, arg2, ...)[seq("@seq")];
    /// </summary>
    internal class ObjAddImpactCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjAddImpactCommand cmd = new ObjAddImpactCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_ImpactId = m_ImpactId.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            cmd.m_HaveSeq = m_HaveSeq;
            cmd.m_SeqVarName = m_SeqVarName.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_ImpactId.Substitute(iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Substitute(iterator, args);
            }
            if (m_HaveSeq) {
                m_SeqVarName.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_ImpactId.Evaluate(instance);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance);
            }
            if (m_HaveSeq) {
                m_SeqVarName.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int impactId = m_ImpactId.Value;
                int seq = 0;
                Dictionary<string, object> locals = new Dictionary<string, object>();
                for (int i = 0; i < m_Args.Count - 1; i += 2) {
                    string key = m_Args[i].Value as string;
                    object val = m_Args[i + 1].Value;
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
                        if (scene.SkillSystem.StartSkill(objId, impactInfo.ConfigData, seq, locals)) {
                            Msg_RC_AddImpact addImpactBuilder = new Msg_RC_AddImpact();
                            addImpactBuilder.sender_id = obj.GetId();
                            addImpactBuilder.target_id = obj.GetId();
                            addImpactBuilder.impact_id = impactId;
                            addImpactBuilder.skill_id = -1;
                            addImpactBuilder.duration = impactInfo.DurationTime;
                            scene.NotifyAllUser(RoomMessageDefine.Msg_RC_AddImpact, addImpactBuilder);
                        }
                    }
                }
                if (m_HaveSeq) {
                    string varName = m_SeqVarName.Value;
                    instance.SetVariable(varName, seq);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_ImpactId.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadVarName(second.Call);
                }
            }
        }

        private void LoadVarName(Dsl.CallData callData)
        {
            if (callData.GetId() == "seq" && callData.GetParamNum() == 1) {
                m_SeqVarName.InitFromDsl(callData.GetParam(0));
                m_HaveSeq = true;
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_ImpactId = new StoryValue<int>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
        private bool m_HaveSeq = false;
        private IStoryValue<string> m_SeqVarName = new StoryValue<string>();
    }
    /// <summary>
    /// objremoveimpact(obj_id, seq);
    /// </summary>
    internal class ObjRemoveImpactCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjRemoveImpactCommand cmd = new ObjRemoveImpactCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Seq = m_Seq.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_Seq.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_Seq.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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

                        scene.SkillSystem.StopSkill(objId, impactInfo.ImpactId, seq, true);
                        obj.GetSkillStateInfo().RemoveImpact(seq);
                    }
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Seq.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_Seq = new StoryValue<int>();
    }
    /// <summary>
    /// objcastskill(obj_id, skillid, arg1, arg2, ...);
    /// </summary>
    internal class ObjCastSkillCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjCastSkillCommand cmd = new ObjCastSkillCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_SkillId = m_SkillId.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_SkillId.Substitute(iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_SkillId.Evaluate(instance);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int skillId = m_SkillId.Value;
                Dictionary<string, object> locals = new Dictionary<string, object>();
                for (int i = 0; i < m_Args.Count - 1; i += 2) {
                    string key = m_Args[i].Value as string;
                    object val = m_Args[i + 1].Value;
                    if (!string.IsNullOrEmpty(key)) {
                        locals.Add(key, val);
                    }
                }
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    SkillInfo skillInfo = obj.GetSkillStateInfo().GetSkillInfoById(skillId);
                    if (null != skillInfo) {
                        if (scene.SkillSystem.StartSkill(objId, skillInfo.ConfigData, 0, locals)) {
                            Msg_RC_NpcSkill skillBuilder = new Msg_RC_NpcSkill();
                            skillBuilder.npc_id = obj.GetId();
                            skillBuilder.skill_id = skillId;
                            float x = obj.GetMovementStateInfo().GetPosition3D().X;
                            float z = obj.GetMovementStateInfo().GetPosition3D().Z;
                            skillBuilder.stand_pos = ProtoHelper.EncodePosition2D(x, z);
                            skillBuilder.face_direction = ProtoHelper.EncodeFloat(obj.GetMovementStateInfo().GetFaceDir());

                            scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcSkill, skillBuilder);
                        }
                    }
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_SkillId.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_SkillId = new StoryValue<int>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// objstopskill(obj_id);
    /// </summary>
    internal class ObjStopSkillCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjStopSkillCommand cmd = new ObjStopSkillCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                scene.SkillSystem.StopAllSkill(objId, true);

                Msg_RC_NpcStopSkill skillBuilder = new Msg_RC_NpcStopSkill();
                skillBuilder.npc_id = objId;
                scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcStopSkill, skillBuilder);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_SkillId = new StoryValue<int>();
    }
    /// <summary>
    /// objaddskill(obj_id, skillid);
    /// </summary>
    internal class ObjAddSkillCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjAddSkillCommand cmd = new ObjAddSkillCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_SkillId = m_SkillId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_SkillId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_SkillId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_SkillId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_SkillId = new StoryValue<int>();
    }
    /// <summary>
    /// objremoveskill(obj_id, skillid);
    /// </summary>
    internal class ObjRemoveSkillCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjRemoveSkillCommand cmd = new ObjRemoveSkillCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_SkillId = m_SkillId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_SkillId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_SkillId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_SkillId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_SkillId = new StoryValue<int>();
    }
    /// <summary>
    /// objlisten(unit_id, 消息类别, true_or_false);
    /// </summary>
    internal class ObjListenCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
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

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_Event.Substitute(iterator, args);
            m_Enable.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_Event.Evaluate(instance);
            m_Enable.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                string eventName = m_Event.Value;
                string enable = m_Enable.Value;
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    if (eventName == "damage") {
                        if (0 == string.Compare(enable, "true"))
                            obj.AddStoryFlag(StoryListenFlagEnum.Damage);
                        else
                            obj.RemoveStoryFlag(StoryListenFlagEnum.Damage);
                    }
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Event.InitFromDsl(callData.GetParam(1));
                m_Enable.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<string> m_Event = new StoryValue<string>();
        private IStoryValue<string> m_Enable = new StoryValue<string>();
    }
    /// <summary>
    /// sethp(objid,value);
    /// </summary>
    internal class SetHpCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SetHpCommand cmd = new SetHpCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_Value.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_Value.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int value = m_Value.Value;
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    charObj.SetHp(Operate_Type.OT_Absolute, value);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_Value = new StoryValue<int>();
    }
    /// <summary>
    /// setenergy(objid,value);
    /// </summary>
    internal class SetEnergyCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SetEnergyCommand cmd = new SetEnergyCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_Value.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_Value.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int value = m_Value.Value;
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    charObj.SetEnergy(Operate_Type.OT_Absolute, value);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_Value = new StoryValue<int>();
    }
    /// <summary>
    /// objset(uniqueid,localname,value);
    /// </summary>
    internal class ObjSetCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjSetCommand cmd = new ObjSetCommand();
            cmd.m_UniqueId = m_UniqueId.Clone();
            cmd.m_AttrName = m_AttrName.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UniqueId.Substitute(iterator, args);
            m_AttrName.Substitute(iterator, args);
            m_Value.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UniqueId.Evaluate(instance);
            m_AttrName.Evaluate(instance);
            m_Value.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int uniqueId = m_UniqueId.Value;
                string localName = m_AttrName.Value;
                object value = m_Value.Value;
                scene.SceneContext.ObjectSet(uniqueId, localName, value);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_UniqueId.InitFromDsl(callData.GetParam(0));
                m_AttrName.InitFromDsl(callData.GetParam(1));
                m_Value.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<int> m_UniqueId = new StoryValue<int>();
        private IStoryValue<string> m_AttrName = new StoryValue<string>();
        private IStoryValue<object> m_Value = new StoryValue();
    }
    /// <summary>
    /// setlevel(objid,value);
    /// </summary>
    internal class SetLevelCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SetLevelCommand cmd = new SetLevelCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_Value.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_Value.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                int value = m_Value.Value;
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    charObj.SetLevel(value);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_Value = new StoryValue<int>();
    }
    /// <summary>
    /// setattr(objid,attrname,value);
    /// </summary>
    internal class SetAttrCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SetAttrCommand cmd = new SetAttrCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_AttrName = m_AttrName.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_AttrName.Substitute(iterator, args);
            m_Value.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_AttrName.Evaluate(instance);
            m_Value.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                string attrName = m_AttrName.Value;
                object value = m_Value.Value;
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    try {
                        Type t = charObj.GetBaseProperty().GetType();
                        t.InvokeMember("Set" + attrName, System.Reflection.BindingFlags.InvokeMethod, null, charObj.GetBaseProperty(), new object[] { Operate_Type.OT_Absolute, value });
                        charObj.LevelChanged = true;
                    } catch (Exception ex) {
                        LogSystem.Warn("setattr throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_AttrName.InitFromDsl(callData.GetParam(1));
                m_Value.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<string> m_AttrName = new StoryValue<string>();
        private IStoryValue<object> m_Value = new StoryValue();
    }
    /// <summary>
    /// markcontrolbystory(objid,true_or_false);
    /// </summary>
    internal class MarkControlByStoryCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            MarkControlByStoryCommand cmd = new MarkControlByStoryCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_Value = m_Value.Clone();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_Value.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_Value.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_Value.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<string> m_Value = new StoryValue<string>();
    }
    /// <summary>
    /// setunitid(obj_id, dir);
    /// </summary>
    internal class SetUnitIdCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SetUnitIdCommand cmd = new SetUnitIdCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_UnitId = m_UnitId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_UnitId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_UnitId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_UnitId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_UnitId = new StoryValue<int>();
    }
    /// objsetsummonerid(objid, objid);
    /// </summary>
    internal class ObjSetSummonerIdCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ObjSetSummonerIdCommand cmd = new ObjSetSummonerIdCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_SummonerId = m_SummonerId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_ObjId.Substitute(iterator, args);
            m_SummonerId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_ObjId.Evaluate(instance);
            m_SummonerId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_SummonerId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_ObjId = new StoryValue<int>();
        private IStoryValue<int> m_SummonerId = new StoryValue<int>();
    }
}
