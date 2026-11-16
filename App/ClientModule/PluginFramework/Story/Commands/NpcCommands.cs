using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptRuntime;
using ScriptableFramework;
using ScriptableFramework.Skill;
using ScriptableFramework.Story;

namespace ScriptableFramework.Story.Commands
{
    /// <summary>
    /// create_npc(npc_unit_id,vector_3(x,y,z),dir,camp,tableId[,ai,string_list("param1 param2 param3 ..."),leaderId])[objid("@objid")];
    /// </summary>
    internal class CreateNpcCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            var instance = calculator.GetFuncContext<StoryInstance>();
            int objId = 0;
            if (m_ParamNum >= 5) {
                int unitId = m_UnitId.Calc(calculator).GetInt();
                Vector3Obj posObj = m_Pos.Calc(calculator);
                Vector3 pos = posObj;
                float dir = m_Dir.Calc(calculator).GetFloat();
                int camp = m_Camp.Calc(calculator).GetInt();
                int tableId = m_TableId.Calc(calculator).GetInt();
                if (m_ParamNum > 6) {
                    string aiLogic = m_AiLogic.Calc(calculator).ToString();
                    List<string> aiParams = new List<string>();
                    IEnumerable aiParamEnumer = m_AiParams.Calc(calculator).GetObject() as IEnumerable;
                    if (aiParamEnumer is List<BoxedValue> bvAiParamEnumer) {
                        foreach (var aiParam in bvAiParamEnumer) {
                            aiParams.Add(aiParam.GetString());
                        }
                    } else {
                        foreach (string aiParam in aiParamEnumer) {
                            aiParams.Add(aiParam);
                        }
                    }
                    objId = PluginFramework.Instance.CreateEntity(unitId, pos.X, pos.Y, pos.Z, dir, camp, tableId, aiLogic, aiParams.ToArray());
                } else {
                    objId = PluginFramework.Instance.CreateEntity(unitId, pos.X, pos.Y, pos.Z, dir, camp, tableId);
                }
                if (m_ParamNum > 6) {
                    EntityInfo charObj = PluginFramework.Instance.GetEntityById(objId);
                    if (null != charObj) {
                        if (m_ParamNum > 7) {
                            int leaderId = m_LeaderId.Calc(calculator).GetInt();
                            charObj.GetAiStateInfo().LeaderId = leaderId;
                        } else {
                            charObj.GetAiStateInfo().LeaderId = 0;
                        }
                    }
                }
            }
            if (m_HaveObjId) {
                string varName = m_ObjIdVarName.Calc(calculator).ToString();
                if (null != instance) {
                    instance.SetVariable(varName, objId);
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            m_ParamNum = callData.GetParamNum();
            if (m_ParamNum >= 5) {
                m_UnitId = CodeStructure.Load(callData.GetParam(0));
                m_Pos = CodeStructure.Load(callData.GetParam(1));
                m_Dir = CodeStructure.Load(callData.GetParam(2));
                m_Camp = CodeStructure.Load(callData.GetParam(3));
                m_TableId = CodeStructure.Load(callData.GetParam(4));
                if (m_ParamNum > 6) {
                    m_AiLogic = CodeStructure.Load(callData.GetParam(5));
                    m_AiParams = CodeStructure.Load(callData.GetParam(6));
                    if (m_ParamNum > 7) {
                        m_LeaderId = CodeStructure.Load(callData.GetParam(7));
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
                m_ObjIdVarName = CodeStructure.Load(callData.GetParam(0));
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
    /// destroy_npc(npc_unit_id);
    /// </summary>
    internal class DestroyNpcCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
            EntityInfo entity = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != entity) {
                entity.NeedDelete = true;
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// destroy_npc_with_obj_id(npc_obj_id);
    /// </summary>
    internal class DestroyNpcWithObjIdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            int objid = operands[0].GetInt();
            EntityInfo entity = PluginFramework.Instance.GetEntityById(objid);
            if (null != entity) {
                entity.NeedDelete = true;
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npc_face(npc_unit_id, dir[, immediately]);
    /// </summary>
    internal class NpcFaceCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
            float dir = operands[1].GetFloat();
            int im = operands.Count > 2 ? operands[2].GetInt() : 0;
            EntityInfo npc = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != npc) {
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npc_move(npc_unit_id,vector_3(x,y,z)[,event]);
    /// </summary>
    internal class NpcMoveCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
            Vector3Obj posObj = operands[1];
            Vector3 pos = posObj;
            string eventName = operands.Count > 2 ? operands[2].ToString() : string.Empty;
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npc_move_with_waypoints(npc_unit_id,vector_3_list("1 2 3 4 5 6")[,event]);
    /// </summary>
    internal class NpcMoveWithWaypointsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
            List<object> poses = operands[1].GetObject() as List<object>;
            string eventName = operands.Count > 2 ? operands[2].ToString() : string.Empty;
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npc_stop(npc_unit_id);
    /// </summary>
    internal class NpcStopCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npc_attack(npc_unit_id[,target_unit_id]);
    /// </summary>
    internal class NpcAttackCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
            EntityInfo entity = PluginFramework.Instance.GetEntityByUnitId(unitId);
            EntityInfo target = null;
            int targetUnitId = operands[1].GetInt();
            target = PluginFramework.Instance.GetEntityByUnitId(targetUnitId);
            if (null != entity && null != target) {
                AiStateInfo aiInfo = entity.GetAiStateInfo();
                aiInfo.Target = target.GetId();
                aiInfo.LastChangeTargetTime = TimeUtility.GetLocalMilliseconds();
                aiInfo.ChangeToState((int)PredefinedAiStateId.Idle);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npc_set_formation(npc_unit_id,index);
    /// </summary>
    internal class NpcSetFormationCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(operands[0].GetInt());
            if (null != obj) {
                obj.GetMovementStateInfo().FormationIndex = operands[1].GetInt();
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npc_enable_ai(npc_unit_id,1_or_0);
    /// </summary>
    internal class NpcEnableAiCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                obj.SetAIEnable(operands[1].GetInt() != 0);
            }
            EntityViewModel viewModel = EntityController.Instance.GetEntityViewByUnitId(unitId);
            if (null != viewModel) {
                viewModel.StopMove();
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npc_set_ai(unitid,ai_logic_id,string_list("param1 param2 param3 ..."));
    /// </summary>
    internal class NpcSetAiCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
            string aiLogic = operands[1].ToString();
            IEnumerable aiParams = operands[2].GetObject() as IEnumerable;
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npc_set_ai_target(unitid,targetId);
    /// </summary>
    internal class NpcSetAiTargetCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
            int targetId = operands[1].GetInt();
            EntityInfo charObj = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != charObj) {
                charObj.GetAiStateInfo().Target = targetId;
                charObj.GetAiStateInfo().HateTarget = targetId;
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npc_animation(unit_id, anim[, normalized_time]);
    /// </summary>
    internal class NpcAnimationCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
            string anim = operands[1].ToString();
            UnityEngine.GameObject obj = EntityController.Instance.GetGameObjectByUnitId(unitId);
            if (null != obj) {
                UnityEngine.Animator animator = obj.GetComponentInChildren<UnityEngine.Animator>();
                if (null != animator) {
                    float time = 0;
                    if (operands.Count > 2) {
                        time = operands[2].GetFloat();
                    }
                    animator.Play(anim, -1, time);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npc_animation_param(unit_id)
    /// {
    ///     float(name,val);
    ///     int(name,val);
    ///     bool(name,val);
    ///     trigger(name,val);
    /// };
    /// </summary>
    internal class NpcAnimationParamCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            int unitId = m_UnitId.Calc(calculator).GetInt();
            UnityEngine.GameObject obj = EntityController.Instance.GetGameObjectByUnitId(unitId);
            if (null != obj) {
                UnityEngine.Animator animator = obj.GetComponentInChildren<UnityEngine.Animator>();
                if (null != animator) {
                    for (int i = 0; i < m_Params.Count; ++i) {
                        var param = m_Params[i];
                        string type = param.Type;
                        string key = param.Key.Calc(calculator).ToString();
                        var val = param.Value.Calc(calculator);
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
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.IsHighOrder) {
                LoadCall(funcData.LowerOrderFunction);
            } else if (funcData.HaveParam()) {
                LoadCall(funcData);
            }
            if (funcData.HaveStatement()) {
                for (int i = 0; i < funcData.GetParamNum(); ++i) {
                    Dsl.ISyntaxComponent statement = funcData.GetParam(i);
                    Dsl.FunctionData stCall = statement as Dsl.FunctionData;
                    if (null != stCall && stCall.GetParamNum() >= 2) {
                        string id = stCall.GetId();
                        IExpression keyExp = CodeStructure.Load(stCall.GetParam(0));
                        IExpression valExp = CodeStructure.Load(stCall.GetParam(1));
                        ParamInfo param = new ParamInfo(id, keyExp, valExp);
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
                m_UnitId = CodeStructure.Load(callData.GetParam(0));
            }
        }
        private class ParamInfo
        {
            internal string Type;
            internal IExpression Key;
            internal IExpression Value;
            internal ParamInfo(string type, IExpression key, IExpression val)
            {
                Type = type;
                Key = key;
                Value = val;
            }
        }
        private IExpression m_UnitId;
        private List<ParamInfo> m_Params = new List<ParamInfo>();
    }
    /// <summary>
    /// npc_add_impact(unit_id, impactid, arg1, arg2, ...)[seq("@seq")];
    /// </summary>
    internal class NpcAddImpactCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            var instance = calculator.GetFuncContext<StoryInstance>();
            int unitId = m_UnitId.Calc(calculator).GetInt();
            int impactId = m_ImpactId.Calc(calculator).GetInt();
            int seq = 0;
            Dictionary<string, object> locals = new Dictionary<string, object>();
            for (int i = 0; i < m_Args.Count - 1; i += 2) {
                string key = m_Args[i].Calc(calculator).ToString();
                object val = m_Args[i + 1].Calc(calculator).GetObject();
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
                string varName = m_SeqVarName.Calc(calculator).ToString();
                if (null != instance) {
                    instance.SetVariable(varName, seq);
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UnitId = CodeStructure.Load(callData.GetParam(0));
                m_ImpactId = CodeStructure.Load(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                IExpression val = CodeStructure.Load(callData.GetParam(i));
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
                m_SeqVarName = CodeStructure.Load(callData.GetParam(0));
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
    /// npc_remove_impact(unit_id, seq);
    /// </summary>
    internal class NpcRemoveImpactCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
            int seq = operands[1].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                ImpactInfo impactInfo = obj.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                if (null != impactInfo) {
                    GfxSkillSystem.Instance.StopSkill(obj.GetId(), impactInfo.ImpactId, seq, true);
                    obj.GetSkillStateInfo().RemoveImpact(seq);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npc_cast_skill(unit_id, skillid, arg1, arg2, ...);
    /// </summary>
    internal class NpcCastSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
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
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                SkillInfo skillInfo = obj.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null != skillInfo) {
                    GfxSkillSystem.Instance.StartSkill(obj.GetId(), skillInfo.ConfigData, 0, locals);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npc_stop_skill(unit_id);
    /// </summary>
    internal class NpcStopSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                GfxSkillSystem.Instance.StopAllSkill(obj.GetId(), true);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// npc_listen(unit_id, message_type, true_or_false);
    /// </summary>
    internal class NpcListenCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
            string eventName = operands[1].ToString();
            string enable = operands[2].ToString();
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                if (StoryListenFlagEnum.Damage == StoryListenFlagUtility.FromString(eventName)) {
                    if (0 == string.Compare(enable, "true"))
                        obj.AddStoryFlag(StoryListenFlagEnum.Damage);
                    else
                        obj.RemoveStoryFlag(StoryListenFlagEnum.Damage);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setcamp(npc_unit_id,camp_id);
    /// </summary>
    internal class NpcSetCampCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(operands[0].GetInt());
            if (null != obj) {
                int campId = operands[1].GetInt();
                obj.SetCampId(campId);
                Utility.EventSystem.Publish("ui_actor_color", "ui", obj.GetId(), CharacterRelation.RELATION_FRIEND == EntityInfo.GetRelation(PluginFramework.Instance.CampId, campId));
            }
            return BoxedValue.NullObject;
        }
    }
    /// setsummonerid(unit_id, objid);
    /// </summary>
    internal class NpcSetSummonerIdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
            int summonerId = operands[1].GetInt();
            EntityInfo npcInfo = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != npcInfo) {
                npcInfo.SummonerId = summonerId;
            }
            return BoxedValue.NullObject;
        }
    }
    /// setsummonskillid(unit_id, objid);
    /// </summary>
    internal class NpcSetSummonSkillIdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int unitId = operands[0].GetInt();
            int summonSkillId = operands[1].GetInt();
            EntityInfo npcInfo = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != npcInfo) {
                npcInfo.SummonSkillId = summonSkillId;
            }
            return BoxedValue.NullObject;
        }
    }
}
