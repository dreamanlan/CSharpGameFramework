﻿using System;
using System.Collections.Generic;
using DotnetStoryScript;
using GameFrameworkMessage;

namespace ScriptableFramework.GmCommands
{
    //---------------------------------------------------------------------------------------------------------------------------------
    //********************************************************delimiter*******************************************************************
    //---------------------------------------------------------------------------------------------------------------------------------
    //Commands that are only valid within the scene (only modify ScriptableFramework combat-related data)
    public class SetPositionCommand : SimpleStoryCommandBase<SetPositionCommand, StoryFunctionParam<int, float, float>>
    {
        protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam<int, float, float> _params, long delta)
        {
            BoxedValue us;
            if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    int objId = _params.Param1Value;
                    float x = _params.Param2Value;
                    float z = _params.Param3Value;
                    EntityInfo charObj = user.SceneContext.GetEntityById(objId);
                    if (null != charObj) {
                        MovementStateInfo msi = charObj.GetMovementStateInfo();
                        msi.SetPosition2D(x, z);

                        Scene scene = user.SceneContext.CustomData as Scene;
                        if (null != scene) {
                            GameFrameworkMessage.Msg_RC_AdjustPosition adjustPos = new GameFrameworkMessage.Msg_RC_AdjustPosition();
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
    public class LevelToCommand : SimpleStoryCommandBase<LevelToCommand, StoryFunctionParam<int>>
    {
        protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam<int> _params, long delta)
        {
            BoxedValue us;
            if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    int lvl = _params.Param1Value;
                    user.Level = lvl;
                }
            }
            return false;
        }
    }
    public class FullCommand : SimpleStoryCommandBase<FullCommand, StoryFunctionParam>
    {
        protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam _params, long delta)
        {
            BoxedValue us;
            if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    user.Hp = user.HpMax;
                    user.Energy = user.EnergyMax;
                }
            }
            return false;
        }
    }
    public class ClearEquipmentsCommand : SimpleStoryCommandBase<ClearEquipmentsCommand, StoryFunctionParam>
    {
        protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam _params, long delta)
        {
            BoxedValue us;
            if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                }
            }
            return false;
        }
    }
    public class AddEquipmentCommand : SimpleStoryCommandBase<AddEquipmentCommand, StoryFunctionParam<int>>
    {
        protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam<int> _params, long delta)
        {
            BoxedValue us;
            if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    int itemId = _params.Param1Value;
                }
            }
            return false;
        }
    }
    public class ClearSkillsCommand : SimpleStoryCommandBase<ClearSkillsCommand, StoryFunctionParam>
    {
        protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam _params, long delta)
        {
            BoxedValue us;
            if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    user.GetSkillStateInfo().RemoveAllSkill();
                }
            }
            return false;
        }
    }
    public class AddSkillCommand : SimpleStoryCommandBase<AddSkillCommand, StoryFunctionParam<int>>
    {
        protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam<int> _params, long delta)
        {
            BoxedValue us;
            if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    int skillId = _params.Param1Value;
                    SkillInfo skillInfo = new SkillInfo(skillId);
                    user.GetSkillStateInfo().AddSkill(skillInfo);
                }
            }
            return false;
        }
    }
    public class ClearBuffsCommand : SimpleStoryCommandBase<ClearBuffsCommand, StoryFunctionParam>
    {
        protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam _params, long delta)
        {
            BoxedValue us;
            if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    user.GetSkillStateInfo().RemoveAllImpact();
                }
            }
            return false;
        }
    }
    public class AddBuffCommand : SimpleStoryCommandBase<AddBuffCommand, StoryFunctionParam<int>>
    {
        protected override bool ExecCommand(StoryInstance instance, StoryFunctionParam<int> _params, long delta)
        {
            BoxedValue us;
            if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
                EntityInfo user = us.ObjectVal as EntityInfo;
                if (null != user) {
                    Scene scene = user.SceneContext.CustomData as Scene;
                    if (null != scene) {
                        int impactId = _params.Param1Value;
                    }
                }
            }
            return false;
        }
    }
    //---------------------------------------------------------------------------------------------------------------------------------
}
