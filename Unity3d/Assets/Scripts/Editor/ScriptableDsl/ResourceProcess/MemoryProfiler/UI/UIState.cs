using System;
using Unity.MemoryProfilerForExtension.Editor.Database;
using Unity.MemoryProfilerForExtension.Editor.EnumerationUtilities;
using Unity.MemoryProfilerForExtension.Editor.Format;
using Unity.Profiling;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

namespace Unity.MemoryProfilerForExtension.Editor.UI
{
    internal class FormattingOptions
    {
        public Editor.ObjectDataFormatter ObjectDataFormatter;
        System.Collections.Generic.Dictionary<string, IDataFormatter> m_DataFormatters = new System.Collections.Generic.Dictionary<string, IDataFormatter>();

        public void AddFormatter(string name, IDataFormatter formatter)
        {
            m_DataFormatters.Add(name, formatter);
        }

        public IDataFormatter GetFormatter(string name)
        {
            if (String.IsNullOrEmpty(name)) return GetDefaultFormatter();

            IDataFormatter formatter;
            if (m_DataFormatters.TryGetValue(name, out formatter))
            {
                return formatter;
            }
            return DefaultDataFormatter.Instance;
        }

        public IDataFormatter GetDefaultFormatter()
        {
            return DefaultDataFormatter.Instance;
        }
    }

    /// <summary>
    /// Holds the current state of the UI such as:
    ///     current mode (snapshot / diff)
    ///     current panel (treemap / memory map / table)
    ///     current display options
    ///     history of passed actions
    /// </summary>
    internal class UIState
    {
        internal abstract class BaseMode
        {
            public string[] TableNames
            {
                get
                {
                    return m_TableNames;
                }
            }

            protected string[] m_TableNames = { "none" };
            Database.Table[] m_Tables = { null };

            public event Action<ViewPane> ViewPaneChanged = delegate {};

            public ViewPane CurrentViewPane { get; private set; }

            public BaseMode() {}
            protected BaseMode(BaseMode copy)
            {
                m_TableNames = copy.m_TableNames;
                m_Tables = copy.m_Tables;
            }

            public abstract ViewPane GetDefaultView(UIState uiState, IViewPaneEventListener viewPaneEventListener);

            public int GetTableIndex(Database.Table tab)
            {
                int index = Array.FindIndex(m_Tables, x => x == tab);
                return index;
            }

            public abstract Database.Table GetTableByIndex(int index);
            public abstract void UpdateTableSelectionNames();

            public virtual HistoryEvent GetCurrentHistoryEvent()
            {
                if (CurrentViewPane == null) return null;
                return CurrentViewPane.GetCurrentHistoryEvent();
            }

            public void TransitPane(ViewPane newPane)
            {
                if (CurrentViewPane != newPane && CurrentViewPane != null)
                {
                    CurrentViewPane.OnClose();
                }
                CurrentViewPane = newPane;
                ViewPaneChanged(newPane);
            }

            public abstract Database.Schema GetSchema();
            protected void UpdateTableSelectionNamesFromSchema(ObjectDataFormatter objectDataFormatter, Database.Schema schema)
            {
                if (schema == null)
                {
                    m_TableNames = new string[1];
                    m_TableNames[0] = "none";
                    m_Tables = new Database.Table[1];
                    m_Tables[0] = null;
                    return;
                }
                m_TableNames = new string[schema.GetTableCount() + 1];
                m_Tables = new Database.Table[schema.GetTableCount() + 1];
                m_TableNames[0] = "none";
                m_Tables[0] = null;
                for (long i = 0; i != schema.GetTableCount(); ++i)
                {
                    var tab = schema.GetTableByIndex(i);
                    long rowCount = tab.GetRowCount();
                    m_TableNames[i + 1] = (objectDataFormatter.ShowPrettyNames ? tab.GetDisplayName() : tab.GetName()) + " (" + (rowCount >= 0 ? rowCount.ToString() : "?") + ")";
                    m_Tables[i + 1] = tab;
                }
            }

            public abstract void Clear();

            // return null if build failed
            public abstract BaseMode BuildViewSchemaClone(Database.View.ViewSchema.Builder builder);
        }

