using System;
using System.Collections;
using System.Collections.Generic;
using ScriptRuntime;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

namespace ScriptableFramework.Story.Functions
{
    internal sealed class BlackboardGetFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            string name = operands[0].ToString();
            object v;
            if (PluginFramework.Instance.BlackBoard.TryGetVariable(name, out v)) {
                return BoxedValue.FromObject(v);
            }
            if (operands.Count > 1) {
                return operands[1];
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class OffsetSplineFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var list = operands[0].GetObject() as IList<object>;
            Vector3Obj offsetObj = operands[1];
            Vector3 offset = offsetObj.Value;
            if (null != list) {
                List<object> npts = new List<object>();
                int ct = list.Count;
                float dir = 0;
                Vector3 curPt = Vector3.Zero;
                for (int i = 0; i < ct; ++i) {
                    if (i == 0) {
                        curPt = (Vector3)list[i];
                    }
                    Vector3 pt = Vector3.Zero;
                    if (i + 1 < ct) {
                        pt = (Vector3)list[i + 1];
                        dir = Geometry.GetYRadian(curPt, pt);
                    }
                    npts.Add(curPt + Geometry.GetRotate(offset, dir));
                    curPt = pt;
                }
                return BoxedValue.FromObject(npts);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class OffsetVector3Function : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            Vector3Obj ptObj = operands[0];
            Vector3 pt = ptObj.Value;
            if (operands.Count >= 3) {
                Vector3Obj pt2Obj = operands[1];
                Vector3 pt2 = pt2Obj.Value;
                Vector3Obj offsetObj = operands[2];
                Vector3 offset = offsetObj.Value;
                float dir = Geometry.GetYRadian(pt, pt2);
                return (Vector3Obj)(pt + Geometry.GetRotate(offset, dir));
            } else {
                Vector3Obj offsetObj = operands[1];
                Vector3 offset = offsetObj.Value;
                return (Vector3Obj)(pt + offset);
            }
        }
    }
    internal sealed class GetDialogItemFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int dlgId = operands[0].GetInt();
            int index = operands[1].GetInt();
            int dlgItemId = TableConfigUtility.GenStoryDlgItemId(dlgId, index);
            TableConfig.StoryDlg cfg = TableConfig.StoryDlgProvider.Instance.GetStoryDlg(dlgItemId);
            if (null != cfg) {
                return BoxedValue.FromObject(cfg);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetMonsterInfoFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int sceneId = PluginFramework.Instance.SceneId;
            int campId = operands[0].GetInt();
            int index = operands[1].GetInt();
            int monstersId = TableConfigUtility.GenLevelMonstersId(sceneId, campId);
            List<TableConfig.LevelMonster> monsterList;
            if (TableConfig.LevelMonsterProvider.Instance.TryGetValue(monstersId, out monsterList)) {
                if (index >= 0 && index < monsterList.Count) {
                    return BoxedValue.FromObject(monsterList[index]);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetAiDataFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            string typeName = operands[1].ToString();
            EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
            if (null != npc) {
                return BoxedValue.FromObject(npc.GetAiStateInfo().AiDatas.GetData(typeName));
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetActorIconFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            int index = operands[0].GetInt();
            UnityEngine.Sprite obj = SpriteManager.GetActorIcon(index);
            if (null != obj) {
                return BoxedValue.FromObject(obj);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetActorFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            int objId = operands[0].GetInt();
            return BoxedValue.FromObject(EntityController.Instance.GetGameObject(objId));
        }
    }
    internal sealed class GetPlayerIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return PluginFramework.Instance.RoomObjId;
        }
    }
    internal sealed class GetPlayerUnitIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return PluginFramework.Instance.RoomUnitId;
        }
    }
    internal sealed class GetLeaderIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 0) {
                int objId = operands[0].GetInt();
                EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
                if (null != npc) {
                    return npc.GetAiStateInfo().LeaderId;
                }
                return 0;
            } else {
                return PluginFramework.Instance.LeaderId;
            }
        }
    }
    internal sealed class GetLeaderTableIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return ClientInfo.Instance.RoleData.HeroId;
        }
    }
    internal sealed class GetMemberCountFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return ClientInfo.Instance.RoleData.Members.Count;
        }
    }
    internal sealed class IsClientFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return 1;
        }
    }
    internal sealed class GetSceneIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return GfxStorySystem.Instance.SceneId;
        }
    }
    internal sealed class GetMemberTableIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int index = operands[0].GetInt();
            if (index >= 0 && index < ClientInfo.Instance.RoleData.Members.Count) {
                return ClientInfo.Instance.RoleData.Members[index].Hero;
            }
            return 0;
        }
    }
    internal sealed class GetMemberLevelFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            int index = operands[0].GetInt();
            if (index >= 0 && index < ClientInfo.Instance.RoleData.Members.Count) {
                return ClientInfo.Instance.RoleData.Members[index].Level;
            }
            return 0;
        }
    }
    internal sealed class DictGetFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            string dictId = operands[0].ToString();
            return Dict.Get(dictId);
        }
    }
    internal sealed class DictFormatFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            string dictId = operands[0].ToString();
            ArrayList arglist = new ArrayList();
            for (int i = 1; i < operands.Count; i++) {
                arglist.Add(operands[i].GetObject());
            }
            object[] args = arglist.ToArray();
            return Dict.Format(dictId, args);
        }
    }
}
