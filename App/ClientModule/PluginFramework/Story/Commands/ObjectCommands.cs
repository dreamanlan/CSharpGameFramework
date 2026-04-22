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
    /// objface(obj_id, dir[, immediately]);
    /// </summary>
    internal class ObjFaceCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            float dir = operands[1].GetFloat();
            int im = 0;
            if (operands.Count > 2)
                im = operands[2].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                MovementStateInfo msi = obj.GetMovementStateInfo();
                if (im != 0) {
                    msi.SetFaceDir(dir);
                    var uobj = PluginFramework.Instance.GetGameObject(objId);
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
    /// objmove(obj_id, vector3(x,y,z)[, event]);
    /// </summary>
    internal class ObjMoveCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            Vector3 pos = operands[1].As<Vector3Obj>();
            string eventName = operands.Count > 2 ? operands[2].GetString() : string.Empty;
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
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
                data.Event = eventName;
                obj.GetMovementStateInfo().TargetPosition = pos;
                aiInfo.Time = 1000;//Movement is triggered on the next frame
                aiInfo.ChangeToState((int)PredefinedAiStateId.MoveCommand);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objmovewithwaypoints(obj_id, vector3list("1 2 3 4 5 6")[, event]);
    /// </summary>
    internal class ObjMoveWithWaypointsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            List<object> poses = operands[1].As<List<object>>();
            string eventName = operands.Count > 2 ? operands[2].GetString() : string.Empty;
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
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
                data.Event = eventName;
                obj.GetMovementStateInfo().TargetPosition = waypoints[0];
                aiInfo.Time = 1000;//Movement is triggered on the next frame
                aiInfo.ChangeToState((int)PredefinedAiStateId.MoveCommand);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objstop(obj_id);
    /// </summary>
    internal class ObjStopCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
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
            EntityViewModel viewModel = EntityController.Instance.GetEntityViewById(objId);
            if (null != viewModel) {
                viewModel.StopMove();
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objattack(npc_obj_id[,target_obj_id]);
    /// </summary>
    internal class ObjAttackCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            EntityInfo entity = PluginFramework.Instance.GetEntityById(objId);
            EntityInfo target = null;
            int targetObjId = operands[1].GetInt();
            if (null != entity && null != target) {
                AiStateInfo aiInfo = entity.GetAiStateInfo();
                aiInfo.Target = targetObjId;
                aiInfo.LastChangeTargetTime = TimeUtility.GetLocalMilliseconds();
                aiInfo.ChangeToState((int)PredefinedAiStateId.Idle);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objsetformation(obj_id, index);
    /// </summary>
    internal class ObjSetFormationCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                obj.GetMovementStateInfo().FormationIndex = operands[1].GetInt();
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objenableai(obj_id, 1_or_0);
    /// </summary>
    internal class ObjEnableAiCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                obj.SetAIEnable(operands[1].GetInt() != 0);
            }
            EntityViewModel viewModel = EntityController.Instance.GetEntityViewById(objId);
            if (null != viewModel) {
                viewModel.StopMove();
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objsetai(objid,ai_logic_id,stringlist("param1 param2 param3 ..."));
    /// </summary>
    internal class ObjSetAiCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            string aiLogic = operands[1].GetString();
            IEnumerable aiParams = operands[2].GetObject() as IEnumerable;
            EntityInfo charObj = PluginFramework.Instance.GetEntityById(objId);
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
    /// objsetaitarget(objid,targetid);
    /// </summary>
    internal class ObjSetAiTargetCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            int targetId = operands[1].GetInt();
            EntityInfo charObj = PluginFramework.Instance.GetEntityById(objId);
            if (null != charObj) {
                charObj.GetAiStateInfo().Target = targetId;
                charObj.GetAiStateInfo().HateTarget = targetId;
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objanimation(obj_id, anim[, normalized_time]);
    /// </summary>
    internal class ObjAnimationCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            string anim = operands[1].GetString();
            UnityEngine.GameObject obj = EntityController.Instance.GetGameObject(objId);
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
    /// objanimationparam(obj_id)
    /// {
    ///     float(name,val);
    ///     int(name,val);
    ///     bool(name,val);
    ///     trigger(name,val);
    /// };
    /// </summary>
    internal class ObjAnimationParamCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            int objId = m_ObjId.Calc().GetInt();
            UnityEngine.GameObject obj = EntityController.Instance.GetGameObject(objId);
            if (null != obj) {
                UnityEngine.Animator animator = obj.GetComponentInChildren<UnityEngine.Animator>();
                if (null != animator) {
                    for (int i = 0; i < m_Params.Count; ++i) {
                        var param = m_Params[i];
                        string type = param.Type;
                        string key = param.Key.Calc().GetString();
                        var val = param.Value.Calc();
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
                        IExpression keyExp = Calculator.Load(stCall.GetParam(0));
                        IExpression valExp = Calculator.Load(stCall.GetParam(1));
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
                m_ObjId = Calculator.Load(callData.GetParam(0));
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

        private IExpression m_ObjId;
        private List<ParamInfo> m_Params = new List<ParamInfo>();
    }
    /// <summary>
    /// objaddimpact(obj_id, impactid, arg1, arg2, ...)[seq("@seq")];
    /// </summary>
    internal class ObjAddImpactCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            int objId = m_ObjId.Calc().GetInt();
            int impactId = m_ImpactId.Calc().GetInt();
            int seq = 0;
            Dictionary<string, object> locals = new Dictionary<string, object>();
            for (int i = 0; i < m_Args.Count - 1; i += 2) {
                string key = m_Args[i].Calc().GetString();
                object val = m_Args[i + 1].Calc().GetObject();
                if (!string.IsNullOrEmpty(key)) {
                    locals.Add(key, val);
                }
            }
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                ImpactInfo impactInfo = new ImpactInfo(impactId);
                impactInfo.StartTime = TimeUtility.GetLocalMilliseconds();
                impactInfo.ImpactSenderId = objId;
                impactInfo.SkillId = 0;
                if (null != impactInfo.ConfigData) {
                    obj.GetSkillStateInfo().AddImpact(impactInfo);
                    seq = impactInfo.Seq;
                    GfxSkillSystem.Instance.StartSkill(objId, impactInfo.ConfigData, seq, locals);
                }
            }
            if (m_HaveSeq) {
                string varName = m_SeqVarName.Calc().GetString();
                var instance = Calculator.GetFuncContext<StoryInstance>();
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
                m_ObjId = Calculator.Load(callData.GetParam(0));
                m_ImpactId = Calculator.Load(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                IExpression val = Calculator.Load(callData.GetParam(i));
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
                m_SeqVarName = Calculator.Load(callData.GetParam(0));
                m_HaveSeq = true;
            }
        }

        private IExpression m_ObjId;
        private IExpression m_ImpactId;
        private List<IExpression> m_Args = new List<IExpression>();
        private bool m_HaveSeq = false;
        private IExpression m_SeqVarName;
    }
    /// <summary>
    /// objremoveimpact(obj_id, seq);
    /// </summary>
    internal class ObjRemoveImpactCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            int seq = operands[1].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                ImpactInfo impactInfo = obj.GetSkillStateInfo().GetImpactInfoBySeq(seq);
                if (null != impactInfo) {
                    GfxSkillSystem.Instance.StopSkill(objId, impactInfo.ImpactId, seq, true);
                    obj.GetSkillStateInfo().RemoveImpact(seq);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objcastskill(obj_id, skillid, arg1, arg2, ...);
    /// </summary>
    internal class ObjCastSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            int skillId = operands[1].GetInt();
            Dictionary<string, object> locals = new Dictionary<string, object>();
            for (int i = operands.Count - 1; i >= 3; i -= 2) {
                string key = operands[i - 1].GetString();
                object val = operands[i].GetObject();
                if (!string.IsNullOrEmpty(key)) {
                    locals.Add(key, val);
                }
            }
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                SkillInfo skillInfo = obj.GetSkillStateInfo().GetSkillInfoById(skillId);
                if (null != skillInfo) {
                    GfxSkillSystem.Instance.StartSkill(objId, skillInfo.ConfigData, 0, locals);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objstopskill(obj_id);
    /// </summary>
    internal class ObjStopSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            GfxSkillSystem.Instance.StopAllSkill(objId, true);
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objlisten(unit_id, message_type, true_or_false);
    /// </summary>
    internal class ObjListenCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            string eventName = operands[1].GetString();
            string enable = operands[2].GetString();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
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
    /// sethp(objid,value);
    /// </summary>
    internal class SetHpCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            int value = operands[1].GetInt();
            EntityInfo charObj = PluginFramework.Instance.GetEntityById(objId);
            if (null != charObj) {
                charObj.Hp = value;
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setenergy(objid,value);
    /// </summary>
    internal class SetEnergyCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            int value = operands[1].GetInt();
            EntityInfo charObj = PluginFramework.Instance.GetEntityById(objId);
            if (null != charObj) {
                charObj.Energy = value;
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objset(uniqueid,localname,value);
    /// </summary>
    internal class ObjSetCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return BoxedValue.NullObject;
            int uniqueId = operands[0].GetInt();
            string localName = operands[1].GetString();
            object value = operands[2].GetObject();
            PluginFramework.Instance.SceneContext.ObjectSet(uniqueId, localName, value);
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setlevel(objid,value);
    /// </summary>
    internal class SetLevelCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            int value = operands[1].GetInt();
            EntityInfo charObj = PluginFramework.Instance.GetEntityById(objId);
            if (null != charObj) {
                charObj.Level = value;
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setattr(objid,attrid,value);
    /// </summary>
    internal class SetAttrCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            int attrId = operands[1].GetInt();
            var value = operands[2];
            EntityInfo charObj = PluginFramework.Instance.GetEntityById(objId);
            if (null != charObj) {
                try {
                    long val = value.GetLong();
                    charObj.BaseProperty.SetLong((CharacterPropertyEnum)attrId, val);
                    charObj.ActualProperty.SetLong((CharacterPropertyEnum)attrId, val);
                } catch (Exception ex) {
                    LogSystem.Warn("setattr throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// markcontrolbystory(objid,true_or_false);
    /// </summary>
    internal class MarkControlByStoryCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            string value = operands[1].GetString();
            EntityInfo charObj = PluginFramework.Instance.GetEntityById(objId);
            if (null != charObj) {
                charObj.IsControlByStory = (0 == value.CompareTo("true"));
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setunitid(obj_id, dir);
    /// </summary>
    internal class SetUnitIdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            int unitId = operands[1].GetInt();
            EntityInfo obj = PluginFramework.Instance.GetEntityById(objId);
            if (null != obj) {
                obj.SetUnitId(unitId);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objsetcamp(objid,camp_id);
    /// </summary>
    internal class ObjSetCampCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            EntityInfo obj = PluginFramework.Instance.GetEntityById(operands[0].GetInt());
            if (null != obj) {
                int campId = operands[1].GetInt();
                obj.SetCampId(campId);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objsetsummonerid(objid, objid);
    /// </summary>
    internal class ObjSetSummonerIdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            int summonerId = operands[1].GetInt();
            EntityInfo npcInfo = PluginFramework.Instance.GetEntityById(objId);
            if (null != npcInfo) {
                npcInfo.SummonerId = summonerId;
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objsetsummonskillid(objid, objid);
    /// </summary>
    internal class ObjSetSummonSkillIdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            int summonSkillId = operands[1].GetInt();
            EntityInfo npcInfo = PluginFramework.Instance.GetEntityById(objId);
            if (null != npcInfo) {
                npcInfo.SummonSkillId = summonSkillId;
            }
            return BoxedValue.NullObject;
        }
    }
}
