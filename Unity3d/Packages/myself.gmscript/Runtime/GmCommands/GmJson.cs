using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

// Minimal JSON parsing/serialization. No Unity dependency, safe to use from socket threads.
internal static class GmJson
{
    internal static Dictionary<string, object> ParseObject(string text)
    {
        var v = Parse(text) as Dictionary<string, object>;
        if (null == v) {
            throw new FormatException("json object expected");
        }
        return v;
    }
    internal static object Parse(string text)
    {
        if (null == text) {
            return null;
        }
        int pos = 0;
        object v = ParseValue(text, ref pos);
        SkipWhitespace(text, ref pos);
        if (pos != text.Length) {
            throw new FormatException(string.Format("unexpected trailing content at {0}", pos));
        }
        return v;
    }
    internal static string Serialize(object value)
    {
        var sb = new StringBuilder(512);
        WriteValue(sb, value);
        return sb.ToString();
    }
    // Returns escaped text without the surrounding quotes, for building JSON fragments.
    internal static string Escape(string text)
    {
        if (null == text) {
            return string.Empty;
        }
        var sb = new StringBuilder(text.Length + 8);
        for (int i = 0; i < text.Length; ++i) {
            WriteEscapedChar(sb, text[i]);
        }
        return sb.ToString();
    }
    internal static string GetString(IDictionary<string, object> dict, string key, string defVal)
    {
        object v;
        if (null == dict || !dict.TryGetValue(key, out v) || null == v) {
            return defVal;
        }
        var s = v as string;
        if (null != s) {
            return s;
        }
        return Convert.ToString(v, CultureInfo.InvariantCulture);
    }
    internal static int GetInt(IDictionary<string, object> dict, string key, int defVal)
    {
        object v;
        if (null == dict || !dict.TryGetValue(key, out v) || null == v) {
            return defVal;
        }
        try {
            return (int)Math.Round(Convert.ToDouble(v, CultureInfo.InvariantCulture));
        }
        catch {
            return defVal;
        }
    }
    internal static long GetLong(IDictionary<string, object> dict, string key, long defVal)
    {
        object v;
        if (null == dict || !dict.TryGetValue(key, out v) || null == v) {
            return defVal;
        }
        try {
            return (long)Math.Round(Convert.ToDouble(v, CultureInfo.InvariantCulture));
        }
        catch {
            return defVal;
        }
    }

    private static object ParseValue(string text, ref int pos)
    {
        SkipWhitespace(text, ref pos);
        if (pos >= text.Length) {
            throw new FormatException("unexpected end of json");
        }
        char c = text[pos];
        switch (c) {
            case '{':
                return ParseObjectValue(text, ref pos);
            case '[':
                return ParseArrayValue(text, ref pos);
            case '"':
                return ParseString(text, ref pos);
            case 't':
                Expect(text, ref pos, "true");
                return true;
            case 'f':
                Expect(text, ref pos, "false");
                return false;
            case 'n':
                Expect(text, ref pos, "null");
                return null;
            default:
                return ParseNumber(text, ref pos);
        }
    }
    private static Dictionary<string, object> ParseObjectValue(string text, ref int pos)
    {
        var dict = new Dictionary<string, object>();
        ++pos;
        SkipWhitespace(text, ref pos);
        if (pos < text.Length && text[pos] == '}') {
            ++pos;
            return dict;
        }
        while (pos < text.Length) {
            SkipWhitespace(text, ref pos);
            if (pos >= text.Length || text[pos] != '"') {
                throw new FormatException("object key expected at " + pos);
            }
            string key = ParseString(text, ref pos);
            SkipWhitespace(text, ref pos);
            if (pos >= text.Length || text[pos] != ':') {
                throw new FormatException("':' expected at " + pos);
            }
            ++pos;
            dict[key] = ParseValue(text, ref pos);
            SkipWhitespace(text, ref pos);
            if (pos >= text.Length) {
                break;
            }
            if (text[pos] == ',') {
                ++pos;
                continue;
            }
            if (text[pos] == '}') {
                ++pos;
                return dict;
            }
            throw new FormatException("',' or '}' expected at " + pos);
        }
        throw new FormatException("unterminated object");
    }
    private static List<object> ParseArrayValue(string text, ref int pos)
    {
        var list = new List<object>();
        ++pos;
        SkipWhitespace(text, ref pos);
        if (pos < text.Length && text[pos] == ']') {
            ++pos;
            return list;
        }
        while (pos < text.Length) {
            list.Add(ParseValue(text, ref pos));
            SkipWhitespace(text, ref pos);
            if (pos >= text.Length) {
                break;
            }
            if (text[pos] == ',') {
                ++pos;
                continue;
            }
            if (text[pos] == ']') {
                ++pos;
                return list;
            }
            throw new FormatException("',' or ']' expected at " + pos);
        }
        throw new FormatException("unterminated array");
    }
    private static string ParseString(string text, ref int pos)
    {
        ++pos;
        var sb = new StringBuilder();
        while (pos < text.Length) {
            char c = text[pos++];
            if (c == '"') {
                return sb.ToString();
            }
            if (c != '\\') {
                sb.Append(c);
                continue;
            }
            if (pos >= text.Length) {
                break;
            }
            char esc = text[pos++];
            switch (esc) {
                case '"': sb.Append('"'); break;
                case '\\': sb.Append('\\'); break;
                case '/': sb.Append('/'); break;
                case 'b': sb.Append('\b'); break;
                case 'f': sb.Append('\f'); break;
                case 'n': sb.Append('\n'); break;
                case 'r': sb.Append('\r'); break;
                case 't': sb.Append('\t'); break;
                case 'u': {
                        if (pos + 4 > text.Length) {
                            throw new FormatException("bad unicode escape");
                        }
                        string hex = text.Substring(pos, 4);
                        pos += 4;
                        sb.Append((char)Convert.ToInt32(hex, 16));
                    }
                    break;
                default:
                    throw new FormatException("bad escape: \\" + esc);
            }
        }
        throw new FormatException("unterminated string");
    }
    private static object ParseNumber(string text, ref int pos)
    {
        int start = pos;
        while (pos < text.Length) {
            char c = text[pos];
            if ((c >= '0' && c <= '9') || c == '-' || c == '+' || c == '.' || c == 'e' || c == 'E') {
                ++pos;
            }
            else {
                break;
            }
        }
        if (start == pos) {
            throw new FormatException("number expected at " + start);
        }
        string num = text.Substring(start, pos - start);
        double d;
        if (!double.TryParse(num, NumberStyles.Float, CultureInfo.InvariantCulture, out d)) {
            throw new FormatException("bad number: " + num);
        }
        return d;
    }
    private static void Expect(string text, ref int pos, string literal)
    {
        if (pos + literal.Length > text.Length || string.CompareOrdinal(text, pos, literal, 0, literal.Length) != 0) {
            throw new FormatException("'" + literal + "' expected at " + pos);
        }
        pos += literal.Length;
    }
    private static void SkipWhitespace(string text, ref int pos)
    {
        while (pos < text.Length) {
            char c = text[pos];
            if (c == ' ' || c == '\t' || c == '\r' || c == '\n') {
                ++pos;
            }
            else {
                break;
            }
        }
    }

