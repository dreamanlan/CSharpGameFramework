using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class StringLower
{
    public static string StrToLower(this string str)
    {
        string ret;
        if (!s_LowerStrings.TryGetValue(str, out ret)) {
            ret = str.ToLower();
            s_LowerStrings.Add(str, ret);
        }
        return ret;
    }
    public static string StrToUpper(this string str)
    {
        string ret;
        if (!s_UpperStrings.TryGetValue(str, out ret)) {
            ret = str.ToUpper();
            s_UpperStrings.Add(str, ret);
        }
        return ret;
    }

    private static Dictionary<string, string> s_LowerStrings = new Dictionary<string, string>();
    private static Dictionary<string, string> s_UpperStrings = new Dictionary<string, string>();
}
