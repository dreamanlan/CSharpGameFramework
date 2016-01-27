using System;
using System.Collections.Generic;

namespace GameFramework
{
    public class Converter
    {
        private static string[] s_ListSplitString = new string[] { ",", " ", ", ", "|" };

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
        public static List<string> ConvertStringList(string vec)
        {
            List<string> list = new List<string>();
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
        public static ScriptRuntime.Vector4 ConvertVector4D(string vec)
        {
            try {
                string strPos = vec;
                string[] resut = strPos.Split(s_ListSplitString, StringSplitOptions.RemoveEmptyEntries);
                ScriptRuntime.Vector4 vector = new ScriptRuntime.Vector4(
                  Convert.ToSingle(resut[0]), Convert.ToSingle(resut[1]), Convert.ToSingle(resut[2]), Convert.ToSingle(resut[3]));
                return vector;
            } catch (System.Exception ex) {
                LogSystem.Error("ConvertVector4D {0} Exception:{1}/{2}", vec, ex.Message, ex.StackTrace);
                throw;
            }
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
        public static List<ScriptRuntime.Vector2> ConvertVector2DList(string vec)
        {
            List<ScriptRuntime.Vector2> list = new List<ScriptRuntime.Vector2>();
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
        public static List<ScriptRuntime.Vector3> ConvertVector3DList(string vec)
        {
            List<ScriptRuntime.Vector3> list = new List<ScriptRuntime.Vector3>();
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
    }
}
