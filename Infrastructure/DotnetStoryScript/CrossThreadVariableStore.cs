using System;
using System.Collections.Generic;
using ScriptableFramework;

namespace DotnetStoryScript
{
    /// <summary>
    /// Cross-thread global variable storage for @@var syntax.
    /// All access is thread-safe via locking.
    /// </summary>
    public static class CrossThreadVariableStore
    {
        public static bool TryGetValue(string name, out BoxedValue val)
        {
            lock (s_Lock) {
                return s_Variables.TryGetValue(name, out val);
            }
        }

        public static void SetValue(string name, BoxedValue val)
        {
            lock (s_Lock) {
                s_Variables[name] = val;
            }
        }

        public static bool Remove(string name)
        {
            lock (s_Lock) {
                return s_Variables.Remove(name);
            }
        }

        public static void Clear()
        {
            lock (s_Lock) {
                s_Variables.Clear();
            }
        }

        private static Dictionary<string, BoxedValue> s_Variables = new Dictionary<string, BoxedValue>();
        private static object s_Lock = new object();
    }
}
