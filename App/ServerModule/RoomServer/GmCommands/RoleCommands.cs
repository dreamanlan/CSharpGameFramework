using System;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFrameworkMessage;

namespace ScriptableFramework.GmCommands
{
    //---------------------------------------------------------------------------------------------------------------------------------
    //********************************************************delimiter*******************************************************************
    //---------------------------------------------------------------------------------------------------------------------------------
    //Commands that are only valid within the scene (only modify ScriptableFramework combat-related data)
    public class SetPositionCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue us;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            if (null != instance && instance.ContextVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    int objId = operands[0].GetInt();
                    float x = operands[1].GetFloat();
                    float z = operands[2].GetFloat();
                    EntityInfo charObj = user.SceneContext.GetEntityById(objId);
                    if (null != charObj) {
                        MovementStateInfo msi = charObj.GetMovementStateInfo();
                        msi.SetPosition2D(x, z);

                        Scene scene = user.SceneContext.CustomData as Scene;
                        if (null != scene) {
                            ScriptableFrameworkMessage.Msg_RC_AdjustPosition adjustPos = new ScriptableFrameworkMessage.Msg_RC_AdjustPosition();
                            adjustPos.role_id = objId;
                            adjustPos.face_dir = msi.GetFaceDir();
                            adjustPos.x = x;
                            adjustPos.z = z;

                            scene.NotifyAllUser(RoomMessageDefine.Msg_RC_AdjustPosition, adjustPos);
                        }
                    }
                }
            }
            return false;
        }
    }
    public class LevelToCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue us;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            if (null != instance && instance.ContextVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    int lvl = operands[0].GetInt();
                    user.Level = lvl;
                }
            }
            return false;
        }
    }
    public class FullCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue us;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            if (null != instance && instance.ContextVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    user.Hp = user.HpMax;
                    user.Energy = user.EnergyMax;
                }
            }
            return false;
        }
    }
    public class ClearEquipmentsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue us;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            if (null != instance && instance.ContextVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                }
            }
            return false;
        }
    }
    public class AddEquipmentCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue us;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            if (null != instance && instance.ContextVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    int itemId = operands[0].GetInt();
                }
            }
            return false;
        }
    }
    public class ClearSkillsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue us;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            if (null != instance && instance.ContextVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    user.GetSkillStateInfo().RemoveAllSkill();
                }
            }
            return false;
        }
    }
    public class AddSkillCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue us;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            if (null != instance && instance.ContextVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    int skillId = operands[0].GetInt();
                    SkillInfo skillInfo = new SkillInfo(skillId);
                    user.GetSkillStateInfo().AddSkill(skillInfo);
                }
            }
            return false;
        }
    }
    public class ClearBuffsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue us;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            if (null != instance && instance.ContextVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    user.GetSkillStateInfo().RemoveAllImpact();
                }
            }
            return false;
        }
    }
    public class AddBuffCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue us;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            if (null != instance && instance.ContextVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    Scene scene = user.SceneContext.CustomData as Scene;
                    if (null != scene) {
                        int impactId = operands[0].GetInt();
                    }
                }
            }
            return false;
        }
    }
    //---------------------------------------------------------------------------------------------------------------------------------
}
