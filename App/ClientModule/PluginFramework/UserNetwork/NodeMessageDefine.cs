using System;
using LitJson;
using GameFrameworkMessage;

namespace ScriptableFramework.Network
{
    internal sealed class NodeMessage
    {
        internal int m_ID = -1;
        internal object m_NodeHeader = null;
        internal object m_ProtoData = null;

        internal NodeMessage(int id)
        {
            m_ID = id;
        }
        internal NodeMessage(LobbyMessageDefine id)
        {
            m_ID = (int)id;
        }
        internal NodeMessage(LobbyMessageDefine id, string account)
        {
            m_ID = (int)id;
            SetHeaderWithAccount(account);
        }
        internal NodeMessage(LobbyMessageDefine id, ulong guid)
        {
            m_ID = (int)id;
            SetHeaderWithGuid(guid);
        }
        internal NodeMessage(LobbyMessageDefine id, string account, ulong guid)
        {
            m_ID = (int)id;
            SetHeaderWithAccountAndGuid(account, guid);
        }
        internal void SetHeaderWithAccount(string account)
        {
            NodeMessageWithAccount header = m_NodeHeader as NodeMessageWithAccount;
            if (null == header) {
                header = new NodeMessageWithAccount();
                m_NodeHeader = header;
            }
            header.m_Account = account;
        }
        internal void SetHeaderWithGuid(ulong guid)
        {
            NodeMessageWithGuid header = m_NodeHeader as NodeMessageWithGuid;
            if (null == header) {
                header = new NodeMessageWithGuid();
                m_NodeHeader = header;
            }
            header.m_Guid = guid;
        }
        internal void SetHeaderWithAccountAndGuid(string account, ulong guid)
        {
            NodeMessageWithAccountAndGuid header = m_NodeHeader as NodeMessageWithAccountAndGuid;
            if (null == header) {
                header = new NodeMessageWithAccountAndGuid();
                m_NodeHeader = header;
            }
            header.m_Account = account;
            header.m_Guid = guid;
        }
    }

    internal delegate void NodeMessageHandlerDelegate(NodeMessage msg);