        internal class SnapshotMode : BaseMode
        {
            QueriedMemorySnapshot m_RawSnapshot;
            RawSchema m_RawSchema;

            public RawSchema RawSchema
            {
                get
                {
                    return m_RawSchema;
                }
            }

            public Database.View.ViewSchema ViewSchema;
            public Database.Schema SchemaToDisplay;
            public CachedSnapshot snapshot
            {
                get
                {
                    if (m_RawSchema == null)
                        return null;
                    return m_RawSchema.m_Snapshot;
                }
            }
            protected SnapshotMode(SnapshotMode copy)
                : base(copy)
            {
                m_RawSnapshot = copy.m_RawSnapshot;
                m_RawSchema = copy.m_RawSchema;
                ViewSchema = copy.ViewSchema;
                SchemaToDisplay = copy.SchemaToDisplay;
                m_RawSchema.formatter.BaseFormatter.PrettyNamesOptionChanged += UpdateTableSelectionNames;
            }

            public SnapshotMode(ObjectDataFormatter objectDataFormatter, QueriedMemorySnapshot snapshot)
            {
                objectDataFormatter.PrettyNamesOptionChanged += UpdateTableSelectionNames;
                SetSnapshot(objectDataFormatter, snapshot);
            }

            public override Database.Schema GetSchema()
            {
                return SchemaToDisplay;
            }

            static ProfilerMarker s_CrawlManagedData = new ProfilerMarker("CrawlManagedData");

            void SetSnapshot(ObjectDataFormatter objectDataFormatter, QueriedMemorySnapshot snapshot)
            {
                if (snapshot == null)
                {
                    m_RawSnapshot = null;
                    m_RawSchema = null;
                    SchemaToDisplay = null;
                    UpdateTableSelectionNames();
                    return;
                }

                m_RawSnapshot = snapshot;

                ProgressBarDisplay.ShowBar(string.Format("Opening snapshot: {0}", System.IO.Path.GetFileNameWithoutExtension(snapshot.GetReader().FilePath)));

                var cachedSnapshot = new CachedSnapshot(snapshot);
                using (s_CrawlManagedData.Auto())
                {
                    var crawling = Crawler.Crawl(cachedSnapshot);
                    crawling.MoveNext(); //start execution

                    var status = crawling.Current as EnumerationStatus;
                    float progressPerStep = 1.0f / status.StepCount;
                    while (crawling.MoveNext())
                    {
                        ProgressBarDisplay.UpdateProgress(status.CurrentStep * progressPerStep, status.StepStatus);
                    }
                }
                ProgressBarDisplay.ClearBar();

                m_RawSchema = new RawSchema();
                m_RawSchema.SetupSchema(cachedSnapshot, objectDataFormatter);

                SchemaToDisplay = m_RawSchema;
                if (k_DefaultViewFilePath.Length > 0)
                {
                    Database.View.ViewSchema.Builder builder = null;
                    builder = Database.View.ViewSchema.Builder.LoadFromXMLFile(k_DefaultViewFilePath);

                    if (builder != null)
                    {
                        ViewSchema = builder.Build(m_RawSchema);
                        if (ViewSchema != null)
                        {
                            SchemaToDisplay = ViewSchema;
                        }
                    }
                }

                UpdateTableSelectionNames();
            }

            public override Database.Table GetTableByIndex(int index)
            {
                return SchemaToDisplay.GetTableByIndex(index);
            }

            public override void UpdateTableSelectionNames()
            {
                if (m_RawSchema != null)
                {
                    UpdateTableSelectionNamesFromSchema(m_RawSchema.formatter.BaseFormatter, SchemaToDisplay);
                }
            }

            public override ViewPane GetDefaultView(UIState uiState, IViewPaneEventListener viewPaneEventListener)
            {
                if (uiState.snapshotMode != null && uiState.snapshotMode.snapshot != null)
                    return new UI.TreeMapPane(uiState, viewPaneEventListener);
                else
                    return null;
            }

            public override void Clear()
            {
                CurrentViewPane.OnClose();
                SchemaToDisplay = null;
                m_RawSchema.Clear();
                m_RawSchema = null;
                if (m_RawSnapshot != null)
                    m_RawSnapshot.Dispose();
            }

