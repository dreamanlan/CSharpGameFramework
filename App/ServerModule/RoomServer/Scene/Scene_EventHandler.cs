using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFrameworkMessage;

namespace GameFramework
{
  internal sealed partial class Scene
  {
    internal void DestroyEntities(int[] unit_ids)
    {
      Msg_RC_DestroyNpc destroyNpcBuilder = new Msg_RC_DestroyNpc();
      for (int i = 0; i < unit_ids.Length; i++) {
        EntityInfo npc = EntityManager.GetEntityInfoByUnitId(unit_ids[i]);
        if (npc != null) {
          destroyNpcBuilder.npc_id = npc.GetId();
          NotifyAllUser(RoomMessageDefine.Msg_RC_DestroyNpc, destroyNpcBuilder);
          EntityManager.RemoveEntity(npc.GetId());
        }
      }
    }

    internal void ReloadObjects()
    {
    }

    private void OnHightlightPrompt(int userId, string dict, object[] args)
    {
      Msg_RC_HighlightPrompt builder = new Msg_RC_HighlightPrompt();
      builder.dict_id = dict;
      foreach (object arg in args) {
        builder.argument.Add(arg.ToString());
      }
      if (userId > 0) {
        EntityInfo info = EntityManager.GetEntityInfo(userId);
        if (null != info) {
          User user = info.CustomData as User;
          if (null != user) {
            user.SendMessage(RoomMessageDefine.Msg_RC_HighlightPrompt, builder);
          }
        }
      } else {
        NotifyAllUser(RoomMessageDefine.Msg_RC_HighlightPrompt, builder);
      }
    }

    private void OnCreateEntity(EntityInfo entity)
    {
        if (null != entity) {
            StorySystem.SendMessage("obj_created", entity.GetId());
            StorySystem.SendMessage(string.Format("npc_created:{0}", entity.GetUnitId()), entity.GetId());
        }
    }
    private void OnDestroyEntity(EntityInfo entity)
    {
        if (null != entity) {
        }
    }
  }
}
