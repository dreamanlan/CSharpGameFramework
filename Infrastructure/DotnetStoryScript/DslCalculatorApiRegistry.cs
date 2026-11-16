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
using System.Data;

#region interpreter
#pragma warning disable 8600,8601,8602,8603,8604,8618,8619,8620,8625
namespace DotnetStoryScript.DslExpression
{
    internal sealed class SubstringExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
                    if (start >= str.Length) {
                        start = str.Length - 1;
                    }
                    if (start < 0) {
                        start = 0;
                    }
                    if (len < 0) {
                        len = 0;
                    }
                    else if (start + len > str.Length) {
                        len = str.Length - start;
                    }
                    r = str.Substring(start, len);
                }
            }
            return r;
        }
    }
    internal sealed class NewStringBuilderExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: new_string_builder() api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 0) {
                r = BoxedValue.FromObject(new StringBuilder());
            }
            return r;
        }
    }
    internal sealed class AppendFormatExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: append_format(sb,fmt,arg1,arg2,...) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: append_format_line(sb,fmt,arg1,arg2,...) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: string_join(sep,list) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var sep = operands[0].AsString;
                var list = operands[1].As<IList>();
                if (null != sep && null != list) {
                    string[] strs = new string[list.Count];
                    for (int i = 0; i < list.Count; i++) {
                        var elem = list[i];
                        if (null != elem) {
                            var s = elem.ToString();
                            if (null == s) {
                                strs[i] = string.Empty;
                            }
                            else {
                                strs[i] = s;
                            }
                        }
                    }
                    r = string.Join(sep, strs);
                }
            }
            return r;
        }
    }
    internal sealed class StringSplitExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: string_split(str,sep_list) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                string str = operands[0].AsString;
                string sepStr = operands[1].AsString;
                if (!string.IsNullOrEmpty(sepStr)) {
                    r = BoxedValue.FromObject(str.Split(new string[] { sepStr }, StringSplitOptions.RemoveEmptyEntries));
                }
                else {
                    IList seps = operands[1].As<IList>();
                    if (str != null && seps != null) {
                        bool isChar = true;
                        List<string> strs = new List<string>();
                        List<char> cs = new List<char>();
                        for (int i = 0; i < seps.Count; i++) {
                            var _sep = seps[i];
                            if (null != _sep) {
                                string sep = _sep.ToString();
                                if (!string.IsNullOrEmpty(sep)) {
                                    strs.Add(sep);
                                    if (sep.Length == 1) {
                                        cs.Add(sep[0]);
                                    }
                                    else {
                                        isChar = false;
                                    }
                                }
                            }
                        }
                        if (isChar) {
                            r = BoxedValue.FromObject(str.Split(cs.ToArray(), StringSplitOptions.RemoveEmptyEntries));
                        }
                        else {
                            r = BoxedValue.FromObject(str.Split(strs.ToArray(), StringSplitOptions.RemoveEmptyEntries));
                        }
                    }
                }
            }
            return r;
        }
    }
    internal sealed class StringTrimExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: string_trim(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (str != null) {
                    r = str.Trim();
                }
            }
            return r;
        }
    }
    internal sealed class StringTrimStartExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: string_trim_start(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (str != null) {
                    r = str.TrimStart();
                }
            }
            return r;
        }
    }
    internal sealed class StringTrimEndExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: string_trim_end(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (str != null) {
                    r = str.TrimEnd();
                }
            }
            return r;
        }
    }
    internal sealed class StringToLowerExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: string_to_lower(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (str != null) {
                    r = str.ToLower();
                }
            }
            return r;
        }
    }
    internal sealed class StringToUpperExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: string_to_upper(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (str != null) {
                    r = str.ToUpper();
                }
            }
            return r;
        }
    }
    internal sealed class StringReplaceExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 3)
                throw new Exception("Expected: string_replace(str,key,rep_str) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 3)
                throw new Exception("Expected: string_replace_char(str,key,char_as_str) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: string_contains(str,str_or_list_1,str_or_list_2,...) api");
            bool r = false;
            if (operands.Count >= 2) {
                string str = operands[0].AsString;
                if (null != str) {
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
            }
            return r;
        }
    }
    internal sealed class StringNotContainsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: string_not_contains(str,str_or_list_1,str_or_list_2,...) api");
            bool r = false;
            if (operands.Count >= 2) {
                string str = operands[0].AsString;
                if (null != str) {
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
            }
            return r;
        }
    }
    internal sealed class StringContainsAnyExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: string_contains_any(str,str_or_list_1,str_or_list_2,...) api");
            bool r = false;
            if (operands.Count >= 2) {
                string str = operands[0].AsString;
                if (null != str) {
                    r = true;
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
            }
            return r;
        }
    }
    internal sealed class StringNotContainsAnyExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: string_not_contains_any(str,str_or_list_1,str_or_list_2,...) api");
            bool r = false;
            if (operands.Count >= 2) {
                string str = operands[0].AsString;
                if (null != str) {
                    r = true;
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
            }
            return r;
        }
    }
    internal sealed class Str2IntExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: str_2_int(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (null != str) {
                    int v;
                    if (int.TryParse(str, System.Globalization.NumberStyles.Number, null, out v)) {
                        r = v;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class Str2UintExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: str_2_uint(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (null != str) {
                    uint v;
                    if (uint.TryParse(str, System.Globalization.NumberStyles.Number, null, out v)) {
                        r = v;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class Str2LongExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: str_2_long(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (null != str) {
                    long v;
                    if (long.TryParse(str, System.Globalization.NumberStyles.Number, null, out v)) {
                        r = v;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class Str2UlongExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: str_2_ulong(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (null != str) {
                    ulong v;
                    if (ulong.TryParse(str, System.Globalization.NumberStyles.Number, null, out v)) {
                        r = v;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class Str2FloatExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: str_2_float(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (null != str) {
                    float v;
                    if (float.TryParse(str, System.Globalization.NumberStyles.Float, null, out v)) {
                        r = v;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class Str2DoubleExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: str_2_double(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (null != str) {
                    double v;
                    if (double.TryParse(str, System.Globalization.NumberStyles.Float, null, out v)) {
                        r = v;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class Hex2IntExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hex_2_int(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (null != str) {
                    int v;
                    if (int.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out v)) {
                        r = v;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class Hex2UintExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hex_2_uint(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (null != str) {
                    uint v;
                    if (uint.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out v)) {
                        r = v;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class Hex2LongExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hex_2_long(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (null != str) {
                    long v;
                    if (long.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out v)) {
                        r = v;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class Hex2UlongExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hex_2_ulong(str) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var str = operands[0].AsString;
                if (null != str) {
                    ulong v;
                    if (ulong.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out v)) {
                        r = v;
                    }
                }
            }
            return r;
        }
    }
    internal sealed class DatetimeStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count > 1)
                throw new Exception("Expected: datetime_str([fmt]) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var fmt = operands[0].AsString;
                if (null != fmt) {
                    r = DateTime.Now.ToString(fmt);
                }
            }
            else {
                r = DateTime.Now.ToString();
            }
            return r;
        }
    }
    internal sealed class LongDateStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: long_date_str() api");
            var r = BoxedValue.FromObject(DateTime.Now.ToLongDateString());
            return r;
        }
    }
    internal sealed class LongTimeStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: long_time_str() api");
            var r = BoxedValue.FromObject(DateTime.Now.ToShortDateString());
            return r;
        }
    }
    internal sealed class ShortDateStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: short_date_str() api");
            var r = BoxedValue.FromObject(DateTime.Now.ToShortDateString());
            return r;
        }
    }
    internal sealed class ShortTimeStrExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: short_time_str() api");
            var r = BoxedValue.FromObject(DateTime.Now.ToShortTimeString());
            return r;
        }
    }
    internal sealed class NowExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: now() api");
            var r = BoxedValue.FromDateTime(DateTime.Now);
            return r;
        }
    }
    internal sealed class ParseDateTimeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: parse_date_time(str) api");
            DateTime.TryParse(operands[0].ToString(), out var dt);
            var r = BoxedValue.FromDateTime(dt);
            return r;
        }
    }
    internal sealed class FormatDateTimeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: format_date_time(str,date_time) api");
            string fmt = operands[0].ToString();
            DateTime dt = operands[1].GetDateTime();
            var r = BoxedValue.FromString(dt.ToString(fmt));
            return r;
        }
    }
    internal sealed class IsNullOrEmptyExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: is_null_or_empty(str) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: to_array(list) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var list = operands[0];
                IEnumerable obj = list.As<IEnumerable>();
                if (null != obj) {
                    List<BoxedValue> al = new List<BoxedValue>();
                    IEnumerator enumer = obj.GetEnumerator();
                    try {
                        while (enumer.MoveNext()) {
                            var val = BoxedValue.FromObject(enumer.Current);
                            al.Add(val);
                        }
                    }
                    finally {
                        (enumer as System.IDisposable)?.Dispose();
                    }
                    r = BoxedValue.FromObject(al.ToArray());
                }
            }
            return r;
        }
    }
    internal sealed class ListSizeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: list_size(list) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2 || operands.Count > 3)
                throw new Exception("Expected: list_get(list,index[,defval]) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 3)
                throw new Exception("Expected: list_set(list,index,val) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: list_index_of(list,val) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: list_add(list,val) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: list_remove(list,val) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 3)
                throw new Exception("Expected: list_insert(list,index,val) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: list_remove_at(list,index) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: list_clear(list) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: list_split(list,ct) api, return list of list");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var enumer = operands[0].As<IEnumerable>();
                var ct = operands[1].GetInt();
                if (null != enumer && enumer is List<BoxedValue> bvList) {
                    var e = bvList.GetEnumerator();
                    try {
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
                    finally {
                        e.Dispose();
                    }
                }
                else if (null != enumer) {
                    var e = enumer.GetEnumerator();
                    if (null != e) {
                        try {
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
                        finally {
                            (e as System.IDisposable)?.Dispose();
                        }
                    }
                }
            }
            return r;
        }
    }
    internal sealed class HashtableSizeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hashtable_size(hash) api");
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
        protected override BoxedValue DoCalc(DslCalculator calculator)
        {
            BoxedValue r = BoxedValue.NullObject;
            var dict = new Dictionary<BoxedValue, BoxedValue>();
            for (int i = 0; i < m_Expressions.Count - 1; i += 2) {
                var key = m_Expressions[i].Calc(calculator);
                var val = m_Expressions[i + 1].Calc(calculator);
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
                    var expKey = CodeStructure.Load(p0);
                    m_Expressions.Add(expKey);
                    var expVal = CodeStructure.Load(p1);
                    m_Expressions.Add(expVal);
                }
            }
            return true;
        }

        private List<IExpression> m_Expressions = new List<IExpression>();
    }
    internal sealed class HashtableGetExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2 || operands.Count > 3)
                throw new Exception("Expected: hashtable_get(hash,key[,defval]) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 3)
                throw new Exception("Expected: hashtable_set(hash,key,val) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 3)
                throw new Exception("Expected: hashtable_add(hash,key,val) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: hashtable_remove(hash,key) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hashtable_clear(hash) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hashtable_keys(hash) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: hashtable_values(hash) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: list_hashtable(hash) api, return list of pair");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: hashtable_split(hash,ct) api, return list of hashtable");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 2) {
                var dict = operands[0].As<IDictionary>();
                var ct = operands[1].GetInt();
                if (null != dict && dict is Dictionary<BoxedValue, BoxedValue> bvDict) {
                    var e = bvDict.GetEnumerator();
                    try {
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
                    finally {
                        e.Dispose();
                    }
                }
                else if (null != dict) {
                    var e = dict.GetEnumerator();
                    if (null != e) {
                        try {
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
                        finally {
                            (e as System.IDisposable)?.Dispose();
                        }
                    }
                }
            }
            return r;
        }
    }
    //The stack and queue share the same peek function.
    internal sealed class PeekExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: peek(queue_or_stack) api");
            BoxedValue r = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var stack = operands[0].As<Stack<BoxedValue>>();
                var queue = operands[0].As<Queue<BoxedValue>>();
                if (null != stack && stack.Count > 0) {
                    r = stack.Peek();
                }
                else if (null != queue && queue.Count > 0) {
                    r = queue.Peek();
                }
            }
            return r;
        }
    }
    internal sealed class StackSizeExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: stack_size(stack) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: stack_clear(stack) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: queue_size(queue) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: queue_clear(queue) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: set_env(k,v) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: get_env(k) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: envs() api");
            return BoxedValue.FromObject(Environment.GetEnvironmentVariables());
        }
    }
    internal sealed class SetCurrentDirectoryExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: pwd() api");
            return Environment.CurrentDirectory;
        }
    }
    internal sealed class CommandLineExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: cmd_line() api");
            return Environment.CommandLine;
        }
    }
    internal sealed class CommandLineArgsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count > 1)
                throw new Exception("Expected: cmd_line_args(prev_arg) or cmd_line_args() api, first return next arg, second return array of arg");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: os() api");
            return Environment.OSVersion.VersionString;
        }
    }
    internal sealed class OsPlatformExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: os_platform() api");
            return Environment.OSVersion.Platform.ToString();
        }
    }
    internal sealed class OsVersionExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: os_version() api");
            return Environment.OSVersion.Version.ToString();
        }
    }
    internal sealed class GetFullPathExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: get_full_path(path) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: get_path_root(path) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: get_random_file_name() api");
            return Path.GetRandomFileName();
        }
    }
    internal sealed class GetTempFileNameExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: get_temp_file_name() api");
            return Path.GetTempFileName();
        }
    }
    internal sealed class GetTempPathExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: get_temp_path() api");
            return Path.GetTempPath();
        }
    }
    internal sealed class HasExtensionExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: has_extension(path) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: is_path_rooted(path) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: get_file_name(path) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: get_file_name_without_extension(path) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: get_extension(path) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: get_directory_name(path) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: combine_path(path1,path2,...) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: change_extension(path,ext) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1 || operands.Count > 3)
                throw new Exception("Expected: quote_path(path[,only_needed,single_quote]) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count > 1)
                throw new Exception("Expected: file_echo(bool) or file_echo() api");
            if (operands.Count >= 1) {
                DslCalculator.FileEchoOn = operands[0].GetBool();
            }
            return DslCalculator.FileEchoOn;
        }
    }
    internal sealed class DirectoryExistsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: dir_exists(dir) api");
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var dir = operands[0].AsString;
                dir = Environment.ExpandEnvironmentVariables(dir);
                ret = Directory.Exists(dir);
            }
            return ret;
        }
    }
    internal sealed class FileExistsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: file_exists(file) api");
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var file = operands[0].AsString;
                file = Environment.ExpandEnvironmentVariables(file);
                ret = File.Exists(file);
            }
            return ret;
        }
    }
    internal sealed class PathExistsExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: path_exists(dir) api");
            var ret = BoxedValue.NullObject;
            if (operands.Count >= 1) {
                var path = operands[0].AsString;
                path = Environment.ExpandEnvironmentVariables(path);
                ret = Directory.Exists(path) || File.Exists(path);
            }
            return ret;
        }
    }
    internal sealed class ListDirectoriesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: list_dirs(dir,filter_list_or_str_1,filter_list_or_str_2,...) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: list_files(dir,filter_list_or_str_1,filter_list_or_str_2,...) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: list_all_dirs(dir,filter_list_or_str_1,filter_list_or_str_2,...) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: list_all_files(dir,filter_list_or_str_1,filter_list_or_str_2,...) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: create_dir(dir) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: copy_dir(dir1,dir2,filter_list_or_str_1,filter_list_or_str_2,...) api, include subdir");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: move_dir(dir1,dir2) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: delete_dir(dir) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: copy_file(file1,file2) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: copy_files(dir1,dir2,filter_list_or_str_1,filter_list_or_str_2,...) api, dont include subdir");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 2)
                throw new Exception("Expected: move_file(file1,file2) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: delete_file(file) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: delete_files(dir,filter_list_or_str_1,filter_list_or_str_2,...) api, dont include subdir");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: delete_all_files(dir,filter_list_or_str_1,filter_list_or_str_2,...) api, include subdir");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: get_file_info(file) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: get_dir_info(dir) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: get_drive_info(drive) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: get_drives_info() api");
            var ret = DriveInfo.GetDrives();
            return BoxedValue.FromObject(ret);
        }
    }
    internal sealed class ReadAllLinesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1 || operands.Count > 2)
                throw new Exception("Expected: read_all_lines(file[,encoding]) api");
            if (operands.Count >= 1) {
                string path = operands[0].AsString;
                if (!string.IsNullOrEmpty(path)) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    Encoding encoding = Encoding.UTF8;
                    if (operands.Count >= 2) {
                        var v = operands[1];
                        encoding = GetEncoding(v, path);
                    }
                    return BoxedValue.FromObject(SharedFileReader.ReadAllLines(path, encoding));
                }
            }
            return BoxedValue.FromObject(new string[0]);
        }
    }
    internal sealed class WriteAllLinesExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2 || operands.Count > 3)
                throw new Exception("Expected: write_all_lines(file,lines[,encoding]) api");
            if (operands.Count >= 2) {
                string path = operands[0].AsString;
                var lines = operands[1].As<IList>();
                if (!string.IsNullOrEmpty(path) && null != lines) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    Encoding encoding = Encoding.UTF8;
                    if (operands.Count >= 3) {
                        var v = operands[2];
                        encoding = GetEncodingForWrite(v, path);
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 1 || operands.Count > 2)
                throw new Exception("Expected: read_all_text(file[,encoding]) api");
            if (operands.Count >= 1) {
                string path = operands[0].AsString;
                if (!string.IsNullOrEmpty(path)) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    Encoding encoding = Encoding.UTF8;
                    if (operands.Count >= 2) {
                        var v = operands[1];
                        encoding = GetEncoding(v, path);
                    }
                    return SharedFileReader.ReadAllText(path, encoding);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class WriteAllTextExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count < 2 || operands.Count > 3)
                throw new Exception("Expected: write_all_text(file,txt[,encoding]) api");
            if (operands.Count >= 2) {
                string path = operands[0].AsString;
                var text = operands[1].AsString;
                if (!string.IsNullOrEmpty(path) && null != text) {
                    path = Environment.ExpandEnvironmentVariables(path);
                    Encoding encoding = Encoding.UTF8;
                    if (operands.Count >= 3) {
                        var v = operands[2];
                        encoding = GetEncodingForWrite(v, path);
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
        protected override BoxedValue DoCalc(DslCalculator calculator)
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
                        exitCode = ExecCommand(calculator, cfg, ims, oms);
                    }
                    else {
                        exitCode = ExecProcess(calculator, cfg, ims, oms);
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
                            var exp = CodeStructure.Load(cd.GetParam(0));
                            var opt = CodeStructure.Load(cd.GetParam(1));
                            if (type == "output") {
                                cmd.m_Output = exp;
                                cmd.m_OutputOptArg = opt;
                            }
                            else if (type == "error") {
                                cmd.m_Error = exp;
                                cmd.m_ErrorOptArg = opt;
                            }
                            else {
                                CodeStructure.Log("[syntax error] {0} line:{1}", cd.ToScriptString(false, Dsl.DelimiterInfo.Default), cd.GetLine());
                            }
                        }
                        else if (num >= 1) {
                            string type = cd.GetId();
                            var exp = CodeStructure.Load(cd.GetParam(0));
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
                            else if (type == "timeout") {
                                cmd.m_Timeout = exp;
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
                                CodeStructure.Log("[syntax error] {0} line:{1}", cd.ToScriptString(false, Dsl.DelimiterInfo.Default), cd.GetLine());
                            }
                        }
                        else {
                            CodeStructure.Log("[syntax error] {0} line:{1}", cd.ToScriptString(false, Dsl.DelimiterInfo.Default), cd.GetLine());
                        }
                    }
                    else {
                        CodeStructure.Log("[syntax error] {0} line:{1}", comp.ToScriptString(false, Dsl.DelimiterInfo.Default), comp.GetLine());
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
                    var exp0 = CodeStructure.Load(param0);
                    cmd.m_FileName = exp0;

                    if (num > 1) {
                        var param1 = callData.GetParam(1);
                        var exp1 = CodeStructure.Load(param1);
                        cmd.m_Argments = exp1;
                    }
                }
                else {
                    CodeStructure.Log("[syntax error] {0} line:{1}", callData.ToScriptString(false, Dsl.DelimiterInfo.Default), callData.GetLine());
                }
            }
            else if (id == "command") {
                int num = callData.GetParamNum();
                if (num > 0) {
                    CodeStructure.Log("[syntax error] {0} line:{1}", callData.ToScriptString(false, Dsl.DelimiterInfo.Default), callData.GetLine());
                }
            }
            else {
                CodeStructure.Log("[syntax error] {0} line:{1}", callData.ToScriptString(false, Dsl.DelimiterInfo.Default), callData.GetLine());
            }
            return true;
        }
        private int ExecProcess(DslCalculator calculator, CommandConfig cfg, Stream istream, Stream ostream)
        {
            string fileName = string.Empty;
            if (null != cfg.m_FileName) {
                fileName = cfg.m_FileName.Calc(calculator).AsString;
            }
            string args = string.Empty;
            if (null != cfg.m_Argments) {
                args = cfg.m_Argments.Calc(calculator).AsString;
            }
            bool noWait = false;
            if (null != cfg.m_NoWait) {
                noWait = cfg.m_NoWait.Calc(calculator).GetBool();
            }
            DslCalculator.ProcessStartOption option = new DslCalculator.ProcessStartOption();
            if (null != cfg.m_Timeout) {
                option.TimeoutMs = cfg.m_Timeout.Calc(calculator).GetInt();
            }
            if (null != cfg.m_UseShellExecute) {
                option.UseShellExecute = cfg.m_UseShellExecute.Calc(calculator).GetBool();
            }
            if (null != cfg.m_Verb) {
                option.Verb = cfg.m_Verb.Calc(calculator).AsString;
            }
            if (null != cfg.m_Domain) {
                option.Domain = cfg.m_Domain.Calc(calculator).AsString;
            }
            if (null != cfg.m_UserName) {
                option.UserName = cfg.m_UserName.Calc(calculator).AsString;
            }
            if (null != cfg.m_Password) {
                option.Password = cfg.m_Password.Calc(calculator).AsString;
            }
            if (null != cfg.m_PasswordInClearText) {
                option.PasswordInClearText = cfg.m_PasswordInClearText.Calc(calculator).AsString;
            }
            if (null != cfg.m_LoadUserProfile) {
                option.LoadUserProfile = cfg.m_LoadUserProfile.Calc(calculator).GetBool();
            }
            if (null != cfg.m_WindowStyle) {
                var str = cfg.m_WindowStyle.Calc(calculator).AsString;
                System.Diagnostics.ProcessWindowStyle style;
                if (Enum.TryParse(str, out style)) {
                    option.WindowStyle = style;
                }
            }
            if (null != cfg.m_CreateWindow) {
                option.CreateWindow = cfg.m_CreateWindow.Calc(calculator).GetBool();
            }
            if (null != cfg.m_ErrorDialog) {
                option.ErrorDialog = cfg.m_ErrorDialog.Calc(calculator).GetBool();
            }
            if (null != cfg.m_WorkingDirectory) {
                option.WorkingDirectory = cfg.m_WorkingDirectory.Calc(calculator).AsString;
            }
            Encoding encoding = null;
            if (null != cfg.m_Encoding) {
                var v = cfg.m_Encoding.Calc(calculator);
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
                var v = cfg.m_Input.Calc(calculator);
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
                            input = SharedFileReader.ReadAllLines(str);
                        }
                    }
                }
                catch (Exception ex) {
                    CodeStructure.Log("input {0} failed:{1}", v, ex.Message);
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
                var v = cfg.m_Output.Calc(calculator);
                var str = v.AsString;
                if (!string.IsNullOrEmpty(str)) {
                    str = Environment.ExpandEnvironmentVariables(str);
                    output = str;
                }
                else {
                    output = v;
                }
                if (null != cfg.m_OutputOptArg)
                    outputIx = cfg.m_OutputOptArg.Calc(calculator).GetInt();
                outputBuilder = new StringBuilder();
            }
            if (null != cfg.m_Error) {
                var v = cfg.m_Error.Calc(calculator);
                var str = v.AsString;
                if (!string.IsNullOrEmpty(str)) {
                    str = Environment.ExpandEnvironmentVariables(str);
                    error = str;
                }
                else {
                    error = v;
                }
                if (null != cfg.m_ErrorOptArg)
                    errorIx = cfg.m_ErrorOptArg.Calc(calculator).GetInt();
                errorBuilder = new StringBuilder();
            }
            if (null != cfg.m_RedirectToConsole) {
                var v = cfg.m_RedirectToConsole.Calc(calculator);
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
                            calculator.SetVariable(file, outputBuilder.ToString());
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
                    CodeStructure.Log("output {0} failed:{1}", output, ex.Message);
                }
            }
            if (null != errorBuilder && !error.IsNullObject) {
                try {
                    var file = error.AsString;
                    if (!string.IsNullOrEmpty(file)) {
                        if (file[0] == '@' || file[0] == '$') {
                            calculator.SetVariable(file, errorBuilder.ToString());
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
                    CodeStructure.Log("error {0} failed:{1}", error, ex.Message);
                }
            }
            return exitCode;
        }
        private int ExecCommand(DslCalculator calculator, CommandConfig cfg, Stream istream, Stream ostream)
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
                    noWait = cfg.m_NoWait.Calc(calculator).GetBool();
                }
                DslCalculator.ProcessStartOption option = new DslCalculator.ProcessStartOption();
                if (null != cfg.m_Timeout) {
                    option.TimeoutMs = cfg.m_Timeout.Calc(calculator).GetInt();
                }
                if (null != cfg.m_UseShellExecute) {
                    option.UseShellExecute = cfg.m_UseShellExecute.Calc(calculator).GetBool();
                }
                if (null != cfg.m_Verb) {
                    option.Verb = cfg.m_Verb.Calc(calculator).AsString;
                }
                if (null != cfg.m_Domain) {
                    option.Domain = cfg.m_Domain.Calc(calculator).AsString;
                }
                if (null != cfg.m_UserName) {
                    option.UserName = cfg.m_UserName.Calc(calculator).AsString;
                }
                if (null != cfg.m_Password) {
                    option.Password = cfg.m_Password.Calc(calculator).AsString;
                }
                if (null != cfg.m_PasswordInClearText) {
                    option.PasswordInClearText = cfg.m_PasswordInClearText.Calc(calculator).AsString;
                }
                if (null != cfg.m_LoadUserProfile) {
                    option.LoadUserProfile = cfg.m_LoadUserProfile.Calc(calculator).GetBool();
                }
                if (null != cfg.m_WindowStyle) {
                    var str = cfg.m_WindowStyle.Calc(calculator).AsString;
                    System.Diagnostics.ProcessWindowStyle style;
                    if (Enum.TryParse(str, out style)) {
                        option.WindowStyle = style;
                    }
                }
                if (null != cfg.m_CreateWindow) {
                    option.CreateWindow = cfg.m_CreateWindow.Calc(calculator).GetBool();
                }
                if (null != cfg.m_ErrorDialog) {
                    option.ErrorDialog = cfg.m_ErrorDialog.Calc(calculator).GetBool();
                }
                if (null != cfg.m_WorkingDirectory) {
                    option.WorkingDirectory = cfg.m_WorkingDirectory.Calc(calculator).AsString;
                }
                Encoding encoding = null;
                if (null != cfg.m_Encoding) {
                    var v = cfg.m_Encoding.Calc(calculator);
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
                    var v = cfg.m_Input.Calc(calculator);
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
                                input = SharedFileReader.ReadAllLines(str);
                            }
                        }
                    }
                    catch (Exception ex) {
                        CodeStructure.Log("input {0} failed:{1}", v, ex.Message);
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
                    var v = cfg.m_Output.Calc(calculator);
                    var str = v.AsString;
                    if (!string.IsNullOrEmpty(str)) {
                        str = Environment.ExpandEnvironmentVariables(str);
                        output = str;
                    }
                    else {
                        output = v;
                    }
                    if (null != cfg.m_OutputOptArg)
                        outputIx = cfg.m_OutputOptArg.Calc(calculator).GetInt();
                    outputBuilder = new StringBuilder();
                }
                if (null != cfg.m_Error) {
                    var v = cfg.m_Error.Calc(calculator);
                    var str = v.AsString;
                    if (!string.IsNullOrEmpty(str)) {
                        str = Environment.ExpandEnvironmentVariables(str);
                        error = str;
                    }
                    else {
                        error = v;
                    }
                    if (null != cfg.m_ErrorOptArg)
                        errorIx = cfg.m_ErrorOptArg.Calc(calculator).GetInt();
                    errorBuilder = new StringBuilder();
                }
                if (null != cfg.m_RedirectToConsole) {
                    var v = cfg.m_RedirectToConsole.Calc(calculator);
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
                                    calculator.SetVariable(file, outputBuilder.ToString());
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
                            CodeStructure.Log("output {0} failed:{1}", output, ex.Message);
                        }
                    }
                    if (null != errorBuilder && !error.IsNullObject) {
                        try {
                            var file = error.AsString;
                            if (!string.IsNullOrEmpty(file)) {
                                if (file[0] == '@' || file[0] == '$') {
                                    calculator.SetVariable(file, errorBuilder.ToString());
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
                            CodeStructure.Log("error {0} failed:{1}", error, ex.Message);
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
            internal IExpression m_Timeout = null;
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count > 1)
                throw new Exception("Expected: kill_me([exit_code]) api");
            int ret = 0;
            var p = System.Diagnostics.Process.GetCurrentProcess();
            if (null != p) {
                ret = p.Id;
                int exitCode = 0;
                if (operands.Count >= 1) {
                    exitCode = operands[0].GetInt();
                }
                if (DslCalculator.FileEchoOn) {
                    Console.WriteLine("kill_me {0}[pid:{1},session id:{2}] exit code:{3}", p.ProcessName, p.Id, p.SessionId, exitCode);
                }
                Environment.Exit(exitCode);
            }
            return ret;
        }
    }
    internal sealed class GetCurrentProcessIdExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: sleep(time) api");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count > 1)
                throw new Exception("Expected: wait_all([timeout]) api, wait all task to exit");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count > 1)
                throw new Exception("Expected: wait_start_interval(time) or wait_start_interval() api, used in Task.Wait for process/command");
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
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: cleanup_completed_tasks() api");
            DslCalculator.CleanupCompletedTasks();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetTaskCountExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 0)
                throw new Exception("Expected: get_task_count() api");
            return DslCalculator.Tasks.Count;
        }
    }
    internal sealed class CalcMd5Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count != 1)
                throw new Exception("Expected: calc_md_5(file) api");
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
    internal static class SharedFileReader
    {
        internal static string ReadAllText(string path, Encoding encoding)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream, encoding)) {
                return reader.ReadToEnd();
            }
        }
        internal static string[] ReadAllLines(string path, Encoding encoding)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream, encoding)) {
                var list = new List<string>();
                string line;
                while ((line = reader.ReadLine()) != null) {
                    list.Add(line);
                }
                return list.ToArray();
            }
        }
        internal static string[] ReadAllLines(string path)
        {
            return ReadAllLines(path, Encoding.UTF8);
        }
    }
    public sealed class DslCalculatorApiRegistry
    {
        public SortedList<string, string> ApiDocs {
            get { return m_ApiDocs; }
        }

        public LinqOperatorRegistry LinqOperatorRegistry {
            get { return m_LinqOperatorRegistry; }
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
            m_LinqOperatorRegistry.RegisterPredefined();

            Register("args", "args() api", new ExpressionFactoryHelper<ArgsGet>());
            Register("arg", "arg(ix) api", new ExpressionFactoryHelper<ArgGet>());
            Register("arg_num", "arg_num() api", new ExpressionFactoryHelper<ArgNumGet>());
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
            Register("atan_2", "atan_2(v1,v2) api", new ExpressionFactoryHelper<Atan2Exp>());
            Register("sinh", "sinh(v) api", new ExpressionFactoryHelper<SinhExp>());
            Register("cosh", "cosh(v) api", new ExpressionFactoryHelper<CoshExp>());
            Register("tanh", "tanh(v) api", new ExpressionFactoryHelper<TanhExp>());
            Register("rnd_int", "rnd_int(min,max) api", new ExpressionFactoryHelper<RndIntExp>());
            Register("rnd_float", "rnd_float(min,max) api", new ExpressionFactoryHelper<RndFloatExp>());
            Register("pow", "pow(v1,v2) api", new ExpressionFactoryHelper<PowExp>());
            Register("sqrt", "sqrt(v) api", new ExpressionFactoryHelper<SqrtExp>());
            Register("exp", "exp(v) api", new ExpressionFactoryHelper<ExpExp>());
            Register("exp_2", "exp_2(v) api", new ExpressionFactoryHelper<Exp2Exp>());
            Register("log", "log(v) api", new ExpressionFactoryHelper<LogExp>());
            Register("log_2", "log_2(v) api", new ExpressionFactoryHelper<Log2Exp>());
            Register("log_10", "log_10(v) api", new ExpressionFactoryHelper<Log10Exp>());
            Register("floor", "floor(v) api", new ExpressionFactoryHelper<FloorExp>());
            Register("ceiling", "ceiling(v) api", new ExpressionFactoryHelper<CeilingExp>());
            Register("round", "round(v) api", new ExpressionFactoryHelper<RoundExp>());
            Register("floor_to_int", "floor_to_int(v) api", new ExpressionFactoryHelper<FloorToIntExp>());
            Register("ceiling_to_int", "ceiling_to_int(v) api", new ExpressionFactoryHelper<CeilingToIntExp>());
            Register("round_to_int", "round_to_int(v) api", new ExpressionFactoryHelper<RoundToIntExp>());
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
            Register("date_time", "date_time(v) api", new ExpressionFactoryHelper<DateTimeExp>());
            Register("is_object", "is_object(v)", new ExpressionFactoryHelper<IsObjectExp>());
            Register("is_string", "is_string(v)", new ExpressionFactoryHelper<IsStringExp>());
            Register("is_boolean", "is_boolean(v)", new ExpressionFactoryHelper<IsBooleanExp>());
            Register("is_char", "is_char(v)", new ExpressionFactoryHelper<IsCharExp>());
            Register("is_integer", "is_integer(v)", new ExpressionFactoryHelper<IsIntegerExp>());
            Register("is_signed_integer", "is_signed_integer(v)", new ExpressionFactoryHelper<IsSignedIntegerExp>());
            Register("is_unsigned_integer", "is_unsigned_integer(v)", new ExpressionFactoryHelper<IsUnsignedIntegerExp>());
            Register("is_number", "is_number(v)", new ExpressionFactoryHelper<IsNumberExp>());
            Register("is_date_time", "is_date_time(v)", new ExpressionFactoryHelper<IsDateTimeExp>());
            Register("is_tuple", "is_tuple(v)", new ExpressionFactoryHelper<IsTupleExp>());
            Register("boxed_value_type", "boxed_value_type(v)", new ExpressionFactoryHelper<BoxedValueTypeExp>());
            Register("boxed_value_type_name", "boxed_value_type_name(v)", new ExpressionFactoryHelper<BoxedValueTypeNameExp>());
            Register("ftoi", "ftoi(v) api", new ExpressionFactoryHelper<FtoiExp>());
            Register("itof", "itof(v) api", new ExpressionFactoryHelper<ItofExp>());
            Register("ftou", "ftou(v) api", new ExpressionFactoryHelper<FtouExp>());
            Register("utof", "utof(v) api", new ExpressionFactoryHelper<UtofExp>());
            Register("dtol", "dtol(v) api", new ExpressionFactoryHelper<DtolExp>());
            Register("ltod", "ltod(v) api", new ExpressionFactoryHelper<LtodExp>());
            Register("dtou", "dtou(v) api", new ExpressionFactoryHelper<DtouExp>());
            Register("utod", "utod(v) api", new ExpressionFactoryHelper<UtodExp>());
            Register("lerp", "lerp(a,b,t) api", new ExpressionFactoryHelper<LerpExp>());
            Register("lerp_unclamped", "lerp_unclamped(a,b,t) api", new ExpressionFactoryHelper<LerpUnclampedExp>());
            Register("lerp_angle", "lerp_angle(a,b,t) api", new ExpressionFactoryHelper<LerpAngleExp>());
            Register("smooth_step", "smooth_step(from,to,t) api", new ExpressionFactoryHelper<SmoothStepExp>());
            Register("clamp_01", "clamp_01(v) api", new ExpressionFactoryHelper<Clamp01Exp>());
            Register("clamp", "clamp(v,v1,v2) api", new ExpressionFactoryHelper<ClampExp>());
            Register("approximately", "approximately(v1,v2) api", new ExpressionFactoryHelper<ApproximatelyExp>());
            Register("is_power_of_two", "is_power_of_two(v) api", new ExpressionFactoryHelper<IsPowerOfTwoExp>());
            Register("closest_power_of_two", "closest_power_of_two(v) api", new ExpressionFactoryHelper<ClosestPowerOfTwoExp>());
            Register("next_power_of_two", "next_power_of_two(v) api", new ExpressionFactoryHelper<NextPowerOfTwoExp>());
            Register("dist", "dist(x1,y1,x2,y2) api", new ExpressionFactoryHelper<DistExp>());
            Register("dist_sqr", "dist_sqr(x1,y1,x2,y2) api", new ExpressionFactoryHelper<DistSqrExp>());
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
            Register("loop_list", "loop_list(list)func(args); or loop_list(list){...}; statement, iterator is $$", new ExpressionFactoryHelper<LoopListExp>());
            Register("for_each_value", "for_each_value(args)func(args); or for_each_value(args){...}; statement, iterator is $$", new ExpressionFactoryHelper<ForEachValueExp>());
            Register("format", "format(fmt,arg1,arg2,...) api", new ExpressionFactoryHelper<FormatExp>());
            Register("get_type_assembly", "get_type_assembly(obj) api", new ExpressionFactoryHelper<GetTypeAssemblyExp>());
            Register("get_type_assembly_name", "get_type_assembly_name(obj) api", new ExpressionFactoryHelper<GetTypeAssemblyNameExp>());
            Register("get_type_full_name", "get_type_full_name(obj) api", new ExpressionFactoryHelper<GetTypeFullNameExp>());
            Register("get_type_name", "get_type_name(obj) api", new ExpressionFactoryHelper<GetTypeNameExp>());
            Register("get_type", "get_type(type_str) api", new ExpressionFactoryHelper<GetTypeExp>());
            Register("change_type", "change_type(obj,type_str) api", new ExpressionFactoryHelper<ChangeTypeExp>());
            Register("parse_enum", "parse_enum(type_str,val_str) api", new ExpressionFactoryHelper<ParseEnumExp>());
            Register("type_call", "type_call(obj_or_type,method,...) api, call Type's method", new ExpressionFactoryHelper<TypeCallExp>());
            Register("type_set", "type_set(obj_or_type,name,val) api, set Type's property", new ExpressionFactoryHelper<TypeSetExp>());
            Register("type_get", "type_get(obj_or_type,name) api, get Type's property", new ExpressionFactoryHelper<TypeGetExp>());
            Register("dotnet_call", "dotnet_call api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<DotnetCallExp>());
            Register("dotnet_set", "dotnet_set api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<DotnetSetExp>());
            Register("dotnet_get", "dotnet_get api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<DotnetGetExp>());
            Register("collection_call", "collection_call api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<CollectionCallExp>());
            Register("collection_set", "collection_set api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<CollectionSetExp>());
            Register("collection_get", "collection_get api, internal implementation, using csharp object syntax", new ExpressionFactoryHelper<CollectionGetExp>());
            Register("linq", "linq(list,method,arg1,arg2,...) statement, internal implementation, using obj.method(arg1,arg2,...) syntax, method can be where/filter/select/map/top/take/skip/distinct/concat/group_by/orderby/order_by_desc/aggregate/reduce/any/all/count/first/last/to_list/sum/min/max/average, iterator is $$ (An additional iterator $$acc for aggregate/reduce operators)", new ExpressionFactoryHelper<LinqExp>());
            Register("is_null", "is_null(obj) api", new ExpressionFactoryHelper<IsNullExp>());
            Register("null", "null() api", new ExpressionFactoryHelper<NullExp>());
            Register("equals_null", "equals_null(obj) api", new ExpressionFactoryHelper<EqualsNullExp>());
            Register("dotnet_load", "dotnet_load(dll_path) api", new ExpressionFactoryHelper<DotnetLoadExp>());
            Register("dotnet_new", "dotnet_new(assembly,type_name,arg1,arg2,...) api", new ExpressionFactoryHelper<DotnetNewExp>());
            Register("call_stack", "call_stack() api", new ExpressionFactoryHelper<CallStackExp>());
            Register("call", "call(func_name,arg1,arg2,...) api", new ExpressionFactoryHelper<CallExp>());
            Register("return", "return([val]) api", new ExpressionFactoryHelper<ReturnExp>());
            Register("redirect", "redirect(arg1,arg2,...) api", new ExpressionFactoryHelper<RedirectExp>());
            Register("prop_set", "prop_set(varname,val) - set variable", new ExpressionFactoryHelper<PropSetExp>());
            Register("prop_get", "prop_get(varname[,defval]) - get variable", new ExpressionFactoryHelper<PropGetExp>());
            Register("prop_exists", "prop_exists(varname) - check variable", new ExpressionFactoryHelper<PropExistsExp>());
            Register("func_exists", "func_exists(funcname) - check function", new ExpressionFactoryHelper<FuncExistsExp>());
            Register("await", "await(\"func_name\", arg1, arg2, ...) api, call async function", new ExpressionFactoryHelper<AwaitExp>());
            Register("await_while", "await_while(\"func_name\", arg1, arg2, ...) api, loop call sync function while result is true", new ExpressionFactoryHelper<AwaitWhileExp>());
            Register("await_until", "await_until(\"func_name\", arg1, arg2, ...) api, loop call sync function until result is true", new ExpressionFactoryHelper<AwaitUntilExp>());
            Register("await_time", "await_time(milliseconds) api, async wait specified time", new ExpressionFactoryHelper<AwaitTimeExp>());

            Register("substring", "substring(str[,start,len]) function", new ExpressionFactoryHelper<SubstringExp>());
            Register("new_string_builder", "new_string_builder() api", new ExpressionFactoryHelper<NewStringBuilderExp>());
            Register("append_format", "append_format(sb,fmt,arg1,arg2,...) api", new ExpressionFactoryHelper<AppendFormatExp>());
            Register("append_format_line", "append_format_line(sb,fmt,arg1,arg2,...) api", new ExpressionFactoryHelper<AppendFormatLineExp>());
            Register("stringbuilder_tostring", "stringbuilder_tostring(sb)", new ExpressionFactoryHelper<StringBuilderToStringExp>());
            Register("string_join", "string_join(sep,list) api", new ExpressionFactoryHelper<StringJoinExp>());
            Register("string_split", "string_split(str,sep_list) api", new ExpressionFactoryHelper<StringSplitExp>());
            Register("string_trim", "string_trim(str) api", new ExpressionFactoryHelper<StringTrimExp>());
            Register("string_trim_start", "string_trim_start(str) api", new ExpressionFactoryHelper<StringTrimStartExp>());
            Register("string_trim_end", "string_trim_end(str) api", new ExpressionFactoryHelper<StringTrimEndExp>());
            Register("string_to_lower", "string_to_lower(str) api", new ExpressionFactoryHelper<StringToLowerExp>());
            Register("string_to_upper", "string_to_upper(str) api", new ExpressionFactoryHelper<StringToUpperExp>());
            Register("string_replace", "string_replace(str,key,rep_str) api", new ExpressionFactoryHelper<StringReplaceExp>());
            Register("string_replace_char", "string_replace_char(str,key,char_as_str) api", new ExpressionFactoryHelper<StringReplaceCharExp>());
            Register("make_string", "make_string(char1_as_str_or_int,char2_as_str_or_int,...) api", new ExpressionFactoryHelper<MakeStringExp>());
            Register("string_contains", "string_contains(str,str_or_list_1,str_or_list_2,...) api", new ExpressionFactoryHelper<StringContainsExp>());
            Register("string_not_contains", "string_not_contains(str,str_or_list_1,str_or_list_2,...) api", new ExpressionFactoryHelper<StringNotContainsExp>());
            Register("string_contains_any", "string_contains_any(str,str_or_list_1,str_or_list_2,...) api", new ExpressionFactoryHelper<StringContainsAnyExp>());
            Register("string_not_contains_any", "string_not_contains_any(str,str_or_list_1,str_or_list_2,...) api", new ExpressionFactoryHelper<StringNotContainsAnyExp>());
            Register("str_2_int", "str_2_int(str) api", new ExpressionFactoryHelper<Str2IntExp>());
            Register("str_2_uint", "str_2_uint(str) api", new ExpressionFactoryHelper<Str2UintExp>());
            Register("str_2_long", "str_2_long(str) api", new ExpressionFactoryHelper<Str2LongExp>());
            Register("str_2_ulong", "str_2_ulong(str) api", new ExpressionFactoryHelper<Str2UlongExp>());
            Register("str_2_float", "str_2_float(str) api", new ExpressionFactoryHelper<Str2FloatExp>());
            Register("str_2_double", "str_2_double(str) api", new ExpressionFactoryHelper<Str2DoubleExp>());
            Register("hex_2_int", "hex_2_int(str) api", new ExpressionFactoryHelper<Hex2IntExp>());
            Register("hex_2_uint", "hex_2_uint(str) api", new ExpressionFactoryHelper<Hex2UintExp>());
            Register("hex_2_long", "hex_2_long(str) api", new ExpressionFactoryHelper<Hex2LongExp>());
            Register("hex_2_ulong", "hex_2_ulong(str) api", new ExpressionFactoryHelper<Hex2UlongExp>());
            Register("datetime_str", "datetime_str([fmt]) api", new ExpressionFactoryHelper<DatetimeStrExp>());
            Register("long_date_str", "long_date_str() api", new ExpressionFactoryHelper<LongDateStrExp>());
            Register("long_time_str", "long_time_str() api", new ExpressionFactoryHelper<LongTimeStrExp>());
            Register("short_date_str", "short_date_str() api", new ExpressionFactoryHelper<ShortDateStrExp>());
            Register("short_time_str", "short_time_str() api", new ExpressionFactoryHelper<ShortTimeStrExp>());
            Register("now", "now() api", new ExpressionFactoryHelper<NowExp>());
            Register("parse_date_time", "parse_date_time(str) api", new ExpressionFactoryHelper<ParseDateTimeExp>());
            Register("format_date_time", "format_date_time(fmt,date_time) api", new ExpressionFactoryHelper<FormatDateTimeExp>());
            Register("is_null_or_empty", "is_null_or_empty(str) api", new ExpressionFactoryHelper<IsNullOrEmptyExp>());
            Register("tuple", "(v1,v2,...) or tuple(v1,v2,...) object", new ExpressionFactoryHelper<TupleExp>());
            Register("tuple_set", "(var1,var2,...) = (v1,v2,...) or tuple_set((var1,var2,...), (v1,v2,...))", new ExpressionFactoryHelper<TupleSetExp>());
            Register("array", "[v1,v2,...] or array(v1,v2,...) object", new ExpressionFactoryHelper<ArrayExp>());
            Register("to_array", "to_array(list) api", new ExpressionFactoryHelper<ToArrayExp>());
            Register("list_size", "list_size(list) api", new ExpressionFactoryHelper<ListSizeExp>());
            Register("list", "list(v1,v2,...) object", new ExpressionFactoryHelper<ListExp>());
            Register("list_get", "list_get(list,index[,defval]) api", new ExpressionFactoryHelper<ListGetExp>());
            Register("list_set", "list_set(list,index,val) api", new ExpressionFactoryHelper<ListSetExp>());
            Register("list_index_of", "list_index_of(list,val) api", new ExpressionFactoryHelper<ListIndexOfExp>());
            Register("list_add", "list_add(list,val) api", new ExpressionFactoryHelper<ListAddExp>());
            Register("list_remove", "list_remove(list,val) api", new ExpressionFactoryHelper<ListRemoveExp>());
            Register("list_insert", "list_insert(list,index,val) api", new ExpressionFactoryHelper<ListInsertExp>());
            Register("list_remove_at", "list_remove_at(list,index) api", new ExpressionFactoryHelper<ListRemoveAtExp>());
            Register("list_clear", "list_clear(list) api", new ExpressionFactoryHelper<ListClearExp>());
            Register("list_split", "list_split(list,ct) api, return list of list", new ExpressionFactoryHelper<ListSplitExp>());
            Register("hashtable_size", "hashtable_size(hash) api", new ExpressionFactoryHelper<HashtableSizeExp>());
            Register("hashtable", "{k1=>v1,k2=>v2,...} or {k1:v1,k2:v2,...} or hashtable(k1=>v1,k2=>v2,...) or hashtable(k1:v1,k2:v2,...) object", new ExpressionFactoryHelper<HashtableExp>());
            Register("hashtable_get", "hashtable_get(hash,key[,defval]) api", new ExpressionFactoryHelper<HashtableGetExp>());
            Register("hashtable_set", "hashtable_set(hash,key,val) api", new ExpressionFactoryHelper<HashtableSetExp>());
            Register("hashtable_add", "hashtable_add(hash,key,val) api", new ExpressionFactoryHelper<HashtableAddExp>());
            Register("hashtable_remove", "hashtable_remove(hash,key) api", new ExpressionFactoryHelper<HashtableRemoveExp>());
            Register("hashtable_clear", "hashtable_clear(hash) api", new ExpressionFactoryHelper<HashtableClearExp>());
            Register("hashtable_keys", "hashtable_keys(hash) api", new ExpressionFactoryHelper<HashtableKeysExp>());
            Register("hashtable_values", "hashtable_values(hash) api", new ExpressionFactoryHelper<HashtableValuesExp>());
            Register("list_hashtable", "list_hashtable(hash) api, return list of pair", new ExpressionFactoryHelper<ListHashtableExp>());
            Register("hashtable_split", "hashtable_split(hash,ct) api, return list of hashtable", new ExpressionFactoryHelper<HashtableSplitExp>());
            Register("peek", "peek(queue_or_stack) api", new ExpressionFactoryHelper<PeekExp>());
            Register("stack_size", "stack_size(stack) api", new ExpressionFactoryHelper<StackSizeExp>());
            Register("stack", "stack(v1,v2,...) object", new ExpressionFactoryHelper<StackExp>());
            Register("push", "push(stack,v) api", new ExpressionFactoryHelper<PushExp>());
            Register("pop", "pop(stack) api", new ExpressionFactoryHelper<PopExp>());
            Register("stack_clear", "stack_clear(stack) api", new ExpressionFactoryHelper<StackClearExp>());
            Register("queue_size", "queue_size(queue) api", new ExpressionFactoryHelper<QueueSizeExp>());
            Register("queue", "queue(v1,v2,...) object", new ExpressionFactoryHelper<QueueExp>());
            Register("enqueue", "enqueue(queue,v) api", new ExpressionFactoryHelper<EnqueueExp>());
            Register("dequeue", "dequeue(queue) api", new ExpressionFactoryHelper<DequeueExp>());
            Register("queue_clear", "queue_clear(queue) api", new ExpressionFactoryHelper<QueueClearExp>());
            Register("set_env", "set_env(k,v) api", new ExpressionFactoryHelper<SetEnvironmentExp>());
            Register("get_env", "get_env(k) api", new ExpressionFactoryHelper<GetEnvironmentExp>());
            Register("expand", "expand(str) api", new ExpressionFactoryHelper<ExpandEnvironmentsExp>());
            Register("envs", "envs() api", new ExpressionFactoryHelper<EnvironmentsExp>());
            Register("cd", "cd(path) api", new ExpressionFactoryHelper<SetCurrentDirectoryExp>());
            Register("pwd", "pwd() api", new ExpressionFactoryHelper<GetCurrentDirectoryExp>());
            Register("cmd_line", "cmd_line() api", new ExpressionFactoryHelper<CommandLineExp>());
            Register("cmd_line_args", "cmd_line_args(prev_arg) or cmd_line_args() api, first return next arg, second return array of arg", new ExpressionFactoryHelper<CommandLineArgsExp>());
            Register("os", "os() api", new ExpressionFactoryHelper<OsExp>());
            Register("os_platform", "os_platform() api", new ExpressionFactoryHelper<OsPlatformExp>());
            Register("os_version", "os_version() api", new ExpressionFactoryHelper<OsVersionExp>());
            Register("get_full_path", "get_full_path(path) api", new ExpressionFactoryHelper<GetFullPathExp>());
            Register("get_path_root", "get_path_root(path) api", new ExpressionFactoryHelper<GetPathRootExp>());
            Register("get_random_file_name", "get_random_file_name() api", new ExpressionFactoryHelper<GetRandomFileNameExp>());
            Register("get_temp_file_name", "get_temp_file_name() api", new ExpressionFactoryHelper<GetTempFileNameExp>());
            Register("get_temp_path", "get_temp_path() api", new ExpressionFactoryHelper<GetTempPathExp>());
            Register("has_extension", "has_extension(path) api", new ExpressionFactoryHelper<HasExtensionExp>());
            Register("is_path_rooted", "is_path_rooted(path) api", new ExpressionFactoryHelper<IsPathRootedExp>());
            Register("get_file_name", "get_file_name(path) api", new ExpressionFactoryHelper<GetFileNameExp>());
            Register("get_file_name_without_extension", "get_file_name_without_extension(path) api", new ExpressionFactoryHelper<GetFileNameWithoutExtensionExp>());
            Register("get_extension", "get_extension(path) api", new ExpressionFactoryHelper<GetExtensionExp>());
            Register("get_directory_name", "get_directory_name(path) api", new ExpressionFactoryHelper<GetDirectoryNameExp>());
            Register("combine_path", "combine_path(path1,path2,...) api", new ExpressionFactoryHelper<CombinePathExp>());
            Register("change_extension", "change_extension(path,ext) api", new ExpressionFactoryHelper<ChangeExtensionExp>());
            Register("quote_path", "quote_path(path[,only_needed,single_quote]) api", new ExpressionFactoryHelper<QuotePathExp>());
            Register("echo", "echo(fmt,arg1,arg2,...) api, Console.WriteLine", new ExpressionFactoryHelper<EchoExp>());
            Register("file_echo", "file_echo(bool) or file_echo() api", new ExpressionFactoryHelper<FileEchoExp>());
            Register("dir_exist", "dir_exist(dir) api", new ExpressionFactoryHelper<DirectoryExistsExp>());
            Register("file_exist", "file_exist(file) api", new ExpressionFactoryHelper<FileExistsExp>());
            Register("path_exist", "path_exist(path) api", new ExpressionFactoryHelper<PathExistsExp>());
            Register("dir_exists", "dir_exists(dir) api", new ExpressionFactoryHelper<DirectoryExistsExp>());
            Register("file_exists", "file_exists(file) api", new ExpressionFactoryHelper<FileExistsExp>());
            Register("path_exists", "path_exists(path) api", new ExpressionFactoryHelper<PathExistsExp>());
            Register("list_dirs", "list_dirs(dir,filter_list_or_str_1,filter_list_or_str_2,...) api", new ExpressionFactoryHelper<ListDirectoriesExp>());
            Register("list_files", "list_files(dir,filter_list_or_str_1,filter_list_or_str_2,...) api", new ExpressionFactoryHelper<ListFilesExp>());
            Register("list_all_dirs", "list_all_dirs(dir,filter_list_or_str_1,filter_list_or_str_2,...) api", new ExpressionFactoryHelper<ListAllDirectoriesExp>());
            Register("list_all_files", "list_all_files(dir,filter_list_or_str_1,filter_list_or_str_2,...) api", new ExpressionFactoryHelper<ListAllFilesExp>());
            Register("create_dir", "create_dir(dir) api", new ExpressionFactoryHelper<CreateDirectoryExp>());
            Register("copy_dir", "copy_dir(dir1,dir2,filter_list_or_str_1,filter_list_or_str_2,...) api, include subdir", new ExpressionFactoryHelper<CopyDirectoryExp>());
            Register("move_dir", "move_dir(dir1,dir2) api", new ExpressionFactoryHelper<MoveDirectoryExp>());
            Register("delete_dir", "delete_dir(dir) api", new ExpressionFactoryHelper<DeleteDirectoryExp>());
            Register("copy_file", "copy_file(file1,file2) api", new ExpressionFactoryHelper<CopyFileExp>());
            Register("copy_files", "copy_files(dir1,dir2,filter_list_or_str_1,filter_list_or_str_2,...) api, dont include subdir", new ExpressionFactoryHelper<CopyFilesExp>());
            Register("move_file", "move_file(file1,file2) api", new ExpressionFactoryHelper<MoveFileExp>());
            Register("delete_file", "delete_file(file) api", new ExpressionFactoryHelper<DeleteFileExp>());
            Register("delete_files", "delete_files(dir,filter_list_or_str_1,filter_list_or_str_2,...) api, dont include subdir", new ExpressionFactoryHelper<DeleteFilesExp>());
            Register("delete_all_files", "delete_all_files(dir,filter_list_or_str_1,filter_list_or_str_2,...) api, include subdir", new ExpressionFactoryHelper<DeleteAllFilesExp>());
            Register("get_file_info", "get_file_info(file) api", new ExpressionFactoryHelper<GetFileInfoExp>());
            Register("get_dir_info", "get_dir_info(dir) api", new ExpressionFactoryHelper<GetDirectoryInfoExp>());
            Register("get_drive_info", "get_drive_info(drive) api", new ExpressionFactoryHelper<GetDriveInfoExp>());
            Register("get_drives_info", "get_drives_info() api", new ExpressionFactoryHelper<GetDrivesInfoExp>());
            Register("read_all_lines", "read_all_lines(file[,encoding]) api", new ExpressionFactoryHelper<ReadAllLinesExp>());
            Register("write_all_lines", "write_all_lines(file,lines[,encoding]) api", new ExpressionFactoryHelper<WriteAllLinesExp>());
            Register("read_all_text", "read_all_text(file[,encoding]) api", new ExpressionFactoryHelper<ReadAllTextExp>());
            Register("write_all_text", "write_all_text(file,txt[,encoding]) api", new ExpressionFactoryHelper<WriteAllTextExp>());
            Register("process", "process(file,arg_str) or process(file,arg_str){[options;]} api", new ExpressionFactoryHelper<CommandExp>());
            Register("command", "command{win{:cmd_str:};unix{:cmd_str:};common{:cmd_str:};[options;]} api", new ExpressionFactoryHelper<CommandExp>());
            Register("kill", "kill(name_or_pid) api", new ExpressionFactoryHelper<KillExp>());
            Register("kill_me", "kill_me([exit_code]) api", new ExpressionFactoryHelper<KillMeExp>());
            Register("pid", "pid() api", new ExpressionFactoryHelper<GetCurrentProcessIdExp>());
            Register("plist", "plist([filter]) api, return list", new ExpressionFactoryHelper<ListProcessesExp>());
            Register("sleep", "sleep(milliseconds) api", new ExpressionFactoryHelper<SleepExp>());
            Register("wait_all", "wait_all([timeout]) api, wait all task to exit", new ExpressionFactoryHelper<WaitAllExp>());
            Register("wait_start_interval", "wait_start_interval(time) or wait_start_interval() api, used in Task.Wait for process/command", new ExpressionFactoryHelper<WaitStartIntervalExp>());
            Register("cleanup_completed_tasks", "cleanup_completed_tasks() api", new ExpressionFactoryHelper<CleanupCompletedTasksExp>());
            Register("get_task_count", "get_task_count() api", new ExpressionFactoryHelper<GetTaskCountExp>());
            Register("calc_md_5", "calc_md_5(file) api", new ExpressionFactoryHelper<CalcMd5Exp>());
        }

        private LinqOperatorRegistry m_LinqOperatorRegistry = new LinqOperatorRegistry();
        private Dictionary<string, IExpressionFactory> m_ApiFactories = new Dictionary<string, IExpressionFactory>();
        private SortedList<string, string> m_ApiDocs = new SortedList<string, string>();
    }
}
#pragma warning restore 8600,8601,8602,8603,8604,8618,8619,8620,8625
#endregion
