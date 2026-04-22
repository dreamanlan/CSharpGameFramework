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
    /// objface(obj_id, dir);
    /// </summary>
    public class ObjFaceCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                float dir = operands[1].GetFloat();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    MovementStateInfo msi = obj.GetMovementStateInfo();
                    msi.SetFaceDir(dir);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objmove(obj_id, vector3(x,y,z));
    /// </summary>
    public class ObjMoveCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                Vector3Obj posObj = operands[1];
                Vector3 pos = posObj;
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
                    aiInfo.Time = 1000;
                    aiInfo.ChangeToState((int)PredefinedAiStateId.MoveCommand);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objmovewithwaypoints(obj_id, vector3list("1 2 3 4 5 6"));
    /// </summary>
    public class ObjMoveWithWaypointsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                List<object> poses = operands[1].GetObject() as List<object>;
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
                    aiInfo.Time = 1000;
                    aiInfo.ChangeToState((int)PredefinedAiStateId.MoveCommand);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objstop(obj_id);
    /// </summary>
    public class ObjStopCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 0)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objattack(npc_obj_id[,target_obj_id]);
    /// </summary>
    public class ObjAttackCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                EntityInfo entity = scene.SceneContext.GetEntityById(objId);
                EntityInfo target = null;
                int targetObjId = operands[1].GetInt();
                if (null != entity && null != target) {
                    AiStateInfo aiInfo = entity.GetAiStateInfo();
                    aiInfo.Target = targetObjId;
                    aiInfo.LastChangeTargetTime = TimeUtility.GetLocalMilliseconds();
                    aiInfo.ChangeToState((int)PredefinedAiStateId.Idle);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objsetformation(obj_id, index);
    /// </summary>
    public class ObjSetFormationCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    obj.GetMovementStateInfo().FormationIndex = operands[1].GetInt();
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objenableai(obj_id, 1_or_0);
    /// </summary>
    public class ObjEnableAiCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    obj.SetAIEnable(operands[1].GetInt() != 0);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objsetai(objid,ai_logic_id,stringlist("param1 param2 param3 ..."));
    /// </summary>
    public class ObjSetAiCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 2)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                string aiLogic = operands[1].GetString();
                IEnumerable aiParams = operands[2].GetObject() as IEnumerable;
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
            return BoxedValue.NullObject;
        }
    }    /// <summary>
    /// objsetaitarget(objid,targetid);
    /// </summary>
    public class ObjSetAiTargetCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count < 2)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                int targetId = operands[1].GetInt();
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    charObj.GetAiStateInfo().Target = targetId;
                    charObj.GetAiStateInfo().HateTarget = targetId;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objanimation(obj_id, anim);
    /// </summary>
    public class ObjAnimationCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                string anim = operands[1].ToString();
                EntityInfo obj = scene.EntityController.GetGameObject(objId);
                if (null != obj) {
                    Msg_RC_PlayAnimation msg = new Msg_RC_PlayAnimation();
                    msg.obj_id = objId;
                    msg.anim_name = anim;
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_PlayAnimation, msg);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objaddimpact(obj_id, impactid, arg1, arg2, ...)[seq("@seq")];
    /// </summary>
    public class ObjAddImpactCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Calc().GetInt();
                int impactId = m_ImpactId.Calc().GetInt();
                int seq = 0;
                Dictionary<string, object> locals = new Dictionary<string, object>();
                for (int i = 0; i < m_Args.Count - 1; i += 2) {
                    string key = m_Args[i].Calc().ToString();
                    object val = m_Args[i + 1].Calc().GetInt();
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
                m_ObjId = Calculator.Load(callData.GetParam(0));
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
        private IExpression m_ObjId;
        private IExpression m_ImpactId;
        private List<IExpression> m_Args = new List<IExpression>();
        private bool m_HaveSeq = false;
        private IExpression m_SeqVarName;
    }
    /// <summary>
    /// objremoveimpact(obj_id, seq);
    /// </summary>
    public class ObjRemoveImpactCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                int seq = operands[1].GetInt();
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objcastskill(obj_id, skillid, arg1, arg2, ...);
    /// </summary>
    public class ObjCastSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                int skillId = operands[1].GetInt();
                Dictionary<string, object> locals = new Dictionary<string, object>();
                for (int i = operands.Count - 1; i >= 3; i -= 2) {
                    string key = operands[i - 1].ToString();
                    object val = operands[i].GetInt();
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objstopskill(obj_id);
    /// </summary>
    public class ObjStopSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 0)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                Msg_RC_NpcStopSkill skillBuilder = new Msg_RC_NpcStopSkill();
                skillBuilder.npc_id = objId;
                scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcStopSkill, skillBuilder);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objaddskill(obj_id, skillid);
    /// </summary>
    public class ObjAddSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                int skillId = operands[1].GetInt();
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objremoveskill(obj_id, skillid);
    /// </summary>
    public class ObjRemoveSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                int skillId = operands[1].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
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
    /// objlisten(unit_id, message_type, true_or_false);
    /// </summary>
    public class ObjListenCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 2)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                string eventName = operands[1].ToString();
                string enable = operands[2].ToString();
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// sethp(objid,value);
    /// </summary>
    public class SetHpCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                int value = operands[1].GetInt();
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    charObj.Hp = value;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setenergy(objid,value);
    /// </summary>
    public class SetEnergyCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                int value = operands[1].GetInt();
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    charObj.Energy = value;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objset(uniqueid,localname,value);
    /// </summary>
    public class ObjSetCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 2)
                    return BoxedValue.NullObject;
                int uniqueId = operands[0].GetInt();
                string localName = operands[1].ToString();
                object value = operands[2].GetInt();
                scene.SceneContext.ObjectSet(uniqueId, localName, value);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setlevel(objid,value);
    /// </summary>
    public class SetLevelCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                int value = operands[1].GetInt();
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    charObj.Level = value;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setattr(objid,attrname,value);
    /// </summary>
    public class SetAttrCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 2)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                string attrName = operands[1].ToString();
                var value = operands[2];
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    try {
                        charObj.LevelChanged = true;
                    } catch (Exception ex) {
                        LogSystem.Warn("setattr throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// markcontrolbystory(objid,true_or_false);
    /// </summary>
    public class MarkControlByStoryCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count < 2)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                string value = operands[1].ToString();
                EntityInfo charObj = scene.SceneContext.GetEntityById(objId);
                if (null != charObj) {
                    charObj.IsControlByStory = (0 == value.CompareTo("true"));
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// setunitid(obj_id, unit_id);
    /// </summary>
    public class SetUnitIdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                int unitId = operands[1].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    obj.SetUnitId(unitId);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objsetcamp(objid,camp_id);
    /// </summary>
    public class ObjSetCampCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                EntityInfo obj = scene.SceneContext.GetEntityById(operands[0].GetInt());
                if (null != obj) {
                    int campId = operands[1].GetInt();
                    obj.SetCampId(campId);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objsetsummonerid(objid, summonerid);
    /// </summary>
    public class ObjSetSummonerIdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                int summonerId = operands[1].GetInt();
                EntityInfo npcInfo = scene.SceneContext.GetEntityById(objId);
                if (null != npcInfo) {
                    npcInfo.SummonerId = summonerId;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// objsetsummonskillid(objid, summonskillid);
    /// </summary>
    public class ObjSetSummonSkillIdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count <= 1)
                    return BoxedValue.NullObject;
                int objId = operands[0].GetInt();
                int summonSkillId = operands[1].GetInt();
                EntityInfo npcInfo = scene.SceneContext.GetEntityById(objId);
                if (null != npcInfo) {
                    npcInfo.SummonSkillId = summonSkillId;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// Dummy command that does nothing (used for server-side no-op commands like setvisible).
    /// Accepts any dsl syntax so it can be used as a placeholder for unimplemented apis.
    /// </summary>
    public class DummyCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.ValueData valData) { return true; }
        protected override bool Load(Dsl.FunctionData funcData) { return true; }
        protected override bool Load(Dsl.StatementData statementData) { return true; }
    }
}