            public override BaseMode BuildViewSchemaClone(Database.View.ViewSchema.Builder builder)
            {
                Database.View.ViewSchema vs;
                vs = builder.Build(m_RawSchema);
                if (vs != null)
                {
                    SnapshotMode copy = new SnapshotMode(this);
                    copy.ViewSchema = vs;
                    copy.SchemaToDisplay = vs;
                    copy.UpdateTableSelectionNames();
                    return copy;
                }
                return null;
            }
        }
        internal class DiffMode : BaseMode
        {
            public BaseMode modeFirst;
            public BaseMode modeSecond;
            Database.Schema m_SchemaFirst;
            Database.Schema m_SchemaSecond;
            Database.Operation.DiffSchema m_SchemaDiff;
            ObjectDataFormatter m_ObjectDataFormatter;

            private const string k_DefaultDiffViewTable = "All Object";

            public DiffMode(ObjectDataFormatter objectDataFormatter, BaseMode snapshotFirst, BaseMode snapshotSecond)
            {
                ProgressBarDisplay.ShowBar("Snapshot diff in progress");
                m_ObjectDataFormatter = objectDataFormatter;
                m_ObjectDataFormatter.PrettyNamesOptionChanged += UpdateTableSelectionNames;
                modeFirst = snapshotFirst;
                modeSecond = snapshotSecond;
                m_SchemaFirst = modeFirst.GetSchema();
                m_SchemaSecond = modeSecond.GetSchema();
                ProgressBarDisplay.UpdateProgress(0.1f, "Building diff schema.");
                m_SchemaDiff = new Database.Operation.DiffSchema(m_SchemaFirst, m_SchemaSecond, () => { ProgressBarDisplay.UpdateProgress(0.3f, "Computing table data"); });
                ProgressBarDisplay.UpdateProgress(0.85f, "Updating table selection.");
                UpdateTableSelectionNames();
                ProgressBarDisplay.ClearBar();
            }

            protected DiffMode(DiffMode copy)
            {
                m_ObjectDataFormatter = copy.m_ObjectDataFormatter;
                m_ObjectDataFormatter.PrettyNamesOptionChanged += UpdateTableSelectionNames;
                modeFirst = copy.modeFirst;
                modeSecond = copy.modeSecond;
                m_SchemaFirst = copy.m_SchemaFirst;
                m_SchemaSecond = copy.m_SchemaSecond;
                m_SchemaDiff = copy.m_SchemaDiff;
            }

            public override Database.Schema GetSchema()
            {
                return m_SchemaDiff;
            }

            public override Database.Table GetTableByIndex(int index)
            {
                return m_SchemaDiff.GetTableByIndex(index);
            }

            public override void UpdateTableSelectionNames()
            {
                UpdateTableSelectionNamesFromSchema(m_ObjectDataFormatter, m_SchemaDiff);
            }

            public override ViewPane GetDefaultView(UIState uiState, IViewPaneEventListener viewPaneEventListener)
            {
                //TODO: delete this method once the default for diff is treemap
                Database.Table table = null;
                for (int i = 1; i < uiState.CurrentMode.TableNames.Length; i++)
                {
                    if (uiState.CurrentMode.TableNames[i].Contains(k_DefaultDiffViewTable))
                    {
                        table = uiState.CurrentMode.GetTableByIndex(i - 1);
                    }
                }
                if (table == null)
                    table = uiState.CurrentMode.GetTableByIndex(Mathf.Min(0, m_TableNames.Length - 1));

                if (table.Update())
                {
                    UpdateTableSelectionNames();
                }

                var pane = new UI.SpreadsheetPane(uiState, viewPaneEventListener);
                pane.OpenTable(new Database.TableReference(table.GetName()), table);
                return pane;
            }

            public override void Clear()
            {
                modeFirst.Clear();
                modeSecond.Clear();
            }

            public override BaseMode BuildViewSchemaClone(Database.View.ViewSchema.Builder builder)
            {
                var newModeFirst = modeFirst.BuildViewSchemaClone(builder);
                if (newModeFirst == null) return null;
                var newModeSecond = modeSecond.BuildViewSchemaClone(builder);
                if (newModeSecond == null) return null;

                DiffMode copy = new DiffMode(this);
                copy.modeFirst = newModeFirst;
                copy.modeSecond = newModeSecond;
                copy.m_SchemaFirst = copy.modeFirst.GetSchema();
                copy.m_SchemaSecond = copy.modeSecond.GetSchema();
                copy.m_SchemaDiff = new Database.Operation.DiffSchema(copy.m_SchemaFirst, copy.m_SchemaSecond);
                copy.UpdateTableSelectionNames();

                return copy;
            }
        }

