using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptRuntime;
using ScriptableFramework;
using ScriptableFramework.Skill;
using ScriptableFramework.Skill.Trigers;
using ScriptableFramework.Story;

namespace ScriptableFramework.Story.Commands
{
    /// <summary>
    /// blackboard_clear();
    /// </summary>
    internal class BlackboardClearCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            PluginFramework.Instance.BlackBoard.ClearVariables();
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// blackboard_set(name,value);
    /// </summary>
    internal class BlackboardSetCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            string name = operands[0].GetString();
            object value = operands[1].GetObject();
            PluginFramework.Instance.BlackBoard.SetVariable(name, value);
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// camera_follow(npc_unit_id1,npc_unit_id2,...)[with(camera_path)];
    /// </summary>
    internal class CameraFollowCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            if (m_UnitIds.Count <= 0) {
                Utility.SendMessage(path, "CameraFollow", PluginFramework.Instance.LeaderId);
            } else {
                for (int i = 0; i < m_UnitIds.Count; i++) {
                    int unitId = m_UnitIds[i].Calc(calculator).GetInt();
                    EntityInfo npc = PluginFramework.Instance.GetEntityByUnitId(unitId);
                    if (null != npc && (!npc.IsDead() || npc.IsBorning)) {
                        Utility.SendMessage(path, "CameraFollow", npc.GetId());
                        break;
                    }
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IExpression val = CodeStructure.Load(callData.GetParam(i));
                m_UnitIds.Add(val);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private List<IExpression> m_UnitIds = new List<IExpression>();
    }
    /// <summary>
    /// cameralookat(npc_unit_id)[with(camera_path)];
    /// or
    /// cameralookat(vector3(x,y,z))[with(camera_path)];
    /// </summary>
    internal class CameraLookCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            object obj = m_Arg.Calc(calculator).GetObject();
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            if (obj is int) {
                int unitId = (int)obj;
                EntityInfo npc = PluginFramework.Instance.GetEntityByUnitId(unitId);
                if (null != npc) {
                    Vector3 pos = npc.GetMovementStateInfo().GetPosition3D();
                    Utility.SendMessage(path, "CameraLook", new object[] { pos.X, pos.Y + npc.GetRadius(), pos.Z });
                }
            } else {
                Vector3 pos = (Vector3)obj;
                Utility.SendMessage(path, "CameraLook", new object[] { pos.X, pos.Y, pos.Z });
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Arg = CodeStructure.Load(callData.GetParam(0));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_Arg;
    }
    /// <summary>
    /// cameralookatimmediately(npc_unit_id)[with(camera_path)];
    /// or
    /// cameralookatimmediately(vector3(x,y,z))[with(camera_path)];
    /// </summary>
    internal class CameraLookImmediatelyCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            object obj = m_Arg.Calc(calculator).GetObject();
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            if (obj is int) {
                int unitId = (int)obj;
                EntityInfo npc = PluginFramework.Instance.GetEntityByUnitId(unitId);
                if (null != npc) {
                    Vector3 pos = npc.GetMovementStateInfo().GetPosition3D();
                    Utility.SendMessage(path, "CameraLookImmediately", new object[] { pos.X, pos.Y + npc.GetRadius(), pos.Z });
                }
            } else {
                Vector3 pos = (Vector3)obj;
                Utility.SendMessage(path, "CameraLookImmediately", new object[] { pos.X, pos.Y, pos.Z });
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Arg = CodeStructure.Load(callData.GetParam(0));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_Arg;
    }
    /// <summary>
    /// camera_look_toward(npc_unit_id)[with(camera_path)];
    /// or
    /// camera_look_toward(vector3(x,y,z))[with(camera_path)];
    /// </summary>
    internal class CameraLookTowardCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            object obj = m_Arg.Calc(calculator).GetObject();
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            if (obj is int) {
                int unitId = (int)obj;
                EntityInfo npc = PluginFramework.Instance.GetEntityByUnitId(unitId);
                if (null != npc) {
                    Vector3 pos = npc.GetMovementStateInfo().GetPosition3D();
                    Utility.SendMessage(path, "CameraLookImmediately", new object[] { pos.X, pos.Y + npc.GetRadius(), pos.Z });
                }
            } else {
                Vector3 pos = (Vector3)obj;
                Utility.SendMessage(path, "CameraLookImmediately", new object[] { pos.X, pos.Y, pos.Z });
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Arg = CodeStructure.Load(callData.GetParam(0));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_Arg;
    }
    /// <summary>
    /// camera_fixed_yaw(yaw)[with(camera_path)];
    /// </summary>
    internal class CameraFixedYawCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            float arg1 = m_Arg1.Calc(calculator).GetFloat();
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            Utility.SendMessage(path, "CameraFixedYaw", arg1);
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Arg1 = CodeStructure.Load(callData.GetParam(0));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_Arg1;
    }
    /// <summary>
    /// camera_yaw(yaw, anglelag, snaplag)[with(camera_path)];
    /// </summary>
    internal class CameraYawCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            float arg1 = m_Arg1.Calc(calculator).GetFloat();
            float arg2 = m_Arg2.Calc(calculator).GetFloat();
            float arg3 = m_Arg3.Calc(calculator).GetFloat();
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            Utility.SendMessage(path, "CameraYaw", new object[] { arg1, arg2, arg3 });
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_Arg1 = CodeStructure.Load(callData.GetParam(0));
                m_Arg2 = CodeStructure.Load(callData.GetParam(1));
                m_Arg3 = CodeStructure.Load(callData.GetParam(2));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_Arg1;
        private IExpression m_Arg2;
        private IExpression m_Arg3;
    }
    /// <summary>
    /// camera_height(height, lag)[with(camera_path)];
    /// </summary>
    internal class CameraHeightCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            float arg1 = m_Arg1.Calc(calculator).GetFloat();
            float arg2 = m_Arg2.Calc(calculator).GetFloat();
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            Utility.SendMessage(path, "CameraHeight", new object[] { arg1, arg2 });
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Arg1 = CodeStructure.Load(callData.GetParam(0));
                m_Arg2 = CodeStructure.Load(callData.GetParam(1));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_Arg1;
        private IExpression m_Arg2;
    }
    /// <summary>
    /// camera_distance(distance, lag)[with(camera_path)];
    /// </summary>
    internal class CameraDistanceCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            float arg1 = m_Arg1.Calc(calculator).GetFloat();
            float arg2 = m_Arg2.Calc(calculator).GetFloat();
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            Utility.SendMessage(path, "CameraDistance", new object[] { arg1, arg2 });
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Arg1 = CodeStructure.Load(callData.GetParam(0));
                m_Arg2 = CodeStructure.Load(callData.GetParam(1));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_Arg1;
        private IExpression m_Arg2;
    }
    /// <summary>
    /// camera_set_distance_height(distance, height)[with(camera_path)];
    /// </summary>
    internal class CameraSetDistanceHeightCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            float arg1 = m_Arg1.Calc(calculator).GetFloat();
            float arg2 = m_Arg2.Calc(calculator).GetFloat();
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            Utility.SendMessage(path, "SetDistanceAndHeight", new object[] { arg1, arg2 });
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Arg1 = CodeStructure.Load(callData.GetParam(0));
                m_Arg2 = CodeStructure.Load(callData.GetParam(1));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_Arg1;
        private IExpression m_Arg2;
    }
    /// <summary>
    /// camera_reset_distance_height()[with(camera_path)];
    /// </summary>
    internal class CameraResetDistanceHeightCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            Utility.SendMessage(path, "ResetDistanceAndHeight", null);
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
    }
    /// <summary>
    /// camera_set_follow_speed(maxdist, mindist, maxspeed, minspeed, power)[with(camera_path)];
    /// </summary>
    internal class CameraSetFollowSpeedCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            float arg1 = m_Arg1.Calc(calculator).GetFloat();
            float arg2 = m_Arg2.Calc(calculator).GetFloat();
            float arg3 = m_Arg3.Calc(calculator).GetFloat();
            float arg4 = m_Arg4.Calc(calculator).GetFloat();
            int arg5 = m_Arg5.Calc(calculator).GetInt();
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            Utility.SendMessage(path, "SetFollowSpeed", new object[] { arg1, arg2, arg3, arg4, arg5 });
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 4) {
                m_Arg1 = CodeStructure.Load(callData.GetParam(0));
                m_Arg2 = CodeStructure.Load(callData.GetParam(1));
                m_Arg3 = CodeStructure.Load(callData.GetParam(2));
                m_Arg4 = CodeStructure.Load(callData.GetParam(3));
                m_Arg5 = CodeStructure.Load(callData.GetParam(4));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_Arg1;
        private IExpression m_Arg2;
        private IExpression m_Arg3;
        private IExpression m_Arg4;
        private IExpression m_Arg5;
    }
    /// <summary>
    /// camera_reset_follow_speed()[with(camera_path)];
    /// </summary>
    internal class CameraResetFollowSpeedCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            Utility.SendMessage(path, "ResetFollowSpeed", null);
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
    }
    /// <summary>
    /// camera_follow_obj(objid)[with(camera_path)];
    /// </summary>
    internal class CameraFollowObjCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            EntityInfo npc = PluginFramework.Instance.GetEntityById(m_ObjId.Calc(calculator).GetInt());
            if (null != npc && (!npc.IsDead() || npc.IsBorning)) {
                Utility.SendMessage(path, "CameraFollow", npc.GetId());
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ObjId = CodeStructure.Load(callData.GetParam(0));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_ObjId;
    }
    /// <summary>
    /// camera_look_obj(objid)[with(camera_path)];
    /// </summary>
    internal class CameraLookObjCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            EntityInfo npc = PluginFramework.Instance.GetEntityById(m_ObjId.Calc(calculator).GetInt());
            if (null != npc && (!npc.IsDead() || npc.IsBorning)) {
                Vector3 pos = npc.GetMovementStateInfo().GetPosition3D();
                Utility.SendMessage(path, "CameraLook", new object[] { pos.X, pos.Y + npc.GetRadius(), pos.Z });
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ObjId = CodeStructure.Load(callData.GetParam(0));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_ObjId;
    }
    /// <summary>
    /// camera_look_obj_immediately(objid)[with(camera_path)];
    /// </summary>
    internal class CameraLookObjImmediatelyCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            EntityInfo npc = PluginFramework.Instance.GetEntityById(m_ObjId.Calc(calculator).GetInt());
            if (null != npc && (!npc.IsDead() || npc.IsBorning)) {
                Vector3 pos = npc.GetMovementStateInfo().GetPosition3D();
                Utility.SendMessage(path, "CameraLookImmediately", new object[] { pos.X, pos.Y + npc.GetRadius(), pos.Z });
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ObjId = CodeStructure.Load(callData.GetParam(0));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_ObjId;
    }
    /// <summary>
    /// camera_look_toward_obj(objid)[with(camera_path)];
    /// </summary>
    internal class CameraLookTowardObjCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            EntityInfo npc = PluginFramework.Instance.GetEntityById(m_ObjId.Calc(calculator).GetInt());
            if (null != npc && (!npc.IsDead() || npc.IsBorning)) {
                var pos = npc.GetMovementStateInfo().GetPosition3D();
                Utility.SendMessage(path, "CameraLookImmediately", new object[] { pos.X, pos.Y + npc.GetRadius(), pos.Z });
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ObjId = CodeStructure.Load(callData.GetParam(0));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_ObjId;
    }
    /// <summary>
    /// camera_look_copy(objpath, unitid or vector3(x,y,z))[with(camera_path)];
    /// </summary>
    internal class CameraLookCopyCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            object obj = m_ObjPath.Calc(calculator).GetObject();
            var pobj = obj as UnityEngine.GameObject;
            if (null == pobj) {
                string objPath = obj as string;
                if (null != objPath) {
                    pobj = UnityEngine.GameObject.Find(objPath);
                }
            }
            if (null != pobj) {
                var argObj = m_Arg.Calc(calculator);
                if (argObj.IsInteger) {
                    int unitId = argObj.GetInt();
                    EntityInfo npc = PluginFramework.Instance.GetEntityByUnitId(unitId);
                    if (null != npc) {
                        Utility.SendMessage(path, "CameraLookObjCopy", new object[] { pobj.transform, npc.GetId() });
                    }
                } else {
                    Vector3 pos = (Vector3Obj)argObj;
                    Utility.SendMessage(path, "CameraLookCopy", new object[] { pobj.transform, pos.X, pos.Y, pos.Z });
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjPath = CodeStructure.Load(callData.GetParam(0));
                m_Arg = CodeStructure.Load(callData.GetParam(1));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_ObjPath;
        private IExpression m_Arg;
    }
    /// <summary>
    /// camera_look_obj_copy(objpath, objid)[with(camera_path)];
    /// </summary>
    internal class CameraLookObjCopyCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            string path = "GameRoot";
            if (null != m_CameraPath) {
                path = m_CameraPath.Calc(calculator).GetString();
            }
            if (string.IsNullOrEmpty(path)) {
                path = "GameRoot";
            }
            object obj = m_ObjPath.Calc(calculator).GetObject();
            var pobj = obj as UnityEngine.GameObject;
            if (null == pobj) {
                string objPath = obj as string;
                if (null != objPath) {
                    pobj = UnityEngine.GameObject.Find(objPath);
                }
            }
            if (null != pobj) {
                int objId = m_ObjId.Calc(calculator).GetInt();
                Utility.SendMessage(path, "CameraLookObjCopy", new object[] { pobj.transform, objId });
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjPath = CodeStructure.Load(callData.GetParam(0));
                m_ObjId = CodeStructure.Load(callData.GetParam(1));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            var first = statementData.First.AsFunction;
            var second = statementData.Second.AsFunction;
            if (null != first && null != second) {
                Load(first);
                LoadCameraPath(second);
            }
            return true;
        }
        private void LoadCameraPath(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_CameraPath = CodeStructure.Load(callData.GetParam(0));
            }
        }

        private IExpression m_CameraPath;
        private IExpression m_ObjPath;
        private IExpression m_ObjId;
    }
    /// <summary>
    /// camera_enable(name, 0_or_1);
    /// </summary>
    internal class CameraEnableCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            string name = operands[0].GetString();
            int v = operands[1].GetInt();
            Utility.SendMessage("GameRoot", "CameraEnable", new object[] { name, v });
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// lock_frame(scale);
    /// </summary>
    internal class LockFrameCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            float scale = m_Scale.Calc(calculator).GetFloat();
            UnityEngine.Time.timeScale = scale;
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Scale = CodeStructure.Load(callData.GetParam(0));
            }
            return true;
        }

        private IExpression m_Scale;
    }
    /// <summary>
    /// set_leader_id([objid,]leaderid);
    /// </summary>
    internal class SetLeaderIdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            if (operands.Count > 1) {
                int objId = operands[0].GetInt();
                int leaderId = operands[1].GetInt();
                EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
                if (null != npc) {
                    npc.GetAiStateInfo().LeaderId = leaderId;
                }
            } else {
                int leaderId = operands[0].GetInt();
                PluginFramework.Instance.LeaderId = leaderId;
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// show_dlg(storyDlgId);
    /// </summary>
    internal class ShowDlgCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            int storyDlgId = m_StoryDlgId.Calc(calculator).GetInt();
            GfxStorySystem.Instance.SendMessage("show_dlg", storyDlgId);
            LogSystem.Info("show_dlg {0}", storyDlgId);
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_StoryDlgId = CodeStructure.Load(callData.GetParam(0));
            }
            return true;
        }

        private IExpression m_StoryDlgId;
    }
    /// <summary>
    /// area_detect(name,radius,type,callback)[set(var,val)];
    /// </summary>
    internal class AreaDetectCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            bool triggered = false;
            string name = m_Name.Calc(calculator).GetString();
            float radius = m_Radius.Calc(calculator).GetFloat();
            string type = m_Type.Calc(calculator).GetString();
            string eventName = m_EventName.Calc(calculator).GetString();
            UnityEngine.GameObject obj = UnityEngine.GameObject.Find(name);
            if (null != obj) {
                UnityEngine.Vector3 pos = obj.transform.position;
                if (type == "myself") {
                    EntityViewModel view = EntityController.Instance.GetEntityViewById(PluginFramework.Instance.LeaderId);
                    if (null != view && null != view.Actor) {
                        if ((view.Actor.transform.position - pos).sqrMagnitude < radius * radius) {
                            GfxStorySystem.Instance.SendMessage(eventName, name, radius, type);
                            triggered = true;
                        }
                    }
                } else if (type == "anyenemy" || type == "anyfriend") {
                    EntityInfo myself = PluginFramework.Instance.GetEntityById(PluginFramework.Instance.LeaderId);
                    PluginFramework.Instance.KdTree.QueryWithFunc(pos.x, pos.y, pos.z, radius, (float distSqr, KdTreeObject kdObj) => {
                        if (type == "anyenemy" && EntityInfo.GetRelation(myself, kdObj.Object) == CharacterRelation.RELATION_ENEMY ||
                            type == "anyfriend" && EntityInfo.GetRelation(myself, kdObj.Object) == CharacterRelation.RELATION_FRIEND) {
                            GfxStorySystem.Instance.SendMessage(eventName, name, radius, type);
                            triggered = true;
                            return false;
                        }
                        return true;
                    });
                }
            }
            string varName = m_SetVar.Calc(calculator).GetString();
            var varVal = m_SetVal.Calc(calculator);
            var elseVal = m_ElseSetVal.Calc(calculator);
            var instance = calculator.GetFuncContext<StoryInstance>();
            if (null != instance) {
                if (triggered) {
                    instance.SetVariable(varName, varVal);
                } else {
                    instance.SetVariable(varName, elseVal);
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 3) {
                m_Name = CodeStructure.Load(callData.GetParam(0));
                m_Radius = CodeStructure.Load(callData.GetParam(1));
                m_Type = CodeStructure.Load(callData.GetParam(2));
                m_EventName = CodeStructure.Load(callData.GetParam(3));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 2) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    m_HaveSet = true;
                    Load(first);
                    LoadSet(second);
                }
            }
            return true;
        }
        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_SetVar = CodeStructure.Load(callData.GetParam(0));
                m_SetVal = CodeStructure.Load(callData.GetParam(1));
                m_ElseSetVal = CodeStructure.Load(callData.GetParam(2));
            }
        }

        private IExpression m_Name;
        private IExpression m_Radius;
        private IExpression m_Type;
        private IExpression m_EventName;
        private IExpression m_SetVar;
        private IExpression m_SetVal;
        private IExpression m_ElseSetVal;
        private bool m_HaveSet = false;
    }
    /// <summary>
    /// load_ui(name, prefab, dslfile[, dontDestroyOld]);
    /// </summary>
    internal class LoadUiCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return BoxedValue.NullObject;
            string name = operands[0].GetString();
            string prefab = operands[1].GetString();
            string dslfile = operands[2].GetString();
            int dontDestroyOld = operands.Count > 3 ? operands[3].GetInt() : 0;
            GfxStorySystem.Instance.LoadStory(name, dslfile);
            UnityEngine.GameObject asset = UiResourceSystem.Instance.GetUiResource(prefab) as UnityEngine.GameObject;
            if (null != asset) {
                if (dontDestroyOld <= 0) {
                    for (; ; ) {
                        var old = UnityEngine.GameObject.Find(name);
                        if (null != old) {
                            old.transform.SetParent(null);
                            UnityEngine.GameObject.Destroy(old);
                        } else {
                            break;
                        }
                    }
                }
                UnityEngine.GameObject uiObj = UnityEngine.GameObject.Instantiate(asset);
                if (null != uiObj) {
                    uiObj.name = name;
                    if (!string.IsNullOrEmpty(dslfile)) {
                        ScriptableFramework.Story.UiStoryInitializer initer = uiObj.GetComponent<ScriptableFramework.Story.UiStoryInitializer>();
                        if (null == initer) {
                            initer = uiObj.AddComponent<ScriptableFramework.Story.UiStoryInitializer>();
                        }
                        if (null != initer) {
                            initer.WindowName = name;
                            initer.Init();
                        }
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// bind_ui(gameobject){
    ///     var("@varname","Panel/Input");
    ///     inputs("",...);
    ///     toggles("",...);
    ///     sliders("",...);
    ///     dropdowns("",...);
    ///     onevent("button","eventtag","Panel/Button");
    /// };
    /// </summary>
    internal class BindUiCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            UnityEngine.GameObject obj = m_Obj.Calc(calculator).GetObject() as UnityEngine.GameObject;
            if (null != obj) {
                UiStoryInitializer initer = obj.GetComponent<UiStoryInitializer>();
                if (null != initer) {
                    UiStoryEventHandler handler0 = obj.GetComponent<UiStoryEventHandler>();
                    if (null == handler0) {
                        handler0 = obj.AddComponent<UiStoryEventHandler>();
                    }
                    if (null != handler0) {
                        int ct = m_VarInfos.Count;
                        for (int ix = 0; ix < ct; ++ix) {
                            string name = m_VarInfos[ix].m_VarName.Calc(calculator).GetString();
                            string path = m_VarInfos[ix].m_ControlPath.Calc(calculator).GetString();
                            UnityEngine.GameObject ctrl = Utility.FindChildObjectByPath(obj, path);
                            AddVariable(calculator, name, ctrl);
                        }

                        handler0.WindowName = initer.WindowName;
                        var list = m_Inputs;
                        for (int k = 0; k < list.Count; ++k) {
                            string path = list[k].Calc(calculator).GetString();
                            var comp = Utility.FindComponentInChildren<UnityEngine.UI.InputField>(obj, path);
                            handler0.InputLabels.Add(comp);
                        }
                        list = m_Toggles;
                        for (int k = 0; k < list.Count; ++k) {
                            string path = list[k].Calc(calculator).GetString();
                            var comp = Utility.FindComponentInChildren<UnityEngine.UI.Toggle>(obj, path);
                            handler0.InputToggles.Add(comp);
                        }
                        list = m_Sliders;
                        for (int k = 0; k < list.Count; ++k) {
                            string path = list[k].Calc(calculator).GetString();
                            var comp = Utility.FindComponentInChildren<UnityEngine.UI.Slider>(obj, path);
                            handler0.InputSliders.Add(comp);
                        }
                        list = m_DropDowns;
                        for (int k = 0; k < list.Count; ++k) {
                            string path = list[k].Calc(calculator).GetString();
                            var comp = Utility.FindComponentInChildren<UnityEngine.UI.Dropdown>(obj, path);
                            handler0.InputDropdowns.Add(comp);
                        }

                        ct = m_EventInfos.Count;
                        for (int ix = 0; ix < ct; ++ix) {
                            string evt = m_EventInfos[ix].m_Event.Calc(calculator).GetString();
                            string tag = m_EventInfos[ix].m_Tag.Calc(calculator).GetString();
                            string path = m_EventInfos[ix].m_Path.Calc(calculator).GetString();
                            if (evt == "button") {
                                UnityEngine.UI.Button button = Utility.FindComponentInChildren<UnityEngine.UI.Button>(obj, path);
                                button.onClick.AddListener(() => { handler0.OnClickHandler(tag); });
                            } else if (evt == "toggle") {
                                UnityEngine.UI.Toggle toggle = Utility.FindComponentInChildren<UnityEngine.UI.Toggle>(obj, path);
                                toggle.onValueChanged.AddListener((bool val) => { handler0.OnToggleHandler(tag, val); });
                            } else if (evt == "dropdown") {
                                UnityEngine.UI.Dropdown dropdown = Utility.FindComponentInChildren<UnityEngine.UI.Dropdown>(obj, path);
                                dropdown.onValueChanged.AddListener((int val) => { handler0.OnDropdownHandler(tag, val); });
                            } else if (evt == "slider") {
                                UnityEngine.UI.Slider slider = Utility.FindComponentInChildren<UnityEngine.UI.Slider>(obj, path);
                                slider.onValueChanged.AddListener((float val) => { handler0.OnSliderHandler(tag, val); });
                            } else if (evt == "input") {
                                UnityEngine.UI.InputField input = Utility.FindComponentInChildren<UnityEngine.UI.InputField>(obj, path);
                                input.onEndEdit.AddListener((string val) => { handler0.OnInputHandler(tag, val); });
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
                    Dsl.FunctionData callData = funcData.GetParam(i) as Dsl.FunctionData;
                    if (null != callData) {
                        string id = callData.GetId();
                        if (id == "var") {
                            LoadVar(callData);
                        } else if (id == "onevent") {
                            LoadEvent(callData);
                        } else if (id == "inputs") {
                            LoadPaths(m_Inputs, callData);
                        } else if (id == "toggles") {
                            LoadPaths(m_Toggles, callData);
                        } else if (id == "sliders") {
                            LoadPaths(m_Sliders, callData);
                        } else if (id == "dropdowns") {
                            LoadPaths(m_DropDowns, callData);
                        }
                    }
                }
            }
            return true;
        }
        private void LoadCall(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Obj = CodeStructure.Load(callData.GetParam(0));
            }
        }
        private void LoadVar(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                VarInfo info = new VarInfo(
                    CodeStructure.Load(callData.GetParam(0)),
                    CodeStructure.Load(callData.GetParam(1)));
                m_VarInfos.Add(info);
            }
        }
        private void LoadEvent(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                EventInfo info = new EventInfo(
                    CodeStructure.Load(callData.GetParam(0)),
                    CodeStructure.Load(callData.GetParam(1)),
                    CodeStructure.Load(callData.GetParam(2)));
                m_EventInfos.Add(info);
            }
        }

        private class VarInfo
        {
            internal IExpression m_VarName;
            internal IExpression m_ControlPath;
            internal VarInfo(IExpression varName, IExpression controlPath)
            {
                m_VarName = varName;
                m_ControlPath = controlPath;
            }
        }
        private class EventInfo
        {
            internal IExpression m_Event;
            internal IExpression m_Tag;
            internal IExpression m_Path;
            internal EventInfo(IExpression evt, IExpression tag, IExpression path)
            {
                m_Event = evt;
                m_Tag = tag;
                m_Path = path;
            }
        }

        private IExpression m_Obj;
        private List<VarInfo> m_VarInfos = new List<VarInfo>();
        internal List<IExpression> m_Inputs = new List<IExpression>();
        internal List<IExpression> m_Toggles = new List<IExpression>();
        internal List<IExpression> m_Sliders = new List<IExpression>();
        internal List<IExpression> m_DropDowns = new List<IExpression>();
        private List<EventInfo> m_EventInfos = new List<EventInfo>();

        private void LoadPaths(List<IExpression> list, Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IExpression path = CodeStructure.Load(callData.GetParam(i));
                list.Add(path);
            }
        }
        private static void AddVariable(DslCalculator calc, string name, UnityEngine.GameObject control)
        {
            var instance = calc.GetFuncContext<StoryInstance>();
            if (null != instance) {
                instance.SetVariable(name, BoxedValue.FromObject(control));
                UnityEngine.UI.Text text = control.GetComponent<UnityEngine.UI.Text>();
                if (null != text) {
                    instance.SetVariable(string.Format("{0}_Text", name), BoxedValue.FromObject(text));
                }
                UnityEngine.UI.Image image = control.GetComponent<UnityEngine.UI.Image>();
                if (null != image) {
                    instance.SetVariable(string.Format("{0}_Image", name), BoxedValue.FromObject(image));
                }
                UnityEngine.UI.RawImage rawImage = control.GetComponent<UnityEngine.UI.RawImage>();
                if (null != rawImage) {
                    instance.SetVariable(string.Format("{0}_RawImage", name), BoxedValue.FromObject(rawImage));
                }
                UnityEngine.UI.Button button = control.GetComponent<UnityEngine.UI.Button>();
                if (null != button) {
                    instance.SetVariable(string.Format("{0}_Button", name), BoxedValue.From(button));
                }
                UnityEngine.UI.Dropdown dropdown = control.GetComponent<UnityEngine.UI.Dropdown>();
                if (null != dropdown) {
                    instance.SetVariable(string.Format("{0}_Dropdown", name), BoxedValue.FromObject(dropdown));
                }
                UnityEngine.UI.InputField inputField = control.GetComponent<UnityEngine.UI.InputField>();
                if (null != inputField) {
                    instance.SetVariable(string.Format("{0}_Input", name), BoxedValue.FromObject(inputField));
                }
                UnityEngine.UI.Slider slider = control.GetComponent<UnityEngine.UI.Slider>();
                if (null != inputField) {
                    instance.SetVariable(string.Format("{0}_Slider", name), BoxedValue.FromObject(slider));
                }
                UnityEngine.UI.Toggle toggle = control.GetComponent<UnityEngine.UI.Toggle>();
                if (null != toggle) {
                    instance.SetVariable(string.Format("{0}_Toggle", name), BoxedValue.FromObject(toggle));
                }
                UnityEngine.UI.ToggleGroup toggleGroup = control.GetComponent<UnityEngine.UI.ToggleGroup>();
                if (null != toggleGroup) {
                    instance.SetVariable(string.Format("{0}_ToggleGroup", name), BoxedValue.FromObject(toggleGroup));
                }
                UnityEngine.UI.Scrollbar scrollbar = control.GetComponent<UnityEngine.UI.Scrollbar>();
                if (null != scrollbar) {
                    instance.SetVariable(string.Format("{0}_Scrollbar", name), BoxedValue.FromObject(scrollbar));
                }
            }
        }
    }
}
