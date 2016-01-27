using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace GameFramework
{
    public static partial class Utility
    {
        [System.Diagnostics.Conditional("DEBUG")]
        public static void GfxLog(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            SendMessageImpl("GameRoot", "LogToConsole", msg, false);
#if DEBUG
            UnityEngine.Debug.LogWarning(msg);
#endif
        }
        [System.Diagnostics.Conditional("DEBUG")]
        public static void GfxErrorLog(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            SendMessageImpl("GameRoot", "LogToConsole", msg, false);
#if DEBUG
            UnityEngine.Debug.LogError(msg);
#endif
        }
        public static float RadianToDegree(float dir)
        {
            return (float)(dir * 180 / Math.PI);
        }
        public static float DegreeToRadian(float dir)
        {
            return (float)(dir * Math.PI / 180);
        }
        public static GameObject FindChildObject(GameObject fromGameObject, string withName)
        {
            Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < ts.Length; ++i) {
                Transform t = ts[i];
                if (t.gameObject.name == withName) {
                    return t.gameObject;
                }
            }
            return null;
        }
        public static Transform FindChildRecursive(Transform parent, string bonePath)
        {
            Transform t = parent.Find(bonePath);
            if (null != t) {
                return t;
            } else {
                int ct = parent.childCount;
                for (int i = 0; i < ct; ++i) {
                    t = FindChildRecursive(parent.GetChild(i), bonePath);
                    if (null != t) {
                        return t;
                    }
                }
            }
            return null;
        }
        public static Vector3 FrontOfTarget(Vector3 from, Vector3 to, float hitDistance)
        {
            Vector3 value = to + (from - to).normalized * hitDistance;
            return value;
        }
        public static GameObject AttachAsset(GameObject targetObject, GameObject asset, string name)
        {
            GameObject result = null;
            Vector3 pos = targetObject.transform.TransformPoint(asset.transform.position);
            Quaternion rot = targetObject.transform.rotation * asset.transform.rotation;
            result = (GameObject)GameObject.Instantiate(asset, pos, rot);
            result.name = name;
            result.transform.parent = targetObject.transform;
            return result;
        }
        public static GameObject AttachUIAsset(GameObject targetObject, GameObject asset, string name)
        {
            GameObject result = null;
            RectTransform assetRect = (RectTransform)asset.transform;
            result = (GameObject)GameObject.Instantiate(asset);
            result.name = name;
            RectTransform rect = (RectTransform)result.transform;
            rect.SetParent(targetObject.transform, false);
            return result;
        }
        public static void DrawGizmosCircle(Vector3 center, float radius, int step = 16)
        {
            for (int i = 1; i < step + 1; i++) {
                Vector3 pos0 = center + Quaternion.AngleAxis(360.0f * (i - 1) / step, Vector3.up) * Vector3.forward * radius;
                Vector3 pos1 = center + Quaternion.AngleAxis(360.0f * i / step, Vector3.up) * Vector3.forward * radius;
                Gizmos.DrawLine(pos0, pos1);
            }
        }
        public static void DrawGizmosArraw(Vector3 start, Vector3 end)
        {
            Vector3 dir = (end - start).normalized;
            float length = (end - start).magnitude;
            Gizmos.DrawLine(start, end);
            Vector3 left = Quaternion.AngleAxis(45, Vector3.up) * (-dir);
            Gizmos.DrawLine(end, end + left * length * 0.5f);
            Vector3 right = Quaternion.AngleAxis(-45, Vector3.up) * (-dir);
            Gizmos.DrawLine(end, end + right * length * 0.5f);
        }
        public static PublishSubscribeSystem EventSystem
        {
            get
            {
                return s_EventSystem;
            }
        }

        public static void SendMessage(string objname, string msg, object arg)
        {
            SendMessage(objname, msg, arg, false);
        }
        public static void SendMessage(string objname, string msg, object arg, bool needReceiver)
        {
            SendMessageImpl(objname, msg, arg, needReceiver);
        }
        public static void SendMessageWithTag(string objtag, string msg, object arg)
        {
            SendMessageWithTag(objtag, msg, arg, false);
        }
        public static void SendMessageWithTag(string objtag, string msg, object arg, bool needReceiver)
        {
            SendMessageWithTagImpl(objtag, msg, arg, needReceiver);
        }
        
        private static void SendMessageImpl(string objname, string msg, object arg, bool needReceiver)
        {
            GameObject obj = GameObject.Find(objname);
            if (null != obj) {
                try {
                    obj.SendMessage(msg, arg, needReceiver ? SendMessageOptions.RequireReceiver : SendMessageOptions.DontRequireReceiver);
                    if (msg.CompareTo("LogToConsole") != 0)
                        LogSystem.Info("SendMessage {0} {1} {2} {3}", objname, msg, arg, needReceiver);
                } catch (Exception ex) {
                    LogSystem.Error("SendMessage({0} {1} {2} {3}) Exception {4}\n{5}", objname, msg, arg, needReceiver, ex.Message, ex.StackTrace);
                }
            }
        }
        private static void SendMessageWithTagImpl(string objtag, string msg, object arg, bool needReceiver)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(objtag);
            if (null != objs) {
                for (int i = 0; i < objs.Length; i++) {
                    try {
                        objs[i].SendMessage(msg, arg, needReceiver ? SendMessageOptions.RequireReceiver : SendMessageOptions.DontRequireReceiver);
                        if (msg.CompareTo("LogToConsole") != 0)
                            LogSystem.Info("SendMessageWithTag {0} {1} {2} {3}", objtag, msg, arg, needReceiver);
                    } catch (Exception ex) {
                        LogSystem.Error("SendMessageWithTag({0} {1} {2} {3}) Exception {4}\n{5}", objtag, msg, arg, needReceiver, ex.Message, ex.StackTrace);
                    }
                }
            }
        }

        private static PublishSubscribeSystem s_EventSystem = new PublishSubscribeSystem();
    }
}
