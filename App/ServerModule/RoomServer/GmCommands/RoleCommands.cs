using System;
using System.Collections.Generic;
using StorySystem;
using GameFrameworkMessage;

namespace GameFramework.GmCommands
{
  //---------------------------------------------------------------------------------------------------------------------------------
  //********************************************************分隔线*******************************************************************
  //---------------------------------------------------------------------------------------------------------------------------------
  //只在场景内有效的命令（仅修改RoomServer战斗相关数据）
  internal class SetPositionCommand : SimpleStoryCommandBase<SetPositionCommand, StoryValueParam<int, float, float>>
  {
    protected override bool ExecCommand(StoryInstance instance, StoryValueParam<int, float, float> _params, long delta)
    {
      object us;
      if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
        EntityInfo user = us as EntityInfo;
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
              adjustPos.face_dir = ProtoHelper.EncodeFloat(msi.GetFaceDir());
              adjustPos.x = ProtoHelper.EncodeFloat(x);
              adjustPos.z = ProtoHelper.EncodeFloat(z);

              scene.NotifyAllUser(RoomMessageDefine.Msg_RC_AdjustPosition, adjustPos);
            }
          }
        }
      }
      return false;
    }
  }
  internal class LevelToCommand : SimpleStoryCommandBase<LevelToCommand, StoryValueParam<int>>
  {
    protected override bool ExecCommand(StoryInstance instance, StoryValueParam<int> _params, long delta)
    {
      object us;
      if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
        EntityInfo user = us as EntityInfo;
        if (null != user) {
          int lvl = _params.Param1Value;
          user.SetLevel(lvl);
        }
      }
      return false;
    }
  }
  internal class FullCommand : SimpleStoryCommandBase<FullCommand, StoryValueParam>
  {
    protected override bool ExecCommand(StoryInstance instance, StoryValueParam _params, long delta)
    {
      object us;
      if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
        EntityInfo user = us as EntityInfo;
        if (null != user) {
          user.SetHp(Operate_Type.OT_Absolute, user.GetActualProperty().HpMax);
          user.SetEnergy(Operate_Type.OT_Absolute, user.GetActualProperty().EnergyMax);
        }
      }
      return false;
    }
  }
  internal class ClearEquipmentsCommand : SimpleStoryCommandBase<ClearEquipmentsCommand, StoryValueParam>
  {
    protected override bool ExecCommand(StoryInstance instance, StoryValueParam _params, long delta)
    {
      object us;
      if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
        EntityInfo user = us as EntityInfo;
        if (null != user) {
        }
      }
      return false;
    }
  }
  internal class AddEquipmentCommand : SimpleStoryCommandBase<AddEquipmentCommand, StoryValueParam<int>>
  {
    protected override bool ExecCommand(StoryInstance instance, StoryValueParam<int> _params, long delta)
    {
      object us;
      if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
        EntityInfo user = us as EntityInfo;
        if (null != user) {
          int itemId = _params.Param1Value;
        }
      }
      return false;
    }
  }
  internal class ClearSkillsCommand : SimpleStoryCommandBase<ClearSkillsCommand, StoryValueParam>
  {
    protected override bool ExecCommand(StoryInstance instance, StoryValueParam _params, long delta)
    {
      object us;
      if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
        EntityInfo user = us as EntityInfo;
        if (null != user) {
          user.GetSkillStateInfo().RemoveAllSkill();
        }
      }
      return false;
    }
  }
  internal class AddSkillCommand : SimpleStoryCommandBase<AddSkillCommand, StoryValueParam<int>>
  {
    protected override bool ExecCommand(StoryInstance instance, StoryValueParam<int> _params, long delta)
    {
      object us;
      if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
        EntityInfo user = us as EntityInfo;
        if (null != user) {
          int skillId = _params.Param1Value;
          SkillInfo skillInfo = new SkillInfo(skillId);
          user.GetSkillStateInfo().AddSkill(skillInfo);
        }
      }
      return false;
    }
  }
  internal class ClearBuffsCommand : SimpleStoryCommandBase<ClearBuffsCommand, StoryValueParam>
  {
    protected override bool ExecCommand(StoryInstance instance, StoryValueParam _params, long delta)
    {
      object us;
      if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
        EntityInfo user = us as EntityInfo;
        if (null != user) {
          user.GetSkillStateInfo().RemoveAllImpact();
        }
      }
      return false;
    }
  }
  internal class AddBuffCommand : SimpleStoryCommandBase<AddBuffCommand, StoryValueParam<int>>
  {
    protected override bool ExecCommand(StoryInstance instance, StoryValueParam<int> _params, long delta)
    {
      object us;
      if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
        EntityInfo user = us as EntityInfo;
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
