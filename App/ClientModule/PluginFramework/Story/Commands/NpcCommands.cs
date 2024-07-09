using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript;
using ScriptRuntime;
using ScriptableFramework;
using ScriptableFramework.Skill;

namespace ScriptableFramework.Story.Commands
{
    /// <summary>
    /// createnpc(npc_unit_id,vector3(x,y,z),dir,camp,tableId[,ai,stringlist("param1 param2 param3 ..."),leaderId])[objid("@objid")];
    /// </summary>
    internal class CreateNpcCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            CreateNpcCommand cmd = new CreateNpcCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_Pos = m_Pos.Clone();
            cmd.m_Dir = m_Dir.Clone();
            cmd.m_Camp = m_Camp.Clone();
            cmd.m_TableId = m_TableId.Clone();
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

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            if (m_ParamNum >= 5) {
                m_UnitId.Evaluate(instance, handler, iterator, args);
                m_Pos.Evaluate(instance, handler, iterator, args);
                m_Dir.Evaluate(instance, handler, iterator, args);
                m_Camp.Evaluate(instance, handler, iterator, args);
                m_TableId.Evaluate(instance, handler, iterator, args);

                if (m_ParamNum > 6) {
                    m_AiLogic.Evaluate(instance, handler, iterator, args);
                    m_AiParams.Evaluate(instance, handler, iterator, args);
                    if (m_ParamNum > 7) {
                        m_LeaderId.Evaluate(instance, handler, iterator, args);
                    }
                }
            }
            if (m_HaveObjId) {
                m_ObjIdVarName.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int objId = 0;
            if (m_ParamNum >= 5) {
                Vector3 pos = m_Pos.Value;
                float dir = m_Dir.Value;
                int camp = m_Camp.Value;
                int tableId = m_TableId.Value;
                if (m_ParamNum > 6) {
                    string aiLogic = m_AiLogic.Value;
                    List<string> aiParams = new List<string>();
                    IEnumerable aiParamEnumer = m_AiParams.Value;
                    foreach (string aiParam in aiParamEnumer) {
                        aiParams.Add(aiParam);
                    }
                    objId = PluginFramework.Instance.CreateEntity(m_UnitId.Value, pos.X, pos.Y, pos.Z, dir, camp, tableId, aiLogic, aiParams.ToArray());
                } else {
                    objId = PluginFramework.Instance.CreateEntity(m_UnitId.Value, pos.X, pos.Y, pos.Z, dir, camp, tableId);
                }
                if (m_ParamNum > 6) {
                    EntityInfo charObj = PluginFramework.Instance.GetEntityById(objId);
                    if (null != charObj) {
                        if (m_ParamNum > 7) {
                            int leaderId = m_LeaderId.Value;
                            charObj.GetAiStateInfo().LeaderId = leaderId;
                        } else {
                            charObj.GetAiStateInfo().LeaderId = 0;
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

        protected override bool Load(Dsl.FunctionData callData)
        {
            m_ParamNum = callData.GetParamNum();
            if (m_ParamNum >= 5) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Pos.InitFromDsl(callData.GetParam(1));
                m_Dir.InitFromDsl(callData.GetParam(2));
                m_Camp.InitFromDsl(callData.GetParam(3));
                m_TableId.InitFromDsl(callData.GetParam(4));

                if (m_ParamNum > 6) {
                    m_AiLogic.InitFromDsl(callData.GetParam(5));
                    m_AiParams.InitFromDsl(callData.GetParam(6));
                    if (m_ParamNum > 7) {
                        m_LeaderId.InitFromDsl(callData.GetParam(7));
                    }
                }
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
            if (callData.GetId() == "objid" && callData.GetParamNum() == 1) {
                m_ObjIdVarName.InitFromDsl(callData.GetParam(0));
                m_HaveObjId = true;
            }
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private int m_ParamNum = 0;
        private IStoryFunction<Vector3> m_Pos = new StoryValue<Vector3>();
        private IStoryFunction<float> m_Dir = new StoryValue<float>();
        private IStoryFunction<int> m_Camp = new StoryValue<int>();
        private IStoryFunction<int> m_TableId = new StoryValue<int>();
        private IStoryFunction<string> m_AiLogic = new StoryValue<string>();
        private IStoryFunction<IEnumerable> m_AiParams = new StoryValue<IEnumerable>();
        private IStoryFunction<int> m_LeaderId = new StoryValue<int>();
        private bool m_HaveObjId = false;
        private IStoryFunction<string> m_ObjIdVarName = new StoryValue<string>();
    }
    /// <summary>
    /// destroynpc(npc_unit_id);
    /// </summary>
    internal class DestroyNpcCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            DestroyNpcCommand cmd = new DestroyNpcCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            EntityInfo entity = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != entity) {
                entity.NeedDelete = true;
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
    }
    /// <summary>
    /// destroynpcwithobjid(npc_obj_id);
    /// </summary>
    internal class DestroyNpcWithObjIdCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            DestroyNpcWithObjIdCommand cmd = new DestroyNpcWithObjIdCommand();
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
            int objid = m_ObjId.Value;
            EntityInfo entity = PluginFramework.Instance.GetEntityById(objid);
            if (null != entity) {
                entity.NeedDelete = true;
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

        private IStoryFunction<int> m_ObjId = new StoryValue<int>();
    }
    /// <summary>
    /// npcface(npc_unit_id, dir[, immediately]);
    /// </summary>
    internal class NpcFaceCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcFaceCommand cmd = new NpcFaceCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_Dir = m_Dir.Clone();
            cmd.m_Immediately = m_Immediately.Clone();
            return cmd;
        }
        protected override void ResetState()
        {
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_Dir.Evaluate(instance, handler, iterator, args);
            m_Immediately.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            float dir = m_Dir.Value;
            int im = 0;
            if (m_Immediately.HaveValue)
                im = m_Immediately.Value;
            EntityInfo npc = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != npc)
            {
                MovementStateInfo msi = npc.GetMovementStateInfo();
                if (im != 0) {
                    msi.SetFaceDir(dir);

                    var uobj = PluginFramework.Instance.GetGameObject(npc.GetId());
                    if (null != uobj) {
                        var e = uobj.transform.eulerAngles;
                        uobj.transform.eulerAngles = new UnityEngine.Vector3(e.x, Geometry.RadianToDegree(dir), e.z);
                    }
                } else {
                    msi.SetWantedFaceDir(dir);
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1)
            {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Dir.InitFromDsl(callData.GetParam(1));
                if (num > 2)
                    m_Immediately.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<float> m_Dir = new StoryValue<float>();
        private IStoryFunction<int> m_Immediately = new StoryValue<int>();
    }
    /// <summary>
    /// npcmove(npc_unit_id,vector3(x,y,z)[,event]);
    /// </summary>
    internal class NpcMoveCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcMoveCommand cmd = new NpcMoveCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_Pos = m_Pos.Clone();
            cmd.m_Event = m_Event.Clone();
            return cmd;
        }
        protected override void ResetState()
        {
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_Pos.Evaluate(instance, handler, iterator, args);
            m_Event.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            Vector3 pos = m_Pos.Value;
            string eventName = m_Event.Value;
            EntityInfo entity = PluginFramework.Instance.GetEntityByUnitId(unitId);
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
                data.Event = eventName;
                entity.GetMovementStateInfo().TargetPosition = pos;
                aiInfo.Time = 1000;//Movement is triggered on the next frame
                aiInfo.ChangeToState((int)PredefinedAiStateId.MoveCommand);
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Pos.InitFromDsl(callData.GetParam(1));
                if (num > 2)
                    m_Event.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<Vector3> m_Pos = new StoryValue<Vector3>();
        private IStoryFunction<string> m_Event = new StoryValue<string>();
    }
    /// <summary>
    /// npcmovewithwaypoints(npc_unit_id,vector3list("1 2 3 4 5 6")[,event]);
    /// </summary>
    internal class NpcMoveWithWaypointsCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcMoveWithWaypointsCommand cmd = new NpcMoveWithWaypointsCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_WayPoints = m_WayPoints.Clone();
            cmd.m_Event = m_Event.Clone();
            return cmd;
        }
        protected override void ResetState()
        {
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_WayPoints.Evaluate(instance, handler, iterator, args);
            m_Event.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            List<object> poses = m_WayPoints.Value;
            string eventName = m_Event.Value;
            EntityInfo entity = PluginFramework.Instance.GetEntityByUnitId(unitId);
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
                data.Event = eventName;
                entity.GetMovementStateInfo().TargetPosition = waypoints[0];
                aiInfo.Time = 1000;//Movement is triggered on the next frame
                aiInfo.ChangeToState((int)PredefinedAiStateId.MoveCommand);
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_WayPoints.InitFromDsl(callData.GetParam(1));
                if (num > 2)
                    m_Event.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<List<object>> m_WayPoints = new StoryValue<List<object>>();
        private IStoryFunction<string> m_Event = new StoryValue<string>();
    }
    /// <summary>
    /// npcstop(npc_unit_id);
    /// </summary>
    internal class NpcStopCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcStopCommand cmd = new NpcStopCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            EntityInfo entity = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != entity) {
                AiStateInfo aiInfo = entity.GetAiStateInfo();
                if (aiInfo.CurState == (int)PredefinedAiStateId.MoveCommand) {
                    aiInfo.Time = 0;
                    aiInfo.Target = 0;
                }
                entity.GetMovementStateInfo().IsMoving = false;
                if (aiInfo.CurState > (int)PredefinedAiStateId.Invalid)
                    aiInfo.ChangeToState((int)PredefinedAiStateId.Idle);
            }
            EntityViewModel viewModel = EntityController.Instance.GetEntityViewByUnitId(unitId);
            if (null != viewModel) {
                viewModel.StopMove();
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
    }
    /// <summary>
    /// npcattack(npc_unit_id[,target_unit_id]);
    /// </summary>
    internal class NpcAttackCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
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

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_TargetUnitId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            EntityInfo entity = PluginFramework.Instance.GetEntityByUnitId(unitId);
            EntityInfo target = null;
            int targetUnitId = m_TargetUnitId.Value;
            target = PluginFramework.Instance.GetEntityByUnitId(targetUnitId);
            if (null != entity && null != target) {
                AiStateInfo aiInfo = entity.GetAiStateInfo();
                aiInfo.Target = target.GetId();
                aiInfo.LastChangeTargetTime = TimeUtility.GetLocalMilliseconds();
                aiInfo.ChangeToState((int)PredefinedAiStateId.Idle);
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            m_ParamNum = callData.GetParamNum();
            if (m_ParamNum > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_TargetUnitId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private int m_ParamNum = 0;
        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<int> m_TargetUnitId = new StoryValue<int>();
    }
    /// <summary>
    /// npcsetformation(npc_unit_id,index);
    /// </summary>
    internal class NpcSetFormationCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcSetFormationCommand cmd = new NpcSetFormationCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_FormationIndex = m_FormationIndex.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_FormationIndex.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(m_UnitId.Value);
            if (null != obj) {
                obj.GetMovementStateInfo().FormationIndex = m_FormationIndex.Value;
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_FormationIndex.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<int> m_FormationIndex = new StoryValue<int>();
    }
    /// <summary>
    /// npcenableai(npc_unit_id,1_or_0);
    /// </summary>
    internal class NpcEnableAiCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcEnableAiCommand cmd = new NpcEnableAiCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_Enable = m_Enable.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_Enable.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(m_UnitId.Value);
            if (null != obj) {
                obj.SetAIEnable(m_Enable.Value != 0);
            }
            EntityViewModel viewModel = EntityController.Instance.GetEntityViewByUnitId(m_UnitId.Value);
            if (null != viewModel) {
                viewModel.StopMove();
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Enable.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<int> m_Enable = new StoryValue<int>();
    }
    /// <summary>
    /// npcsetai(unitid,ai_logic_id,stringlist("param1 param2 param3 ..."));
    /// </summary>
    internal class NpcSetAiCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcSetAiCommand cmd = new NpcSetAiCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_AiLogic = m_AiLogic.Clone();
            cmd.m_AiParams = m_AiParams.Clone();
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_AiLogic.Evaluate(instance, handler, iterator, args);
            m_AiParams.Evaluate(instance, handler, iterator, args);

        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            string aiLogic = m_AiLogic.Value;
            IEnumerable aiParams = m_AiParams.Value;
            EntityInfo charObj = PluginFramework.Instance.GetEntityByUnitId(unitId);
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

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_AiLogic.InitFromDsl(callData.GetParam(1));
                m_AiParams.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<string> m_AiLogic = new StoryValue<string>();
        private IStoryFunction<IEnumerable> m_AiParams = new StoryValue<IEnumerable>();
    }
    /// <summary>
    /// npcsetaitarget(unitid,targetId);
    /// </summary>
    internal class NpcSetAiTargetCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcSetAiTargetCommand cmd = new NpcSetAiTargetCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_TargetId = m_TargetId.Clone();
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_TargetId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            int targetId = m_TargetId.Value;
            EntityInfo charObj = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != charObj) {
                charObj.GetAiStateInfo().Target = targetId;
                charObj.GetAiStateInfo().HateTarget = targetId;
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_TargetId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<int> m_TargetId = new StoryValue<int>();
    }
    /// <summary>
    /// npcanimation(unit_id, anim[, normalized_time]);
    /// </summary>
    internal class NpcAnimationCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
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

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_Anim.Evaluate(instance, handler, iterator, args);
            if (m_ParamNum > 2) {
                m_Time.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            string anim = m_Anim.Value;
            UnityEngine.GameObject obj = EntityController.Instance.GetGameObjectByUnitId(unitId);
            if (null != obj) {
                UnityEngine.Animator animator = obj.GetComponentInChildren<UnityEngine.Animator>();
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

        protected override bool Load(Dsl.FunctionData callData)
        {
            m_ParamNum = callData.GetParamNum();
            if (m_ParamNum > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Anim.InitFromDsl(callData.GetParam(1));
            }
            if (m_ParamNum > 2) {
                m_Time.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }

        private int m_ParamNum = 0;
        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<string> m_Anim = new StoryValue<string>();
        private IStoryFunction<float> m_Time = new StoryValue<float>();
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
        protected override IStoryCommand CloneCommand()
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

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Params.Count; ++i) {
                var pair = m_Params[i];
                pair.Key.Evaluate(instance, handler, iterator, args);
                pair.Value.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            UnityEngine.GameObject obj = EntityController.Instance.GetGameObjectByUnitId(unitId);
            if (null != obj) {
                UnityEngine.Animator animator = obj.GetComponentInChildren<UnityEngine.Animator>();
                if (null != animator) {
                    for (int i = 0; i < m_Params.Count; ++i) {
                        var param = m_Params[i];
                        string type = param.Type;
                        string key = param.Key.Value;
                        var val = param.Value.Value;
                        if (type == "int") {
                            int v = val.GetInt();
                            animator.SetInteger(key, v);
                        } else if (type == "float") {
                            float v = val.GetFloat();
                            animator.SetFloat(key, v);
                        } else if (type == "bool") {
                            bool v = val.GetBool();
                            animator.SetBool(key, v);
                        } else if (type == "trigger") {
                            string v = val.ToString();
                            if (v == "false") {
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

        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.IsHighOrder) {
                LoadCall(funcData.LowerOrderFunction);
            }
            else if (funcData.HaveParam()) {
                LoadCall(funcData);
            }
            if (funcData.HaveStatement()) {
                for (int i = 0; i < funcData.GetParamNum(); ++i) {
                    Dsl.ISyntaxComponent statement = funcData.GetParam(i);
                    Dsl.FunctionData stCall = statement as Dsl.FunctionData;
                    if (null != stCall && stCall.GetParamNum() >= 2) {
                        string id = stCall.GetId();
                        ParamInfo param = new ParamInfo(id, stCall.GetParam(0), stCall.GetParam(1));
                        m_Params.Add(param);
                    }
                }
            }
            return true;
        }
        private void LoadCall(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
        }

        private class ParamInfo
        {
            internal string Type;
            internal IStoryFunction<string> Key;
            internal IStoryFunction Value;

            internal ParamInfo()
            {
                Init();
            }
            internal ParamInfo(string type, Dsl.ISyntaxComponent keyDsl, Dsl.ISyntaxComponent valDsl)
                : this()
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

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private List<ParamInfo> m_Params = new List<ParamInfo>();
    }
    /// <summary>
    /// npcaddimpact(unit_id, impactid, arg1, arg2, ...)[seq("@seq")];
    /// </summary>
    internal class NpcAddImpactCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcAddImpactCommand cmd = new NpcAddImpactCommand();
            cmd.m_UnitId = m_UnitId.Clone();
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
            m_UnitId.Evaluate(instance, handler, iterator, args);
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
            int unitId = m_UnitId.Value;
            int impactId = m_ImpactId.Value;
            int seq = 0;
            Dictionary<string, object> locals = new Dictionary<string, object>();
            for (int i = 0; i < m_Args.Count - 1; i += 2) {
                string key = m_Args[i].Value;
                object val = m_Args[i + 1].Value.GetObject();
                if (!string.IsNullOrEmpty(key)) {
                    locals.Add(key, val);
                }
            }
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
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

        protected override bool Load(Dsl.FunctionData callData)
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

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<int> m_ImpactId = new StoryValue<int>();
        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
        private bool m_HaveSeq = false;
        private IStoryFunction<string> m_SeqVarName = new StoryValue<string>();
    }
    /// <summary>
    /// npcremoveimpact(unit_id, seq);
    /// </summary>
    internal class NpcRemoveImpactCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcRemoveImpactCommand cmd = new NpcRemoveImpactCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_Seq = m_Seq.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_Seq.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            int seq = m_Seq.Value;
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                ImpactInfo impactInfo = obj.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                if (null != impactInfo) {
                    GfxSkillSystem.Instance.StopSkill(obj.GetId(), impactInfo.ImpactId, seq, true);
                    obj.GetSkillStateInfo().RemoveImpact(seq);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Seq.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<int> m_Seq = new StoryValue<int>();
    }
    /// <summary>
    /// npccastskill(unit_id, skillid, arg1, arg2, ...);
    /// </summary>
    internal class NpcCastSkillCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcCastSkillCommand cmd = new NpcCastSkillCommand();
            cmd.m_UnitId = m_UnitId.Clone();
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
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_SkillId.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            int skillId = m_SkillId.Value;
            Dictionary<string, object> locals = new Dictionary<string, object>();
            for (int i = 0; i < m_Args.Count - 1; i += 2) {
                string key = m_Args[i].Value;
                object val = m_Args[i + 1].Value.GetObject();
                if (!string.IsNullOrEmpty(key)) {
                    locals.Add(key, val);
                }
            }
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                SkillInfo skillInfo = obj.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null != skillInfo) {
                    GfxSkillSystem.Instance.StartSkill(obj.GetId(), skillInfo.ConfigData, 0, locals);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
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
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<int> m_SkillId = new StoryValue<int>();
        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
    }
    /// <summary>
    /// npcstopskill(unit_id);
    /// </summary>
    internal class NpcStopSkillCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcStopSkillCommand cmd = new NpcStopSkillCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                GfxSkillSystem.Instance.StopAllSkill(obj.GetId(), true);
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<int> m_SkillId = new StoryValue<int>();
    }
    /// <summary>
    /// npclisten(unit_id, message_type, true_or_false);
    /// </summary>
    internal class NpcListenCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
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

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_Event.Evaluate(instance, handler, iterator, args);
            m_Enable.Evaluate(instance, handler, iterator, args);

        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            string eventName = m_Event.Value;
            string enable = m_Enable.Value;
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                if (StoryListenFlagEnum.Damage == StoryListenFlagUtility.FromString(eventName)) {
                    if (0 == string.Compare(enable, "true"))
                        obj.AddStoryFlag(StoryListenFlagEnum.Damage);
                    else
                        obj.RemoveStoryFlag(StoryListenFlagEnum.Damage);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_Event.InitFromDsl(callData.GetParam(1));
                m_Enable.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<string> m_Event = new StoryValue<string>();
        private IStoryFunction<string> m_Enable = new StoryValue<string>();
    }
    /// <summary>
    /// setcamp(npc_unit_id,camp_id);
    /// </summary>
    internal class NpcSetCampCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcSetCampCommand cmd = new NpcSetCampCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_CampId = m_CampId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_CampId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(m_UnitId.Value);
            if (null != obj) {
                int campId = m_CampId.Value;
                obj.SetCampId(campId);
                Utility.EventSystem.Publish("ui_actor_color", "ui", obj.GetId(), CharacterRelation.RELATION_FRIEND == EntityInfo.GetRelation(PluginFramework.Instance.CampId, campId));
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_CampId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<int> m_CampId = new StoryValue<int>();
    }
    /// setsummonerid(unit_id, objid);
    /// </summary>
    internal class NpcSetSummonerIdCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcSetSummonerIdCommand cmd = new NpcSetSummonerIdCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_SummonerId = m_SummonerId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_SummonerId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            int summonerId = m_SummonerId.Value;
            EntityInfo npcInfo = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != npcInfo) {
                npcInfo.SummonerId = summonerId;
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_SummonerId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<int> m_SummonerId = new StoryValue<int>();
    }
    /// setsummonskillid(unit_id, objid);
    /// </summary>
    internal class NpcSetSummonSkillIdCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            NpcSetSummonSkillIdCommand cmd = new NpcSetSummonSkillIdCommand();
            cmd.m_UnitId = m_UnitId.Clone();
            cmd.m_SummonSkillId = m_SummonSkillId.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UnitId.Evaluate(instance, handler, iterator, args);
            m_SummonSkillId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            int unitId = m_UnitId.Value;
            int summonSkillId = m_SummonSkillId.Value;
            EntityInfo npcInfo = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != npcInfo) {
                npcInfo.SummonSkillId = summonSkillId;
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId.InitFromDsl(callData.GetParam(0));
                m_SummonSkillId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_UnitId = new StoryValue<int>();
        private IStoryFunction<int> m_SummonSkillId = new StoryValue<int>();
    }
    /// <summary>
}
