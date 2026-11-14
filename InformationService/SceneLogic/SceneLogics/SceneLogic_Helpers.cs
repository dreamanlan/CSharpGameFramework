using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // Shared parameter parsing for scene logic. No spatial index exists here,
    // so all area queries walk the EntityManager linked list on the XZ plane.
    internal static class SceneLogicParamHelper
    {
        public static List<float> Numbers(SceneLogicInfo info)
        {
            List<float> vals = new List<float>();
            SceneLogicConfig sc = info.SceneLogicConfig;
            if (null == sc || null == sc.m_Params) return vals;
            for (int i = 0; i < sc.m_Params.Length; ++i) {
                float v;
                if (float.TryParse(sc.m_Params[i], out v)) vals.Add(v);
            }
            return vals;
        }
        public static string LastWord(SceneLogicInfo info, string def)
        {
            SceneLogicConfig sc = info.SceneLogicConfig;
            if (null == sc || null == sc.m_Params) return def;
            for (int i = sc.m_Params.Length - 1; i >= 0; --i) {
                float v;
                if (!float.TryParse(sc.m_Params[i], out v)) return sc.m_Params[i];
            }
            return def;
        }
        public static bool InPolygon(List<float> pts, int count, float x, float z)
        {
            if (count < 3) return false;
            bool inside = false;
            for (int i = 0, j = count - 1; i < count; j = i++) {
                float xi = pts[i * 2], zi = pts[i * 2 + 1];
                float xj = pts[j * 2], zj = pts[j * 2 + 1];
                if (((zi > z) != (zj > z)) && (x < (xj - xi) * (z - zi) / (zj - zi) + xi)) inside = !inside;
            }
            return inside;
        }
        public static bool InCircle(List<float> vals, float x, float z)
        {
            if (vals.Count < 3) return false;
            float radius = vals[vals.Count - 1];
            float dx = x - vals[0];
            float dz = z - vals[1];
            return dx * dx + dz * dz <= radius * radius;
        }
        public static bool IsUser(EntityInfo entity)
        {
            return null != entity && entity.EntityType == (int)EntityTypeEnum.Hero;
        }
        public static Vector3 Position(EntityInfo entity)
        {
            return entity.GetMovementStateInfo().GetPosition3D();
        }
        public static void Send(SceneLogicInfo info, string msgId, int arg1)
        {
            SceneLogicSendStoryMessageStatic.Send(info, msgId, arg1);
        }
    }

    internal static class SceneLogicSendStoryMessageStatic
    {
        public static void Send(SceneLogicInfo info, string msgId, int arg1)
        {
            if (null == AbstractSceneLogic.OnSceneLogicSendStoryMessage) return;
            BoxedValueList args = null != AbstractSceneLogic.OnSceneLogicNewBoxedValueList
                ? AbstractSceneLogic.OnSceneLogicNewBoxedValueList(info) : null;
            if (null == args) return;
            args.Add(arg1);
            AbstractSceneLogic.OnSceneLogicSendStoryMessage(info, msgId, args);
        }
        public static void Send2(SceneLogicInfo info, string msgId, int arg1, int arg2)
        {
            if (null == AbstractSceneLogic.OnSceneLogicSendStoryMessage) return;
            BoxedValueList args = null != AbstractSceneLogic.OnSceneLogicNewBoxedValueList
                ? AbstractSceneLogic.OnSceneLogicNewBoxedValueList(info) : null;
            if (null == args) return;
            args.Add(arg1);
            args.Add(arg2);
            AbstractSceneLogic.OnSceneLogicSendStoryMessage(info, msgId, args);
        }
    }
}
