using System;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;
using ScriptRuntime;

namespace ScriptableFramework.Story.Functions
{
    public sealed class BlackboardGetFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                string name = operands[0].ToString();
                object v;
                if (scene.SceneContext.BlackBoard.TryGetVariable(name, out v)) {
                    return BoxedValue.FromObject(v);
                }
                if (operands.Count > 1) {
                    return operands[1];
                }
            }
            return BoxedValue.NullObject;
        }
    }
    public sealed class GetDialogItemFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int dlgId = operands[0].GetInt();
                int index = operands[1].GetInt();
                int dlgItemId = TableConfigUtility.GenStoryDlgItemId(dlgId, index);
                TableConfig.StoryDlg cfg = TableConfig.StoryDlgProvider.Instance.GetStoryDlg(dlgItemId);
                if (null != cfg) {
                    return BoxedValue.FromObject(cfg);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    public sealed class GetActorIconValue : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int index = operands[0].GetInt();
                return BoxedValue.NullObject;
            }
            return BoxedValue.NullObject;
        }
    }
    public sealed class GetMonsterInfoFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int sceneId = scene.SceneResId;
                int campId = operands[0].GetInt();
                int index = operands[1].GetInt();
                int monstersId = TableConfigUtility.GenLevelMonstersId(sceneId, campId);
                List<TableConfig.LevelMonster> monsterList;
                if (TableConfig.LevelMonsterProvider.Instance.TryGetValue(monstersId, out monsterList)) {
                    if (index >= 0 && index < monsterList.Count) {
                        return BoxedValue.FromObject(monsterList[index]);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    public sealed class GetAiDataFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                string typeName = operands[1].ToString();
                EntityInfo npc = scene.SceneContext.GetEntityById(objId);
                if (null != npc) {
                }
            }
            return BoxedValue.NullObject;
        }
    }
    public sealed class GetLeaderIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count > 0) {
                    int objId = operands[0].GetInt();
                    EntityInfo npc = scene.GetEntityById(objId);
                    if (null != npc) {
                        return npc.GetAiStateInfo().LeaderId;
                    }
                }
            }
            return 0;
        }
    }
    public sealed class GetLeaderTableIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return 0;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                EntityInfo obj = scene.SceneContext.GetEntityById(objId);
                if (null != obj) {
                    int leaderId = obj.GetAiStateInfo().LeaderId;
                    EntityInfo leader = scene.SceneContext.GetEntityById(leaderId);
                    if (null != leader) {
                        return leader.GetTableId();
                    }
                }
            }
            return 0;
        }
    }
    public sealed class IsClientFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            return 0;
        }
    }
    internal sealed class GetRoomIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                return scene.GetRoomUserManager().RoomId;
            }
            return 0;
        }
    }
    internal sealed class GetSceneIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                return scene.SceneResId;
            }
            return 0;
        }
    }
}