    private static void WriteValue(StringBuilder sb, object value)
    {
        if (null == value) {
            sb.Append("null");
            return;
        }
        var s = value as string;
        if (null != s) {
            WriteString(sb, s);
            return;
        }
        if (value is bool) {
            sb.Append((bool)value ? "true" : "false");
            return;
        }
        if (value is int || value is long || value is short || value is byte || value is uint || value is ulong) {
            sb.Append(Convert.ToInt64(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture));
            return;
        }
        if (value is float) {
            WriteDouble(sb, (float)value);
            return;
        }
        if (value is double) {
            WriteDouble(sb, (double)value);
            return;
        }
        if (value is decimal) {
            WriteDouble(sb, (double)(decimal)value);
            return;
        }
        var dict = value as IDictionary<string, object>;
        if (null != dict) {
            sb.Append('{');
            bool first = true;
            foreach (var pair in dict) {
                if (!first) {
                    sb.Append(',');
                }
                first = false;
                WriteString(sb, pair.Key);
                sb.Append(':');
                WriteValue(sb, pair.Value);
            }
            sb.Append('}');
            return;
        }
        var strDict = value as IDictionary;
        if (null != strDict) {
            sb.Append('{');
            bool first = true;
            foreach (DictionaryEntry entry in strDict) {
                if (!first) {
                    sb.Append(',');
                }
                first = false;
                WriteString(sb, Convert.ToString(entry.Key, CultureInfo.InvariantCulture));
                sb.Append(':');
                WriteValue(sb, entry.Value);
            }
            sb.Append('}');
            return;
        }
        var seq = value as IEnumerable;
        if (null != seq) {
            sb.Append('[');
            bool first = true;
            foreach (var item in seq) {
                if (!first) {
                    sb.Append(',');
                }
                first = false;
                WriteValue(sb, item);
            }
            sb.Append(']');
            return;
        }
        WriteString(sb, value.ToString());
    }
    private static void WriteDouble(StringBuilder sb, double d)
    {
        if (double.IsNaN(d) || double.IsInfinity(d)) {
            sb.Append("null");
            return;
        }
        if (Math.Abs(d) < 1e15 && d == Math.Floor(d)) {
            sb.Append(((long)d).ToString(CultureInfo.InvariantCulture));
            return;
        }
        sb.Append(d.ToString("R", CultureInfo.InvariantCulture));
    }
    private static void WriteString(StringBuilder sb, string text)
    {
        sb.Append('"');
        if (null != text) {
            for (int i = 0; i < text.Length; ++i) {
                WriteEscapedChar(sb, text[i]);
            }
        }
        sb.Append('"');
    }
    private static void WriteEscapedChar(StringBuilder sb, char c)
    {
        switch (c) {
            case '"': sb.Append("\\\""); break;
            case '\\': sb.Append("\\\\"); break;
            case '\b': sb.Append("\\b"); break;
            case '\f': sb.Append("\\f"); break;
            case '\n': sb.Append("\\n"); break;
            case '\r': sb.Append("\\r"); break;
            case '\t': sb.Append("\\t"); break;
            default:
                if (c < ' ') {
                    sb.Append("\\u");
                    sb.Append(((int)c).ToString("x4", CultureInfo.InvariantCulture));
                }
                else {
                    sb.Append(c);
                }
                break;
        }
    }
}
