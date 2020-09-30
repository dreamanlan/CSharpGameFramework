using UnityEngine;
#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
#else
using UnityEngine.Experimental.UIElements;
#endif
using System;
using Unity.Profiling;

namespace Unity.MemoryProfilerForExtension.Editor.UI
{
    internal interface IViewPaneEventListener
    {
        void OnOpenLink(Database.LinkRequest link);
        void OnOpenLink(Database.LinkRequest link, UIState.SnapshotMode mode);
        void OnOpenMemoryMap();
        void OnOpenTreeMap();
        void OnRepaint();
    }
    internal abstract class ViewPane : UI.IViewEventListener
    {
        public UIState m_UIState;
        public IViewPaneEventListener m_EventListener;
        public ViewPane(UIState s, IViewPaneEventListener l)
        {
            m_UIState = s;
            m_EventListener = l;
        }

        protected VisualElement[] m_VisualElements;
        protected Action<Rect>[] m_VisualElementsOnGUICalls;

        public virtual VisualElement[] VisualElements
        {
            get
            {
                if (m_VisualElements == null)
                {
                    m_VisualElements = new VisualElement[]
                    {
                        new IMGUIContainer(() => OnGUI(0))
                        {
                            style =
                            {
                                flexGrow = 1,
                            }
                        }
                    };
                    m_VisualElementsOnGUICalls = new Action<Rect>[]
                    {
                        OnGUI,
                    };
                }
                return m_VisualElements;
            }
        }

        public abstract UI.HistoryEvent GetCurrentHistoryEvent();

        static ProfilerMarker s_OnGui = new ProfilerMarker("ViewPane.OnGUI");

        protected virtual void OnGUI(int elementIndex)
        {
            using (s_OnGui.Auto())
            {
                try
                {
                    var rect = m_VisualElements[elementIndex].contentRect;
                    if (float.IsNaN(rect.width) || float.IsNaN(rect.height))
                    {
                        rect = new Rect(0, 0, 1, 1);
                    }
                    m_VisualElementsOnGUICalls[elementIndex](rect);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public abstract void OnGUI(Rect r);
        void UI.IViewEventListener.OnRepaint()
        {
            m_EventListener.OnRepaint();
        }

        public abstract void OnClose();
    }
}
