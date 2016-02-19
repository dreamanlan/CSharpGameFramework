using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
  public static class DataProtoUtility
  {
    public static List<T> SplitNumericList<T>(char[] splits, string vec)
    {
      List<T> list = new List<T>();
      try {
        string strPos = vec;
        string[] resut = strPos.Split(splits, StringSplitOptions.RemoveEmptyEntries);
        if (resut != null && resut.Length > 0) {
          for (int index = 0; index < resut.Length; index++) {
            list.Add((T)Convert.ChangeType(resut[index], typeof(T)));
          }
        }
      } catch {
        list.Clear();
      }
      return list;
    }
    public static string JoinNumericList<T>(string separator, List<T> list)
    {
      List<string> slist = new List<string>();
      foreach (T v in list) {
        slist.Add(v.ToString());
      };
      return string.Join(separator, slist.ToArray());
    }
    public static Dictionary<KeyT, ValueT> SplitNumericDictionary<KeyT, ValueT>(char[] splits1, char[] splits2, string vec)
    {
      Dictionary<KeyT, ValueT> dict = new Dictionary<KeyT, ValueT>();
      try {
        string strPos = vec;
        string[] resut = strPos.Split(splits1, StringSplitOptions.RemoveEmptyEntries);
        if (resut != null && resut.Length > 0) {
          for (int index = 0; index < resut.Length; index++) {
            string[] pair = resut[index].Split(splits2);
            if (pair.Length == 2) {
              dict.Add((KeyT)Convert.ChangeType(pair[0], typeof(KeyT)), (ValueT)Convert.ChangeType(pair[1], typeof(ValueT)));
            }
          }
        }
      } catch {
        dict.Clear();
      }
      return dict;
    }
    public static string JoinNumericDictionary<KeyT, ValueT>(string separator1, string separator2, Dictionary<KeyT, ValueT> dict)
    {
      List<string> slist = new List<string>();
      foreach(var pair in dict){
        slist.Add(string.Format("{0}{1}{2}", pair.Key, separator2, pair.Value));
      };
      return string.Join(separator1, slist.ToArray());
    }
    public static List<DateTime> SplitDateTimeList(char[] splits, string vec)
    {
      List<DateTime> list = new List<DateTime>();
      try {
        string strPos = vec;
        string[] resut = strPos.Split(splits, StringSplitOptions.RemoveEmptyEntries);
        if (resut != null && resut.Length > 0) {
          for (int index = 0; index < resut.Length; index++) {
            list.Add(DateTime.ParseExact(resut[index], "yyyyMMddHHmmss", null));
          }
        }
      } catch {
        list.Clear();
      }
      return list;
    }
    public static string JoinDateTimeList(string separator, List<DateTime> list)
    {
      List<string> slist = new List<string>();
      foreach (DateTime v in list) {
        slist.Add(v.ToString("yyyyMMddHHmmss"));
      };
      return string.Join(separator, slist.ToArray());
    }
    public static Dictionary<DateTime, DateTime> SplitDateTimeDictionary(char[] splits1, char[] splits2, string vec)
    {
      Dictionary<DateTime, DateTime> dict = new Dictionary<DateTime, DateTime>();
      try {
        string strPos = vec;
        string[] resut = strPos.Split(splits1, StringSplitOptions.RemoveEmptyEntries);
        if (resut != null && resut.Length > 0) {
          for (int index = 0; index < resut.Length; index++) {
            string[] pair = resut[index].Split(splits2);
            if (pair.Length == 2) {
              dict.Add(DateTime.ParseExact(pair[0], "yyyyMMddHHmmss", null), DateTime.ParseExact(pair[1], "yyyyMMddHHmmss", null));
            }
          }
        }
      } catch {
        dict.Clear();
      }
      return dict;
    }
    public static string JoinDateTimeDictionary(string separator1, string separator2, Dictionary<DateTime, DateTime> dict)
    {
      List<string> slist = new List<string>();
      foreach (var pair in dict) {
        slist.Add(string.Format("{0}{1}{2}", pair.Key.ToString("yyyyMMddHHmmss"), separator2, pair.Value.ToString("yyyyMMddHHmmss")));
      };
      return string.Join(separator1, slist.ToArray());
    }
    public static Dictionary<DateTime, ValueT> SplitDateTimeKeyDictionary<ValueT>(char[] splits1, char[] splits2, string vec)
    {
      Dictionary<DateTime, ValueT> dict = new Dictionary<DateTime, ValueT>();
      try {
        string strPos = vec;
        string[] resut = strPos.Split(splits1, StringSplitOptions.RemoveEmptyEntries);
        if (resut != null && resut.Length > 0) {
          for (int index = 0; index < resut.Length; index++) {
            string[] pair = resut[index].Split(splits2);
            if (pair.Length == 2) {
              dict.Add(DateTime.ParseExact(pair[0], "yyyyMMddHHmmss", null), (ValueT)Convert.ChangeType(pair[1], typeof(ValueT)));
            }
          }
        }
      } catch {
        dict.Clear();
      }
      return dict;
    }
    public static string JoinDateTimeKeyDictionary<ValueT>(string separator1, string separator2, Dictionary<DateTime, ValueT> dict)
    {
      List<string> slist = new List<string>();
      foreach (var pair in dict) {
        slist.Add(string.Format("{0}{1}{2}", pair.Key.ToString("yyyyMMddHHmmss"), separator2, pair.Value));
      };
      return string.Join(separator1, slist.ToArray());
    }
    public static Dictionary<KeyT, DateTime> SplitDateTimeValueDictionary<KeyT>(char[] splits1, char[] splits2, string vec)
    {
      Dictionary<KeyT, DateTime> dict = new Dictionary<KeyT, DateTime>();
      try {
        string strPos = vec;
        string[] resut = strPos.Split(splits1, StringSplitOptions.RemoveEmptyEntries);
        if (resut != null && resut.Length > 0) {
          for (int index = 0; index < resut.Length; index++) {
            string[] pair = resut[index].Split(splits2);
            if (pair.Length == 2) {
              dict.Add((KeyT)Convert.ChangeType(pair[0], typeof(KeyT)), DateTime.ParseExact(pair[1], "yyyyMMddHHmmss", null));
            }
          }
        }
      } catch {
        dict.Clear();
      }
      return dict;
    }
    public static string JoinDateTimeValueDictionary<KeyT>(string separator1, string separator2, Dictionary<KeyT, DateTime> dict)
    {
      List<string> slist = new List<string>();
      foreach (var pair in dict) {
        slist.Add(string.Format("{0}{1}{2}", pair.Key, separator2, pair.Value.ToString("yyyyMMddHHmmss")));
      };
      return string.Join(separator1, slist.ToArray());
    }
  }
}
