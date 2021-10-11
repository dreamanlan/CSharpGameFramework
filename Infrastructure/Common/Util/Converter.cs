using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameFramework
{
    public static class Converter
    {
        private static string[] s_ListSplitString = new string[] { ";", " ", "|" };

        public static string IntList2String(params int[] vals)
        {
            return IntList2String((IList<int>)vals);
        }
        public static string IntList2String(IList<int> vals)
        {
            if (vals.Count <= 0)
                return string.Empty;
            string[] strVals = new string[vals.Count];
            for (int i = 0; i < vals.Count; ++i) {
                strVals[i] = vals[i].ToString();
            }
            return StringList2String(strVals);
        }
        public static string FloatList2String(params float[] vals)
        {
            return FloatList2String((IList<float>)vals);
        }
        public static string FloatList2String(IList<float> vals)
        {
            if (vals.Count <= 0)
                return string.Empty;
            string[] strVals = new string[vals.Count];
            for (int i = 0; i < vals.Count; ++i) {
                strVals[i] = vals[i].ToString();
            }
            return StringList2String(strVals);
        }
        public static string BoolList2String(params bool[] vals)
        {
            return BoolList2String((IList<bool>)vals);
        }
        public static string BoolList2String(IList<bool> vals)
        {
            if (vals.Count <= 0)
                return string.Empty;
            string[] strVals = new string[vals.Count];
            for (int i = 0; i < vals.Count; ++i) {
                strVals[i] = vals[i] ? "1" : "0";
            }
            return StringList2String(strVals);
        }
        public static string StringList2String(IList<string> vals)
        {
            string[] strVals = new string[vals.Count];
            for (int i = 0; i < vals.Count; ++i) {
                strVals[i] = vals[i];
            }
            return StringList2String(strVals);
        }
        public static string StringList2String(params string[] vals)
        {
            return string.Join(";", vals);
        }
        public static List<T> ConvertNumericList<T>(string vec)
        {
            List<T> list = new List<T>();
            try {
                string strPos = vec;
                string[] resut = strPos.Split(s_ListSplitString, StringSplitOptions.RemoveEmptyEntries);
                if (resut != null && resut.Length > 0 && resut[0] != "") {
                    for (int index = 0; index < resut.Length; index++) {
                        list.Add((T)Convert.ChangeType(resut[index], typeof(T)));
                    }
                }
            } catch (System.Exception ex) {
                list.Clear();
                LogSystem.Error("ConvertNumericList {0} Exception:{1}/{2}", vec, ex.Message, ex.StackTrace);
                throw;
            }

            return list;
        }
        public static IntList ConvertIntList(string vec)
        {
            IntList list = new IntList();
            try {
                string strPos = vec;
                string[] resut = strPos.Split(s_ListSplitString, StringSplitOptions.RemoveEmptyEntries);
                if (resut != null && resut.Length > 0 && resut[0] != "") {
                    for (int index = 0; index < resut.Length; index++) {
                        list.Add((int)Convert.ChangeType(resut[index], typeof(int)));
                    }
                }
            } catch (System.Exception ex) {
                list.Clear();
                LogSystem.Error("ConvertIntList {0} Exception:{1}/{2}", vec, ex.Message, ex.StackTrace);
                throw;
            }

            return list;
        }
        public static UintList ConvertUintList(string vec)
        {
            UintList list = new UintList();
            try {
                string strPos = vec;
                string[] resut = strPos.Split(s_ListSplitString, StringSplitOptions.RemoveEmptyEntries);
                if (resut != null && resut.Length > 0 && resut[0] != "") {
                    for (int index = 0; index < resut.Length; index++) {
                        list.Add((uint)Convert.ChangeType(resut[index], typeof(uint)));
                    }
                }
            } catch (System.Exception ex) {
                list.Clear();
                LogSystem.Error("ConvertUintList {0} Exception:{1}/{2}", vec, ex.Message, ex.StackTrace);
                throw;
            }

            return list;
        }
        public static FloatList ConvertFloatList(string vec)
        {
            FloatList list = new FloatList();
            try {
                string strPos = vec;
                string[] resut = strPos.Split(s_ListSplitString, StringSplitOptions.RemoveEmptyEntries);
                if (resut != null && resut.Length > 0 && resut[0] != "") {
                    for (int index = 0; index < resut.Length; index++) {
                        list.Add((float)Convert.ChangeType(resut[index], typeof(float)));
                    }
                }
            } catch (System.Exception ex) {
                list.Clear();
                LogSystem.Error("ConvertFloatList {0} Exception:{1}/{2}", vec, ex.Message, ex.StackTrace);
                throw;
            }

            return list;
        }
        public static DoubleList ConvertDoubleList(string vec)
        {
            DoubleList list = new DoubleList();
            try {
                string strPos = vec;
                string[] resut = strPos.Split(s_ListSplitString, StringSplitOptions.RemoveEmptyEntries);
                if (resut != null && resut.Length > 0 && resut[0] != "") {
                    for (int index = 0; index < resut.Length; index++) {
                        list.Add((double)Convert.ChangeType(resut[index], typeof(double)));
                    }
                }
            } catch (System.Exception ex) {
                list.Clear();
                LogSystem.Error("ConvertDoubleList {0} Exception:{1}/{2}", vec, ex.Message, ex.StackTrace);
                throw;
            }

            return list;
        }
        public static BoolList ConvertBoolList(string vec)
        {
            BoolList list = new BoolList();
            try {
                string[] resut = vec.Split(s_ListSplitString, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < resut.Length; ++i) {
                    string str = resut[i];
                    if (str == "true" || str.Length > 0 && char.IsNumber(str[0]) && int.Parse(str) != 0) {
                        list.Add(true);
                    } else {
                        list.Add(false);
                    }
                }
                return list;
            } catch (System.Exception ex) {
                LogSystem.Error("ConvertBoolList {0} Exception:{1}/{2}", vec, ex.Message, ex.StackTrace);
                throw;
            }
        }
        public static StrList ConvertStringList(string vec)
        {
            StrList list = new StrList();
            try {
                string[] resut = vec.Split(s_ListSplitString, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < resut.Length; ++i) {
                    string str = resut[i];
                    list.Add(str);
                }
                return list;
            } catch (System.Exception ex) {
                LogSystem.Error("ConvertStringList {0} Exception:{1}/{2}", vec, ex.Message, ex.StackTrace);
                throw;
            }
        }
        public static ScriptRuntime.Vector2 ConvertVector2D(string vec)
        {
            try {
                string strPos = vec;
                string[] resut = strPos.Split(s_ListSplitString, StringSplitOptions.RemoveEmptyEntries);
                ScriptRuntime.Vector2 vector = new ScriptRuntime.Vector2(Convert.ToSingle(resut[0]), Convert.ToSingle(resut[1]));
                return vector;
            } catch (System.Exception ex) {
                LogSystem.Error("ConvertVector2D {0} Exception:{1}/{2}", vec, ex.Message, ex.StackTrace);
                throw;
            }
        }
        public static ScriptRuntime.Vector3 ConvertVector3D(string vec)
        {
            try {
                string strPos = vec;
                string[] resut = strPos.Split(s_ListSplitString, StringSplitOptions.RemoveEmptyEntries);
                ScriptRuntime.Vector3 vector = new ScriptRuntime.Vector3(
                  Convert.ToSingle(resut[0]), Convert.ToSingle(resut[1]), Convert.ToSingle(resut[2]));
                return vector;
            } catch (System.Exception ex) {
                LogSystem.Error("ConvertVector3D {0} Exception:{1}/{2}", vec, ex.Message, ex.StackTrace);
                throw;
            }
        }
        public static Vector2List ConvertVector2DList(string vec)
        {
            Vector2List list = new Vector2List();
            try {
                string strPos = vec;
                string[] resut = strPos.Split(s_ListSplitString, StringSplitOptions.RemoveEmptyEntries);
                if (resut != null && resut.Length > 0 && resut[0] != "") {
                    for (int index = 0; index < resut.Length; ) {
                        list.Add(new ScriptRuntime.Vector2(Convert.ToSingle(resut[index]), Convert.ToSingle(resut[index + 1])));
                        index += 2;
                    }
                }
            } catch (System.Exception ex) {
                list.Clear();
                LogSystem.Error("ConvertVector2DList {0} Exception:{1}/{2}", vec, ex.Message, ex.StackTrace);
                throw;
            }
            return list;
        }
        public static Vector3List ConvertVector3DList(string vec)
        {
            Vector3List list = new Vector3List();
            try {
                string strPos = vec;
                string[] resut = strPos.Split(s_ListSplitString, StringSplitOptions.RemoveEmptyEntries);
                if (resut != null && resut.Length > 0 && resut[0] != "") {
                    for (int index = 0; index < resut.Length; ) {
                        list.Add(new ScriptRuntime.Vector3(Convert.ToSingle(resut[index]),
                              Convert.ToSingle(resut[index + 1]),
                              Convert.ToSingle(resut[index + 2])));
                        index += 3;
                    }
                }
            } catch (System.Exception ex) {
                list.Clear();
                LogSystem.Error("ConvertVector3DList {0} Exception:{1}/{2}", vec, ex.Message, ex.StackTrace);
                throw;
            }
            return list;
        }
        public static T ConvertStrToEnum<T>(string name)
        {
            try {
                return (T)(Enum.Parse(typeof(T), name));
            } catch (System.Exception ex) {
                LogSystem.Error("ConvertStrToEnum {0} Exception:{1}/{2}", name, ex.Message, ex.StackTrace);
                throw;
            }
        }
        public static string ConvertEnumToStr<T>(T tVal)
        {
            try {
                return Enum.GetName(typeof(T), tVal);
            } catch (System.Exception ex) {
                LogSystem.Error("ConvertEnumToStr {0} Exception:{1}/{2}", tVal, ex.Message, ex.StackTrace);
                throw;
            }
        }
        public static void CastArgsForCall(Type t, string method, BindingFlags flags, params object[] args)
        {
            var mis = t.GetMember(method, flags);
            foreach (var mi in mis) {
                var info = mi as MethodInfo;
                if (null != info) {
                    var pis = info.GetParameters();
                    if (pis.Length == args.Length) {
                        for (int i = 0; i < pis.Length; ++i) {
                            if (null != args[i] && args[i].GetType() != pis[i].ParameterType && args[i].GetType().Name != "MonoType") {
                                args[i] = CastTo(pis[i].ParameterType, args[i]);
                            }
                        }
                        break;
                    }
                }
            }
        }
        public static void CastArgsForSet(Type t, string property, BindingFlags flags, params object[] args)
        {
            var p = t.GetProperty(property, flags);
            if (null != p) {
                var info = p.GetSetMethod(true);
                if (null != info) {
                    var pis = info.GetParameters();
                    if (pis.Length == args.Length) {
                        for (int i = 0; i < pis.Length; ++i) {
                            if (null != args[i] && args[i].GetType() != pis[i].ParameterType && args[i].GetType().Name != "MonoType") {
                                args[i] = CastTo(pis[i].ParameterType, args[i]);
                            }
                        }
                    }
                }
            } else {
                var f = t.GetField(property, flags);
                if (null != f && args.Length == 1 && null != args[0] && args[0].GetType() != f.FieldType && args[0].GetType().Name != "MonoType") {
                    args[0] = CastTo(f.FieldType, args[0]);
                }
            }
        }
        public static void CastArgsForGet(Type t, string property, BindingFlags flags, params object[] args)
        {
            var p = t.GetProperty(property, flags);
            if (null != p) {
                var info = p.GetGetMethod(true);
                if (null != info) {
                    var pis = info.GetParameters();
                    if (pis.Length == args.Length) {
                        for (int i = 0; i < pis.Length; ++i) {
                            if (null != args[i] && args[i].GetType() != pis[i].ParameterType && args[i].GetType().Name != "MonoType") {
                                args[i] = CastTo(pis[i].ParameterType, args[i]);
                            }
                        }
                    }
                }
            } else {
                var f = t.GetField(property, flags);
                if (null != f && args.Length == 0) {
                }
            }
        }
        public static T CastTo<T>(object obj)
        {
            if (obj is BoxedValue) {
                return ((BoxedValue)obj).Get<T>();
            }
            else if (obj is T) {
                return (T)obj;
            }
            else {
                try {
                    return (T)Convert.ChangeType(obj, typeof(T));
                }
                catch (OverflowException) {
                    return (T)Convert.ChangeType(obj.ToString(), typeof(T));
                }
                catch {
                    return default(T);
                }
            }
        }
        public static object CastTo(Type t, object obj)
        {
            if (null == obj)
                return null;
            Type st = obj.GetType();
            if (obj is BoxedValue) {
                return ((BoxedValue)obj).Get(t);
            }
            else if (t.IsAssignableFrom(st) || st.IsSubclassOf(t)) {
                return obj;
            }
            else {
                try {
                    return Convert.ChangeType(obj, t);
                }
                catch (OverflowException) {
                    return Convert.ChangeType(obj.ToString(), t);
                }
                catch {
                    return null;
                }
            }
        }
        public static string FileContent2Utf8String(byte[] bytes)
        {
            if (null == bytes)
                return string.Empty;
            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF) {
                return System.Text.Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
            } else {
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
        }
    }
}
