using UnityEngine;
using System;
using Unity.MemoryProfilerForExtension.Editor.UI;
using UnityEditor.Profiling.Memory.Experimental;

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal class OpenSnapshotsManager
    {
        OpenSnapshotsWindow m_OpenSnapshotsPane;

        public enum OpenSnapshotSlot
        {
            First,
            Second,
        }

        [NonSerialized]
        SnapshotFileData First;
        [NonSerialized]
        SnapshotFileData Second;

        UIState m_UIState;

        private UI.ViewPane currentViewPane
        {
            get
            {
                if (m_UIState.CurrentMode == null) return null;
                return m_UIState.CurrentMode.CurrentViewPane;
            }
        }

        public void RegisterUIState(UIState uiState)
        {
            m_UIState = uiState;
            uiState.ModeChanged += OnModeChanged;
            OnModeChanged(uiState.CurrentMode, uiState.CurrentViewMode);
        }

        public OpenSnapshotsWindow InitializeOpenSnapshotsWindow(float initialWidth)
        {
            m_OpenSnapshotsPane = new OpenSnapshotsWindow(initialWidth);

            m_OpenSnapshotsPane.SwapOpenSnapshots += SwapOpenSnapshots;
            m_OpenSnapshotsPane.ShowDiffOfOpenSnapshots += ShowDiffOfOpenSnapshots;
            m_OpenSnapshotsPane.ShowFirstOpenSnapshot += ShowFirstOpenSnapshot;
            m_OpenSnapshotsPane.ShowSecondOpenSnapshot += ShowSecondOpenSnapshot;
            return m_OpenSnapshotsPane;
        }

        public void OpenSnapshot(SnapshotFileData snapshot)
        {
            if (First != null)
            {
                if (Second != null)
                {
                    Second.GuiData.CurrentState = SnapshotFileGUIData.State.Closed;
                    UIElementsHelper.SwitchVisibility(Second.GuiData.dynamicVisualElements.openButton, Second.GuiData.dynamicVisualElements.closeButton);
                }
                Second = First;


                m_OpenSnapshotsPane.SetSnapshotUIData(false, Second.GuiData, false);
                Second.GuiData.CurrentState = SnapshotFileGUIData.State.Open;
            }

            First = snapshot;

            m_OpenSnapshotsPane.SetSnapshotUIData(true, snapshot.GuiData, true);
            First.GuiData.CurrentState = SnapshotFileGUIData.State.InView;

            var loadedPackedSnapshot = snapshot.LoadSnapshot();
            if (loadedPackedSnapshot != null)
            {
                m_UIState.SetFirstSnapshot(loadedPackedSnapshot);
            }
        }

        public bool IsSnapshotOpen(SnapshotFileData snapshot)
        {
            return snapshot == First || snapshot == Second;
        }

        public void CloseCapture(SnapshotFileData snapshot)
        {
            if (snapshot == null)
                return;
            try
            {
                if (Second != null)
                {
                    if (snapshot == Second)
                    {
                        m_UIState.ClearSecondMode();
                        Second.GuiData.CurrentState = SnapshotFileGUIData.State.Closed;
                    }
                    else if (snapshot == First)
                    {
                        m_UIState.ClearFirstMode();
                        if (First != null)
                            First.GuiData.CurrentState = SnapshotFileGUIData.State.Closed;
                        First = Second;
                        m_UIState.SwapLastAndCurrentSnapshot();
                    }
                    else
                    {
                        // The snapshot wasn't open, there is nothing left todo here.
                        return;
                    }
                    UIElementsHelper.SwitchVisibility(snapshot.GuiData.dynamicVisualElements.openButton, snapshot.GuiData.dynamicVisualElements.closeButton);
                    Second = null;
                    m_UIState.CurrentViewMode = UIState.ViewMode.ShowFirst;

                    if (First != null)
                        m_OpenSnapshotsPane.SetSnapshotUIData(true, First.GuiData, true);
                    else
                        m_OpenSnapshotsPane.SetSnapshotUIData(true, null, true);
                    m_OpenSnapshotsPane.SetSnapshotUIData(false, null, false);
                    // With two snapshots open, there could also be a diff to be closed/cleared.
                    m_UIState.ClearDiffMode();
                }
                else
                {
                    if (snapshot == First)
                    {
                        First.GuiData.CurrentState = SnapshotFileGUIData.State.Closed;
                        First = null;
                        m_UIState.ClearAllOpenModes();
                    }
                    else if (snapshot == Second)
                    {
                        Second.GuiData.CurrentState = SnapshotFileGUIData.State.Closed;
                        Second = null;
                        m_UIState.ClearAllOpenModes();
                    }
                    else
                    {
                        // The snapshot wasn't open, there is nothing left todo here.
                        return;
                    }
                    m_OpenSnapshotsPane.SetSnapshotUIData(true, null, false);
                    m_OpenSnapshotsPane.SetSnapshotUIData(false, null, false);
                }
                UIElementsHelper.SwitchVisibility(snapshot.GuiData.dynamicVisualElements.openButton, snapshot.GuiData.dynamicVisualElements.closeButton);
            }
            catch (Exception)
            {
                throw;
            }
        }

        void SetSnapshot(PackedMemorySnapshot snapshot)
        {
            //UpdateSnapshotCollectionUI();
        }

        //void UpdateSnapshotCollectionUI()
        //{
        //    bool diffmode = Second != null;
        //    bool oneSnapshotIsLoaded = !diffmode && First != null;
        //    using (var enumerator = m_MemorySnapshotsCollection.GetEnumerator())
        //    {
        //        while (enumerator.MoveNext())
        //        {
        //            bool isFirst = enumerator.Current == First;
        //            bool isSecond = enumerator.Current == Second;
        //            bool currentSnapshotIsLoaded = isFirst || isSecond;

        //            UIElementsHelper.SwitchVisibility(enumerator.Current.GuiData.dynamicVisualElements.closeButton, enumerator.Current.GuiData.dynamicVisualElements.openButton, currentSnapshotIsLoaded);

        //            bool enableDiff = oneSnapshotIsLoaded && !currentSnapshotIsLoaded;

        //            bool currentSnapshotIsInDiffMode = diffmode && currentSnapshotIsLoaded;
        //        }
        //    }
        //}

        public void CloseAllOpenSnapshots()
        {
            if (Second != null)
            {
                CloseCapture(Second);
                Second = null;
            }
            if (First != null)
            {
                CloseCapture(First);
                First = null;
            }
        }

        void OnModeChanged(UIState.BaseMode newMode, UIState.ViewMode newViewMode)
        {
            switch (newViewMode)
            {
                case UIState.ViewMode.ShowDiff:
                    if (m_OpenSnapshotsPane != null)
                        m_OpenSnapshotsPane.FocusDiff();

                    if (First != null)
                        First.GuiData.CurrentState = SnapshotFileGUIData.State.Open;
                    if (Second != null)
                        Second.GuiData.CurrentState = SnapshotFileGUIData.State.Open;
                    break;
                case UIState.ViewMode.ShowFirst:
                    if (m_OpenSnapshotsPane != null)
                        m_OpenSnapshotsPane.FocusFirst();

                    if (First != null)
                        First.GuiData.CurrentState = SnapshotFileGUIData.State.InView;
                    if (Second != null)
                        Second.GuiData.CurrentState = SnapshotFileGUIData.State.Open;
                    break;
                case UIState.ViewMode.ShowSecond:
                    if (m_OpenSnapshotsPane != null)
                        m_OpenSnapshotsPane.FocusSecond();

                    if (First != null)
                        First.GuiData.CurrentState = SnapshotFileGUIData.State.Open;
                    if (Second != null)
                        Second.GuiData.CurrentState = SnapshotFileGUIData.State.InView;
                    break;
                default:
                    break;
            }
        }

        void SwapOpenSnapshots()
        {
            var temp = Second;
            Second = First;
            First = temp;

            m_UIState.SwapLastAndCurrentSnapshot();

            if (First != null)
                m_OpenSnapshotsPane.SetSnapshotUIData(true, First.GuiData, m_UIState.CurrentViewMode == UIState.ViewMode.ShowFirst);
            else
                m_OpenSnapshotsPane.SetSnapshotUIData(true, null, false);

            if (Second != null)
                m_OpenSnapshotsPane.SetSnapshotUIData(false, Second.GuiData, m_UIState.CurrentViewMode == UIState.ViewMode.ShowSecond);
            else
                m_OpenSnapshotsPane.SetSnapshotUIData(false, null, false);
        }

        void ShowDiffOfOpenSnapshots()
        {
            if (m_UIState.diffMode != null)
            {
                SwitchSnapshotMode(UIState.ViewMode.ShowDiff);
            }
            else if (First != null && Second != null)
            {
                try
                {
                    MemoryProfilerAnalytics.StartEvent<MemoryProfilerAnalytics.DiffedSnapshotEvent>();

                    m_UIState.DiffLastAndCurrentSnapshot(First.GuiData.UtcDateTime.CompareTo(Second.GuiData.UtcDateTime) < 0);

                    MemoryProfilerAnalytics.EndEvent(new MemoryProfilerAnalytics.DiffedSnapshotEvent());
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                Debug.LogError("No second snapshot opened to diff to!");
            }
        }

        void ShowFirstOpenSnapshot()
        {
            if (First != null)
            {
                SwitchSnapshotMode(UIState.ViewMode.ShowFirst);
            }
        }

        void ShowSecondOpenSnapshot()
        {
            if (Second != null)
            {
                SwitchSnapshotMode(UIState.ViewMode.ShowSecond);
            }
        }

        void SwitchSnapshotMode(UIState.ViewMode mode)
        {
            if (m_UIState.CurrentViewMode == mode)
                return;

            var currentViewName = "Unknown";
            if (currentViewPane is UI.TreeMapPane)
            {
                currentViewName = "TreeMap";
            }
            else if (currentViewPane is UI.MemoryMapPane)
            {
                currentViewName = "MemoryMap";
            }
            else if (currentViewPane is UI.SpreadsheetPane)
            {
                currentViewName = (currentViewPane as UI.SpreadsheetPane).TableDisplayName;
            }
            MemoryProfilerAnalytics.StartEvent<MemoryProfilerAnalytics.DiffToggledEvent>();

            var oldMode = m_UIState.CurrentViewMode;

            m_UIState.CurrentViewMode = mode;

            MemoryProfilerAnalytics.EndEvent(new MemoryProfilerAnalytics.DiffToggledEvent()
            {
                show = (int)ConvertUIModeToAnalyticsDiffToggleEventData(m_UIState.CurrentViewMode),
                shown = (int)ConvertUIModeToAnalyticsDiffToggleEventData(oldMode),
                viewName = currentViewName
            });
        }

        void BackToSnapshotDiffView()
        {
            m_UIState.CurrentViewMode = UIState.ViewMode.ShowDiff;
        }

        MemoryProfilerAnalytics.DiffToggledEvent.ShowSnapshot ConvertUIModeToAnalyticsDiffToggleEventData(UIState.ViewMode mode)
        {
            switch (mode)
            {
                case UIState.ViewMode.ShowDiff:
                    return MemoryProfilerAnalytics.DiffToggledEvent.ShowSnapshot.Both;
                case UIState.ViewMode.ShowFirst:
                    return MemoryProfilerAnalytics.DiffToggledEvent.ShowSnapshot.First;
                case UIState.ViewMode.ShowSecond:
                    return MemoryProfilerAnalytics.DiffToggledEvent.ShowSnapshot.Second;
                default:
                    throw new NotImplementedException();
            }
        }

        internal void RefreshScreenshots()
        {
            SnapshotFileGUIData firstGUIData = null, secondGUIData = null;
            if (First != null)
            {
                First.RefreshScreenshot();
                firstGUIData = First.GuiData;
            }
            if (Second != null)
            {
                Second.RefreshScreenshot();
                secondGUIData = Second.GuiData;
            }
            m_OpenSnapshotsPane.RefreshScreenshots(firstGUIData, secondGUIData);
        }
    }
}
