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
    /// createnpc(npc_unit_id,vector3(x,y,z),dir,camp,tableId[,ai,stringlist("param1 param2 param3 ..."),leaderId])[objid("@objid")];
    /// </summary>
    public class CreateNpcCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = 0;
                if (m_ParamNum >= 5) {
                    int unitId = m_UnitId.Calc().GetInt();
                    Vector3Obj posObj = m_Pos.Calc();
                    Vector3 pos = posObj;
                    float dir = m_Dir.Calc().GetFloat();
                    int camp = m_Camp.Calc().GetInt();
                    int tableId = m_TableId.Calc().GetInt();

                    if (m_ParamNum > 6) {
                        string aiLogic = m_AiLogic.Calc().ToString();
                        List<string> aiParams = new List<string>();
                        IEnumerable aiParamEnumer = m_AiParams.Calc().GetObject() as IEnumerable;
                        if (aiParamEnumer is List<BoxedValue> bvAiParamEnumer) {
                            foreach (var aiParam in bvAiParamEnumer) {
                                aiParams.Add(aiParam.GetString());
                            }
                        } else {
                            foreach (string aiParam in aiParamEnumer) {
                                aiParams.Add(aiParam);
                            }
                        }
                        objId = scene.CreateEntity(unitId, pos.X, pos.Y, pos.Z, dir, camp, tableId, aiLogic, aiParams.ToArray());
                    } else {
                        objId = scene.CreateEntity(unitId, pos.X, pos.Y, pos.Z, dir, camp, tableId);
                    }
                    if (m_ParamNum > 6) {
                        EntityInfo charObj = scene.GetEntityById(objId);
                        if (null != charObj) {
                            if (m_ParamNum > 7) {
                                int leaderId = m_LeaderId.Calc().GetInt();
                                charObj.GetAiStateInfo().LeaderId = leaderId;
                            } else {
                                charObj.GetAiStateInfo().LeaderId = 0;
                            }
                        }
                    }
                    EntityInfo obj = scene.GetEntityById(objId);
                    if (null != obj) {
                        Msg_RC_CreateNpc msg = DataSyncUtility.BuildCreateNpcMessage(obj);
                        scene.NotifyAllUser(RoomMessageDefine.Msg_RC_CreateNpc, msg);
                    }
                }
                if (m_HaveObjId) {
                    string varName = m_ObjIdVarName.Calc().ToString();
                    instance.SetVariable(varName, objId);
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            m_ParamNum = callData.GetParamNum();
            if (m_ParamNum >= 5) {
                m_UnitId = Calculator.Load(callData.GetParam(0));
                m_Pos = Calculator.Load(callData.GetParam(1));
                m_Dir = Calculator.Load(callData.GetParam(2));
                m_Camp = Calculator.Load(callData.GetParam(3));
                m_TableId = Calculator.Load(callData.GetParam(4));

                if (m_ParamNum > 6) {
                    m_AiLogic = Calculator.Load(callData.GetParam(5));
                    m_AiParams = Calculator.Load(callData.GetParam(6));
                    if (m_ParamNum > 7) {
                        m_LeaderId = Calculator.Load(callData.GetParam(7));
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
                m_ObjIdVarName = Calculator.Load(callData.GetParam(0));
                m_HaveObjId = true;
            }
        }
        private IExpression m_UnitId;
        private int m_ParamNum = 0;
        private IExpression m_Pos;
        private IExpression m_Dir;
        private IExpression m_Camp;
        private IExpression m_TableId;
        private IExpression m_AiLogic;
        private IExpression m_AiParams;
        private IExpression m_LeaderId;
        private bool m_HaveObjId = false;
        private IExpression m_ObjIdVarName;
    }
    /// <summary>
    /// destroynpc(npc_unit_id);
    /// </summary>
    public class DestroyNpcCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 0)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != entity) {
                    entity.NeedDelete = true;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// destroynpcwithobjid(npc_obj_id);
    /// </summary>
    public class DestroyNpcWithObjIdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 0)
                    return BoxedValue.NullObject;
                int objid = operands[0].GetInt();
                EntityInfo entity = scene.SceneContext.GetEntityById(objid);
                if (null != entity) {
                    entity.NeedDelete = true;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcface(npc_unit_id,dir);
    /// </summary>
    public class NpcFaceCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                float dir = operands[1].GetFloat();
                EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != entity) {
                    MovementStateInfo msi = entity.GetMovementStateInfo();
                    msi.SetFaceDir(dir);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcmove(npc_unit_id,vector3(x,y,z));
    /// </summary>
    public class NpcMoveCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                Vector3Obj posObj = operands[1];
                Vector3 pos = posObj;
                EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
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
                    aiInfo.Time = 1000;//Movement is triggered on the next frame
                    aiInfo.ChangeToState((int)PredefinedAiStateId.MoveCommand);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcmovewithwaypoints(npc_unit_id,vector3list("1 2 3 4 5 6"));
    /// </summary>
    public class NpcMoveWithWaypointsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                List<object> poses = operands[1].GetObject() as List<object>;
                EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
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
                    aiInfo.Time = 1000;//Movement is triggered on the next frame
                    aiInfo.ChangeToState((int)PredefinedAiStateId.MoveCommand);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcstop(npc_unit_id);
    /// </summary>
    public class NpcStopCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 0)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
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
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcattack(npc_unit_id[,target_unit_id]);
    /// </summary>
    public class NpcAttackCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 0)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                EntityInfo entity = scene.SceneContext.GetEntityByUnitId(unitId);
                int targetId = 0;
                if (operands.Count > 1) {
                    targetId = operands[1].GetInt();
                } else {
                    targetId = entity.GetAiStateInfo().Target;
                }
                EntityInfo target = scene.SceneContext.GetEntityByUnitId(targetId);
                if (null != entity && null != target) {
                    AiStateInfo aiInfo = entity.GetAiStateInfo();
                    aiInfo.Target = target.GetId();
                    aiInfo.LastChangeTargetTime = TimeUtility.GetLocalMilliseconds();
                    aiInfo.ChangeToState((int)PredefinedAiStateId.Idle);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcsetformation(npc_unit_id,index);
    /// </summary>
    public class NpcSetFormationCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != obj) {
                    obj.GetMovementStateInfo().FormationIndex = operands[1].GetInt();
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcenableai(npc_unit_id,1_or_0);
    /// </summary>
    public class NpcEnableAiCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != obj) {
                    obj.SetAIEnable(operands[1].GetInt() != 0);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcsetai(unitid,ai_logic_id,stringlist("param1 param2 param3 ..."));
    /// </summary>
    public class NpcSetAiCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 2)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                string aiLogic = operands[1].ToString();
                IEnumerable aiParams = operands[2].GetObject() as IEnumerable;
                EntityInfo charObj = scene.SceneContext.GetEntityByUnitId(unitId);
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcsetaitarget(unitid,targetId);
    /// </summary>
    public class NpcSetAiTargetCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count < 2)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                int targetId = operands[1].GetInt();
                EntityInfo charObj = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != charObj) {
                    charObj.GetAiStateInfo().Target = targetId;
                    charObj.GetAiStateInfo().HateTarget = targetId;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcanimation(unit_id, anim);
    /// </summary>
    public class NpcAnimationCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                string anim = operands[1].ToString();
                EntityInfo npcInfo = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != npcInfo) {
                    int objId = npcInfo.GetId();
                    EntityInfo obj = scene.EntityController.GetGameObject(objId);
                    if (null != obj) {
                        Msg_RC_PlayAnimation msg = new Msg_RC_PlayAnimation();
                        msg.obj_id = objId;
                        msg.anim_name = anim;
                        scene.NotifyAllUser(RoomMessageDefine.Msg_RC_PlayAnimation, msg);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcaddimpact(unit_id, impactid, arg1, arg2, ...)[seq("@seq")];
    /// </summary>
    public class NpcAddImpactCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int unitId = m_UnitId.Calc().GetInt();
                int impactId = m_ImpactId.Calc().GetInt();
                int seq = 0;
                Dictionary<string, object> locals = new Dictionary<string, object>();
                for (int i = 0; i < m_Args.Count - 1; i += 2) {
                    string key = m_Args[i].Calc().ToString();
                    object val = m_Args[i + 1].Calc().GetObject();
                    if (!string.IsNullOrEmpty(key)) {
                        locals.Add(key, val);
                    }
                }
                EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != obj) {
                    ImpactInfo impactInfo = new ImpactInfo(impactId);
                    impactInfo.StartTime = TimeUtility.GetLocalMilliseconds();
                    impactInfo.ImpactSenderId = obj.GetId();
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
                    string varName = m_SeqVarName.Calc().ToString();
                    instance.SetVariable(varName, seq);
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId = Calculator.Load(callData.GetParam(0));
                m_ImpactId = Calculator.Load(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                m_Args.Add(Calculator.Load(callData.GetParam(i)));
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
                m_SeqVarName = Calculator.Load(callData.GetParam(0));
                m_HaveSeq = true;
            }
        }
        private IExpression m_UnitId;
        private IExpression m_ImpactId;
        private List<IExpression> m_Args = new List<IExpression>();
        private bool m_HaveSeq = false;
        private IExpression m_SeqVarName;
    }
    /// <summary>
    /// npcremoveimpact(unit_id, seq);
    /// </summary>
    public class NpcRemoveImpactCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                int seq = operands[1].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npccastskill(unit_id, skillid, arg1, arg2, ...);
    /// </summary>
    public class NpcCastSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                int skillId = operands[1].GetInt();
                Dictionary<string, object> locals = new Dictionary<string, object>();
                for (int i = 3; i < operands.Count; i += 2) {
                    string key = operands[i - 1].ToString();
                    object val = operands[i].GetObject();
                    if (!string.IsNullOrEmpty(key)) {
                        locals.Add(key, val);
                    }
                }
                EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcstopskill(unit_id);
    /// </summary>
    public class NpcStopSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 0)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != obj) {
                    Msg_RC_NpcStopSkill skillBuilder = new Msg_RC_NpcStopSkill();
                    skillBuilder.npc_id = obj.GetId();
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcStopSkill, skillBuilder);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcaddskill(unit_id, skillid);
    /// </summary>
    public class NpcAddSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                int skillId = operands[1].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcremoveskill(unit_id, skillid);
    /// </summary>
    public class NpcRemoveSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                int skillId = operands[1].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != obj) {
                    obj.GetSkillStateInfo().RemoveSkill(skillId);

                    Msg_RC_RemoveSkill msg = new Msg_RC_RemoveSkill();
                    msg.obj_id = obj.GetId();
                    msg.skill_id = skillId;
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_RemoveSkill, msg);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npclisten(unit_id, message_type, true_or_false);
    /// </summary>
    public class NpcListenCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 2)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                string eventName = operands[1].ToString();
                string enable = operands[2].ToString();
                EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != obj) {
                    if (StoryListenFlagEnum.Damage == StoryListenFlagUtility.FromString(eventName)) {
                        if (0 == string.Compare(enable, "true"))
                            obj.AddStoryFlag(StoryListenFlagEnum.Damage);
                        else
                            obj.RemoveStoryFlag(StoryListenFlagEnum.Damage);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcsetcamp(npc_unit_id,camp_id);
    /// </summary>
    public class NpcSetCampCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                int campId = operands[1].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != obj) {
                    obj.SetCampId(campId);

                    Msg_RC_CampChanged msg = new Msg_RC_CampChanged();
                    msg.obj_id = obj.GetId();
                    msg.camp_id = campId;
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_CampChanged, msg);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcsetsummonerid(unit_id, objid);
    /// </summary>
    public class NpcSetSummonerIdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                int summonerId = operands[1].GetInt();
                EntityInfo npcInfo = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != npcInfo) {
                    npcInfo.SummonerId = summonerId;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npcsetsummonskillid(unit_id, skillid);
    /// </summary>
    public class NpcSetSummonSkillIdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int unitId = operands[0].GetInt();
                int summonSkillId = operands[1].GetInt();
                EntityInfo npcInfo = scene.SceneContext.GetEntityByUnitId(unitId);
                if (null != npcInfo) {
                    npcInfo.SummonSkillId = summonSkillId;
                }
            }
            return BoxedValue.NullObject;
        }
    }
}