    internal static class JsonDataExtensions
    {
        //--------------------------------------------------------------------------
        internal static bool AsBoolean(this JsonData data)
        {
            if (null == data)
                return false;
            bool ret = false;
            if (data.IsInt) {
                ret = (int)data != 0;
            }
            else if (data.IsLong) {
                ret = (long)data != 0;
            }
            else if (data.IsDouble) {
                ret = (float)data != 0;
            }
            else if (data.IsBoolean) {
                ret = (bool)data;
            }
            else if (data.IsString) {
                ret = long.Parse((string)data) != 0;
            }
            return ret;
        }
        internal static int AsInt(this JsonData data)
        {
            if (null == data)
                return 0;
            int ret = 0;
            if (data.IsInt) {
                ret = (int)data;
            }
            else if (data.IsLong) {
                ret = (int)(long)data;
            }
            else if (data.IsDouble) {
                ret = (int)(float)(double)data;
            }
            else if (data.IsBoolean) {
                ret = ((bool)data ? 1 : 0);
            }
            else if (data.IsString) {
                ret = int.Parse((string)data);
            }
            return ret;
        }
        internal static long AsLong(this JsonData data)
        {
            if (null == data)
                return 0;
            long ret = 0;
            if (data.IsInt) {
                ret = (int)data;
            }
            else if (data.IsLong) {
                ret = (long)data;
            }
            else if (data.IsDouble) {
                ret = (int)(float)(double)data;
            }
            else if (data.IsBoolean) {
                ret = ((bool)data ? 1 : 0);
            }
            else if (data.IsString) {
                ret = long.Parse((string)data);
            }
            return ret;
        }
        internal static float AsDouble(this JsonData data)
        {
            if (null == data)
                return 0;
            float ret = 0;
            if (data.IsInt) {
                ret = (int)data;
            }
            else if (data.IsLong) {
                ret = (long)data;
            }
            else if (data.IsDouble) {
                ret = (float)data;
            }
            else if (data.IsBoolean) {
                ret = ((bool)data ? 1 : 0);
            }
            else if (data.IsString) {
                ret = float.Parse((string)data);
            }
            return ret;
        }
        internal static string AsString(this JsonData data)
        {
            if (null == data)
                return "";
            string ret = "";
            if (data.IsInt) {
                ret = ((int)data).ToString();
            }
            else if (data.IsLong) {
                ret = ((long)data).ToString();
            }
            else if (data.IsDouble) {
                ret = ((int)(float)(double)data).ToString();
            }
            else if (data.IsBoolean) {
                ret = ((bool)data ? 1 : 0).ToString();
            }
            else if (data.IsString) {
                ret = (string)data;
            }
            return ret;
        }
        internal static uint AsUint(this JsonData data)
        {
            return (uint)data.AsInt();
        }
        internal static ulong AsUlong(this JsonData data)
        {
            return (ulong)data.AsLong();
        }
        internal static float AsFloat(this JsonData data)
        {
            return (float)data.AsDouble();
        }
        //--------------------------------------------------------------------------
        internal static bool Get(this JsonData data, ref bool val)
        {
            bool ret = false;
            if (null != data && data.IsBoolean) {
                ret = true;
                val = (bool)data;
            }
            return ret;
        }
        internal static bool Get(this JsonData data, ref int val)
        {
            bool ret = false;
            if (null != data && (data.IsDouble || data.IsInt || data.IsLong)) {
                ret = true;
                val = data.AsInt();
            }
            return ret;
        }
        internal static bool Get(this JsonData data, ref long val)
        {
            bool ret = false;
            if (null != data && (data.IsDouble || data.IsInt || data.IsLong)) {
                ret = true;
                val = data.AsLong();
            }
            return ret;
        }
        internal static bool Get(this JsonData data, ref double val)
        {
            bool ret = false;
            if (null != data && (data.IsDouble || data.IsInt || data.IsLong)) {
                ret = true;
                val = data.AsDouble();
            }
            return ret;
        }
        internal static bool Get(this JsonData data, ref string val)
        {
            bool ret = false;
            if (null != data && data.IsString) {
                ret = true;
                val = (string)data;
            }
            return ret;
        }
        internal static bool Get(this JsonData data, ref uint val)
        {
            int temp = 0;
            bool ret = Get(data, ref temp);
            if (ret) {
                val = (uint)temp;
            }
            return ret;
        }
        internal static bool Get(this JsonData data, ref ulong val)
        {
            long temp = 0;
            bool ret = Get(data, ref temp);
            if (ret) {
                val = (ulong)temp;
            }
            return ret;
        }
        internal static bool Get(this JsonData data, ref float val)
        {
            double temp = 0;
            bool ret = Get(data, ref temp);
            if (ret) {
                val = (float)temp;
            }
            return ret;
        }
        //--------------------------------------------------------------------------
        internal static bool Get(this JsonData data, string key, ref bool val)
        {
            bool ret = false;
            if (null != data && data.IsObject) {
                ret = data[key].Get(ref val);
            }
            return ret;
        }
        internal static bool Get(this JsonData data, string key, ref int val)
        {
            bool ret = false;
            if (null != data && data.IsObject) {
                ret = data[key].Get(ref val);
            }
            return ret;
        }
        internal static bool Get(this JsonData data, string key, ref long val)
        {
            bool ret = false;
            if (null != data && data.IsObject) {
                ret = data[key].Get(ref val);
            }
            return ret;
        }
        internal static bool Get(this JsonData data, string key, ref double val)
        {
            bool ret = false;
            if (null != data && data.IsObject) {
                ret = data[key].Get(ref val);
            }
            return ret;
        }
        internal static bool Get(this JsonData data, string key, ref string val)
        {
            bool ret = false;
            if (null != data && data.IsObject) {
                ret = data[key].Get(ref val);
            }
            return ret;
        }
        internal static bool Get(this JsonData data, string key, ref uint val)
        {
            bool ret = false;
            if (null != data && data.IsObject) {
                ret = data[key].Get(ref val);
            }
            return ret;
        }
        internal static bool Get(this JsonData data, string key, ref ulong val)
        {
            long temp = 0;
            bool ret = Get(data, key, ref temp);
            if (ret) {
                val = (ulong)temp;
            }
            return ret;
        }
        internal static bool Get(this JsonData data, string key, ref float val)
        {
            double temp = 0;
            bool ret = Get(data, key, ref temp);
            if (ret) {
                val = (float)temp;
            }
            return ret;
        }
        //--------------------------------------------------------------------------
        internal static void Set(this JsonData data, string key, bool val)
        {
            data[key] = val;
        }
        internal static void Set(this JsonData data, string key, int val)
        {
            data[key] = val;
        }
        internal static void Set(this JsonData data, string key, long val)
        {
            data[key] = val;
        }
        internal static void Set(this JsonData data, string key, double val)
        {
            data[key] = val;
        }
        internal static void Set(this JsonData data, string key, string val)
        {
            data[key] = val;
        }
        internal static void Set(this JsonData data, string key, uint val)
        {
            data[key] = (int)val;
        }
        internal static void Set(this JsonData data, string key, ulong val)
        {
            data[key] = (long)val;
        }
        internal static void Set(this JsonData data, string key, float val)
        {
            data[key] = (float)val;
        }
        internal static void Set(this JsonData data, string key, int[] val)
        {
            string json_string = "";
            for (int i = 0; i < val.Length; i++) {
                json_string += val[i].ToString();
                if (val.Length - 1 != i) {
                    json_string += "|";
                }
            }
            data[key] = (string)json_string;
        }
        //--------------------------------------------------------------------------
        internal static bool Get(this JsonData data, int index, ref bool val)
        {
            bool ret = false;
            if (null != data && (data.IsObject || data.IsArray) && data.Count > index) {
                ret = data[index].Get(ref val);
            }
            return ret;
        }
        internal static bool Get(this JsonData data, int index, ref int val)
        {
            bool ret = false;
            if (null != data && (data.IsObject || data.IsArray) && data.Count > index) {
                ret = data[index].Get(ref val);
            }
            return ret;
        }
        internal static bool Get(this JsonData data, int index, ref long val)
        {
            bool ret = false;
            if (null != data && (data.IsObject || data.IsArray) && data.Count > index) {
                ret = data[index].Get(ref val);
            }
            return ret;
        }
        internal static bool Get(this JsonData data, int index, ref double val)
        {
            bool ret = false;
            if (null != data && (data.IsObject || data.IsArray) && data.Count > index) {
                ret = data[index].Get(ref val);
            }
            return ret;
        }
        internal static bool Get(this JsonData data, int index, ref string val)
        {
            bool ret = false;
            if (null != data && (data.IsObject || data.IsArray) && data.Count > index) {
                ret = data[index].Get(ref val);
            }
            return ret;
        }
        internal static bool Get(this JsonData data, int index, ref uint val)
        {
            bool ret = false;
            if (null != data && (data.IsObject || data.IsArray) && data.Count > index) {
                ret = data[index].Get(ref val);
            }
            return ret;
        }
        internal static bool Get(this JsonData data, int index, ref ulong val)
        {
            long temp = 0;
            bool ret = Get(data, index, ref temp);
            if (ret) {
                val = (ulong)temp;
            }
            return ret;
        }
        internal static bool Get(this JsonData data, int index, ref float val)
        {
            float temp = 0;
            bool ret = Get(data, index, ref temp);
            if (ret) {
                val = (float)temp;
            }
            return ret;
        }
        //--------------------------------------------------------------------------
        internal static void Set(this JsonData data, int index, bool val)
        {
            data[index] = val;
        }
        internal static void Set(this JsonData data, int index, int val)
        {
            data[index] = val;
        }
        internal static void Set(this JsonData data, int index, long val)
        {
            data[index] = val;
        }
        internal static void Set(this JsonData data, int index, double val)
        {
            data[index] = val;
        }
        internal static void Set(this JsonData data, int index, string val)
        {
            data[index] = val;
        }
        internal static void Set(this JsonData data, int index, uint val)
        {
            data[index] = (int)val;
        }
        internal static void Set(this JsonData data, int index, ulong val)
        {
            data[index] = (long)val;
        }
        internal static void Set(this JsonData data, int index, float val)
        {
            data[index] = (float)val;
        }
        //--------------------------------------------------------------------------
        internal static bool GetBoolean(this JsonData data)
        {
            bool ret = false;
            data.Get(ref ret);
            return ret;
        }
        internal static int GetInt(this JsonData data)
        {
            int ret = 0;
            data.Get(ref ret);
            return ret;
        }
        internal static long GetLong(this JsonData data)
        {
            long ret = 0;
            data.Get(ref ret);
            return ret;
        }
        internal static float GetDouble(this JsonData data)
        {
            float ret = 0;
            data.Get(ref ret);
            return ret;
        }
        internal static string GetString(this JsonData data)
        {
            string ret = "";
            data.Get(ref ret);
            return ret;
        }
        internal static uint GetUint(this JsonData data)
        {
            uint ret = 0;
            data.Get(ref ret);
            return ret;
        }
        internal static ulong GetUlong(this JsonData data)
        {
            ulong ret = 0;
            data.Get(ref ret);
            return ret;
        }
        internal static float GetFloat(this JsonData data)
        {
            float ret = 0;
            data.Get(ref ret);
            return ret;
        }
        //--------------------------------------------------------------------------
        internal static bool GetBoolean(this JsonData data, string key)
        {
            bool ret = false;
            data.Get(key, ref ret);
            return ret;
        }
        internal static int GetInt(this JsonData data, string key)
        {
            int ret = 0;
            data.Get(key, ref ret);
            return ret;
        }
        internal static long GetLong(this JsonData data, string key)
        {
            long ret = 0;
            data.Get(key, ref ret);
            return ret;
        }
        internal static float GetDouble(this JsonData data, string key)
        {
            float ret = 0;
            data.Get(key, ref ret);
            return ret;
        }
        internal static string GetString(this JsonData data, string key)
        {
            string ret = "";
            data.Get(key, ref ret);
            return ret;
        }
        internal static uint GetUint(this JsonData data, string key)
        {
            uint ret = 0;
            data.Get(key, ref ret);
            return ret;
        }
        internal static ulong GetUlong(this JsonData data, string key)
        {
            ulong ret = 0;
            data.Get(key, ref ret);
            return ret;
        }
        internal static float GetFloat(this JsonData data, string key)
        {
            float ret = 0;
            data.Get(key, ref ret);
            return ret;
        }
        //--------------------------------------------------------------------------
        internal static bool GetBoolean(this JsonData data, int index)
        {
            bool ret = false;
            data.Get(index, ref ret);
            return ret;
        }
        internal static int GetInt(this JsonData data, int index)
        {
            int ret = 0;
            data.Get(index, ref ret);
            return ret;
        }
        internal static long GetLong(this JsonData data, int index)
        {
            long ret = 0;
            data.Get(index, ref ret);
            return ret;
        }
        internal static float GetDouble(this JsonData data, int index)
        {
            float ret = 0;
            data.Get(index, ref ret);
            return ret;
        }
        internal static string GetString(this JsonData data, int index)
        {
            string ret = "";
            data.Get(index, ref ret);
            return ret;
        }
        internal static uint GetUint(this JsonData data, int index)
        {
            uint ret = 0;
            data.Get(index, ref ret);
            return ret;
        }
        internal static ulong GetUlong(this JsonData data, int index)
        {
            ulong ret = 0;
            data.Get(index, ref ret);
            return ret;
        }
        internal static float GetFloat(this JsonData data, int index)
        {
            float ret = 0;
            data.Get(index, ref ret);
            return ret;
        }
    }
}