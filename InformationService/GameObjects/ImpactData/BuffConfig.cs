using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // BuffData.txt only carries numeric attribute deltas; behavioural buffs live in the logic classes.
    public sealed class BuffConfig
    {
        public int BuffId;
        public string Desc;
        public List<int> Attrs = new List<int>();
        public List<int> Values = new List<int>();
    }
}
