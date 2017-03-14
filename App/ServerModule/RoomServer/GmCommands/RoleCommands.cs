using System;
using System.Collections.Generic;
using StorySystem;
using GameFrameworkMessage;

namespace GameFramework.GmCommands
{
  //---------------------------------------------------------------------------------------------------------------------------------
  //********************************************************分隔线*******************************************************************
  //---------------------------------------------------------------------------------------------------------------------------------
  //只在场景内有效的命令（仅修改GameFramework战斗相关数据）
  public class SetPositionCommand : SimpleStoryCommandBase<SetPositionCommand, StoryValueParam<int, float, float>>
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
  public class LevelToCommand : SimpleStoryCommandBase<LevelToCommand, StoryValueParam<int>>
  {
    protected override bool ExecCommand(StoryInstance instance, StoryValueParam<int> _params, long delta)
    {
      object us;
      if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
        EntityInfo user = us as EntityInfo;
        if (null != user) {
          int lvl = _params.Param1Value;
          user.Level = lvl;
        }
      }
      return false;
    }
  }
  public class FullCommand : SimpleStoryCommandBase<FullCommand, StoryValueParam>
  {
    protected override bool ExecCommand(StoryInstance instance, StoryValueParam _params, long delta)
    {
      object us;
      if (instance.GlobalVariables.TryGetValue("EntityInfo", out us)) {
        EntityInfo user = us as EntityInfo;
        if (null != user) {
          user.Hp = user.HpMax;
          user.Energy = user.EnergyMax;
        }
      }
      return false;
    }
  }
  public class ClearEquipmentsCommand : SimpleStoryCommandBase<ClearEquipmentsCommand, StoryValueParam>
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
  public class AddEquipmentCommand : SimpleStoryCommandBase<AddEquipmentCommand, StoryValueParam<int>>
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
  public class ClearSkillsCommand : SimpleStoryCommandBase<ClearSkillsCommand, StoryValueParam>
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
  public class AddSkillCommand : SimpleStoryCommandBase<AddSkillCommand, StoryValueParam<int>>
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
  public class ClearBuffsCommand : SimpleStoryCommandBase<ClearBuffsCommand, StoryValueParam>
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
  public class AddBuffCommand : SimpleStoryCommandBase<AddBuffCommand, StoryValueParam<int>>
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
