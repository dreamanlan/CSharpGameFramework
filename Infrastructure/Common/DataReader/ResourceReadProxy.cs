using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ScriptableFramework
{
    public delegate byte[] ReadAsArrayDelegation(string path);
    public static class ResourceReadProxy
    {
        public static ReadAsArrayDelegation OnReadAsArray;
        public static MemoryStream ReadAsMemoryStream(string filePath)
        {
            try {
                byte[] buffer = ReadAsArray(filePath);
                if (buffer == null) {
                    LogSystem.Error("Err ReadFileAsMemoryStream failed:{0}\n", filePath);
                    return null;
                }
                return new MemoryStream(buffer);
            } catch (Exception e) {
                LogSystem.Error("Exception:{0}\n", e.Message);
                Helper.LogCallStack();
                return null;
            }
        }

        public static byte[] ReadAsArray(string filePath)
        {
            byte[] buffer = null;
            try {
                if (OnReadAsArray != null) {
                    buffer = OnReadAsArray(filePath);
                } else {
                    LogSystem.Error("ReadFileByEngine handler have not register: {0}", filePath);
                }
            } catch (Exception e) {
                LogSystem.Error("Exception:{0}\n", e.Message);
                Helper.LogCallStack();
                return null;
            }
            return buffer;
        }

        public static bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
