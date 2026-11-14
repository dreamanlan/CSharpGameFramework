using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Assertions.Must;
using StoryScript;
using StoryScript.DslExpression;
using UnityEngine.Profiling;
using UnityEngine.UI;
using Unity.Profiling;
using System.Linq;

namespace GmCommands
{
    internal static class AppGmRegister
    {
        internal static void Register(DslCalculatorApiRegistry registry)
        {
            //register Gm command
            registry.Register("gm", "gm(str) command", new ExpressionFactoryHelper<GmCommand>());
            registry.Register("reloadstartups", "reloadstartups() command", new ExpressionFactoryHelper<ReloadStartupsCommand>());

            registry.Register("loadui", "loadui(ui_name_dsl) command", new ExpressionFactoryHelper<LoadUiCommand>());
            registry.Register("showui", "showui() command", new ExpressionFactoryHelper<ShowUiCommand>());
            registry.Register("hideui", "hideui() command", new ExpressionFactoryHelper<HideUiCommand>());

            registry.Register("cam_info", "cam_info() - runtime camera pose, fov and SimpleCameraController state (read-only)", new ExpressionFactoryHelper<CamInfoCommand>());
            registry.Register("cam_set", "cam_set(x,y,z[,pitch,yaw,roll]) - absolute runtime camera pose", new ExpressionFactoryHelper<CamSetCommand>());
            registry.Register("cam_move", "cam_move(dx,dy,dz[,local]) - relative runtime camera translation", new ExpressionFactoryHelper<CamMoveCommand>());
            registry.Register("cam_look", "cam_look(x,y,z) - aim runtime camera at a world point", new ExpressionFactoryHelper<CamLookCommand>());
            registry.Register("cam_control", "cam_control(enable) - enable/disable SimpleCameraController; OnEnable re-syncs state from transform", new ExpressionFactoryHelper<CamControlCommand>());
            registry.Register("cam_sweep", "cam_sweep(enable[,degPerSec,radius,cx,cy,cz]) - continuous non-blocking runtime camera sweep", new ExpressionFactoryHelper<CamSweepCommand>());

            Game.RegisterGmApi(registry);
        }
    }
    internal sealed class GmCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string gmToServer = operands[0].GetString();
            LogSystem.Warn("todo: gm {0}", gmToServer);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ReloadStartupsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            bool runGm = operands[0].GetBool();
            Game.RunStartup(runGm);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class LoadUiCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string uiRes = operands[0].GetString();

            var fv = GmRootScript.GameObj;
            if (null != fv) {
                var uihandler = fv.GetComponent<UiHandler>();
                if (null != uihandler) {
                    uihandler.LoadUi(uiRes);
                }
            }
            else {
                LogSystem.Error("can't find GmScript");
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ShowUiCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var fv = GmRootScript.GameObj;
            if (null != fv) {
                var uihandler = fv.GetComponent<UiHandler>();
                if (null != uihandler) {
                    uihandler.ShowUi();
                }
            }
            else {
                LogSystem.Error("can't find GmScript");
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class HideUiCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var fv = GmRootScript.GameObj;
            if (null != fv) {
                var uihandler = fv.GetComponent<UiHandler>();
                if (null != uihandler) {
                    uihandler.HideUi();
                }
            }
            else {
                LogSystem.Error("can't find GmScript");
            }
            return BoxedValue.NullObject;
        }
    }
    // ---------- Runtime camera DSL commands (thin wrappers) ----------
    // The camera is driven by the project's SimpleCameraController: its Update() owns the
    // transform and rebuilds it from an internal CameraState every frame. All commands here
    // only parse DSL operands and call the controller's public Automation API. There is no
    // second camera implementation and no transform write from this side.
    internal static class CamCommandsUtil
    {
        public static UnityTemplateProjects.SimpleCameraController FindController(out string source)
        {
            source = "none";
            var ctrl = UnityEngine.Object.FindFirstObjectByType<UnityTemplateProjects.SimpleCameraController>();
            if (ctrl != null)
            {
                source = ctrl.gameObject.name;
                return ctrl;
            }
            // Fall back to locating the camera, then its controller component.
            Camera cam = Camera.main;
            if (cam == null)
            {
                foreach (Camera c in Camera.allCameras)
                {
                    if (c != null && c.cameraType == CameraType.Game && c.isActiveAndEnabled)
                    {
                        cam = c;
                        break;
                    }
                }
            }
            if (cam == null)
            {
                GameObject go = GameObject.Find("Camera");
                if (go != null) cam = go.GetComponent<Camera>();
            }
            if (cam == null) return null;
            source = cam.name;
            return cam.GetComponent<UnityTemplateProjects.SimpleCameraController>();
        }

        public static string Fmt(Vector3 v)
        {
            return string.Format("({0:F2},{1:F2},{2:F2})", v.x, v.y, v.z);
        }
        public static float F(IList<BoxedValue> o, int i, float def)
        {
            return o.Count > i ? o[i].GetFloat() : def;
        }
        public static string Status(UnityTemplateProjects.SimpleCameraController ctrl, string source)
        {
            Vector3 pos, euler;
            ctrl.ReadPose(out pos, out euler);
            return string.Format("controller={0} pos={1} euler={2} sweeping={3}",
                source, Fmt(pos), Fmt(euler), ctrl.IsSweeping);
        }
    }

