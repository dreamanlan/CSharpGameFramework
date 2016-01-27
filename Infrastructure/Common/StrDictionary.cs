using System;
using System.Collections.Generic;
using GameFramework;

public static class Dict
{
    public delegate string FindDictionaryDelegation(string key);
    public static FindDictionaryDelegation OnFindDictionary;
    public static string Parse(string txtFromServer)
    {
        string[] txts = txtFromServer.Split('$');
        int ct = txts.Length;
        for (int i = 1; i < ct; i += 2) {
            if (txts[i].Length == 0) {
                txts[i] = "$";
            } else {
                txts[i] = Get(txts[i]);
            }
        }
        return string.Join("", txts);
    }
    public static string Get(string key)
    {
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(key.Trim())) {
            return "";
        }
        string str = GetStrByKey(key);
        if (str == null) {
#if DEBUG
            str = "Dict Null: " + key + "\n" + Environment.StackTrace;
#else
      str = "--";
#endif
            LogSystem.Error("Dict Null: {0}\n{1}", key, Environment.StackTrace);
        }
        return str.Replace(@"[\n]", "\n").Replace(@"\n", "\n");
    }
    public static string Format(string key, params object[] args)
    {
        string format = Get(key);
        try {
            return string.Format(format, args);
        } catch (Exception ex) {
            return ("Dict Format Err: \n" + ex.StackTrace);
        }
    }

    private static string GetStrByKey(string key)
    {
        string ret = null;
        if (null != OnFindDictionary) {
            ret = OnFindDictionary(key);
        }
        return ret;
    }
}
