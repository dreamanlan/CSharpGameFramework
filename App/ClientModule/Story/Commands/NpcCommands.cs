using System;
using System.Collections;
using System.Collections.Generic;
using StorySystem;
using ScriptRuntime;
using GameFramework;
using GameFramework.Skill;

namespace GameFramework.Story.Commands
{
    /// <summary>
    /// createnpc(npc_unit_id,vector3(x,y,z),dir,camp,linkId[,ai,stringlist("param1 param2 param3 ..."),leaderId])[objid("@objid")];
    /// </summary>
    internal class CreateNpcCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            CreateNpcCommand cmd = new CreateNpcCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_Pos = m_Pos.Clone();
            cmd.m_Dir = m_Dir.Clone();
            cmd.m_Camp = m_Camp.Clone();
            cmd.m_LinkId = m_LinkId.Clone();
            cmd.m_AiLogic = m_AiLogic.Clone();
            cmd.m_AiParams = m_AiParams.Clone();
            cmd.m_LeaderId = m_LeaderId.Clone();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_HaveObjId = m_HaveObjId;
            cmd.m_ObjIdVarName = m_ObjIdVarName.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            if (m_ParamNum >= 5) {
                m_UnitId.Substitute(iterator, args);
                m_Pos.Substitute(iterator, args);
                m_Dir.Substitute(iterator, args);
                m_Camp.Substitute(iterator, args);
                m_LinkId.Substitute(iterator, args);

                if (m_ParamNum > 6) {
                    m_AiLogic.Substitute(iterator, args);
                    m_AiParams.Substitute(iterator, args);
                    if (m_ParamNum > 7) {
                        m_LeaderId.Substitute(iterator, args);
                    }
                }
            }
            if (m_HaveObjId) {
                m_ObjIdVarName.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            if (m_ParamNum >= 5) {
                m_UnitId.Evaluate(instance);
                m_Pos.Evaluate(instance);
                m_Dir.Evaluate(instance);
                m_Camp.Evaluate(instance);
                m_LinkId.Evaluate(instance);

                if (m_ParamNum > 6) {
                    m_AiLogic.Evaluate(instance);
                    m_AiParams.Evaluate(instance);
                    if (m_ParamNum > 7) {
                        m_LeaderId.Evaluate(instance);
                    }
                }
            }
            if (m_HaveObjId) {
                m_ObjIdVarName.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int objId = 0;
            if (m_ParamNum >= 5) {
                Vector3 pos = m_Pos.Value;
                float dir = m_Dir.Value;
                int camp = m_Camp.Value;
                int linkId = m_LinkId.Value;
                objId = ClientModule.Instance.CreateEntity(m_UnitId.Value, pos.X, pos.Y, pos.Z, dir, camp, linkId);
                if (m_ParamNum > 6) {
                    int aiLogic = m_AiLogic.Value;

                    EntityInfo charObj = ClientModule.Instance.GetEntityById(objId);
                    if (null != charObj) {
                        charObj.GetAiStateInfo().Reset();
                        charObj.GetAiStateInfo().AiLogic = aiLogic;
                        if (m_ParamNum > 7) {
                            int leaderId = m_LeaderId.Value;
                            charObj.GetAiStateInfo().LeaderID = leaderId;
                        } else {
                            charObj.GetAiStateInfo().LeaderID = 0;
                        }
                        IEnumerable aiParams = m_AiParams.Value;
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
            }
            if (m_HaveObjId) {
                string varName = m_ObjIdVarName.Value;
                instance.SetVariable(varName, objId);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            m_ParamNum = callData.GetParamNum();
            if (m_ParamNum >= 5) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Pos.InitFromDsl(callData.GetParam(1));
                m_Dir.InitFromDsl(callData.GetParam(2));
                m_Camp.InitFromDsl(callData.GetParam(3));
                m_LinkId.InitFromDsl(callData.GetParam(4));

                if (m_ParamNum > 6) {
                    m_AiLogic.InitFromDsl(callData.GetParam(5));
                    m_AiParams.InitFromDsl(callData.GetParam(6));
                    if (m_ParamNum > 7) {
                        m_LeaderId.InitFromDsl(callData.GetParam(7));
                    }
                }
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
            if (callData.GetId() == "objid" && callData.GetParamNum() == 1) {
                m_ObjIdVarName.InitFromDsl(callData.GetParam(0));
                m_HaveObjId = true;
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private int m_ParamNum = 0;
        private IStoryValue<Vector3> m_Pos = new StoryValue<Vector3>();
        private IStoryValue<float> m_Dir = new StoryValue<float>();
        private IStoryValue<int> m_Camp = new StoryValue<int>();
        private IStoryValue<int> m_LinkId = new StoryValue<int>();
        private IStoryValue<int> m_AiLogic = new StoryValue<int>();
        private IStoryValue<IEnumerable> m_AiParams = new StoryValue<IEnumerable>();
        private IStoryValue<int> m_LeaderId = new StoryValue<int>();
        private bool m_HaveObjId = false;
        private IStoryValue<string> m_ObjIdVarName = new StoryValue<string>();
    }
    /// <summary>
    /// destroynpc(npc_unit_id);
    /// </summary>
    internal class DestroyNpcCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            DestroyNpcCommand cmd = new DestroyNpcCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            EntityInfo entity = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != entity) {
                entity.NeedDelete = true;
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
    }
    /// <summary>
    /// destroynpcwithobjid(npc_obj_id);
    /// </summary>
    internal class DestroyNpcWithObjIdCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            DestroyNpcWithObjIdCommand cmd = new DestroyNpcWithObjIdCommand();
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
            int objid = m_ObjId.Value;
            EntityInfo entity = ClientModule.Instance.GetEntityById(objid);
            if (null != entity) {
                entity.NeedDelete = true;
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
    /// npcface(npc_unit_id,dir);
    /// </summary>
    internal class NpcFaceCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcFaceCommand cmd = new NpcFaceCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_Dir = m_Dir.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_Dir.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_Dir.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            float dir = m_Dir.Value;
            EntityInfo entity = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != entity) {
                MovementStateInfo msi = entity.GetMovementStateInfo();
                msi.SetFaceDir(dir);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Dir.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<float> m_Dir = new StoryValue<float>();
    }
    /// <summary>
    /// npcmove(npc_unit_id,vector3(x,y,z));
    /// </summary>
    internal class NpcMoveCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcMoveCommand cmd = new NpcMoveCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_Pos = m_Pos.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_Pos.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_Pos.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            Vector3 pos = m_Pos.Value;
            EntityInfo entity = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != entity) {
                List<Vector3> waypoints = new List<Vector3>();
                waypoints.Add(pos);
                AiStateInfo aiInfo = entity.GetAiStateInfo();
                AiData_ForMoveCommand data = aiInfo.AiDatas.GetData<AiData_ForMoveCommand>();
                if (null == data) {
                    data = new AiData_ForMoveCommand(waypoints);
                    aiInfo.AiDatas.AddData(data);
                }
                data.WayPoints = waypoints;
                data.Index = 0;
                data.IsFinish = false;
                entity.GetMovementStateInfo().TargetPosition = pos;
                aiInfo.Time = 1000;//下一帧即触发移动
                aiInfo.ChangeToState((int)AiStateId.MoveCommand);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Pos.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<Vector3> m_Pos = new StoryValue<Vector3>();
    }
    /// <summary>
    /// npcmovewithwaypoints(npc_unit_id,vector3list("1 2 3 4 5 6"));
    /// </summary>
    internal class NpcMoveWithWaypointsCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcMoveWithWaypointsCommand cmd = new NpcMoveWithWaypointsCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_WayPoints = m_WayPoints.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_WayPoints.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_WayPoints.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            List<object> poses = m_WayPoints.Value;
            EntityInfo entity = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != entity && null != poses && poses.Count > 0) {
                List<Vector3> waypoints = new List<Vector3>();
                waypoints.Add(entity.GetMovementStateInfo().GetPosition3D());
                for (int i = 0; i < poses.Count; ++i) {
                    Vector3 pt = (Vector3)poses[i];
                    waypoints.Add(pt);
                }
                AiStateInfo aiInfo = entity.GetAiStateInfo();
                AiData_ForMoveCommand data = aiInfo.AiDatas.GetData<AiData_ForMoveCommand>();
                if (null == data) {
                    data = new AiData_ForMoveCommand(waypoints);
                    aiInfo.AiDatas.AddData(data);
                }
                data.WayPoints = waypoints;
                data.Index = 0;
                data.IsFinish = false;
                entity.GetMovementStateInfo().TargetPosition = waypoints[0];
                aiInfo.Time = 1000;//下一帧即触发移动
                aiInfo.ChangeToState((int)AiStateId.MoveCommand);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_WayPoints.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<List<object>> m_WayPoints = new StoryValue<List<object>>();
    }
    /// <summary>
    /// npcpatrol(npc_unit_id,vector3list("1 2 3 4 5 6")[,isloop]);
    /// </summary>
    internal class NpcPatrolCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcPatrolCommand cmd = new NpcPatrolCommand();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_WayPoints = m_WayPoints.Clone();
            cmd.m_IsLoop = m_IsLoop.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_WayPoints.Substitute(iterator, args);
            if(m_ParamNum>2)
                m_IsLoop.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_WayPoints.Evaluate(instance);
            if (m_ParamNum > 2)
                m_IsLoop.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            List<object> poses = m_WayPoints.Value;
            bool isLoop = m_ParamNum > 2 ? 0 == m_IsLoop.Value.CompareTo("isloop") : false;
            EntityInfo entity = ClientModule.Instance.GetEntityByUnitId(unitId);
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
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int m_ParamNum = callData.GetParamNum();
            if (m_ParamNum > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_WayPoints.InitFromDsl(callData.GetParam(1));
            }
            if (m_ParamNum > 2) {
                m_IsLoop.InitFromDsl(callData.GetParam(2));
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<List<object>> m_WayPoints = new StoryValue<List<object>>();
        private IStoryValue<string> m_IsLoop = new StoryValue<string>();
    }
    /// <summary>
    /// npcstop(npc_unit_id);
    /// </summary>
    internal class NpcStopCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcStopCommand cmd = new NpcStopCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            EntityInfo entity = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != entity) {
                AiStateInfo aiInfo = entity.GetAiStateInfo();
                if (aiInfo.CurState == (int)AiStateId.MoveCommand || aiInfo.CurState == (int)AiStateId.PursuitCommand || aiInfo.CurState == (int)AiStateId.PatrolCommand) {
                    aiInfo.Time = 0;
                    aiInfo.Target = 0;
                }
                entity.GetMovementStateInfo().IsMoving = false;
                if (aiInfo.CurState > (int)AiStateId.Invalid && aiInfo.CurState < (int)AiStateId.MaxNum)
                    aiInfo.ChangeToState((int)AiStateId.Idle);
            }
            EntityViewModel viewModel = EntityController.Instance.GetEntityViewByUnitId(unitId);
            if (null != viewModel) {
                viewModel.StopMove();
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
    }
    /// <summary>
    /// npcpursuit(unit_id, target_obj_id);
    /// </summary>
    internal class NpcPursuitCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcPursuitCommand cmd = new NpcPursuitCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_TargetId = m_TargetId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_TargetId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_TargetId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            int targetId = m_TargetId.Value;
            EntityInfo obj = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                AiStateInfo aiInfo = obj.GetAiStateInfo();
                aiInfo.Target = targetId;
                aiInfo.Time = 1000;//下一帧即触发移动
                aiInfo.ChangeToState((int)AiStateId.PursuitCommand);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_TargetId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_TargetId = new StoryValue<int>();
    }
    /// <summary>
    /// npcattack(npc_unit_id[,target_unit_id]);
    /// </summary>
    internal class NpcAttackCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcAttackCommand cmd = new NpcAttackCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_TargetUnitId = m_TargetUnitId.Clone();
            cmd.m_ParamNum = m_ParamNum;
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_TargetUnitId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_TargetUnitId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            EntityInfo entity = ClientModule.Instance.GetEntityByUnitId(unitId);
            EntityInfo target = null;
            int targetUnitId = m_TargetUnitId.Value;
            target = ClientModule.Instance.GetEntityByUnitId(targetUnitId);
            if (null != entity && null != target) {
                AiStateInfo aiInfo = entity.GetAiStateInfo();
                aiInfo.Target = target.GetId();
                aiInfo.LastChangeTargetTime = TimeUtility.GetLocalMilliseconds();
                aiInfo.ChangeToState((int)AiStateId.Idle);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            m_ParamNum = callData.GetParamNum();
            if (m_ParamNum > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_TargetUnitId.InitFromDsl(callData.GetParam(1));
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_TargetUnitId = new StoryValue<int>();
    }
    /// <summary>
    /// npcsetformation(npc_unit_id,index);
    /// </summary>
    internal class NpcSetFormationCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcSetFormationCommand cmd = new NpcSetFormationCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_FormationIndex = m_FormationIndex.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_FormationIndex.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_FormationIndex.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            EntityInfo obj = ClientModule.Instance.GetEntityByUnitId(m_UnitId.Value);
            if (null != obj) {
                obj.GetMovementStateInfo().FormationIndex = m_FormationIndex.Value;
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_FormationIndex.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_FormationIndex = new StoryValue<int>();
    }
    /// <summary>
    /// npcenableai(npc_unit_id,true_or_false);
    /// </summary>
    internal class NpcEnableAiCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcEnableAiCommand cmd = new NpcEnableAiCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_Enable = m_Enable.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_Enable.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_Enable.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            EntityInfo obj = ClientModule.Instance.GetEntityByUnitId(m_UnitId.Value);
            if (null != obj) {
                obj.SetAIEnable(m_Enable.Value != "false");
            }
            EntityViewModel viewModel = EntityController.Instance.GetEntityViewByUnitId(m_UnitId.Value);
            if (null != viewModel) {
                viewModel.StopMove();
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Enable.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<string> m_Enable = new StoryValue<string>();
    }
    /// <summary>
    /// npcsetai(unitid,ai_logic_id,stringlist("param1 param2 param3 ..."));
    /// </summary>
    internal class NpcSetAiCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcSetAiCommand cmd = new NpcSetAiCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_AiLogic = m_AiLogic.Clone();
            cmd.m_AiParams = m_AiParams.Clone();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_AiLogic.Substitute(iterator, args);
            m_AiParams.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_AiLogic.Evaluate(instance);
            m_AiParams.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            int aiLogic = m_AiLogic.Value;
            IEnumerable aiParams = m_AiParams.Value;
            EntityInfo charObj = ClientModule.Instance.GetEntityByUnitId(unitId);
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
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_AiLogic.InitFromDsl(callData.GetParam(1));
                m_AiParams.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_AiLogic = new StoryValue<int>();
        private IStoryValue<IEnumerable> m_AiParams = new StoryValue<IEnumerable>();
    }
    /// <summary>
    /// npcsetaitarget(unitid,targetId);
    /// </summary>
    internal class NpcSetAiTargetCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcSetAiTargetCommand cmd = new NpcSetAiTargetCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_TargetId = m_TargetId.Clone();
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_TargetId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_TargetId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            int targetId = m_TargetId.Value;
            EntityInfo charObj = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != charObj) {
                charObj.GetAiStateInfo().Target = targetId;
                charObj.GetAiStateInfo().HateTarget = targetId;
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_TargetId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_TargetId = new StoryValue<int>();
    }
    /// <summary>
    /// npcanimation(unit_id, anim[, normalized_time]);
    /// </summary>
    internal class NpcAnimationCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcAnimationCommand cmd = new NpcAnimationCommand();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_Anim = m_Anim.Clone();
            cmd.m_Time = m_Time.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_Anim.Substitute(iterator, args);
            if (m_ParamNum > 2) {
                m_Time.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_Anim.Evaluate(instance);
            if (m_ParamNum > 2) {
                m_Time.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            string anim = m_Anim.Value;
            UnityEngine.GameObject obj = EntityController.Instance.GetGameObjectByUnitId(unitId);
            if (null != obj) {
                UnityEngine.Animator animator = obj.GetComponent<UnityEngine.Animator>();
                if (null != animator) {
                    float time = 0;
                    if (m_ParamNum > 2) {
                        time = m_Time.Value;
                    }
                    animator.Play(anim, -1, time);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            m_ParamNum = callData.GetParamNum();
            if (m_ParamNum > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Anim.InitFromDsl(callData.GetParam(1));
            }
            if (m_ParamNum > 2) {
                m_Time.InitFromDsl(callData.GetParam(2));
            }
        }

        private int m_ParamNum = 0;
        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<string> m_Anim = new StoryValue<string>();
        private IStoryValue<float> m_Time = new StoryValue<float>();
    }
    /// <summary>
    /// npcanimationparam(unit_id)
    /// {
    ///     float(name,val);
    ///     int(name,val);
    ///     bool(name,val);
    ///     trigger(name,val);
    /// };
    /// </summary>
    internal class NpcAnimationParamCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcAnimationParamCommand cmd = new NpcAnimationParamCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            for (int i = 0; i < m_Params.Count; ++i) {
                ParamInfo param = new ParamInfo();
                param.CopyFrom(m_Params[i]);
                cmd.m_Params.Add(param);
            }
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            for (int i = 0; i < m_Params.Count; ++i) {
                var pair = m_Params[i];
                pair.Key.Substitute(iterator, args);
                pair.Value.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            for (int i = 0; i < m_Params.Count; ++i) {
                var pair = m_Params[i];
                pair.Key.Evaluate(instance);
                pair.Value.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            UnityEngine.GameObject obj = EntityController.Instance.GetGameObjectByUnitId(unitId);
            if (null != obj) {
                UnityEngine.Animator animator = obj.GetComponent<UnityEngine.Animator>();
                if (null != animator) {
                    for (int i = 0; i < m_Params.Count; ++i) {
                        var param = m_Params[i];
                        string type = param.Type;
                        string key = param.Key.Value;
                        object val = param.Value.Value;
                        if (type == "int") {
                            int v = (int)Convert.ChangeType(val, typeof(int));
                            animator.SetInteger(key, v);
                        } else if (type == "float") {
                            float v = (float)Convert.ChangeType(val, typeof(float));
                            animator.SetFloat(key, v);
                        } else if (type == "bool") {
                            bool v = (bool)Convert.ChangeType(val, typeof(bool));
                            animator.SetBool(key, v);
                        } else if (type == "trigger") {
                            string v = val.ToString();
                            if(v=="false"){
                                animator.ResetTrigger(key);
                            } else {
                                animator.SetTrigger(key);
                            }
                        }
                    }
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }

        protected override void Load(Dsl.FunctionData funcData)
        {
            Dsl.CallData callData = funcData.Call;
            if (null != callData) {
                Load(callData);
                for (int i = 0; i < funcData.Statements.Count; ++i) {
                    Dsl.ISyntaxComponent statement = funcData.Statements[i];
                    Dsl.CallData stCall = statement as Dsl.CallData;
                    if (null != stCall && stCall.GetParamNum() >= 2) {
                        string id = stCall.GetId();
                        ParamInfo param = new ParamInfo(id, stCall.GetParam(0), stCall.GetParam(1));
                        m_Params.Add(param);
                    }
                }
            }
        }

        private class ParamInfo
        {
            internal string Type;
            internal IStoryValue<string> Key;
            internal IStoryValue<object> Value;

            internal ParamInfo()
            {
                Init();
            }
            internal ParamInfo(string type, Dsl.ISyntaxComponent keyDsl, Dsl.ISyntaxComponent valDsl) : this()
            {
                Type = type;
                Key.InitFromDsl(keyDsl);
                Value.InitFromDsl(valDsl);
            }
            internal void CopyFrom(ParamInfo other)
            {
                Type = other.Type;
                Key = other.Key.Clone();
                Value = other.Value.Clone();
            }

            private void Init()
            {
                Type = string.Empty;
                Key = new StoryValue<string>();
                Value = new StoryValue();
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private List<ParamInfo> m_Params = new List<ParamInfo>();
    }
    /// <summary>
    /// npcaddimpact(unit_id, impactid, arg1, arg2, ...)[seq("@seq")];
    /// </summary>
    internal class NpcAddImpactCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcAddImpactCommand cmd = new NpcAddImpactCommand();
            cmd.m_UnitId = m_UnitId.Clone();
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
            m_UnitId.Substitute(iterator, args);
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
            m_UnitId.Evaluate(instance);
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
            int unitId = m_UnitId.Value;
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
            EntityInfo obj = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                ImpactInfo impactInfo = new ImpactInfo(impactId);
                impactInfo.StartTime = TimeUtility.GetLocalMilliseconds();
                impactInfo.ImpactSenderId = obj.GetId();
                impactInfo.SkillId = 0;
                if (null != impactInfo.ConfigData) {
                    obj.GetSkillStateInfo().AddImpact(impactInfo);
                    seq = impactInfo.Seq;
                    GfxSkillSystem.Instance.StartSkill(obj.GetId(), impactInfo.ConfigData, seq, locals);
                }
            }
            if (m_HaveSeq) {
                string varName = m_SeqVarName.Value;
                instance.SetVariable(varName, seq);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
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

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_ImpactId = new StoryValue<int>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
        private bool m_HaveSeq = false;
        private IStoryValue<string> m_SeqVarName = new StoryValue<string>();
    }
    /// <summary>
    /// npcremoveimpact(unit_id, seq);
    /// </summary>
    internal class NpcRemoveImpactCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcRemoveImpactCommand cmd = new NpcRemoveImpactCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_Seq = m_Seq.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_Seq.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_Seq.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            int seq = m_Seq.Value;
            EntityInfo obj = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                ImpactInfo impactInfo = obj.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                if (null != impactInfo) {
                    GfxSkillSystem.Instance.StopSkill(obj.GetId(), impactInfo.ImpactId, seq, true);
                    obj.GetSkillStateInfo().RemoveImpact(seq);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Seq.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_Seq = new StoryValue<int>();
    }
    /// <summary>
    /// npccastskill(unit_id, skillid, arg1, arg2, ...);
    /// </summary>
    internal class NpcCastSkillCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcCastSkillCommand cmd = new NpcCastSkillCommand();
            cmd.m_UnitId = m_UnitId.Clone();
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
            m_UnitId.Substitute(iterator, args);
            m_SkillId.Substitute(iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_SkillId.Evaluate(instance);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            int skillId = m_SkillId.Value;
            Dictionary<string, object> locals = new Dictionary<string, object>();
            for (int i = 0; i < m_Args.Count - 1; i += 2) {
                string key = m_Args[i].Value as string;
                object val = m_Args[i + 1].Value;
                if (!string.IsNullOrEmpty(key)) {
                    locals.Add(key, val);
                }
            }
            EntityInfo obj = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                SkillInfo skillInfo = obj.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null != skillInfo) {
                    GfxSkillSystem.Instance.StartSkill(obj.GetId(), skillInfo.ConfigData, 0, locals);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_SkillId.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_SkillId = new StoryValue<int>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// npcstopskill(unit_id);
    /// </summary>
    internal class NpcStopSkillCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcStopSkillCommand cmd = new NpcStopSkillCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            EntityInfo obj = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                GfxSkillSystem.Instance.StopAllSkill(obj.GetId(), true);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_SkillId = new StoryValue<int>();
    }
    /// <summary>
    /// npcaddskill(unit_id, skillid);
    /// </summary>
    internal class NpcAddSkillCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcAddSkillCommand cmd = new NpcAddSkillCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_SkillId = m_SkillId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_SkillId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_SkillId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            int skillId = m_SkillId.Value;
            EntityInfo obj = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                if (obj.GetSkillStateInfo().GetSkillInfoById(skillId) == null) {
                    obj.GetSkillStateInfo().AddSkill(new SkillInfo(skillId));
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_SkillId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_SkillId = new StoryValue<int>();
    }
    /// <summary>
    /// npcremoveskill(unit_id, skillid);
    /// </summary>
    internal class NpcRemoveSkillCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcRemoveSkillCommand cmd = new NpcRemoveSkillCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_SkillId = m_SkillId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_SkillId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_SkillId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            int skillId = m_SkillId.Value;
            EntityInfo obj = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                obj.GetSkillStateInfo().RemoveSkill(skillId);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_SkillId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_SkillId = new StoryValue<int>();
    }
    /// <summary>
    /// npclisten(unit_id, 消息类别, true_or_false);
    /// </summary>
    internal class NpcListenCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcListenCommand cmd = new NpcListenCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_Event = m_Event.Clone();
            cmd.m_Enable = m_Enable.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_Event.Substitute(iterator, args);
            m_Enable.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_Event.Evaluate(instance);
            m_Enable.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            string eventName = m_Event.Value;
            string enable = m_Enable.Value;
            EntityInfo obj = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                if (eventName == "damage") {
                    if (0 == string.Compare(enable, "true"))
                        obj.AddStoryFlag(StoryListenFlagEnum.Damage);
                    else
                        obj.RemoveStoryFlag(StoryListenFlagEnum.Damage);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Event.InitFromDsl(callData.GetParam(1));
                m_Enable.InitFromDsl(callData.GetParam(2));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<string> m_Event = new StoryValue<string>();
        private IStoryValue<string> m_Enable = new StoryValue<string>();
    }
    /// <summary>
    /// setcamp(npc_unit_id,camp_id);
    /// </summary>
    internal class NpcSetCampCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcSetCampCommand cmd = new NpcSetCampCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_CampId = m_CampId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_CampId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_CampId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            EntityInfo obj = ClientModule.Instance.GetEntityByUnitId(m_UnitId.Value);
            if (null != obj) {
                int campId = m_CampId.Value;
                obj.SetCampId(campId);
                Utility.EventSystem.Publish("ui_actor_color", "ui", obj.GetId(), CharacterRelation.RELATION_FRIEND == EntityInfo.GetRelation(ClientModule.Instance.CampId, campId));
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_CampId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_CampId = new StoryValue<int>();
    }
    /// setsummonerid(unit_id, objid);
    /// </summary>
    internal class NpcSetSummonerIdCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcSetSummonerIdCommand cmd = new NpcSetSummonerIdCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_SummonerId = m_SummonerId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_SummonerId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_SummonerId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            int summonerId = m_SummonerId.Value;
            EntityInfo npcInfo = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != npcInfo) {
                npcInfo.SummonerId = summonerId;
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_SummonerId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_SummonerId = new StoryValue<int>();
    }
    /// setsummonskillid(unit_id, objid);
    /// </summary>
    internal class NpcSetSummonSkillIdCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            NpcSetSummonSkillIdCommand cmd = new NpcSetSummonSkillIdCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_SummonSkillId = m_SummonSkillId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UnitId.Substitute(iterator, args);
            m_SummonSkillId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UnitId.Evaluate(instance);
            m_SummonSkillId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int unitId = m_UnitId.Value;
            int summonSkillId = m_SummonSkillId.Value;
            EntityInfo npcInfo = ClientModule.Instance.GetEntityByUnitId(unitId);
            if (null != npcInfo) {
                npcInfo.SummonSkillId = summonSkillId;
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_SummonSkillId.InitFromDsl(callData.GetParam(1));
            }
        }

        private IStoryValue<int> m_UnitId = new StoryValue<int>();
        private IStoryValue<int> m_SummonSkillId = new StoryValue<int>();
    }
    /// <summary>
}