    internal sealed class CamInfoCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var sb = new StringBuilder();
            string src;
            var ctrl = CamCommandsUtil.FindController(out src);
            sb.AppendLine("mode=play source=" + src);
            if (ctrl == null)
            {
                sb.AppendLine("ERROR: SimpleCameraController not found; runtime camera commands unavailable");
                return BoxedValue.FromString(sb.ToString());
            }
            var cam = ctrl.GetComponent<Camera>();
            if (cam != null)
                sb.AppendLine(string.Format("fov={0:F1} near={1:F2} far={2:F1} cameraType={3}",
                    cam.fieldOfView, cam.nearClipPlane, cam.farClipPlane, cam.cameraType));
            sb.AppendLine("enabled=" + ctrl.enabled);
            sb.AppendLine(CamCommandsUtil.Status(ctrl, src));
            return BoxedValue.FromString(sb.ToString());
        }
    }

    internal sealed class CamSetCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 3) return BoxedValue.FromString("ERROR: cam_set(x,y,z[,pitch,yaw,roll])");
            string src;
            var ctrl = CamCommandsUtil.FindController(out src);
            if (ctrl == null) return BoxedValue.FromString("ERROR: SimpleCameraController not found");
            Vector3 curPos, curEuler;
            ctrl.ReadPose(out curPos, out curEuler);
            var pos = new Vector3(CamCommandsUtil.F(operands, 0, 0),
                CamCommandsUtil.F(operands, 1, 0), CamCommandsUtil.F(operands, 2, 0));
            var euler = new Vector3(CamCommandsUtil.F(operands, 3, curEuler.x),
                CamCommandsUtil.F(operands, 4, curEuler.y), CamCommandsUtil.F(operands, 5, curEuler.z));
            ctrl.ApplyPose(pos, euler);
            return BoxedValue.FromString("cam_set: " + CamCommandsUtil.Status(ctrl, src));
        }
    }

    internal sealed class CamMoveCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 3) return BoxedValue.FromString("ERROR: cam_move(dx,dy,dz[,local])");
            string src;
            var ctrl = CamCommandsUtil.FindController(out src);
            if (ctrl == null) return BoxedValue.FromString("ERROR: SimpleCameraController not found");
            var d = new Vector3(CamCommandsUtil.F(operands, 0, 0),
                CamCommandsUtil.F(operands, 1, 0), CamCommandsUtil.F(operands, 2, 0));
            bool local = operands.Count > 3 && operands[3].GetBool();
            ctrl.MoveBy(d, local);
            return BoxedValue.FromString("cam_move: " + CamCommandsUtil.Status(ctrl, src));
        }
    }

    internal sealed class CamLookCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 3) return BoxedValue.FromString("ERROR: cam_look(x,y,z)");
            string src;
            var ctrl = CamCommandsUtil.FindController(out src);
            if (ctrl == null) return BoxedValue.FromString("ERROR: SimpleCameraController not found");
            var target = new Vector3(CamCommandsUtil.F(operands, 0, 0),
                CamCommandsUtil.F(operands, 1, 0), CamCommandsUtil.F(operands, 2, 0));
            Vector3 pos, euler;
            ctrl.ReadPose(out pos, out euler);
            if ((target - pos).sqrMagnitude < 0.0001f)
                return BoxedValue.FromString("ERROR: target == camera position");
            ctrl.AimAt(target);
            return BoxedValue.FromString("cam_look: " + CamCommandsUtil.Status(ctrl, src));
        }
    }

    internal sealed class CamControlCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            bool enable = operands.Count < 1 || operands[0].GetBool();
            string src;
            var ctrl = CamCommandsUtil.FindController(out src);
            if (ctrl == null) return BoxedValue.FromString("ERROR: SimpleCameraController not found");
            ctrl.enabled = enable;
            ctrl.SetSweep(false, 0f, 0f, Vector3.zero);
            return BoxedValue.FromString(string.Format("cam_control: enabled={0} on {1} (sweep stopped)", enable, src));
        }
    }

    internal sealed class CamSweepCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            bool enable = operands.Count < 1 || operands[0].GetBool();
            string src;
            var ctrl = CamCommandsUtil.FindController(out src);
            if (ctrl == null) return BoxedValue.FromString("ERROR: SimpleCameraController not found");
            var center = new Vector3(CamCommandsUtil.F(operands, 3, 0),
                CamCommandsUtil.F(operands, 4, 0), CamCommandsUtil.F(operands, 5, 0));
            float speed = CamCommandsUtil.F(operands, 1, 12f);
            float radius = CamCommandsUtil.F(operands, 2, 0f);
            if (enable) ctrl.enabled = true;
            ctrl.SetSweep(enable, speed, radius, center);
            return BoxedValue.FromString(enable
                ? string.Format("cam_sweep: ON speed={0:F1}deg/s radius={1:F1} center={2} on {3}",
                    speed, radius, CamCommandsUtil.Fmt(center), src)
                : "cam_sweep: stopped");
        }
    }
}