        const string k_DefaultViewFilePath = "Assets/Editor Default Resources/MemoryProfiler_PackageResources/MemView.xml";

        public History history = new History();

        public event Action<BaseMode, ViewMode> ModeChanged = delegate {};

        public BaseMode CurrentMode
        {
            get
            {
                switch (m_CurrentViewMode)
                {
                    case ViewMode.ShowNone:
                        return noMode;
                    case ViewMode.ShowFirst:
                        return FirstMode;
                    case ViewMode.ShowSecond:
                        return SecondMode;
                    case ViewMode.ShowDiff:
                        return diffMode;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public BaseMode FirstMode { get; private set; }
        public BaseMode SecondMode { get; private set; }

        public enum ViewMode
        {
            ShowNone = -1,
            ShowDiff,
            ShowFirst,
            ShowSecond,
        }
        ViewMode m_CurrentViewMode = ViewMode.ShowNone;
        public ViewMode CurrentViewMode
        {
            get
            {
                return m_CurrentViewMode;
            }
            set
            {
                if (m_CurrentViewMode != value)
                {
                    m_CurrentViewMode = value;
                    ModeChanged(CurrentMode, value);
                }
            }
        }

        public SnapshotMode snapshotMode { get { return CurrentMode as SnapshotMode; } }
        public DiffMode diffMode;

        public SnapshotMode noMode;

        public readonly DefaultHotKey HotKey = new DefaultHotKey();
        public readonly FormattingOptions FormattingOptions;

        public UIState()
        {
            FormattingOptions = new FormattingOptions();
            FormattingOptions.ObjectDataFormatter = new ObjectDataFormatter();
            var sizeDataFormatter = new Database.SizeDataFormatter();
            FormattingOptions.AddFormatter("size", sizeDataFormatter);
            // TODO add a format named "integer" that output in base 16,10,8,2
            //FormattingOptions.AddFormatter("integer", PointerFormatter);
            // TODO add a format named "pointer" that output in hex
            //FormattingOptions.AddFormatter("pointer", PointerFormatter);

            noMode = new SnapshotMode(FormattingOptions.ObjectDataFormatter, null);
        }

        public void AddHistoryEvent(HistoryEvent he)
        {
            if (he != null)
            {
                history.AddEvent(he);
            }
        }

        public void ClearDiffMode()
        {
            diffMode = null;
            if (CurrentViewMode == ViewMode.ShowDiff)
            {
                if (FirstMode != null)
                    CurrentViewMode = ViewMode.ShowFirst;
                else if (SecondMode != null)
                    CurrentViewMode = ViewMode.ShowSecond;
                else
                    CurrentViewMode = ViewMode.ShowNone;
            }
        }

        public void ClearAllOpenModes()
        {
            if (SecondMode != null)
                SecondMode.Clear();
            SecondMode = null;
            if (FirstMode != null)
                FirstMode.Clear();
            FirstMode = null;
            CurrentViewMode = ViewMode.ShowNone;
            diffMode = null;
            history.Clear();
        }

        public void ClearFirstMode()
        {
            if (FirstMode != null)
                FirstMode.Clear();
            FirstMode = null;

            if (diffMode != null)
            {
                ClearDiffMode();
            }

            if (CurrentViewMode == ViewMode.ShowFirst)
            {
                history.Clear();
                if (SecondMode != null)
                    CurrentViewMode = ViewMode.ShowSecond;
                else
                    CurrentViewMode = ViewMode.ShowNone;
            }
        }

        public void ClearSecondMode()
        {
            if (SecondMode != null)
                SecondMode.Clear();
            SecondMode = null;

            if (diffMode != null)
            {
                ClearDiffMode();
            }

            if (CurrentViewMode == ViewMode.ShowSecond)
            {
                history.Clear();
                if (FirstMode != null)
                    CurrentViewMode = ViewMode.ShowFirst;
                else
                    CurrentViewMode = ViewMode.ShowNone;
            }
        }

        public void SetFirstSnapshot(QueriedMemorySnapshot snapshot)
        {
            if (snapshot == null)
            {
                Debug.LogError("UIState.SetFirstSnapshot can't be called with null, if you meant to clear the open snapshots, call ClearAllOpenSnapshots");
                return;
            }
            history.Clear();
            if (FirstMode != null)
            {
                if (SecondMode != null)
                    SecondMode.Clear();
                SecondMode = FirstMode;
            }
            FirstMode = new SnapshotMode(FormattingOptions.ObjectDataFormatter, snapshot);

            // Make sure that the first mode is shown and that ModeChanged (fired by ShownMode if set to something different) is fired.
            if (CurrentViewMode != ViewMode.ShowFirst)
                CurrentViewMode = ViewMode.ShowFirst;
            else
                ModeChanged(CurrentMode, CurrentViewMode);
            ClearDiffMode();
        }

        public void SwapLastAndCurrentSnapshot()
        {
            // TODO: find out if we actually need to clear this or if it can be saved with the mode
            history.Clear();
            var temp = SecondMode;
            SecondMode = FirstMode;
            FirstMode = temp;
            if (CurrentViewMode != ViewMode.ShowDiff)
            {
                CurrentViewMode = CurrentViewMode == ViewMode.ShowFirst ? ViewMode.ShowSecond : ViewMode.ShowFirst;
                ModeChanged(CurrentMode, CurrentViewMode);
            }
        }

        public void DiffLastAndCurrentSnapshot(bool firstIsOlder)
        {
            history.Clear();
            diffMode = new DiffMode(FormattingOptions.ObjectDataFormatter, firstIsOlder ? FirstMode : SecondMode , firstIsOlder ? SecondMode : FirstMode);
            CurrentViewMode = ViewMode.ShowDiff;
        }

        public bool LoadView(string filename)
        {
            if (CurrentViewMode == ViewMode.ShowNone)
            {
                Debug.LogWarning("Must open a snapshot before loading a view file");
                MemoryProfilerAnalytics.AddMetaDatatoEvent<MemoryProfilerAnalytics.LoadViewXMLEvent>(1);
                return false;
            }

            if (String.IsNullOrEmpty(filename)) return false;

            var builder = Database.View.ViewSchema.Builder.LoadFromXMLFile(filename);
            if (builder == null) return false;

            BaseMode newMode = CurrentMode.BuildViewSchemaClone(builder);
            if (newMode == null) return false;

            switch (CurrentViewMode)
            {
                case ViewMode.ShowFirst:
                    FirstMode = newMode;
                    break;
                case ViewMode.ShowSecond:
                    SecondMode = newMode;
                    break;
                case ViewMode.ShowDiff:
                    diffMode = newMode as DiffMode;
                    FirstMode = diffMode.modeFirst;
                    SecondMode = diffMode.modeSecond;
                    break;
                default:
                    break;
            }
            history.Clear();
            ModeChanged(CurrentMode, CurrentViewMode);
            return true;
        }

        public void TransitModeToOwningTable(Table table)
        {
            if (diffMode != null)
            {
                //open the appropriate snapshot mode, the one the table is from.
                if (diffMode.modeFirst.GetSchema().OwnsTable(table))
                {
                    TransitMode(diffMode.modeFirst);
                }
                else if (diffMode.modeSecond.GetSchema().OwnsTable(table))
                {
                    TransitMode(diffMode.modeSecond);
                }
                else if (diffMode.GetSchema().OwnsTable(table))
                {
                    TransitMode(diffMode);
                }
            }
        }

        public void TransitMode(UIState.BaseMode newMode)
        {
            if (newMode == diffMode)
            {
                CurrentViewMode = ViewMode.ShowDiff;
            }
            else if (newMode == FirstMode)
            {
                CurrentViewMode = ViewMode.ShowFirst;
            }
            else if (newMode == SecondMode)
            {
                CurrentViewMode = ViewMode.ShowSecond;
            }
            else
            {
                FirstMode = newMode;
                CurrentViewMode = ViewMode.ShowFirst;
                ModeChanged(newMode, CurrentViewMode);
            }
        }
    }
}
