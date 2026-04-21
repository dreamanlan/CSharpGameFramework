using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Linq;
using ScriptableFramework;
using DotnetStoryScript;

#region interpreter
#pragma warning disable 8600,8601,8602,8603,8604,8618,8619,8620,8625
namespace DotnetStoryScript.DslExpression
{
    internal sealed class SubstringExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1 || operands.Count > 3)
                throw new Exception("Expected: substring(str[,start,len]) function");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                string str = operands[0].GetString();
                if (null != str) {
                    int start = 0;
                    int len = str.Length;
                    if (operands.Count >= 2) {
                        start = operands[1].GetInt();
                        len -= start;
                    }
                    if (operands.Count >= 3) {
                        len = operands[2].GetInt();
                    }
                    r = str.Substring(start, len);
                }
            }
            return r;
        }
    }
    internal sealed class NewStringBuilderExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: newstringbuilder() api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 0) {
                r = BoxedValue.FromObject(new StringBuilder());
            }
            return r;
        }
    }
    internal sealed class AppendFormatExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: appendformat(sb,fmt,arg1,arg2,...) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var sb = operands[0].As<StringBuilder>();
                string fmt = string.Empty;
                var al = new ArrayList();
                for (int i = 1; i < operands.Count; ++i) {
                    if (i == 1)
                        fmt = operands[i].AsString;
                    else
                        al.Add(operands[i].GetObject());
                }
                if (null != sb && !string.IsNullOrEmpty(fmt)) {
                    sb.AppendFormat(fmt, al.ToArray());
                    r = BoxedValue.FromObject(sb);
                }
            }
            return r;
        }
    }
    internal sealed class AppendFormatLineExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: appendformatline(sb,fmt,arg1,arg2,...) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var sb = operands[0].As<StringBuilder>();
                string fmt = string.Empty;
                var al = new ArrayList();
                for (int i = 1; i < operands.Count; ++i) {
                    if (i == 1)
                        fmt = operands[i].AsString;
                    else
                        al.Add(operands[i].GetObject());
                }
                if (null != sb) {
                    if (string.IsNullOrEmpty(fmt)) {
                        sb.AppendLine();
                    }
                    else {
                        sb.AppendFormat(fmt, al.ToArray());
                        sb.AppendLine();
                    }
                    r = BoxedValue.FromObject(sb);
                }
            }
            return r;
        }
    }
    internal sealed class StringBuilderToStringExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: stringbuilder_tostring(sb)");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var sb = operands[0].As<StringBuilder>();
                if (null != sb) {
                    r = sb.ToString();
                }
            }
            return r;
        }
    }
    internal sealed class StringJoinExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: stringjoin(sep,list) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var sep = operands[0].AsString;
                var list = operands[1].As<IList>();
                if (null != sep && null != list) {
                    string[] strs = new string[list.Count];
                    for (int i = 0; i < list.Count; ++i) {
                        strs[i] = list[i].ToString();
                    }
                    r = string.Join(sep, strs);
                }
            }
            return r;
        }
    }
    internal sealed class StringSplitExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: stringsplit(str,sep_list) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var str = operands[0].AsString;
                var seps = operands[1].As<IList>();
                if (null != str && null != seps) {
                    char[] cs = new char[seps.Count];
                    for (int i = 0; i < seps.Count; ++i) {
                        string sep = seps[i].ToString();
                        if (sep.Length > 0) {
                            cs[i] = sep[0];
                        }
                        else {
                            cs[i] = '\0';
                        }
                    }
                    r = BoxedValue.FromObject(str.Split(cs));
                }
            }
            return r;
        }
    }
    internal sealed class StringTrimExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: stringtrim(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                r = str.Trim();
            }
            return r;
        }
    }
    internal sealed class StringTrimStartExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: stringtrimstart(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                r = str.TrimStart();
            }
            return r;
        }
    }
    internal sealed class StringTrimEndExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: stringtrimend(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                r = str.TrimEnd();
            }
            return r;
        }
    }
    internal sealed class StringToLowerExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: stringtolower(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                r = str.ToLower();
            }
            return r;
        }
    }
    internal sealed class StringToUpperExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: stringtoupper(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                r = str.ToUpper();
            }
            return r;
        }
    }
    internal sealed class StringReplaceExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 3)
                throw new Exception("Expected: stringreplace(str,key,rep_str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 3) {
                var str = operands[0].AsString;
                var key = operands[1].AsString;
                var val = operands[2].AsString;
                r = str.Replace(key, val);
            }
            return r;
        }
    }
    internal sealed class StringReplaceCharExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 3)
                throw new Exception("Expected: stringreplacechar(str,key,char_as_str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 3) {
                var str = operands[0].AsString;
                var key = operands[1].AsString;
                var val = operands[2].AsString;
                if (null != str && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(val)) {
                    r = str.Replace(key[0], val[0]);
                }
            }
            return r;
        }
    }
    internal sealed class MakeStringExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            List<char> chars = new List<char>();
            for (int i = 0; i < operands.Count; ++i) {
                var v = operands[i];
                var str = v.AsString;
                if (null != str) {
                    char c = '\0';
                    if (str.Length > 0) {
                        c = str[0];
                    }
                    chars.Add(c);
                }
                else {
                    char c = operands[i].GetChar();
                    chars.Add(c);
                }
            }
            return new String(chars.ToArray());
        }
    }
    internal sealed class StringContainsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: stringcontains(str,str_or_list_1,str_or_list_2,...) api");
            bool r = false;
            if (operands.Count >= 2) {
                string str = operands[0].AsString;
                r = true;
                for (int i = 1; i < operands.Count; ++i) {
                    var list = operands[i].As<IList>();
                    if (null != list) {
                        foreach (var o in list) {
                            var key = o as string;
                            if (!string.IsNullOrEmpty(key) && !str.Contains(key)) {
                                return false;
                            }
                        }
                    }
                    else {
                        var key = operands[i].AsString;
                        if (!string.IsNullOrEmpty(key) && !str.Contains(key)) {
                            return false;
                        }
                    }
                }
            }
            return r;
        }
    }
    internal sealed class StringNotContainsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: stringnotcontains(str,str_or_list_1,str_or_list_2,...) api");
            bool r = false;
            if (operands.Count >= 2) {
                string str = operands[0].AsString;
                r = true;
                for (int i = 1; i < operands.Count; ++i) {
                    var list = operands[i].As<IList>();
                    if (null != list) {
                        foreach (var o in list) {
                            var key = o as string;
                            if (!string.IsNullOrEmpty(key) && str.Contains(key)) {
                                return false;
                            }
                        }
                    }
                    else {
                        var key = operands[i].AsString;
                        if (!string.IsNullOrEmpty(key) && str.Contains(key)) {
                            return false;
                        }
                    }
                }
            }
            return r;
        }
    }
    internal sealed class StringContainsAnyExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: stringcontainsany(str,str_or_list_1,str_or_list_2,...) api");
            bool r = false;
            if (operands.Count >= 2) {
                r = true;
                string str = operands[0].AsString;
                for (int i = 1; i < operands.Count; ++i) {
                    var list = operands[i].As<IList>();
                    if (null != list) {
                        foreach (var o in list) {
                            var key = o as string;
                            if (!string.IsNullOrEmpty(key)) {
                                if (str.Contains(key)) {
                                    return true;
                                }
                                else {
                                    r = false;
                                }
                            }
                        }
                    }
                    else {
                        var key = operands[i].AsString;
                        if (!string.IsNullOrEmpty(key)) {
                            if (str.Contains(key)) {
                                return true;
                            }
                            else {
                                r = false;
                            }
                        }
                    }
                }
            }
            return r;
        }
    }
    internal sealed class StringNotContainsAnyExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: stringnotcontainsany(str,str_or_list_1,str_or_list_2,...) api");
            bool r = false;
            if (operands.Count >= 2) {
                r = true;
                string str = operands[0].AsString;
                for (int i = 1; i < operands.Count; ++i) {
                    var list = operands[i].As<IList>();
                    if (null != list) {
                        foreach (var o in list) {
                            var key = o as string;
                            if (!string.IsNullOrEmpty(key)) {
                                if (!str.Contains(key)) {
                                    return true;
                                }
                                else {
                                    r = false;
                                }
                            }
                        }
                    }
                    else {
                        var key = operands[i].AsString;
                        if (!string.IsNullOrEmpty(key)) {
                            if (!str.Contains(key)) {
                                return true;
                            }
                            else {
                                r = false;
                            }
                        }
                    }
                }
            }
            return r;
        }
    }
    internal sealed class Str2IntExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: str2int(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                int v;
                if (int.TryParse(str, System.Globalization.NumberStyles.Number, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Str2UintExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: str2uint(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                uint v;
                if (uint.TryParse(str, System.Globalization.NumberStyles.Number, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Str2LongExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: str2long(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                long v;
                if (long.TryParse(str, System.Globalization.NumberStyles.Number, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Str2UlongExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: str2ulong(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                ulong v;
                if (ulong.TryParse(str, System.Globalization.NumberStyles.Number, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Str2FloatExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: str2float(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                float v;
                if (float.TryParse(str, System.Globalization.NumberStyles.Float, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Str2DoubleExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: str2double(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                double v;
                if (double.TryParse(str, System.Globalization.NumberStyles.Float, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Hex2IntExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hex2int(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                int v;
                if (int.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Hex2UintExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hex2uint(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                uint v;
                if (uint.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Hex2LongExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hex2long(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                long v;
                if (long.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class Hex2UlongExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hex2ulong(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                ulong v;
                if (ulong.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out v)) {
                    r = v;
                }
            }
            return r;
        }
    }
    internal sealed class DatetimeStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 1)
                throw new Exception("Expected: datetimestr([fmt]) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var fmt = operands[0].AsString;
                r = DateTime.Now.ToString(fmt);
            }
            else {
                r = DateTime.Now.ToString();
            }
            return r;
        }
    }
    internal sealed class LongDateStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: longdatestr() api");
            var r = BoxedValue.FromObject(DateTime.Now.ToLongDateString());
            return r;
        }
    }
    internal sealed class LongTimeStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: longtimestr() api");
            var r = BoxedValue.FromObject(DateTime.Now.ToShortDateString());
            return r;
        }
    }
    internal sealed class ShortDateStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: shortdatestr() api");
            var r = BoxedValue.FromObject(DateTime.Now.ToShortDateString());
            return r;
        }
    }
    internal sealed class ShortTimeStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: shorttimestr() api");
            var r = BoxedValue.FromObject(DateTime.Now.ToShortTimeString());
            return r;
        }
    }
    internal sealed class IsNullOrEmptyExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: isnullorempty(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                r = string.IsNullOrEmpty(str);
            }
            return r;
        }
    }
    internal sealed class ArrayExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue[] r = new BoxedValue[operands.Count];
            for (int i = 0; i < operands.Count; ++i) {
                r[i] = operands[i];
            }
            return BoxedValue.FromObject(r);
        }
    }
    internal sealed class ToArrayExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: toarray(list) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var list = operands[0];
                IEnumerable obj = list.As<IEnumerable>();
                if (null != obj) {
                    List<BoxedValue> al = new List<BoxedValue>();
                    IEnumerator enumer = obj.GetEnumerator();
                    while (enumer.MoveNext()) {
                        var val = BoxedValue.FromObject(enumer.Current);
                        al.Add(val);
                    }
                    r = BoxedValue.FromObject(al.ToArray());
                }
            }
            return r;
        }
    }
    internal sealed class ListSizeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: listsize(list) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var list = operands[0].As<IList>();
                if (null != list) {
                    r = list.Count;
                }
            }
            return r;
        }
    }
    internal sealed class ListExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            List<BoxedValue> al = new List<BoxedValue>();
            for (int i = 0; i < operands.Count; ++i) {
                al.Add(operands[i]);
            }
            r = BoxedValue.FromObject(al);
            return r;
        }
    }
    internal sealed class ListGetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2 || operands.Count > 3)
                throw new Exception("Expected: listget(list,index[,defval]) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var list = operands[0].As<IList>();
                var index = operands[1].GetInt();
                var defVal = BoxedValue.NullObject;
                if (operands.Count >= 3) {
                    defVal = operands[2];
                }
                if (null != list) {
                    if (index >= 0 && index < list.Count) {
                        r = BoxedValue.FromObject(list[index]);
                    }
                    else {
                        r = defVal;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class ListSetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 3)
                throw new Exception("Expected: listset(list,index,val) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 3) {
                var list = operands[0].As<IList>();
                var index = operands[1].GetInt();
                var val = operands[2];
                if (null != list && list is List<BoxedValue> bvList) {
                    if (index >= 0 && index < list.Count) {
                        bvList[index] = val;
                    }
                }
                else if (null != list) {
                    if (index >= 0 && index < list.Count) {
                        list[index] = val.GetObject();
                    }
                }
            }
            return r;
        }
    }
    internal sealed class ListIndexOfExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: listindexof(list,val) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var list = operands[0].As<IList>();
                var val = operands[1];
                if (null != list && list is List<BoxedValue> bvList) {
                    r = bvList.IndexOf(val);
                }
                else if (null != list) {
                    r = list.IndexOf(val.GetObject());
                }
            }
            return r;
        }
    }
    internal sealed class ListAddExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: listadd(list,val) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var list = operands[0].As<IList>();
                var val = operands[1];
                if (null != list && list is List<BoxedValue> bvList) {
                    bvList.Add(val);
                }
                else if (null != list) {
                    list.Add(val.GetObject());
                }
            }
            return r;
        }
    }
    internal sealed class ListRemoveExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: listremove(list,val) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var list = operands[0].As<IList>();
                var val = operands[1];
                if (null != list && list is List<BoxedValue> bvList) {
                    bvList.Remove(val);
                }
                else if (null != list) {
                    list.Remove(val.GetObject());
                }
            }
            return r;
        }
    }
    internal sealed class ListInsertExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 3)
                throw new Exception("Expected: listinsert(list,index,val) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 3) {
                var list = operands[0].As<IList>();
                var index = operands[1].GetInt();
                var val = operands[2];
                if (null != list && list is List<BoxedValue> bvList) {
                    bvList.Insert(index, val);
                }
                else if (null != list) {
                    list.Insert(index, val.GetObject());
                }
            }
            return r;
        }
    }
    internal sealed class ListRemoveAtExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: listremoveat(list,index) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var list = operands[0].As<IList>();
                var index = operands[1].GetInt();
                if (null != list) {
                    list.RemoveAt(index);
                }
            }
            return r;
        }
    }
    internal sealed class ListClearExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: listclear(list) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var list = operands[0].As<IList>();
                if (null != list) {
                    list.Clear();
                }
            }
            return r;
        }
    }
    internal sealed class ListSplitExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: listsplit(list,ct) api, return list of list");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var enumer = operands[0].As<IEnumerable>();
                var ct = operands[1].GetInt();
                if (null != enumer && enumer is List<BoxedValue> bvList) {
                    var e = bvList.GetEnumerator();
                    List<List<BoxedValue>> al = new List<List<BoxedValue>>();
                    List<BoxedValue> arr = new List<BoxedValue>();
                    int ix = 0;
                    while (e.MoveNext()) {
                        if (ix < ct) {
                            arr.Add(e.Current);
                            ++ix;
                        }
                        if (ix >= ct) {
                            al.Add(arr);
                            arr = new List<BoxedValue>();
                            ix = 0;
                        }
                    }
                    if (arr.Count > 0) {
                        al.Add(arr);
                    }
                    r = BoxedValue.FromObject(al);
                }
                else if (null != enumer) {
                    var e = enumer.GetEnumerator();
                    if (null != e) {
                        List<List<BoxedValue>> al = new List<List<BoxedValue>>();
                        List<BoxedValue> arr = new List<BoxedValue>();
                        int ix = 0;
                        while (e.MoveNext()) {
                            if (ix < ct) {
                                arr.Add(BoxedValue.FromObject(e.Current));
                                ++ix;
                            }
                            if (ix >= ct) {
                                al.Add(arr);
                                arr = new List<BoxedValue>();
                                ix = 0;
                            }
                        }
                        if (arr.Count > 0) {
                            al.Add(arr);
                        }
                        r = BoxedValue.FromObject(al);
                    }
                }
            }
            return r;
        }
    }
    internal sealed class HashtableSizeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hashtablesize(hash) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var dict = operands[0].As<IDictionary>();
                if (null != dict) {
                    r = dict.Count;
                }
            }
            return r;
        }
    }
    internal sealed class HashtableExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            BoxedValue r = BoxedValue.NullObject;
            var dict = new Dictionary<BoxedValue, BoxedValue>();
            for (int i = 0; i < m_Expressions.Count - 1; i += 2) {
                var key = m_Expressions[i].Calc();
                var val = m_Expressions[i + 1].Calc();
                dict.Add(key, val);
            }
            r = BoxedValue.FromObject(dict);
            return r;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            for (int i = 0; i < funcData.GetParamNum(); ++i) {
                Dsl.FunctionData callData = funcData.GetParam(i) as Dsl.FunctionData;
                if (null != callData && callData.GetParamNum() == 2) {
                    var p0 = callData.GetParam(0);
                    var p1 = callData.GetParam(1);
                    if (p0 is Dsl.ValueData vd0 && vd0.GetIdType() == Dsl.ValueData.ID_TOKEN) {
                        var vid = vd0.GetId();
                        if (vid.Length > 0 && (vid[0] == '$' || vid[0] == '@')) {
                            // Enable variables with prefixed tags
                        }
                        else {
                            vd0.SetType(Dsl.ValueData.STRING_TOKEN);
                        }
                    }
                    var expKey = Calculator.Load(p0);
                    m_Expressions.Add(expKey);
                    var expVal = Calculator.Load(p1);
                    m_Expressions.Add(expVal);
                }
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class HashtableGetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2 || operands.Count > 3)
                throw new Exception("Expected: hashtableget(hash,key[,defval]) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var dict = operands[0].As<IDictionary>();
                var index = operands[1];
                var defVal = BoxedValue.NullObject;
                if (operands.Count >= 3) {
                    defVal = operands[2];
                }
                if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict) {
                    r = bvDict.TryGetValue(index, out var val) ? val : defVal;
                }
                else {
                    var indexObj = index.GetObject();
                    if (null != dict && dict.Contains(indexObj)) {
                        r = BoxedValue.FromObject(dict[indexObj]);
                    }
                    else {
                        r = defVal;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class HashtableSetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 3)
                throw new Exception("Expected: hashtableset(hash,key,val) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 3) {
                var dict = operands[0].As<IDictionary>();
                var index = operands[1];
                var val = operands[2];
                if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict) {
                    bvDict[index] = val;
                }
                else {
                    var indexObj = index.GetObject();
                    var valObj = val.GetObject();
                    if (null != dict) {
                        dict[indexObj] = valObj;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class HashtableAddExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 3)
                throw new Exception("Expected: hashtableadd(hash,key,val) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 3) {
                var dict = operands[0].As<IDictionary>();
                var key = operands[1];
                var val = operands[2];
                if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict) {
                    bvDict.Add(key, val);
                }
                else {
                    var keyObj = key.GetObject();
                    var valObj = val.GetObject();
                    if (null != dict && null != keyObj) {
                        dict.Add(keyObj, valObj);
                    }
                }
            }
            return r;
        }
    }
    internal sealed class HashtableRemoveExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: hashtableremove(hash,key) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var dict = operands[0].As<IDictionary>();
                var key = operands[1];
                if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict) {
                    bvDict.Remove(key);
                }
                else {
                    var keyObj = key.GetObject();
                    if (null != dict && null != keyObj) {
                        dict.Remove(keyObj);
                    }
                }
            }
            return r;
        }
    }
    internal sealed class HashtableClearExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hashtableclear(hash) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var dict = operands[0].As<IDictionary>();
                if (null != dict) {
                    dict.Clear();
                }
            }
            return r;
        }
    }
    internal sealed class HashtableKeysExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hashtablekeys(hash) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var dict = operands[0].As<IDictionary>();
                if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict) {
                    var list = new List<BoxedValue>();
                    list.AddRange(bvDict.Keys);
                    r = BoxedValue.FromObject(list);
                }
                else {
                    if (null != dict) {
                        var list = new List<BoxedValue>();
                        foreach (var key in dict.Keys) {
                            list.Add(BoxedValue.FromObject(key));
                        }
                        r = BoxedValue.FromObject(list);
                    }
                }
            }
            return r;
        }
    }
    internal sealed class HashtableValuesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hashtablevalues(hash) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var dict = operands[0].As<IDictionary>();
                if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict) {
                    var list = new List<BoxedValue>();
                    list.AddRange(bvDict.Values);
                    r = BoxedValue.FromObject(list);
                }
                else {
                    if (null != dict) {
                        var list = new List<BoxedValue>();
                        foreach (var val in dict.Values) {
                            list.Add(BoxedValue.FromObject(val));
                        }
                        r = BoxedValue.FromObject(list);
                    }
                }
            }
            return r;
        }
    }
    internal sealed class ListHashtableExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: listhashtable(hash) api, return list of pair");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var dict = operands[0].As<IDictionary>();
                if (null != dict) {
                    var list = new ArrayList();
                    foreach (var pair in dict) {
                        list.Add(pair);
                    }
                    r = list;
                }
            }
            return r;
        }
    }
    internal sealed class HashtableSplitExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: hashtablesplit(hash,ct) api, return list of hashtable");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var dict = operands[0].As<IDictionary>();
                var ct = operands[1].GetInt();
                if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict) {
                    var e = bvDict.GetEnumerator();
                    var al = new List<Dictionary<BoxedValue, BoxedValue>>();
                    var ht = new Dictionary<BoxedValue, BoxedValue>();
                    int ix = 0;
                    while (e.MoveNext()) {
                        if (ix < ct) {
                            ht.Add(e.Current.Key, e.Current.Value);
                            ++ix;
                        }
                        if (ix >= ct) {
                            al.Add(ht);
                            ht = new Dictionary<BoxedValue, BoxedValue>();
                            ix = 0;
                        }
                    }
                    if (ht.Count > 0) {
                        al.Add(ht);
                    }
                    r = BoxedValue.FromObject(al);
                }
                else if (null != dict) {
                    var e = dict.GetEnumerator();
                    if (null != e) {
                        var al = new List<Dictionary<BoxedValue, BoxedValue>>();
                        var ht = new Dictionary<BoxedValue, BoxedValue>();
                        int ix = 0;
                        while (e.MoveNext()) {
                            if (ix < ct) {
                                ht.Add(BoxedValue.FromObject(e.Key), BoxedValue.FromObject(e.Value));
                                ++ix;
                            }
                            if (ix >= ct) {
                                al.Add(ht);
                                ht = new Dictionary<BoxedValue, BoxedValue>();
                                ix = 0;
                            }
                        }
                        if (ht.Count > 0) {
                            al.Add(ht);
                        }
                        r = BoxedValue.FromObject(al);
                    }
                }
            }
            return r;
        }
    }
    //The stack and queue share the same peek function.
    internal sealed class PeekExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: peek(queue_or_stack) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var stack = operands[0].As<Stack<BoxedValue>>();
                var queue = operands[0].As<Queue<BoxedValue>>();
                if (null != stack) {
                    r = stack.Peek();
                }
                else if (null != queue) {
                    r = queue.Peek();
                }
            }
            return r;
        }
    }
    internal sealed class StackSizeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: stacksize(stack) api");
            int r = 0;
            if (operands.Count >= 1) {
                var stack = operands[0].As<Stack<BoxedValue>>();
                if (null != stack) {
                    r = stack.Count;
                }
            }
            return r;
        }
    }
    internal sealed class StackExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            var stack = new Stack<BoxedValue>();
            for (int i = 0; i < operands.Count; ++i) {
                stack.Push(operands[i]);
            }
            r = BoxedValue.FromObject(stack);
            return r;
        }
    }
    internal sealed class PushExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: push(stack,v) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var stack = operands[0].As<Stack<BoxedValue>>();
                var val = operands[1];
                if (null != stack) {
                    stack.Push(val);
                }
            }
            return r;
        }
    }
    internal sealed class PopExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: pop(stack) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var stack = operands[0].As<Stack<BoxedValue>>();
                if (null != stack) {
                    r = stack.Pop();
                }
            }
            return r;
        }
    }
    internal sealed class StackClearExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: stackclear(stack) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var stack = operands[0].As<Stack<BoxedValue>>();
                if (null != stack) {
                    stack.Clear();
                }
            }
            return r;
        }
    }
    internal sealed class QueueSizeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: queuesize(queue) api");
            int r = 0;
            if (operands.Count >= 1) {
                var queue = operands[0].As<Queue<BoxedValue>>();
                if (null != queue) {
                    r = queue.Count;
                }
            }
            return r;
        }
    }
    internal sealed class QueueExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            var queue = new Queue<BoxedValue>();
            for (int i = 0; i < operands.Count; ++i) {
                queue.Enqueue(operands[i]);
            }
            r = BoxedValue.FromObject(queue);
            return r;
        }
    }
    internal sealed class EnqueueExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: enqueue(queue,v) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var queue = operands[0].As<Queue<BoxedValue>>();
                var val = operands[1];
                if (null != queue) {
                    queue.Enqueue(val);
                }
            }
            return r;
        }
    }
    internal sealed class DequeueExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: dequeue(queue) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var queue = operands[0].As<Queue<BoxedValue>>();
                if (null != queue) {
                    r = queue.Dequeue();
                }
            }
            return r;
        }
    }
    internal sealed class QueueClearExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: queueclear(queue) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var queue = operands[0].As<Queue<BoxedValue>>();
                if (null != queue) {
                    queue.Clear();
                }
            }
            return r;
        }
    }
    internal sealed class SetEnvironmentExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: setenv(k,v) api");
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var key = operands[0].AsString;
                var val = operands[1].AsString;
                val = Environment.ExpandEnvironmentVariables(val);
                Environment.SetEnvironmentVariable(key, val);
            }
            return ret;
        }
    }
    internal sealed class GetEnvironmentExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: getenv(k) api");
            string ret = string.Empty;
            if (operands.Count >= 1) {
                var key = operands[0].AsString;
                return Environment.GetEnvironmentVariable(key);
            }
            return ret;
        }
    }
    internal sealed class ExpandEnvironmentsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: expand(str) api");
            string ret = string.Empty;
            if (operands.Count >= 1) {
                var key = operands[0].AsString;
                return Environment.ExpandEnvironmentVariables(key);
            }
            return ret;
        }
    }
    internal sealed class EnvironmentsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: envs() api");
            return BoxedValue.FromObject(Environment.GetEnvironmentVariables());
        }
    }
    internal sealed class SetCurrentDirectoryExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: cd(path) api");
            string ret = string.Empty;
            if (operands.Count >= 1) {
                var dir = operands[0].AsString;
                string path = Environment.ExpandEnvironmentVariables(dir);
                if (Path.IsPathRooted(path)) {
                    Environment.CurrentDirectory = path;
                }
                else {
                    Environment.CurrentDirectory = Path.Combine(Environment.CurrentDirectory, path);
                }
                ret = dir;
            }
            return ret;
        }
    }
    internal sealed class GetCurrentDirectoryExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: pwd() api");
            return Environment.CurrentDirectory;
        }
    }
    internal sealed class CommandLineExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: cmdline() api");
            return Environment.CommandLine;
        }
    }
    internal sealed class CommandLineArgsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 1)
                throw new Exception("Expected: cmdlineargs(prev_arg) or cmdlineargs() api, first return next arg, second return array of arg");
            if (operands.Count >= 1) {
                string name = operands[0].AsString;
                if (!string.IsNullOrEmpty(name)) {
                    string[] args = System.Environment.GetCommandLineArgs();
                    int suffixIndex = Array.FindIndex(args, item => item == name);
                    if (suffixIndex != -1 && suffixIndex < args.Length - 1) {
                        return args[suffixIndex + 1];
                    }
                }
                return string.Empty;
            }
            else {
                return BoxedValue.FromObject(Environment.GetCommandLineArgs());
            }
        }
    }
    internal sealed class OsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: os() api");
            return Environment.OSVersion.VersionString;
        }
    }
    internal sealed class OsPlatformExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: osplatform() api");
            return Environment.OSVersion.Platform.ToString();
        }
    }
    internal sealed class OsVersionExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: osversion() api");
            return Environment.OSVersion.Version.ToString();
        }
    }
    internal sealed class GetFullPathExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: getfullpath(path) api");
            string ret = string.Empty;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    return Path.GetFullPath(path);
                }
            }
            return ret;
        }
    }
    internal sealed class GetPathRootExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: getpathroot(path) api");
            string ret = string.Empty;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    return Path.GetPathRoot(path);
                }
            }
            return ret;
        }
    }
    internal sealed class GetRandomFileNameExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: getrandomfilename() api");
            return Path.GetRandomFileName();
        }
    }
    internal sealed class GetTempFileNameExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: gettempfilename() api");
            return Path.GetTempFileName();
        }
    }
    internal sealed class GetTempPathExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: gettemppath() api");
            return Path.GetTempPath();
        }
    }
    internal sealed class HasExtensionExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hasextension(path) api");
            bool ret = false;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    return Path.HasExtension(path);
                }
            }
            return ret;
        }
    }
    internal sealed class IsPathRootedExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: ispathrooted(path) api");
            bool ret = false;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    return Path.IsPathRooted(path);
                }
            }
            return ret;
        }
    }
    internal sealed class GetFileNameExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: getfilename(path) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.GetFileName(path);
                }
            }
            return r;
        }
    }
    internal sealed class GetFileNameWithoutExtensionExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: getfilenamewithoutextension(path) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.GetFileNameWithoutExtension(path);
                }
            }
            return r;
        }
    }
    internal sealed class GetExtensionExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: getextension(path) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.GetExtension(path);
                }
            }
            return r;
        }
    }
    internal sealed class GetDirectoryNameExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: getdirectoryname(path) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                if (null != path) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.GetDirectoryName(path);
                }
            }
            return r;
        }
    }
    internal sealed class CombinePathExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: combinepath(path1,path2,...) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                List<string> list = new List<string>();
                for (int ix = 0; ix < operands.Count; ++ix) {
                    var v = operands[ix];
                    var str = v.AsString;
                    if (string.IsNullOrEmpty(str)) {
                        throw new Exception(string.Format("Path {0} is null or empty", ix));
                    }
                    list.Add(str);
                }
                var path = Path.Combine(list.ToArray());
                path = Environment.ExpandEnvironmentVariables(path);
                r = path;
            }
            return r;
        }
    }
    internal sealed class ChangeExtensionExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: changeextension(path,ext) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var path = operands[0].AsString;
                var ext = operands[1].AsString;
                if (null != path && null != ext) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    r = Path.ChangeExtension(path, ext);
                }
            }
            return r;
        }
    }
    internal sealed class QuotePathExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1 || operands.Count > 3)
                throw new Exception("Expected: quotepath(path[,only_needed,single_quote]) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                bool onlyNeeded = operands.Count >= 2 ? operands[1].GetBool() : true;
                bool singleQuotes = operands.Count >= 3 ? operands[2].GetBool() : false;
                if (null != path && path.Length > 0) {
                    path = Environment.ExpandEnvironmentVariables(path).Trim();
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                        //On Windows, file names can contain single quotes, but paths must be quoted using double quotes.
                        string delim = "\"";
                        if (onlyNeeded) {
                            char first = path[0];
                            char last = path[path.Length - 1];
                            int ix = path.IndexOf(' ');
                            if (ix > 0 && !CharIsQuote(first) && !CharIsQuote(last)) {
                                path = delim + path + delim;
                            }
                        }
                        else {
                            char first = path[0];
                            char last = path[path.Length - 1];
                            if (!CharIsQuote(first) && !CharIsQuote(last)) {
                                path = delim + path + delim;
                            }
                        }
                    }
                    else {
                        string delim = singleQuotes ? "'" : "\"";
                        if (onlyNeeded) {
                            char first = path[0];
                            char last = path[path.Length - 1];
                            int ix = path.IndexOf(' ');
                            if (ix > 0 && !CharIsQuote(first) && !CharIsQuote(last)) {
                                path = delim + path + delim;
                            }
                        }
                        else {
                            char first = path[0];
                            char last = path[path.Length - 1];
                            if (!CharIsQuote(first) && !CharIsQuote(last)) {
                                path = delim + path + delim;
                            }
                        }
                    }
                    r = path;
                }
            }
            return r;
        }
        private static bool CharIsQuote(char c)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                return c == '"';
            }
            else {
                return c == '"' || c == '\'';
            }
        }
    }
    internal sealed class EchoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var obj = operands[0];
                if (obj.IsString) {
                    var fmt = obj.StringVal;
                    if (operands.Count > 1 && null != fmt) {
                        ArrayList arrayList = new ArrayList();
                        for (int i = 1; i < operands.Count; ++i) {
                            arrayList.Add(operands[i].GetObject());
                        }
                        Console.WriteLine(fmt, arrayList.ToArray());
                    }
                    else {
                        Console.WriteLine(obj.GetObject());
                    }
                }
                else {
                    Console.WriteLine(obj.GetObject());
                }
            }
            else {
                Console.WriteLine();
            }
            return r;
        }
    }
    internal sealed class FileEchoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 1)
                throw new Exception("Expected: fileecho(bool) or fileecho() api");
            if (operands.Count >= 1) {
                DslCalculator.FileEchoOn = operands[0].GetBool();
            }
            return DslCalculator.FileEchoOn;
        }
    }
    internal sealed class DirectoryExistExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: direxist(dir) api");
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var dir = operands[0].AsString;
                dir = Environment.ExpandEnvironmentVariables(dir);
                ret = Directory.Exists(dir);
            }
            return ret;
        }
    }
    internal sealed class FileExistExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: fileexist(file) api");
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var file = operands[0].AsString;
                file = Environment.ExpandEnvironmentVariables(file);
                ret = File.Exists(file);
            }
            return ret;
        }
    }
    internal sealed class ListDirectoriesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: listdirs(dir,filter_list_or_str_1,filter_list_or_str_2,...) api");
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var baseDir = operands[0].AsString;
                baseDir = Environment.ExpandEnvironmentVariables(baseDir);
                IList<string> filterList = new string[] { "*" };
                if (operands.Count >= 2) {
                    var list = new List<string>();
                    for (int i = 1; i < operands.Count; ++i) {
                        var str = operands[i].AsString;
                        if (null != str) {
                            list.Add(str);
                        }
                        else {
                            var strList = operands[i].As<IList>();
                            if (null != strList) {
                                foreach (var strObj in strList) {
                                    var tempStr = strObj as string;
                                    if (null != tempStr)
                                        list.Add(tempStr);
                                }
                            }
                        }
                    }
                    filterList = list;
                }
                if (null != baseDir && Directory.Exists(baseDir)) {
                    var fullList = new List<string>();
                    foreach (var filter in filterList) {
                        var list = Directory.GetDirectories(baseDir, filter, SearchOption.TopDirectoryOnly);
                        fullList.AddRange(list);
                    }
                    ret = BoxedValue.FromObject(fullList);
                }
            }
            return ret;
        }
    }
    internal sealed class ListFilesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: listfiles(dir,filter_list_or_str_1,filter_list_or_str_2,...) api");
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var baseDir = operands[0].AsString;
                baseDir = Environment.ExpandEnvironmentVariables(baseDir);
                IList<string> filterList = new string[] { "*" };
                if (operands.Count >= 2) {
                    var list = new List<string>();
                    for (int i = 1; i < operands.Count; ++i) {
                        var str = operands[i].AsString;
                        if (null != str) {
                            list.Add(str);
                        }
                        else {
                            var strList = operands[i].As<IList>();
                            if (null != strList) {
                                foreach (var strObj in strList) {
                                    var tempStr = strObj as string;
                                    if (null != tempStr)
                                        list.Add(tempStr);
                                }
                            }
                        }
                    }
                    filterList = list;
                }
                if (null != baseDir && Directory.Exists(baseDir)) {
                    var fullList = new List<string>();
                    foreach (var filter in filterList) {
                        var list = Directory.GetFiles(baseDir, filter, SearchOption.TopDirectoryOnly);
                        fullList.AddRange(list);
                    }
                    ret = BoxedValue.FromObject(fullList);
                }
            }
            return ret;
        }
    }
    internal sealed class ListAllDirectoriesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: listalldirs(dir,filter_list_or_str_1,filter_list_or_str_2,...) api");
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var baseDir = operands[0].AsString;
                baseDir = Environment.ExpandEnvironmentVariables(baseDir);
                IList<string> filterList = new string[] { "*" };
                if (operands.Count >= 2) {
                    var list = new List<string>();
                    for (int i = 1; i < operands.Count; ++i) {
                        var str = operands[i].AsString;
                        if (null != str) {
                            list.Add(str);
                        }
                        else {
                            var strList = operands[i].As<IList>();
                            if (null != strList) {
                                foreach (var strObj in strList) {
                                    var tempStr = strObj as string;
                                    if (null != tempStr)
                                        list.Add(tempStr);
                                }
                            }
                        }
                    }
                    filterList = list;
                }
                if (null != baseDir && Directory.Exists(baseDir)) {
                    var fullList = new List<string>();
                    foreach (var filter in filterList) {
                        var list = Directory.GetDirectories(baseDir, filter, SearchOption.AllDirectories);
                        fullList.AddRange(list);
                    }
                    ret = BoxedValue.FromObject(fullList);
                }
            }
            return ret;
        }
    }
    internal sealed class ListAllFilesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: listallfiles(dir,filter_list_or_str_1,filter_list_or_str_2,...) api");
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var baseDir = operands[0].AsString;
                baseDir = Environment.ExpandEnvironmentVariables(baseDir);
                IList<string> filterList = new string[] { "*" };
                if (operands.Count >= 2) {
                    var list = new List<string>();
                    for (int i = 1; i < operands.Count; ++i) {
                        var str = operands[i].AsString;
                        if (null != str) {
                            list.Add(str);
                        }
                        else {
                            var strList = operands[i].As<IList>();
                            if (null != strList) {
                                foreach (var strObj in strList) {
                                    var tempStr = strObj as string;
                                    if (null != tempStr)
                                        list.Add(tempStr);
                                }
                            }
                        }
                    }
                    filterList = list;
                }
                if (null != baseDir && Directory.Exists(baseDir)) {
                    var fullList = new List<string>();
                    foreach (var filter in filterList) {
                        var list = Directory.GetFiles(baseDir, filter, SearchOption.AllDirectories);
                        fullList.AddRange(list);
                    }
                    ret = BoxedValue.FromObject(fullList);
                }
            }
            return ret;
        }
    }
    internal sealed class CreateDirectoryExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: createdir(dir) api");
            bool ret = false;
            if (operands.Count >= 1) {
                var dir = operands[0].AsString;
                dir = Environment.ExpandEnvironmentVariables(dir);
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                    ret = true;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("create directory {0}", dir);
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class CopyDirectoryExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: copydir(dir1,dir2,filter_list_or_str_1,filter_list_or_str_2,...) api, include subdir");
            int ct = 0;
            if (operands.Count >= 2) {
                var dir1 = operands[0].AsString;
                var dir2 = operands[1].AsString;
                dir1 = Environment.ExpandEnvironmentVariables(dir1);
                dir2 = Environment.ExpandEnvironmentVariables(dir2);
                List<string> filterAndNewExts = new List<string>();
                for (int i = 2; i < operands.Count; ++i) {
                    var str = operands[i].AsString;
                    if (null != str) {
                        filterAndNewExts.Add(str);
                    }
                    else {
                        var strList = operands[i].As<IList>();
                        if (null != strList) {
                            foreach (var strObj in strList) {
                                var tempStr = strObj as string;
                                if (null != tempStr)
                                    filterAndNewExts.Add(tempStr);
                            }
                        }
                    }
                }
                if (filterAndNewExts.Count <= 0) {
                    filterAndNewExts.Add("*");
                }
                var targetRoot = Path.GetFullPath(dir2);
                if (Directory.Exists(dir1)) {
                    CopyFolder(targetRoot, dir1, dir2, filterAndNewExts, ref ct);
                }
            }
            return ct;
        }
        private static void CopyFolder(string targetRoot, string from, string to, IList<string> filterAndNewExts, ref int ct)
        {
            if (!string.IsNullOrEmpty(to) && !Directory.Exists(to))
                Directory.CreateDirectory(to);
            // sub directories
            foreach (string sub in Directory.GetDirectories(from)) {
                var srcPath = Path.GetFullPath(sub);
                if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) {
                    if (srcPath.IndexOf(targetRoot) == 0)
                        continue;
                }
                else {
                    if (srcPath.IndexOf(targetRoot, StringComparison.CurrentCultureIgnoreCase) == 0)
                        continue;
                }
                var sName = Path.GetFileName(sub);
                CopyFolder(targetRoot, sub, Path.Combine(to, sName), filterAndNewExts, ref ct);
            }
            // file
            for (int i = 0; i < filterAndNewExts.Count; i += 2) {
                string filter = filterAndNewExts[i];
                string newExt = string.Empty;
                if (i + 1 < filterAndNewExts.Count) {
                    newExt = filterAndNewExts[i + 1];
                }
                foreach (string file in Directory.GetFiles(from, filter, SearchOption.TopDirectoryOnly)) {
                    string targetFile;
                    if (string.IsNullOrEmpty(newExt))
                        targetFile = Path.Combine(to, Path.GetFileName(file));
                    else
                        targetFile = Path.Combine(to, Path.ChangeExtension(Path.GetFileName(file), newExt));
                    File.Copy(file, targetFile, true);
                    ++ct;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("copy file {0} => {1}", file, targetFile);
                    }
                }
            }
        }
    }
    internal sealed class MoveDirectoryExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: movedir(dir1,dir2) api");
            bool ret = false;
            if (operands.Count >= 2) {
                var dir1 = operands[0].AsString;
                var dir2 = operands[1].AsString;
                dir1 = Environment.ExpandEnvironmentVariables(dir1);
                dir2 = Environment.ExpandEnvironmentVariables(dir2);
                if (Directory.Exists(dir1)) {
                    if (Directory.Exists(dir2)) {
                        Directory.Delete(dir2);
                    }
                    Directory.Move(dir1, dir2);
                    ret = true;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("move directory {0} => {1}", dir1, dir2);
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class DeleteDirectoryExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: deletedir(dir) api");
            bool ret = false;
            if (operands.Count >= 1) {
                var dir = operands[0].AsString;
                dir = Environment.ExpandEnvironmentVariables(dir);
                if (Directory.Exists(dir)) {
                    Directory.Delete(dir, true);
                    ret = true;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("delete directory {0}", dir);
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class CopyFileExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: copyfile(file1,file2) api");
            bool ret = false;
            if (operands.Count >= 2) {
                var file1 = operands[0].AsString;
                var file2 = operands[1].AsString;
                file1 = Environment.ExpandEnvironmentVariables(file1);
                file2 = Environment.ExpandEnvironmentVariables(file2);
                if (File.Exists(file1)) {
                    var dir = Path.GetDirectoryName(file2);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) {
                        Directory.CreateDirectory(dir);
                    }
                    File.Copy(file1, file2, true);
                    ret = true;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("copy file {0} => {1}", file1, file2);
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class CopyFilesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: copyfiles(dir1,dir2,filter_list_or_str_1,filter_list_or_str_2,...) api, dont include subdir");
            int ct = 0;
            if (operands.Count >= 2) {
                var dir1 = operands[0].AsString;
                var dir2 = operands[1].AsString;
                dir1 = Environment.ExpandEnvironmentVariables(dir1);
                dir2 = Environment.ExpandEnvironmentVariables(dir2);
                List<string> filterAndNewExts = new List<string>();
                for (int i = 2; i < operands.Count; ++i) {
                    var str = operands[i].AsString;
                    if (null != str) {
                        filterAndNewExts.Add(str);
                    }
                    else {
                        var strList = operands[i].As<IList>();
                        if (null != strList) {
                            foreach (var strObj in strList) {
                                var tempStr = strObj as string;
                                if (null != tempStr)
                                    filterAndNewExts.Add(tempStr);
                            }
                        }
                    }
                }
                if (filterAndNewExts.Count <= 0) {
                    filterAndNewExts.Add("*");
                }
                if (Directory.Exists(dir1)) {
                    CopyFolder(dir1, dir2, filterAndNewExts, ref ct);
                }
            }
            return ct;
        }
        private static void CopyFolder(string from, string to, IList<string> filterAndNewExts, ref int ct)
        {
            if (!string.IsNullOrEmpty(to) && !Directory.Exists(to))
                Directory.CreateDirectory(to);
            // file
            for (int i = 0; i < filterAndNewExts.Count; i += 2) {
                string filter = filterAndNewExts[i];
                string newExt = string.Empty;
                if (i + 1 < filterAndNewExts.Count) {
                    newExt = filterAndNewExts[i + 1];
                }
                foreach (string file in Directory.GetFiles(from, filter, SearchOption.TopDirectoryOnly)) {
                    string targetFile;
                    if (string.IsNullOrEmpty(newExt))
                        targetFile = Path.Combine(to, Path.GetFileName(file));
                    else
                        targetFile = Path.Combine(to, Path.ChangeExtension(Path.GetFileName(file), newExt));
                    File.Copy(file, targetFile, true);
                    ++ct;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("copy file {0} => {1}", file, targetFile);
                    }
                }
            }
        }
    }
    internal sealed class MoveFileExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: movefile(file1,file2) api");
            bool ret = false;
            if (operands.Count >= 2) {
                var file1 = operands[0].AsString;
                var file2 = operands[1].AsString;
                file1 = Environment.ExpandEnvironmentVariables(file1);
                file2 = Environment.ExpandEnvironmentVariables(file2);
                if (File.Exists(file1)) {
                    var dir = Path.GetDirectoryName(file2);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) {
                        Directory.CreateDirectory(dir);
                    }
                    if (File.Exists(file2)) {
                        File.Delete(file2);
                    }
                    File.Move(file1, file2);
                    ret = true;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("move file {0} => {1}", file1, file2);
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class DeleteFileExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: deletefile(file) api");
            bool ret = false;
            if (operands.Count >= 1) {
                var file = operands[0].AsString;
                file = Environment.ExpandEnvironmentVariables(file);
                if (File.Exists(file)) {
                    File.Delete(file);
                    ret = true;

                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("delete file {0}", file);
                    }
                }
            }
            return ret;
        }
    }
    internal sealed class DeleteFilesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: deletefiles(dir,filter_list_or_str_1,filter_list_or_str_2,...) api, dont include subdir");
            int ct = 0;
            if (operands.Count >= 1) {
                var dir = operands[0].AsString;
                List<string> filters = new List<string>();
                for (int i = 1; i < operands.Count; ++i) {
                    var str = operands[i].AsString;
                    if (null != str) {
                        filters.Add(str);
                    }
                    else {
                        var strList = operands[i].As<IList>();
                        if (null != strList) {
                            foreach (var strObj in strList) {
                                var tempStr = strObj as string;
                                if (null != tempStr)
                                    filters.Add(tempStr);
                            }
                        }
                    }
                }
                if (filters.Count <= 0) {
                    filters.Add("*");
                }
                dir = Environment.ExpandEnvironmentVariables(dir);
                if (Directory.Exists(dir)) {
                    foreach (var filter in filters) {
                        foreach (string file in Directory.GetFiles(dir, filter, SearchOption.TopDirectoryOnly)) {
                            File.Delete(file);
                            ++ct;

                            if (DslCalculator.FileEchoOn) {
                                Console.WriteLine("delete file {0}", file);
                            }
                        }
                    }
                }
            }
            return ct;
        }
    }
    internal sealed class DeleteAllFilesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: deleteallfiles(dir,filter_list_or_str_1,filter_list_or_str_2,...) api, include subdir");
            int ct = 0;
            if (operands.Count >= 1) {
                var dir = operands[0].AsString;
                List<string> filters = new List<string>();
                for (int i = 1; i < operands.Count; ++i) {
                    var str = operands[i].AsString;
                    if (null != str) {
                        filters.Add(str);
                    }
                    else {
                        var strList = operands[i].As<IList>();
                        if (null != strList) {
                            foreach (var strObj in strList) {
                                var tempStr = strObj as string;
                                if (null != tempStr)
                                    filters.Add(tempStr);
                            }
                        }
                    }
                }
                if (filters.Count <= 0) {
                    filters.Add("*");
                }
                dir = Environment.ExpandEnvironmentVariables(dir);
                if (Directory.Exists(dir)) {
                    foreach (var filter in filters) {
                        foreach (string file in Directory.GetFiles(dir, filter, SearchOption.AllDirectories)) {
                            File.Delete(file);
                            ++ct;

                            if (DslCalculator.FileEchoOn) {
                                Console.WriteLine("delete file {0}", file);
                            }
                        }
                    }
                }
            }
            return ct;
        }
    }
    internal sealed class GetFileInfoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: getfileinfo(file) api");
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var file = operands[0].AsString;
                file = Environment.ExpandEnvironmentVariables(file);
                if (File.Exists(file)) {
                    ret = BoxedValue.FromObject(new FileInfo(file));
                }
            }
            return ret;
        }
    }
    internal sealed class GetDirectoryInfoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: getdirinfo(dir) api");
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var file = operands[0].AsString;
                file = Environment.ExpandEnvironmentVariables(file);
                if (Directory.Exists(file)) {
                    ret = BoxedValue.FromObject(new DirectoryInfo(file));
                }
            }
            return ret;
        }
    }
    internal sealed class GetDriveInfoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: getdriveinfo(drive) api");
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var drive = operands[0].AsString;
                ret = BoxedValue.FromObject(new DriveInfo(drive));
            }
            return ret;
        }
    }
    internal sealed class GetDrivesInfoExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: getdrivesinfo() api");
            var ret = DriveInfo.GetDrives();
            return BoxedValue.FromObject(ret);
        }
    }
    internal sealed class ReadAllLinesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1 || operands.Count > 2)
                throw new Exception("Expected: readalllines(file[,encoding]) api");
            if (operands.Count >= 1) {
                string path = operands[0].AsString;
                if (!string.IsNullOrEmpty(path)) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    Encoding encoding = Encoding.UTF8;
                    if (operands.Count >= 2) {
                        var v = operands[1];
                        encoding = GetEncoding(v);
                    }
                    return BoxedValue.FromObject(File.ReadAllLines(path, encoding));
                }
            }
            return BoxedValue.FromObject(new string[0]);
        }
    }
    internal sealed class WriteAllLinesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2 || operands.Count > 3)
                throw new Exception("Expected: writealllines(file,lines[,encoding]) api");
            if (operands.Count >= 2) {
                string path = operands[0].AsString;
                var lines = operands[1].As<IList>();
                if (!string.IsNullOrEmpty(path) && null != lines) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    Encoding encoding = Encoding.UTF8;
                    if (operands.Count >= 3) {
                        var v = operands[2];
                        encoding = GetEncoding(v);
                    }
                    var strs = new List<string>();
                    foreach (var line in lines) {
                        strs.Add(line.ToString());
                    }
                    File.WriteAllLines(path, strs, encoding);
                    return true;
                }
            }
            return false;
        }
    }
    internal sealed class ReadAllTextExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1 || operands.Count > 2)
                throw new Exception("Expected: readalltext(file[,encoding]) api");
            if (operands.Count >= 1) {
                string path = operands[0].AsString;
                if (!string.IsNullOrEmpty(path)) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    Encoding encoding = Encoding.UTF8;
                    if (operands.Count >= 2) {
                        var v = operands[1];
                        encoding = GetEncoding(v);
                    }
                    return File.ReadAllText(path, encoding);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class WriteAllTextExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2 || operands.Count > 3)
                throw new Exception("Expected: writealltext(file,txt[,encoding]) api");
            if (operands.Count >= 2) {
                string path = operands[0].AsString;
                var text = operands[1].AsString;
                if (!string.IsNullOrEmpty(path) && null != text) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    Encoding encoding = Encoding.UTF8;
                    if (operands.Count >= 3) {
                        var v = operands[2];
                        encoding = GetEncoding(v);
                    }
                    File.WriteAllText(path, text, encoding);
                    return true;
                }
            }
            return false;
        }
    }
    internal sealed class CommandExp : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            int exitCode = 0;
            MemoryStream ims = null, oms = null;
            int ct = m_CommandConfigs.Count;
            for (int i = 0; i < ct; ++i) {
                try {
                    if (i > 0) {
                        ims = oms;
                        oms = null;
                    }
                    if (i < ct - 1) {
                        oms = new MemoryStream();
                    }
                    var cfg = m_CommandConfigs[i];
                    if (cfg.m_Commands.Count > 0) {
                        exitCode = ExecCommand(cfg, ims, oms);
                    }
                    else {
                        exitCode = ExecProcess(cfg, ims, oms);
                    }
                }
                finally {
                    if (null != ims) {
                        ims.Close();
                        ims.Dispose();
                        ims = null;
                    }
                }
            }
            return exitCode;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            var id = funcData.GetId();
            if (funcData.IsHighOrder) {
                var callData = funcData.LowerOrderFunction;
                LoadCall(callData);
            }
            else if (funcData.HaveParam()) {
                LoadCall(funcData);
            }
            else {
                var cmd = new CommandConfig();
                m_CommandConfigs.Add(cmd);
            }
            if (funcData.HaveStatement()) {
                var cmd = m_CommandConfigs[m_CommandConfigs.Count - 1];
                for (int i = 0; i < funcData.GetParamNum(); ++i) {
                    var comp = funcData.GetParam(i);
                    var cd = comp as Dsl.FunctionData;
                    if (null != cd) {
                        int num = cd.GetParamNum();
                        if (cd.HaveExternScript()) {
                            string os = cd.GetId();
                            string txt = cd.GetParamId(0);
                            cmd.m_Commands.Add(os, txt);
                        }
                        else if (num >= 2) {
                            string type = cd.GetId();
                            var exp = Calculator.Load(cd.GetParam(0));
                            var opt = Calculator.Load(cd.GetParam(1));
                            if (type == "output") {
                                cmd.m_Output = exp;
                                cmd.m_OutputOptArg = opt;
                            }
                            else if (type == "error") {
                                cmd.m_Error = exp;
                                cmd.m_ErrorOptArg = opt;
                            }
                            else {
                                Calculator.Log("[syntax error] {0} line:{1}", cd.ToScriptString(false, Dsl.DelimiterInfo.Default), cd.GetLine());
                            }
                        }
                        else if (num >= 1) {
                            string type = cd.GetId();
                            var exp = Calculator.Load(cd.GetParam(0));
                            if (type == "input") {
                                cmd.m_Input = exp;
                            }
                            else if (type == "output") {
                                cmd.m_Output = exp;
                            }
                            else if (type == "error") {
                                cmd.m_Error = exp;
                            }
                            else if (type == "redirecttoconsole") {
                                cmd.m_RedirectToConsole = exp;
                            }
                            else if (type == "nowait") {
                                cmd.m_NoWait = exp;
                            }
                            else if (type == "useshellexecute") {
                                cmd.m_UseShellExecute = exp;
                            }
                            else if (type == "verb") {
                                cmd.m_Verb = exp;
                            }
                            else if (type == "domain") {
                                cmd.m_Domain = exp;
                            }
                            else if (type == "user") {
                                cmd.m_UserName = exp;
                            }
                            else if (type == "password") {
                                cmd.m_Password = exp;
                            }
                            else if (type == "passwordincleartext") {
                                cmd.m_PasswordInClearText = exp;
                            }
                            else if (type == "loadprofile") {
                                cmd.m_LoadUserProfile = exp;
                            }
                            else if (type == "windowstyle") {
                                cmd.m_WindowStyle = exp;
                            }
                            else if (type == "createwindow") {
                                cmd.m_CreateWindow = exp;
                            }
                            else if (type == "errordialog") {
                                cmd.m_ErrorDialog = exp;
                            }
                            else if (type == "workingdirectory") {
                                cmd.m_WorkingDirectory = exp;
                            }
                            else if (type == "encoding") {
                                cmd.m_Encoding = exp;
                            }
                            else {
                                Calculator.Log("[syntax error] {0} line:{1}", cd.ToScriptString(false, Dsl.DelimiterInfo.Default), cd.GetLine());
                            }
                        }
                        else {
                            Calculator.Log("[syntax error] {0} line:{1}", cd.ToScriptString(false, Dsl.DelimiterInfo.Default), cd.GetLine());
                        }
                    }
                    else {
                        Calculator.Log("[syntax error] {0} line:{1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
                    }
                }
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            for (int i = 0; i < statementData.GetFunctionNum(); ++i) {
                var func = statementData.GetFunction(i);
                var vd = func.AsValue;
                if (null != vd) {
                    Load(vd);
                }
                else {
                    var fd = func.AsFunction;
                    Load(fd);
                }
            }
            return true;
        }

        private bool LoadCall(Dsl.FunctionData callData)
        {
            var cmd = new CommandConfig();
            m_CommandConfigs.Add(cmd);

            var id = callData.GetId();
            if (id == "process") {
                int num = callData.GetParamNum();
                if (num > 0) {
                    var param0 = callData.GetParam(0);
                    var exp0 = Calculator.Load(param0);
                    cmd.m_FileName = exp0;

                    if (num > 1) {
                        var param1 = callData.GetParam(1);
                        var exp1 = Calculator.Load(param1);
                        cmd.m_Argments = exp1;
                    }
                }
                else {
                    Calculator.Log("[syntax error] {0} line:{1}", callData.ToScriptString(false, Dsl.DelimiterInfo.Default), callData.GetLine());
                }
            }
            else if (id == "command") {
                int num = callData.GetParamNum();
                if (num > 0) {
                    Calculator.Log("[syntax error] {0} line:{1}", callData.ToScriptString(false, Dsl.DelimiterInfo.Default), callData.GetLine());
                }
            }
            else {
                Calculator.Log("[syntax error] {0} line:{1}", callData.ToScriptString(false, Dsl.DelimiterInfo.Default), callData.GetLine());
            }
            return true;
        }
        private int ExecProcess(CommandConfig cfg, Stream istream, Stream ostream)
        {
            string fileName = string.Empty;
            if (null != cfg.m_FileName) {
                fileName = cfg.m_FileName.Calc().AsString;
            }
            string args = string.Empty;
            if (null != cfg.m_Argments) {
                args = cfg.m_Argments.Calc().AsString;
            }
            bool noWait = false;
            if (null != cfg.m_NoWait) {
                noWait = cfg.m_NoWait.Calc().GetBool();
            }
            DslCalculator.ProcessStartOption option = new DslCalculator.ProcessStartOption();
            if (null != cfg.m_UseShellExecute) {
                option.UseShellExecute = cfg.m_UseShellExecute.Calc().GetBool();
            }
            if (null != cfg.m_Verb) {
                option.Verb = cfg.m_Verb.Calc().AsString;
            }
            if (null != cfg.m_Domain) {
                option.Domain = cfg.m_Domain.Calc().AsString;
            }
            if (null != cfg.m_UserName) {
                option.UserName = cfg.m_UserName.Calc().AsString;
            }
            if (null != cfg.m_Password) {
                option.Password = cfg.m_Password.Calc().AsString;
            }
            if (null != cfg.m_PasswordInClearText) {
                option.PasswordInClearText = cfg.m_PasswordInClearText.Calc().AsString;
            }
            if (null != cfg.m_LoadUserProfile) {
                option.LoadUserProfile = cfg.m_LoadUserProfile.Calc().GetBool();
            }
            if (null != cfg.m_WindowStyle) {
                var str = cfg.m_WindowStyle.Calc().AsString;
                System.Diagnostics.ProcessWindowStyle style;
                if (Enum.TryParse(str, out style)) {
                    option.WindowStyle = style;
                }
            }
            if (null != cfg.m_CreateWindow) {
                option.CreateWindow = cfg.m_CreateWindow.Calc().GetBool();
            }
            if (null != cfg.m_ErrorDialog) {
                option.ErrorDialog = cfg.m_ErrorDialog.Calc().GetBool();
            }
            if (null != cfg.m_WorkingDirectory) {
                option.WorkingDirectory = cfg.m_WorkingDirectory.Calc().AsString;
            }
            Encoding encoding = null;
            if (null != cfg.m_Encoding) {
                var v = cfg.m_Encoding.Calc();
                var name = v.AsString;
                if (!string.IsNullOrEmpty(name)) {
                    encoding = Encoding.GetEncoding(name);
                }
                else if (v.IsInteger) {
                    int codePage = v.GetInt();
                    encoding = Encoding.GetEncoding(codePage);
                }
            }

            fileName = Environment.ExpandEnvironmentVariables(fileName);
            args = Environment.ExpandEnvironmentVariables(args);

            IList<string> input = null;
            if (null != cfg.m_Input) {
                var v = cfg.m_Input.Calc();
                try {
                    var list = v.As<IList>();
                    if (null != list) {
                        var slist = new List<string>();
                        foreach (var s in list) {
                            slist.Add(s.ToString());
                        }
                        input = slist;
                    }
                    else {
                        var str = v.AsString;
                        if (!string.IsNullOrEmpty(str)) {
                            str = Environment.ExpandEnvironmentVariables(str);
                            input = File.ReadAllLines(str);
                        }
                    }
                }
                catch (Exception ex) {
                    Calculator.Log("input {0} failed:{1}", v, ex.Message);
                }
            }
            bool redirectToConsole = DslCalculator.FileEchoOn;
            StringBuilder outputBuilder = null;
            StringBuilder errorBuilder = null;
            var output = BoxedValue.NullObject;
            int outputIx = -1;
            var error = BoxedValue.NullObject;
            int errorIx = -1;
            if (null != cfg.m_Output) {
                var v = cfg.m_Output.Calc();
                var str = v.AsString;
                if (!string.IsNullOrEmpty(str)) {
                    str = Environment.ExpandEnvironmentVariables(str);
                    output = str;
                }
                else {
                    output = v;
                }
                if (null != cfg.m_OutputOptArg)
                    outputIx = cfg.m_OutputOptArg.Calc().GetInt();
                outputBuilder = new StringBuilder();
            }
            if (null != cfg.m_Error) {
                var v = cfg.m_Error.Calc();
                var str = v.AsString;
                if (!string.IsNullOrEmpty(str)) {
                    str = Environment.ExpandEnvironmentVariables(str);
                    error = str;
                }
                else {
                    error = v;
                }
                if (null != cfg.m_ErrorOptArg)
                    errorIx = cfg.m_ErrorOptArg.Calc().GetInt();
                errorBuilder = new StringBuilder();
            }
            if (null != cfg.m_RedirectToConsole) {
                var v = cfg.m_RedirectToConsole.Calc();
                redirectToConsole = v.GetBool();
            }
            int exitCode = DslCalculator.NewProcess(noWait, fileName, args, option, istream, ostream, input, outputBuilder, errorBuilder, redirectToConsole, encoding);
            if (DslCalculator.FileEchoOn) {
                Console.WriteLine("new process:{0} {1}, exit code:{2}", fileName, args, exitCode);
            }

            if (null != outputBuilder && !output.IsNullObject) {
                try {
                    var file = output.AsString;
                    if (!string.IsNullOrEmpty(file)) {
                        if (file[0] == '@' || file[0] == '$') {
                            Calculator.SetVariable(file, outputBuilder.ToString());
                        }
                        else {
                            File.WriteAllText(file, outputBuilder.ToString());
                        }
                    }
                    else if (outputIx >= 0) {
                        var list = output.As<IList>();
                        while (list.Count <= outputIx) {
                            list.Add(null);
                        }
                        list[outputIx] = outputBuilder.ToString();
                    }
                }
                catch (Exception ex) {
                    Calculator.Log("output {0} failed:{1}", output, ex.Message);
                }
            }
            if (null != errorBuilder && !error.IsNullObject) {
                try {
                    var file = error.AsString;
                    if (!string.IsNullOrEmpty(file)) {
                        if (file[0] == '@' || file[0] == '$') {
                            Calculator.SetVariable(file, errorBuilder.ToString());
                        }
                        else {
                            File.WriteAllText(file, errorBuilder.ToString());
                        }
                    }
                    else if (errorIx >= 0) {
                        var list = error.As<IList>();
                        while (list.Count <= errorIx) {
                            list.Add(null);
                        }
                        list[errorIx] = errorBuilder.ToString();
                    }
                }
                catch (Exception ex) {
                    Calculator.Log("error {0} failed:{1}", error, ex.Message);
                }
            }
            return exitCode;
        }
        private int ExecCommand(CommandConfig cfg, Stream istream, Stream ostream)
        {
            int exitCode = 0;
            string os = string.Empty;
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                os = "unix";
            else
                os = "win";
            string cmd;
            if (cfg.m_Commands.TryGetValue(os, out cmd) || cfg.m_Commands.TryGetValue("common", out cmd)) {
                bool noWait = false;
                if (null != cfg.m_NoWait) {
                    noWait = cfg.m_NoWait.Calc().GetBool();
                }
                DslCalculator.ProcessStartOption option = new DslCalculator.ProcessStartOption();
                if (null != cfg.m_UseShellExecute) {
                    option.UseShellExecute = cfg.m_UseShellExecute.Calc().GetBool();
                }
                if (null != cfg.m_Verb) {
                    option.Verb = cfg.m_Verb.Calc().AsString;
                }
                if (null != cfg.m_Domain) {
                    option.Domain = cfg.m_Domain.Calc().AsString;
                }
                if (null != cfg.m_UserName) {
                    option.UserName = cfg.m_UserName.Calc().AsString;
                }
                if (null != cfg.m_Password) {
                    option.Password = cfg.m_Password.Calc().AsString;
                }
                if (null != cfg.m_PasswordInClearText) {
                    option.PasswordInClearText = cfg.m_PasswordInClearText.Calc().AsString;
                }
                if (null != cfg.m_LoadUserProfile) {
                    option.LoadUserProfile = cfg.m_LoadUserProfile.Calc().GetBool();
                }
                if (null != cfg.m_WindowStyle) {
                    var str = cfg.m_WindowStyle.Calc().AsString;
                    System.Diagnostics.ProcessWindowStyle style;
                    if (Enum.TryParse(str, out style)) {
                        option.WindowStyle = style;
                    }
                }
                if (null != cfg.m_CreateWindow) {
                    option.CreateWindow = cfg.m_CreateWindow.Calc().GetBool();
                }
                if (null != cfg.m_ErrorDialog) {
                    option.ErrorDialog = cfg.m_ErrorDialog.Calc().GetBool();
                }
                if (null != cfg.m_WorkingDirectory) {
                    option.WorkingDirectory = cfg.m_WorkingDirectory.Calc().AsString;
                }
                Encoding encoding = null;
                if (null != cfg.m_Encoding) {
                    var v = cfg.m_Encoding.Calc();
                    var name = v.AsString;
                    if (!string.IsNullOrEmpty(name)) {
                        encoding = Encoding.GetEncoding(name);
                    }
                    else if (v.IsInteger) {
                        int codePage = v.GetInt();
                        encoding = Encoding.GetEncoding(codePage);
                    }
                }
                IList<string> input = null;
                if (null != cfg.m_Input) {
                    var v = cfg.m_Input.Calc();
                    try {
                        var list = v.As<IList>();
                        if (null != list) {
                            var slist = new List<string>();
                            foreach (var s in list) {
                                slist.Add(s.ToString());
                            }
                            input = slist;
                        }
                        else {
                            var str = v.AsString;
                            if (!string.IsNullOrEmpty(str)) {
                                str = Environment.ExpandEnvironmentVariables(str);
                                input = File.ReadAllLines(str);
                            }
                        }
                    }
                    catch (Exception ex) {
                        Calculator.Log("input {0} failed:{1}", v, ex.Message);
                    }
                }
                bool redirectToConsole = DslCalculator.FileEchoOn;
                StringBuilder outputBuilder = null;
                StringBuilder errorBuilder = null;
                var output = BoxedValue.NullObject;
                int outputIx = -1;
                var error = BoxedValue.NullObject;
                int errorIx = -1;
                if (null != cfg.m_Output) {
                    var v = cfg.m_Output.Calc();
                    var str = v.AsString;
                    if (!string.IsNullOrEmpty(str)) {
                        str = Environment.ExpandEnvironmentVariables(str);
                        output = str;
                    }
                    else {
                        output = v;
                    }
                    if (null != cfg.m_OutputOptArg)
                        outputIx = cfg.m_OutputOptArg.Calc().GetInt();
                    outputBuilder = new StringBuilder();
                }
                if (null != cfg.m_Error) {
                    var v = cfg.m_Error.Calc();
                    var str = v.AsString;
                    if (!string.IsNullOrEmpty(str)) {
                        str = Environment.ExpandEnvironmentVariables(str);
                        error = str;
                    }
                    else {
                        error = v;
                    }
                    if (null != cfg.m_ErrorOptArg)
                        errorIx = cfg.m_ErrorOptArg.Calc().GetInt();
                    errorBuilder = new StringBuilder();
                }
                if (null != cfg.m_RedirectToConsole) {
                    var v = cfg.m_RedirectToConsole.Calc();
                    redirectToConsole = v.GetBool();
                }

                cmd = cmd.Trim();
                var lines = cmd.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                string oneCmd = string.Join(" ", lines).Trim();
                if (!string.IsNullOrEmpty(oneCmd)) {
                    int split = oneCmd.IndexOfAny(new char[] { ' ', '\t' });
                    string fileName = oneCmd;
                    string args = string.Empty;
                    if (split > 0) {
                        fileName = oneCmd.Substring(0, split).Trim();
                        args = oneCmd.Substring(split).Trim();
                    }

                    fileName = Environment.ExpandEnvironmentVariables(fileName);
                    args = Environment.ExpandEnvironmentVariables(args);

                    exitCode = DslCalculator.NewProcess(noWait, fileName, args, option, istream, ostream, input, outputBuilder, errorBuilder, redirectToConsole, encoding);
                    if (DslCalculator.FileEchoOn) {
                        Console.WriteLine("new process:{0} {1}, exit code:{2}", fileName, args, exitCode);
                    }

                    if (null != outputBuilder && !output.IsNullObject) {
                        try {
                            var file = output.AsString;
                            if (!string.IsNullOrEmpty(file)) {
                                if (file[0] == '@' || file[0] == '$') {
                                    Calculator.SetVariable(file, outputBuilder.ToString());
                                }
                                else {
                                    File.WriteAllText(file, outputBuilder.ToString());
                                }
                            }
                            else if (outputIx >= 0) {
                                var list = output.As<IList>();
                                while (list.Count <= outputIx) {
                                    list.Add(null);
                                }
                                list[outputIx] = outputBuilder.ToString();
                            }
                        }
                        catch (Exception ex) {
                            Calculator.Log("output {0} failed:{1}", output, ex.Message);
                        }
                    }
                    if (null != errorBuilder && !error.IsNullObject) {
                        try {
                            var file = error.AsString;
                            if (!string.IsNullOrEmpty(file)) {
                                if (file[0] == '@' || file[0] == '$') {
                                    Calculator.SetVariable(file, errorBuilder.ToString());
                                }
                                else {
                                    File.WriteAllText(file, errorBuilder.ToString());
                                }
                            }
                            else if (errorIx >= 0) {
                                var list = error.As<IList>();
                                while (list.Count <= errorIx) {
                                    list.Add(null);
                                }
                                list[errorIx] = errorBuilder.ToString();
                            }
                        }
                        catch (Exception ex) {
                            Calculator.Log("error {0} failed:{1}", error, ex.Message);
                        }
                    }
                }
            }
            return exitCode;
        }

        private class CommandConfig
        {
            internal IExpression m_FileName = null;
            internal IExpression m_Argments = null;
            internal Dictionary<string, string> m_Commands = new Dictionary<string, string>();

            internal IExpression m_NoWait = null;
            internal IExpression m_UseShellExecute = null;
            internal IExpression m_Verb = null;
            internal IExpression m_Domain = null;
            internal IExpression m_UserName = null;
            internal IExpression m_Password = null;
            internal IExpression m_PasswordInClearText = null;
            internal IExpression m_LoadUserProfile = null;
            internal IExpression m_WindowStyle = null;
            internal IExpression m_CreateWindow = null;
            internal IExpression m_ErrorDialog = null;
            internal IExpression m_WorkingDirectory = null;
            internal IExpression m_Encoding = null;
            internal IExpression m_Input = null;
            internal IExpression m_Output = null;
            internal IExpression m_OutputOptArg = null;
            internal IExpression m_Error = null;
            internal IExpression m_ErrorOptArg = null;
            internal IExpression m_RedirectToConsole = null;
        }

        private List<CommandConfig> m_CommandConfigs = new List<CommandConfig>();
    }
    internal sealed class KillExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: kill(name_or_pid) api");
            int ret = 0;
            if (operands.Count >= 1) {
                int myselfId = 0;
                var myself = System.Diagnostics.Process.GetCurrentProcess();
                if (null != myself) {
                    myselfId = myself.Id;
                }
                var vObj = operands[0];
                var name = vObj.AsString;
                if (!string.IsNullOrEmpty(name)) {
                    int ct = 0;
                    var ps = System.Diagnostics.Process.GetProcessesByName(name);
                    foreach (var p in ps) {
                        if (p.Id != myselfId) {
                            if (DslCalculator.FileEchoOn) {
                                Console.WriteLine("kill {0}[pid:{1},session id:{2}]", p.ProcessName, p.Id, p.SessionId);
                            }
                            p.Kill();
                            ++ct;
                        }
                    }
                    ret = ct;
                }
                else if (vObj.IsInteger) {
                    int pid = vObj.GetInt();
                    var p = System.Diagnostics.Process.GetProcessById(pid);
                    if (null != p && p.Id != myselfId) {
                        if (DslCalculator.FileEchoOn) {
                            Console.WriteLine("kill {0}[pid:{1},session id:{2}]", p.ProcessName, p.Id, p.SessionId);
                        }
                        p.Kill();
                        ret = 1;
                    }
                }
                else {

                }
            }
            return BoxedValue.From(ret);
        }
    }
    internal sealed class KillMeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 1)
                throw new Exception("Expected: killme([exit_code]) api");
            int ret = 0;
            var p = System.Diagnostics.Process.GetCurrentProcess();
            if (null != p) {
                ret = p.Id;
                int exitCode = 0;
                if (operands.Count >= 1) {
                    exitCode = operands[0].GetInt();
                }
                if (DslCalculator.FileEchoOn) {
                    Console.WriteLine("killme {0}[pid:{1},session id:{2}] exit code:{3}", p.ProcessName, p.Id, p.SessionId, exitCode);
                }
                Environment.Exit(exitCode);
            }
            return ret;
        }
    }
    internal sealed class GetCurrentProcessIdExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: pid() api");
            int ret = 0;
            var p = System.Diagnostics.Process.GetCurrentProcess();
            if (null != p) {
                ret = p.Id;
            }
            return ret;
        }
    }
    internal sealed class ListProcessesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 1)
                throw new Exception("Expected: plist([filter]) api, return list");
            IList<System.Diagnostics.Process> ret = null;
            var ps = System.Diagnostics.Process.GetProcesses();
            string filter = null;
            if (operands.Count >= 1) {
                filter = operands[0].AsString;
            }
            if (null == filter)
                filter = string.Empty;
            if (!string.IsNullOrEmpty(filter)) {
                var list = new List<System.Diagnostics.Process>();
                foreach (var p in ps) {
                    try {
                        if (!p.HasExited) {
                            if (p.ProcessName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) {
                                list.Add(p);
                            }
                        }
                    }
                    catch {
                    }
                }
                ret = list;
            }
            else {
                ret = ps;
            }
            return BoxedValue.FromObject(ret);
        }
    }
    internal sealed class SleepExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: wait(time) api");
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var time = operands[0].GetInt();
                System.Threading.Thread.Sleep(time);
                ret = time;
            }
            return ret;
        }
    }
    internal sealed class WaitAllExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 1)
                throw new Exception("Expected: waitall([timeout]) api, wait all task to exit");
            var tasks = DslCalculator.Tasks;
            int timeout = -1;
            if (operands.Count >= 1) {
                timeout = operands[0].GetInt();
            }
            List<int> results = new List<int>();
            if (Task.WaitAll(tasks.ToArray(), timeout)) {
                foreach (var task in tasks) {
                    results.Add(task.Result);
                }
            }
            return BoxedValue.FromObject(results);
        }
    }
    internal sealed class WaitStartIntervalExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count > 1)
                throw new Exception("Expected: waitstartinterval(time) or waitstartinterval() api, used in Task.Wait for process/command");
            if (operands.Count >= 1) {
                var v = operands[0];
                if (!v.IsNullObject) {
                    DslCalculator.CheckStartInterval = v.GetInt();
                }
            }
            return DslCalculator.CheckStartInterval;
        }
    }
    internal sealed class CleanupCompletedTasksExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: cleanupcompletedtasks() api");
            DslCalculator.CleanupCompletedTasks();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetTaskCountExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: gettaskcount() api");
            return DslCalculator.Tasks.Count;
        }
    }
    internal sealed class CalcMd5Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: calcmd5(file) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var file = operands[0].AsString;
                if (null != file) {
                    r = CalcMD5(file);
                }
            }
            return r;
        }
        public string CalcMD5(string file)
        {
            byte[] array = null;
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                MD5 md5 = MD5.Create();
                array = md5.ComputeHash(stream);
                stream.Close();
            }
            if (null != array) {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < array.Length; i++) {
                    stringBuilder.Append(array[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
            else {
                return string.Empty;
            }
        }
    }
    public sealed class DslCalculatorApiRegistry
    {
        public SortedList<string, string> ApiDocs
        {
            get { return m_ApiDocs; }
        }

        public void Register(string name, string doc, IExpressionFactory factory)
        {
            if (m_ApiFactories.ContainsKey(name)) {
                m_ApiFactories[name] = factory;
            }
            else {
                m_ApiFactories.Add(name, factory);
            }
            if (m_ApiDocs.ContainsKey(name)) {
                m_ApiDocs[name] = doc;
            }
            else {
                m_ApiDocs.Add(name, doc);
            }
        }

        public bool TryGetFactory(string name, out IExpressionFactory factory)
        {
            return m_ApiFactories.TryGetValue(name, out factory);
        }

        public void Init()
        {
            Register("args", "args() api", new ExpressionFactoryHelper<ArgsGet>());
            Register("arg", "arg(ix) api", new ExpressionFactoryHelper<ArgGet>());
            Register("argnum", "argnum() api", new ExpressionFactoryHelper<ArgNumGet>());
            Register("inc", "inc(var) or inc(var,val) api", new ExpressionFactoryHelper<IncExp>());
            Register("dec", "dec(var) or dec(var,val) api", new ExpressionFactoryHelper<DecExp>());
            Register("+", "add operator", new ExpressionFactoryHelper<AddExp>());
            Register("-", "sub operator", new ExpressionFactoryHelper<SubExp>());
            Register("*", "mul operator", new ExpressionFactoryHelper<MulExp>());
            Register("/", "div operator", new ExpressionFactoryHelper<DivExp>());
            Register("%", "mod operator", new ExpressionFactoryHelper<ModExp>());
            Register("&", "bitand operator", new ExpressionFactoryHelper<BitAndExp>());
            Register("|", "bitor operator", new ExpressionFactoryHelper<BitOrExp>());
            Register("^", "bitxor operator", new ExpressionFactoryHelper<BitXorExp>());
            Register("~", "bitnot operator", new ExpressionFactoryHelper<BitNotExp>());
            Register("<<", "left shift operator", new ExpressionFactoryHelper<LShiftExp>());
            Register(">>", "right shift operator", new ExpressionFactoryHelper<RShiftExp>());
            Register("max", "max(v1,v2) api", new ExpressionFactoryHelper<MaxExp>());
            Register("min", "min(v1,v2) api", new ExpressionFactoryHelper<MinExp>());
            Register("abs", "abs(v) api", new ExpressionFactoryHelper<AbsExp>());
            Register("sin", "sin(v) api", new ExpressionFactoryHelper<SinExp>());
            Register("cos", "cos(v) api", new ExpressionFactoryHelper<CosExp>());
            Register("tan", "tan(v) api", new ExpressionFactoryHelper<TanExp>());
            Register("asin", "asin(v) api", new ExpressionFactoryHelper<AsinExp>());
            Register("acos", "acos(v) api", new ExpressionFactoryHelper<AcosExp>());
            Register("atan", "atan(v) api", new ExpressionFactoryHelper<AtanExp>());
            Register("atan2", "atan2(v1,v2) api", new ExpressionFactoryHelper<Atan2Exp>());
            Register("sinh", "sinh(v) api", new ExpressionFactoryHelper<SinhExp>());
            Register("cosh", "cosh(v) api", new ExpressionFactoryHelper<CoshExp>());
            Register("tanh", "tanh(v) api", new ExpressionFactoryHelper<TanhExp>());
            Register("rndint", "rndint(min,max) api", new ExpressionFactoryHelper<RndIntExp>());
            Register("rndfloat", "rndfloat(min,max) api", new ExpressionFactoryHelper<RndFloatExp>());
            Register("pow", "pow(v1,v2) api", new ExpressionFactoryHelper<PowExp>());
            Register("sqrt", "sqrt(v) api", new ExpressionFactoryHelper<SqrtExp>());
            Register("exp", "exp(v) api", new ExpressionFactoryHelper<ExpExp>());
            Register("exp2", "exp2(v) api", new ExpressionFactoryHelper<Exp2Exp>());
            Register("log", "log(v) api", new ExpressionFactoryHelper<LogExp>());
            Register("log2", "log2(v) api", new ExpressionFactoryHelper<Log2Exp>());
            Register("log10", "log10(v) api", new ExpressionFactoryHelper<Log10Exp>());
            Register("floor", "floor(v) api", new ExpressionFactoryHelper<FloorExp>());
            Register("ceiling", "ceiling(v) api", new ExpressionFactoryHelper<CeilingExp>());
            Register("round", "round(v) api", new ExpressionFactoryHelper<RoundExp>());
            Register("floortoint", "floortoint(v) api", new ExpressionFactoryHelper<FloorToIntExp>());
            Register("ceilingtoint", "ceilingtoint(v) api", new ExpressionFactoryHelper<CeilingToIntExp>());
            Register("roundtoint", "roundtoint(v) api", new ExpressionFactoryHelper<RoundToIntExp>());
            Register("bool", "bool(v) api", new ExpressionFactoryHelper<BoolExp>());
            Register("sbyte", "sbyte(v) api", new ExpressionFactoryHelper<SByteExp>());
            Register("byte", "byte(v) api", new ExpressionFactoryHelper<ByteExp>());
            Register("char", "char(v) api", new ExpressionFactoryHelper<CharExp>());
            Register("short", "short(v) api", new ExpressionFactoryHelper<ShortExp>());
            Register("ushort", "ushort(v) api", new ExpressionFactoryHelper<UShortExp>());
            Register("int", "int(v) api", new ExpressionFactoryHelper<IntExp>());
            Register("uint", "uint(v) api", new ExpressionFactoryHelper<UIntExp>());
            Register("long", "long(v) api", new ExpressionFactoryHelper<LongExp>());
            Register("ulong", "ulong(v) api", new ExpressionFactoryHelper<ULongExp>());
            Register("float", "float(v) api", new ExpressionFactoryHelper<FloatExp>());
            Register("double", "double(v) api", new ExpressionFactoryHelper<DoubleExp>());
            Register("decimal", "decimal(v) api", new ExpressionFactoryHelper<DecimalExp>());
            Register("ftoi", "ftoi(v) api", new ExpressionFactoryHelper<FtoiExp>());
            Register("itof", "itof(v) api", new ExpressionFactoryHelper<ItofExp>());
            Register("ftou", "ftou(v) api", new ExpressionFactoryHelper<FtouExp>());
            Register("utof", "utof(v) api", new ExpressionFactoryHelper<UtofExp>());
            Register("dtol", "dtol(v) api", new ExpressionFactoryHelper<DtolExp>());
            Register("ltod", "ltod(v) api", new ExpressionFactoryHelper<LtodExp>());
            Register("dtou", "dtou(v) api", new ExpressionFactoryHelper<DtouExp>());
            Register("utod", "utod(v) api", new ExpressionFactoryHelper<UtodExp>());
            Register("lerp", "lerp(a,b,t) api", new ExpressionFactoryHelper<LerpExp>());
            Register("lerpunclamped", "lerpunclamped(a,b,t) api", new ExpressionFactoryHelper<LerpUnclampedExp>());
            Register("lerpangle", "lerpangle(a,b,t) api", new ExpressionFactoryHelper<LerpAngleExp>());
            Register("smoothstep", "smoothstep(from,to,t) api", new ExpressionFactoryHelper<SmoothStepExp>());
            Register("clamp01", "clamp01(v) api", new ExpressionFactoryHelper<Clamp01Exp>());
            Register("clamp", "clamp(v,v1,v2) api", new ExpressionFactoryHelper<ClampExp>());
            Register("approximately", "approximately(v1,v2) api", new ExpressionFactoryHelper<ApproximatelyExp>());
            Register("ispoweroftwo", "ispoweroftwo(v) api", new ExpressionFactoryHelper<IsPowerOfTwoExp>());
            Register("closestpoweroftwo", "closestpoweroftwo(v) api", new ExpressionFactoryHelper<ClosestPowerOfTwoExp>());
            Register("nextpoweroftwo", "nextpoweroftwo(v) api", new ExpressionFactoryHelper<NextPowerOfTwoExp>());
            Register("dist", "dist(x1,y1,x2,y2) api", new ExpressionFactoryHelper<DistExp>());
            Register("distsqr", "distsqr(x1,y1,x2,y2) api", new ExpressionFactoryHelper<DistSqrExp>());
            Register(">", "great operator", new ExpressionFactoryHelper<GreatExp>());
            Register(">=", "great equal operator", new ExpressionFactoryHelper<GreatEqualExp>());
            Register("<", "less operator", new ExpressionFactoryHelper<LessExp>());
            Register("<=", "less equal operator", new ExpressionFactoryHelper<LessEqualExp>());
            Register("==", "equal operator", new ExpressionFactoryHelper<EqualExp>());
            Register("!=", "not equal operator", new ExpressionFactoryHelper<NotEqualExp>());
            Register("&&", "logical and operator", new ExpressionFactoryHelper<AndExp>());
            Register("||", "logical or operator", new ExpressionFactoryHelper<OrExp>());
            Register("!", "logical not operator", new ExpressionFactoryHelper<NotExp>());
            Register("?", "conditional expression", new ExpressionFactoryHelper<CondExp>());
            Register("if", "if(cond)func(args); or if(cond){...}[elseif/elif(cond){...}else{...}]; statement", new ExpressionFactoryHelper<IfExp>());
            Register("while", "while(cond)func(args); or while(cond){...}; statement, iterator is $$", new ExpressionFactoryHelper<WhileExp>());
            Register("loop", "loop(ct)func(args); or loop(ct){...}; statement, iterator is $$", new ExpressionFactoryHelper<LoopExp>());
            Register("looplist", "looplist(list)func(args); or looplist(list){...}; statement, iterator is $$", new ExpressionFactoryHelper<LoopListExp>());
            Register("foreach", "foreach(args)func(args); or foreach(args){...}; statement, iterator is $$", new ExpressionFactoryHelper<ForeachExp>());
            Register("format", "format(fmt,arg1,arg2,...) api", new ExpressionFactoryHelper<FormatExp>());
            Register("gettypeassemblyname", "gettypeassemblyname(obj) api", new ExpressionFactoryHelper<GetTypeAssemblyNameExp>());
            Register("gettypefullname", "gettypefullname(obj) api", new ExpressionFactoryHelper<GetTypeFullNameExp>());
            Register("gettypename", "gettypename(obj) api", new ExpressionFactoryHelper<GetTypeNameExp>());
            Register("gettype", "gettype(type_str) api", new ExpressionFactoryHelper<GetTypeExp>());
            Register("changetype", "changetype(obj,type_str) api", new ExpressionFactoryHelper<ChangeTypeExp>());
            Register("parseenum", "parseenum(type_str,val_str) api", new ExpressionFactoryHelper<ParseEnumExp>());
            Register("dotnetcall", "dotnetcall api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<DotnetCallExp>());
            Register("dotnetset", "dotnetset api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<DotnetSetExp>());
            Register("dotnetget", "dotnetget api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<DotnetGetExp>());
            Register("collectioncall", "collectioncall api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<CollectionCallExp>());
            Register("collectionset", "collectionset api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<CollectionSetExp>());
            Register("collectionget", "collectionget api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<CollectionGetExp>());
            Register("linq", "linq(list,method,arg1,arg2,...) statement, internal implementation, using obj.method(arg1,arg2,...) syntax, method can be orderby/orderbydesc/where/top, iterator is $$", new ExpressionFactoryHelper<LinqExp>());
            Register("isnull", "isnull(obj) api", new ExpressionFactoryHelper<IsNullExp>());
            Register("null", "null() api", new ExpressionFactoryHelper<NullExp>());
            Register("equalsnull", "equalsnull(obj) api", new ExpressionFactoryHelper<EqualsNullExp>());
            Register("dotnetload", "dotnetload(dll_path) api", new ExpressionFactoryHelper<DotnetLoadExp>());
            Register("dotnetnew", "dotnetnew(assembly,type_name,arg1,arg2,...) api", new ExpressionFactoryHelper<DotnetNewExp>());
            Register("callstack", "callstack() api", new ExpressionFactoryHelper<CallStackExp>());
            Register("call", "call(func_name,arg1,arg2,...) api", new ExpressionFactoryHelper<CallExp>());
            Register("return", "return([val]) api", new ExpressionFactoryHelper<ReturnExp>());
            Register("redirect", "redirect(arg1,arg2,...) api", new ExpressionFactoryHelper<RedirectExp>());
            Register("await", "await(\"func_name\", arg1, arg2, ...) api, call async function", new ExpressionFactoryHelper<AwaitExp>());
            Register("awaitwhile", "awaitwhile(\"func_name\", arg1, arg2, ...) api, loop call sync function while result is true", new ExpressionFactoryHelper<AwaitWhileExp>());
            Register("awaituntil", "awaituntil(\"func_name\", arg1, arg2, ...) api, loop call sync function until result is true", new ExpressionFactoryHelper<AwaitUntilExp>());
            Register("awaittime", "awaittime(milliseconds) api, async wait specified time", new ExpressionFactoryHelper<AwaitTimeExp>());

            Register("substring", "substring(str[,start,len]) function", new ExpressionFactoryHelper<SubstringExp>());
            Register("newstringbuilder", "newstringbuilder() api", new ExpressionFactoryHelper<NewStringBuilderExp>());
            Register("appendformat", "appendformat(sb,fmt,arg1,arg2,...) api", new ExpressionFactoryHelper<AppendFormatExp>());
            Register("appendformatline", "appendformatline(sb,fmt,arg1,arg2,...) api", new ExpressionFactoryHelper<AppendFormatLineExp>());
            Register("stringbuilder_tostring", "stringbuilder_tostring(sb)", new ExpressionFactoryHelper<StringBuilderToStringExp>());
            Register("stringjoin", "stringjoin(sep,list) api", new ExpressionFactoryHelper<StringJoinExp>());
            Register("stringsplit", "stringsplit(str,sep_list) api", new ExpressionFactoryHelper<StringSplitExp>());
            Register("stringtrim", "stringtrim(str) api", new ExpressionFactoryHelper<StringTrimExp>());
            Register("stringtrimstart", "stringtrimstart(str) api", new ExpressionFactoryHelper<StringTrimStartExp>());
            Register("stringtrimend", "stringtrimend(str) api", new ExpressionFactoryHelper<StringTrimEndExp>());
            Register("stringtolower", "stringtolower(str) api", new ExpressionFactoryHelper<StringToLowerExp>());
            Register("stringtoupper", "stringtoupper(str) api", new ExpressionFactoryHelper<StringToUpperExp>());
            Register("stringreplace", "stringreplace(str,key,rep_str) api", new ExpressionFactoryHelper<StringReplaceExp>());
            Register("stringreplacechar", "stringreplacechar(str,key,char_as_str) api", new ExpressionFactoryHelper<StringReplaceCharExp>());
            Register("makestring", "makestring(char1_as_str_or_int,char2_as_str_or_int,...) api", new ExpressionFactoryHelper<MakeStringExp>());
            Register("stringcontains", "stringcontains(str,str_or_list_1,str_or_list_2,...) api", new ExpressionFactoryHelper<StringContainsExp>());
            Register("stringnotcontains", "stringnotcontains(str,str_or_list_1,str_or_list_2,...) api", new ExpressionFactoryHelper<StringNotContainsExp>());
            Register("stringcontainsany", "stringcontainsany(str,str_or_list_1,str_or_list_2,...) api", new ExpressionFactoryHelper<StringContainsAnyExp>());
            Register("stringnotcontainsany", "stringnotcontainsany(str,str_or_list_1,str_or_list_2,...) api", new ExpressionFactoryHelper<StringNotContainsAnyExp>());
            Register("str2int", "str2int(str) api", new ExpressionFactoryHelper<Str2IntExp>());
            Register("str2uint", "str2uint(str) api", new ExpressionFactoryHelper<Str2UintExp>());
            Register("str2long", "str2long(str) api", new ExpressionFactoryHelper<Str2LongExp>());
            Register("str2ulong", "str2ulong(str) api", new ExpressionFactoryHelper<Str2UlongExp>());
            Register("str2float", "str2float(str) api", new ExpressionFactoryHelper<Str2FloatExp>());
            Register("str2double", "str2double(str) api", new ExpressionFactoryHelper<Str2DoubleExp>());
            Register("hex2int", "hex2int(str) api", new ExpressionFactoryHelper<Hex2IntExp>());
            Register("hex2uint", "hex2uint(str) api", new ExpressionFactoryHelper<Hex2UintExp>());
            Register("hex2long", "hex2long(str) api", new ExpressionFactoryHelper<Hex2LongExp>());
            Register("hex2ulong", "hex2ulong(str) api", new ExpressionFactoryHelper<Hex2UlongExp>());
            Register("datetimestr", "datetimestr([fmt]) api", new ExpressionFactoryHelper<DatetimeStrExp>());
            Register("longdatestr", "longdatestr() api", new ExpressionFactoryHelper<LongDateStrExp>());
            Register("longtimestr", "longtimestr() api", new ExpressionFactoryHelper<LongTimeStrExp>());
            Register("shortdatestr", "shortdatestr() api", new ExpressionFactoryHelper<ShortDateStrExp>());
            Register("shorttimestr", "shorttimestr() api", new ExpressionFactoryHelper<ShortTimeStrExp>());
            Register("isnullorempty", "isnullorempty(str) api", new ExpressionFactoryHelper<IsNullOrEmptyExp>());
            Register("tuple", "(v1,v2,...) or tuple(v1,v2,...) object", new ExpressionFactoryHelper<TupleExp>());
            Register("tupleset", "(var1,var2,...) = (v1,v2,...) or tupleset((var1,var2,...), (v1,v2,...))", new ExpressionFactoryHelper<TupleSetExp>());
            Register("array", "[v1,v2,...] or array(v1,v2,...) object", new ExpressionFactoryHelper<ArrayExp>());
            Register("toarray", "toarray(list) api", new ExpressionFactoryHelper<ToArrayExp>());
            Register("listsize", "listsize(list) api", new ExpressionFactoryHelper<ListSizeExp>());
            Register("list", "list(v1,v2,...) object", new ExpressionFactoryHelper<ListExp>());
            Register("listget", "listget(list,index[,defval]) api", new ExpressionFactoryHelper<ListGetExp>());
            Register("listset", "listset(list,index,val) api", new ExpressionFactoryHelper<ListSetExp>());
            Register("listindexof", "listindexof(list,val) api", new ExpressionFactoryHelper<ListIndexOfExp>());
            Register("listadd", "listadd(list,val) api", new ExpressionFactoryHelper<ListAddExp>());
            Register("listremove", "listremove(list,val) api", new ExpressionFactoryHelper<ListRemoveExp>());
            Register("listinsert", "listinsert(list,index,val) api", new ExpressionFactoryHelper<ListInsertExp>());
            Register("listremoveat", "listremoveat(list,index) api", new ExpressionFactoryHelper<ListRemoveAtExp>());
            Register("listclear", "listclear(list) api", new ExpressionFactoryHelper<ListClearExp>());
            Register("listsplit", "listsplit(list,ct) api, return list of list", new ExpressionFactoryHelper<ListSplitExp>());
            Register("hashtablesize", "hashtablesize(hash) api", new ExpressionFactoryHelper<HashtableSizeExp>());
            Register("hashtable", "{k1=>v1,k2=>v2,...} or {k1:v1,k2:v2,...} or hashtable(k1=>v1,k2=>v2,...) or hashtable(k1:v1,k2:v2,...) object", new ExpressionFactoryHelper<HashtableExp>());
            Register("hashtableget", "hashtableget(hash,key[,defval]) api", new ExpressionFactoryHelper<HashtableGetExp>());
            Register("hashtableset", "hashtableset(hash,key,val) api", new ExpressionFactoryHelper<HashtableSetExp>());
            Register("hashtableadd", "hashtableadd(hash,key,val) api", new ExpressionFactoryHelper<HashtableAddExp>());
            Register("hashtableremove", "hashtableremove(hash,key) api", new ExpressionFactoryHelper<HashtableRemoveExp>());
            Register("hashtableclear", "hashtableclear(hash) api", new ExpressionFactoryHelper<HashtableClearExp>());
            Register("hashtablekeys", "hashtablekeys(hash) api", new ExpressionFactoryHelper<HashtableKeysExp>());
            Register("hashtablevalues", "hashtablevalues(hash) api", new ExpressionFactoryHelper<HashtableValuesExp>());
            Register("listhashtable", "listhashtable(hash) api, return list of pair", new ExpressionFactoryHelper<ListHashtableExp>());
            Register("hashtablesplit", "hashtablesplit(hash,ct) api, return list of hashtable", new ExpressionFactoryHelper<HashtableSplitExp>());
            Register("peek", "peek(queue_or_stack) api", new ExpressionFactoryHelper<PeekExp>());
            Register("stacksize", "stacksize(stack) api", new ExpressionFactoryHelper<StackSizeExp>());
            Register("stack", "stack(v1,v2,...) object", new ExpressionFactoryHelper<StackExp>());
            Register("push", "push(stack,v) api", new ExpressionFactoryHelper<PushExp>());
            Register("pop", "pop(stack) api", new ExpressionFactoryHelper<PopExp>());
            Register("stackclear", "stackclear(stack) api", new ExpressionFactoryHelper<StackClearExp>());
            Register("queuesize", "queuesize(queue) api", new ExpressionFactoryHelper<QueueSizeExp>());
            Register("queue", "queue(v1,v2,...) object", new ExpressionFactoryHelper<QueueExp>());
            Register("enqueue", "enqueue(queue,v) api", new ExpressionFactoryHelper<EnqueueExp>());
            Register("dequeue", "dequeue(queue) api", new ExpressionFactoryHelper<DequeueExp>());
            Register("queueclear", "queueclear(queue) api", new ExpressionFactoryHelper<QueueClearExp>());
            Register("setenv", "setenv(k,v) api", new ExpressionFactoryHelper<SetEnvironmentExp>());
            Register("getenv", "getenv(k) api", new ExpressionFactoryHelper<GetEnvironmentExp>());
            Register("expand", "expand(str) api", new ExpressionFactoryHelper<ExpandEnvironmentsExp>());
            Register("envs", "envs() api", new ExpressionFactoryHelper<EnvironmentsExp>());
            Register("cd", "cd(path) api", new ExpressionFactoryHelper<SetCurrentDirectoryExp>());
            Register("pwd", "pwd() api", new ExpressionFactoryHelper<GetCurrentDirectoryExp>());
            Register("cmdline", "cmdline() api", new ExpressionFactoryHelper<CommandLineExp>());
            Register("cmdlineargs", "cmdlineargs(prev_arg) or cmdlineargs() api, first return next arg, second return array of arg", new ExpressionFactoryHelper<CommandLineArgsExp>());
            Register("os", "os() api", new ExpressionFactoryHelper<OsExp>());
            Register("osplatform", "osplatform() api", new ExpressionFactoryHelper<OsPlatformExp>());
            Register("osversion", "osversion() api", new ExpressionFactoryHelper<OsVersionExp>());
            Register("getfullpath", "getfullpath(path) api", new ExpressionFactoryHelper<GetFullPathExp>());
            Register("getpathroot", "getpathroot(path) api", new ExpressionFactoryHelper<GetPathRootExp>());
            Register("getrandomfilename", "getrandomfilename() api", new ExpressionFactoryHelper<GetRandomFileNameExp>());
            Register("gettempfilename", "gettempfilename() api", new ExpressionFactoryHelper<GetTempFileNameExp>());
            Register("gettemppath", "gettemppath() api", new ExpressionFactoryHelper<GetTempPathExp>());
            Register("hasextension", "hasextension(path) api", new ExpressionFactoryHelper<HasExtensionExp>());
            Register("ispathrooted", "ispathrooted(path) api", new ExpressionFactoryHelper<IsPathRootedExp>());
            Register("getfilename", "getfilename(path) api", new ExpressionFactoryHelper<GetFileNameExp>());
            Register("getfilenamewithoutextension", "getfilenamewithoutextension(path) api", new ExpressionFactoryHelper<GetFileNameWithoutExtensionExp>());
            Register("getextension", "getextension(path) api", new ExpressionFactoryHelper<GetExtensionExp>());
            Register("getdirectoryname", "getdirectoryname(path) api", new ExpressionFactoryHelper<GetDirectoryNameExp>());
            Register("combinepath", "combinepath(path1,path2,...) api", new ExpressionFactoryHelper<CombinePathExp>());
            Register("changeextension", "changeextension(path,ext) api", new ExpressionFactoryHelper<ChangeExtensionExp>());
            Register("quotepath", "quotepath(path[,only_needed,single_quote]) api", new ExpressionFactoryHelper<QuotePathExp>());
            Register("echo", "echo(fmt,arg1,arg2,...) api, Console.WriteLine", new ExpressionFactoryHelper<EchoExp>());
            Register("fileecho", "fileecho(bool) or fileecho() api", new ExpressionFactoryHelper<FileEchoExp>());
            Register("direxist", "direxist(dir) api", new ExpressionFactoryHelper<DirectoryExistExp>());
            Register("fileexist", "fileexist(file) api", new ExpressionFactoryHelper<FileExistExp>());
            Register("listdirs", "listdirs(dir,filter_list_or_str_1,filter_list_or_str_2,...) api", new ExpressionFactoryHelper<ListDirectoriesExp>());
            Register("listfiles", "listfiles(dir,filter_list_or_str_1,filter_list_or_str_2,...) api", new ExpressionFactoryHelper<ListFilesExp>());
            Register("listalldirs", "listalldirs(dir,filter_list_or_str_1,filter_list_or_str_2,...) api", new ExpressionFactoryHelper<ListAllDirectoriesExp>());
            Register("listallfiles", "listallfiles(dir,filter_list_or_str_1,filter_list_or_str_2,...) api", new ExpressionFactoryHelper<ListAllFilesExp>());
            Register("createdir", "createdir(dir) api", new ExpressionFactoryHelper<CreateDirectoryExp>());
            Register("copydir", "copydir(dir1,dir2,filter_list_or_str_1,filter_list_or_str_2,...) api, include subdir", new ExpressionFactoryHelper<CopyDirectoryExp>());
            Register("movedir", "movedir(dir1,dir2) api", new ExpressionFactoryHelper<MoveDirectoryExp>());
            Register("deletedir", "deletedir(dir) api", new ExpressionFactoryHelper<DeleteDirectoryExp>());
            Register("copyfile", "copyfile(file1,file2) api", new ExpressionFactoryHelper<CopyFileExp>());
            Register("copyfiles", "copyfiles(dir1,dir2,filter_list_or_str_1,filter_list_or_str_2,...) api, dont include subdir", new ExpressionFactoryHelper<CopyFilesExp>());
            Register("movefile", "movefile(file1,file2) api", new ExpressionFactoryHelper<MoveFileExp>());
            Register("deletefile", "deletefile(file) api", new ExpressionFactoryHelper<DeleteFileExp>());
            Register("deletefiles", "deletefiles(dir,filter_list_or_str_1,filter_list_or_str_2,...) api, dont include subdir", new ExpressionFactoryHelper<DeleteFilesExp>());
            Register("deleteallfiles", "deleteallfiles(dir,filter_list_or_str_1,filter_list_or_str_2,...) api, include subdir", new ExpressionFactoryHelper<DeleteAllFilesExp>());
            Register("getfileinfo", "getfileinfo(file) api", new ExpressionFactoryHelper<GetFileInfoExp>());
            Register("getdirinfo", "getdirinfo(dir) api", new ExpressionFactoryHelper<GetDirectoryInfoExp>());
            Register("getdriveinfo", "getdriveinfo(drive) api", new ExpressionFactoryHelper<GetDriveInfoExp>());
            Register("getdrivesinfo", "getdrivesinfo() api", new ExpressionFactoryHelper<GetDrivesInfoExp>());
            Register("readalllines", "readalllines(file[,encoding]) api", new ExpressionFactoryHelper<ReadAllLinesExp>());
            Register("writealllines", "writealllines(file,lines[,encoding]) api", new ExpressionFactoryHelper<WriteAllLinesExp>());
            Register("readalltext", "readalltext(file[,encoding]) api", new ExpressionFactoryHelper<ReadAllTextExp>());
            Register("writealltext", "writealltext(file,txt[,encoding]) api", new ExpressionFactoryHelper<WriteAllTextExp>());
            Register("process", "process(file,arg_str) or process(file,arg_str){[options;]} api", new ExpressionFactoryHelper<CommandExp>());
            Register("command", "command{win{:cmd_str:};unix{:cmd_str:};common{:cmd_str:};[options;]} api", new ExpressionFactoryHelper<CommandExp>());
            Register("kill", "kill(name_or_pid) api", new ExpressionFactoryHelper<KillExp>());
            Register("killme", "killme([exit_code]) api", new ExpressionFactoryHelper<KillMeExp>());
            Register("pid", "pid() api", new ExpressionFactoryHelper<GetCurrentProcessIdExp>());
            Register("plist", "plist([filter]) api, return list", new ExpressionFactoryHelper<ListProcessesExp>());
            Register("sleep", "sleep(milliseconds) api", new ExpressionFactoryHelper<SleepExp>());
            Register("waitall", "waitall([timeout]) api, wait all task to exit", new ExpressionFactoryHelper<WaitAllExp>());
            Register("waitstartinterval", "waitstartinterval(time) or waitstartinterval() api, used in Task.Wait for process/command", new ExpressionFactoryHelper<WaitStartIntervalExp>());
            Register("cleanupcompletedtasks", "cleanupcompletedtasks() api", new ExpressionFactoryHelper<CleanupCompletedTasksExp>());
            Register("gettaskcount", "gettaskcount() api", new ExpressionFactoryHelper<GetTaskCountExp>());
            Register("calcmd5", "calcmd5(file) api", new ExpressionFactoryHelper<CalcMd5Exp>());
        }

        private Dictionary<string, IExpressionFactory> m_ApiFactories = new Dictionary<string, IExpressionFactory>();
        private SortedList<string, string> m_ApiDocs = new SortedList<string, string>();
    }
}
#pragma warning restore 8600,8601,8602,8603,8604,8618,8619,8620,8625
#endregion
