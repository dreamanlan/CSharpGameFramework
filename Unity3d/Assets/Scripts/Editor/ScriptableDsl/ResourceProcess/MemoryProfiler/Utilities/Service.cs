using System;
using System.Collections.Generic;

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal class Service<IServieT>
    {
        static private Stack<IServieT> m_Implementation = new Stack<IServieT>();

        static public bool Available { get { return m_Implementation.Count > 0; } }

        static public IServieT Current
        {
            get
            {
                if (m_Implementation.Count > 0) return m_Implementation.Peek();
                throw new InvalidOperationException("No " + typeof(IServieT).FullName + " service currently registered");
            }
        }

        // RAII class for scoping services. Must be instantiated into a using statement
        public class ScopeService : IDisposable
        {
            public ScopeService(IServieT service)
            {
                m_Implementation.Push(service);
            }

            void IDisposable.Dispose()
            {
                m_Implementation.Pop();
            }
        }
    }
}
