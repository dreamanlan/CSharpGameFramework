namespace Unity.MemoryProfilerForExtension.Editor.UI
{
    internal abstract class HistoryEvent
    {
        protected const string seperator = "::";
        //public abstract bool IsSame(HistoryEvent e);
        public UIState.BaseMode Mode;
    }

    /// <summary>
    /// Keeps a time-line of events that may be revisited on demand.
    /// Useful for navigation and undo mechanisms
    /// </summary>
    internal class History
    {
        public System.Collections.Generic.List<HistoryEvent> events = new System.Collections.Generic.List<HistoryEvent>();
        public int backCount = 0;
        public bool hasPresentEvent = false;

        public event System.Action historyChanged = delegate {};

        public void Clear()
        {
            backCount = 0;
            hasPresentEvent = false;
            events.Clear();
            historyChanged();
        }

        protected int eventCount
        {
            get
            {
                if (hasPresentEvent) return events.Count;
                return events.Count + 1;
            }
        }
        public bool isPresent
        {
            get
            {
                return backCount == 0;
            }
        }
        public bool hasPast
        {
            get
            {
                return backCount + 1 < eventCount;
            }
        }
        public bool hasFuture
        {
            get
            {
                return backCount > 0;
            }
        }


        public void AddEvent(HistoryEvent e)
        {
            if (hasFuture)
            {
                //remove future
                var i = events.Count - backCount;
                events.RemoveRange(i, backCount);
            }
            backCount = 0;
            if (events.Count > 0)
            {
                var last = events[events.Count - 1];
                if (!last.Equals(e))
                {
                    events.Add(e);
                }
            }
            else
            {
                events.Add(e);
            }
            hasPresentEvent = false;
            //UnityEngine.Debug.Log("History add: " + e.ToString());
            //PrintHistory();
            historyChanged();
        }

        public void SetPresentEvent(HistoryEvent ePresent)
        {
            if (ePresent == null) return;
            events.Add(ePresent);
            hasPresentEvent = true;
            historyChanged();
        }

        public HistoryEvent Backward()
        {
            if (hasPast)
            {
                if (isPresent && !hasPresentEvent)
                {
                    //remove last event
                    int l = events.Count - 1;
                    var e = events[l];
                    events.RemoveAt(l);
                    historyChanged();
                    return e;
                }
                else
                {
                    ++backCount;
                    var i = GetCurrent();
                    historyChanged();
                    return events[i];
                }
            }

            historyChanged();
            return null;
        }

        public HistoryEvent Forward()
        {
            if (hasFuture)
            {
                --backCount;
                var i = GetCurrent();
                historyChanged();
                return events[i];
            }

            historyChanged();
            return null;
        }

        protected int GetCurrent()
        {
            return events.Count - 1 - backCount;
        }

        void PrintHistory()
        {
            string strOut = "";
            foreach (var e in events)
            {
                strOut += e.ToString() + "\n";
            }
            UnityEngine.Debug.Log(strOut);
        }
    }
}
