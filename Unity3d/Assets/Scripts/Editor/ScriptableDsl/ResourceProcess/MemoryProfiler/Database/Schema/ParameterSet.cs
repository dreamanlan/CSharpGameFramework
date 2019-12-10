namespace Unity.MemoryProfilerForExtension.Editor.Database
{
    internal class ParameterSet
    {
        System.Collections.Generic.Dictionary<string, Operation.Expression> m_Parameters = new System.Collections.Generic.Dictionary<string, Operation.Expression>();
        public int Count { get { return m_Parameters.Count; } }
        public void Add(string key, Operation.Expression value)
        {
            m_Parameters.Add(key, value);
        }

        public bool TryGet(string key, out Operation.Expression value)
        {
            return m_Parameters.TryGetValue(key, out value);
        }

        public void AddValue<DataT>(string key, DataT value) where DataT : System.IComparable
        {
            m_Parameters.Add(key, new Operation.ExpConst<DataT>(value));
        }

        public bool TryGetValue<DataT>(string key, out DataT value) where DataT : System.IComparable
        {
            Operation.Expression exp;
            if (m_Parameters.TryGetValue(key, out exp))
            {
                if (exp is Operation.ExpConst<DataT>)
                {
                    value = (exp as Operation.ExpConst<DataT>).GetValue(0);
                    return true;
                }
            }
            value = default(DataT);
            return false;
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("(");
            bool needComma = false;
            foreach (var p in m_Parameters)
            {
                if (needComma)
                {
                    sb.Append(", ");
                }
                else
                {
                    needComma = true;
                }
                sb.AppendFormat("{0}={1}", p.Key, p.Value.GetValueString(0, DefaultDataFormatter.Instance));
            }
            sb.Append(")");
            return sb.ToString();
        }

        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, Operation.Expression>> AllParameters
        {
            get
            {
                return m_Parameters;
            }
        }
    }
}
