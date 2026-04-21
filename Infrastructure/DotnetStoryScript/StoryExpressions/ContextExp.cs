using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

namespace DotnetStoryScript
{
    // ========== Context Info ==========

    /// <summary>
    /// eval(exp1, exp2, ...) - evaluate all expressions and return the last value
    /// </summary>
    internal sealed class EvalExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count == 0)
                return BoxedValue.NullObject;
            return operands[operands.Count - 1];
        }
    }

    /// <summary>
    /// namespace() - get current story namespace
    /// </summary>
    internal sealed class NamespaceExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst != null)
                return storyInst.Namespace;
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// storyid() - get current story ID
    /// </summary>
    internal sealed class StoryIdExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst != null)
                return storyInst.StoryId;
            return BoxedValue.NullObject;
        }
    }

    /// <summary>
    /// messageid() - get current message ID
    /// </summary>
    internal sealed class MessageIdExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var storyInst = Calculator.GetFuncContext<StoryInstance>();
            if (storyInst != null && storyInst.CurrentCoroutine != null)
                return storyInst.CurrentCoroutine.MessageId;
            return BoxedValue.NullObject;
        }
    }

    // ========== List Utilities ==========

    /// <summary>
    /// stringlist(str_split_by_sep) - parse comma-separated string into list of strings
    /// </summary>
    internal sealed class StringListExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: stringlist(str_split_by_sep)");

            string str = operands[0].ToString();
            var list = new List<string>();
            if (!string.IsNullOrEmpty(str))
            {
                string[] parts = str.Split(',');
                foreach (var part in parts)
                    list.Add(part.Trim());
            }
            return BoxedValue.FromObject(list);
        }
    }

    /// <summary>
    /// intlist(str_split_by_sep) - parse comma-separated string into list of ints
    /// </summary>
    internal sealed class IntListExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: intlist(str_split_by_sep)");

            string str = operands[0].ToString();
            var list = Converter.ConvertNumericList<int>(str);
            return BoxedValue.FromObject(list);
        }
    }

    /// <summary>
    /// floatlist(str_split_by_sep) - parse comma-separated string into list of floats
    /// </summary>
    internal sealed class FloatListExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: floatlist(str_split_by_sep)");

            string str = operands[0].ToString();
            var list = Converter.ConvertNumericList<float>(str);
            return BoxedValue.FromObject(list);
        }
    }

    /// <summary>
    /// rndfromlist(list[, defval]) - return a random element from list
    /// </summary>
    internal sealed class RndFromListExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: rndfromlist(list[, defval])");

            var listVal = operands[0].ObjectVal as IList;
            if (listVal == null)
                return operands.Count > 1 ? operands[1] : BoxedValue.NullObject;

            int ct = listVal.Count;
            if (ct <= 0)
                return operands.Count > 1 ? operands[1] : BoxedValue.NullObject;

            int ix = Helper.Random.Next(ct);
            if (ix >= 0 && ix < ct)
                return BoxedValue.FromObject(listVal[ix]);
            return BoxedValue.FromObject(listVal[0]);
        }
    }

    // ========== JSON ==========

    /// <summary>
    /// tojson(obj) - serialize object to JSON string
    /// </summary>
    internal sealed class ToJsonExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: tojson(obj)");

            object obj = operands[0].GetObject();
            if (obj == null)
                return string.Empty;

            LitJson.JsonData json = ToJson(obj);
            return LitJson.JsonMapper.ToJson(json);
        }

        private static LitJson.JsonData ToJson(object obj)
        {
            var dict = obj as IDictionary;
            var enumer = obj as IEnumerable;
            if (dict != null)
            {
                var jsonData = new LitJson.JsonData();
                var e = dict.GetEnumerator();
                while (e.MoveNext())
                {
                    string key = e.Key?.ToString();
                    if (!string.IsNullOrEmpty(key))
                        jsonData[key] = ToJson(e.Value);
                }
                return jsonData;
            }
            else if (enumer != null)
            {
                var jsonData = new LitJson.JsonData();
                var e = enumer.GetEnumerator();
                while (e.MoveNext())
                    jsonData.Add(ToJson(e.Current));
                return jsonData;
            }
            else if (obj == null)
            {
                return new LitJson.JsonData();
            }
            else if (obj is int i) return new LitJson.JsonData(i);
            else if (obj is long l) return new LitJson.JsonData(l);
            else if (obj is bool b) return new LitJson.JsonData(b);
            else if (obj is float f) return new LitJson.JsonData((double)f);
            else if (obj is double d) return new LitJson.JsonData(d);
            else if (obj is string s) return new LitJson.JsonData(s);
            else return new LitJson.JsonData(obj);
        }
    }

    /// <summary>
    /// fromjson(json_str) - deserialize JSON string to object
    /// </summary>
    internal sealed class FromJsonExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: fromjson(json_str)");

            string str = operands[0].ToString();
            var json = LitJson.JsonMapper.ToObject(str);
            return ToValue(json, BoxedValue.NullObject);
        }

        private static BoxedValue ToValue(LitJson.JsonData data, BoxedValue defVal)
        {
            if (data == null) return defVal;
            if (data.IsObject)
            {
                var dict = new Dictionary<BoxedValue, BoxedValue>();
                foreach (var key in data.Keys)
                {
                    var val = ToValue(data[key], defVal);
                    dict.Add(key, val);
                }
                return BoxedValue.FromObject(dict);
            }
            else if (data.IsArray)
            {
                var list = new List<BoxedValue>();
                for (int i = 0; i < data.Count; ++i)
                {
                    var val = ToValue(data[i], defVal);
                    list.Add(val);
                }
                return BoxedValue.FromObject(list);
            }
            else if (data.IsInt) return BoxedValue.From((int)data);
            else if (data.IsLong) return BoxedValue.From((int)(long)data);
            else if (data.IsDouble) return BoxedValue.From((float)(double)data);
            else if (data.IsBoolean) return BoxedValue.From((bool)data ? 1 : 0);
            else if (data.IsString) return BoxedValue.FromString((string)data);
            else return defVal;
        }
    }
}
