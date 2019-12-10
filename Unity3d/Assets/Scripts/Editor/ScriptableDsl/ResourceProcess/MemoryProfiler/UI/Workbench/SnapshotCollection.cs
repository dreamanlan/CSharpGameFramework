using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using Unity.MemoryProfilerForExtension.Editor;
#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
#else
using UnityEngine.Experimental.UIElements;
#endif
using UnityEditorInternal;

namespace Unity.MemoryProfilerForExtension.Editor
{
    internal class SnapshotCollectionEnumerator : IEnumerator<SnapshotFileData>
    {
        int m_Index;
        List<SnapshotFileData> m_Files;

        public SnapshotFileData Current { get { return m_Files[m_Index]; } }
        object IEnumerator.Current { get { return Current; } }
        public int Count { get { return m_Files.Count; } }

        internal SnapshotCollectionEnumerator(List<SnapshotFileData> files)
        {
            m_Files = files;
            Reset();
        }

        public void Dispose()
        {
            m_Files = null;
        }

        public bool MoveNext()
        {
            ++m_Index;

            return m_Index < m_Files.Count;
        }

        public void Reset()
        {
            m_Index = -1;
        }
    }

    internal enum ImportMode
    {
        Copy,
        Move
    }

    internal class SnapshotCollection
    {
        DirectoryInfo m_Info;
        List<SnapshotFileData> m_Snapshots;
        public Action collectionRefresh;

        public string Name { get { return m_Info.Name; } }

        public SnapshotCollection(string collectionPath)
        {
            m_Info = new DirectoryInfo(collectionPath);
            if (!m_Info.Exists)
            {
                m_Info = Directory.CreateDirectory(collectionPath);
                if (!m_Info.Exists)
                    throw new UnityException("Failed to create directory, with provided preferences path: " + collectionPath);
            }

            RefreshFileListInternal(m_Info);
        }

        internal void RefreshScreenshots()
        {
            foreach (var snapshot in m_Snapshots)
            {
                snapshot.RefreshScreenshot();
            }
        }

        void RefreshFileListInternal(DirectoryInfo info)
        {
            m_Snapshots = new List<SnapshotFileData>();
            var fileEnumerator = info.GetFiles('*' + MemoryProfilerWindow.k_SnapshotFileExtension, SearchOption.AllDirectories);
            for (int i = 0; i < fileEnumerator.Length; ++i)
            {
                FileInfo fInfo = fileEnumerator[i];
                if (fInfo.Length != 0)
                {
                    try
                    {
                        m_Snapshots.Add(new SnapshotFileData(fInfo));
                    }
                    catch (IOException e)
                    {
                        Debug.LogError("Failed to load snapshot, error: " + e.Message);
                    }
                }
            }
        }

        public void RenameSnapshot(SnapshotFileData snapshot, string name)
        {
            int nameStart = snapshot.FileInfo.FullName.LastIndexOf(snapshot.FileInfo.Name);
            string targetPath = snapshot.FileInfo.FullName.Substring(0, nameStart) + name + MemoryProfilerWindow.k_SnapshotFileExtension;

            if (targetPath == snapshot.FileInfo.FullName)
            {
                snapshot.GuiData.dynamicVisualElements.snapshotNameLabel.text = snapshot.GuiData.name.text;
                snapshot.GuiData.RenamingFieldVisible = false;
                return;
            }

            snapshot.GuiData.name = new GUIContent(name);
            snapshot.GuiData.dynamicVisualElements.snapshotNameLabel.text = name;
            snapshot.GuiData.RenamingFieldVisible = false;

#if UNITY_2019_3_OR_NEWER
            if (snapshot.GuiData.texture != null)
            {
                string possibleSSPath = Path.ChangeExtension(snapshot.FileInfo.FullName, ".png");
                if (File.Exists(possibleSSPath))
                {
                    File.Move(possibleSSPath, Path.ChangeExtension(targetPath, ".png"));
                }
            }
#endif
            //move snapshot after screenshot
            snapshot.FileInfo.MoveTo(targetPath);
            m_Info.Refresh();
        }

        public void RemoveSnapshotFromCollection(SnapshotFileData snapshot)
        {
            snapshot.FileInfo.Delete();
            m_Snapshots.Remove(snapshot);
#if UNITY_2019_3_OR_NEWER
            string possibleSSPath = Path.ChangeExtension(snapshot.FileInfo.FullName, ".png");
            if (File.Exists(possibleSSPath))
            {
                File.Delete(possibleSSPath);
            }
#endif
            m_Info.Refresh();
        }

        public void RemoveSnapshotFromCollection(SnapshotCollectionEnumerator iter)
        {
            RemoveSnapshotFromCollection(iter.Current);
        }

        public SnapshotFileData AddSnapshotToCollection(string path, ImportMode mode = ImportMode.Copy)
        {
            FileInfo file = new FileInfo(path);
            if (file.FullName.StartsWith(m_Info.FullName))
            {
                if (m_Snapshots.Find(item => item.FileInfo == file) == null)
                {
                    m_Snapshots.Add(new SnapshotFileData(file));
                    m_Info.Refresh();
                    return m_Snapshots[m_Snapshots.Count - 1];
                }
            }
            else
            {
                string newPath = m_Info.FullName + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(file.Name) + "-import-" + DateTime.Now.Ticks + MemoryProfilerWindow.k_SnapshotFileExtension;
                switch (mode)
                {
                    case ImportMode.Copy:
                        file = file.CopyTo(newPath);
                        break;
                    case ImportMode.Move:
                        file.MoveTo(newPath);
                        break;
                }
                m_Snapshots.Add(new SnapshotFileData(file));
                m_Info.Refresh();
                return m_Snapshots[m_Snapshots.Count - 1];
            }
            return null;
        }

        public void RefreshCollection()
        {
            DirectoryInfo rootDir = new DirectoryInfo(m_Info.FullName);
            if (rootDir.LastWriteTime != m_Info.LastWriteTime)
            {
                m_Info = new DirectoryInfo(m_Info.FullName);
                RefreshFileListInternal(m_Info);

                if (collectionRefresh != null)
                {
                    collectionRefresh();
                }
            }
        }

        public SnapshotCollectionEnumerator GetEnumerator()
        {
            return new SnapshotCollectionEnumerator(m_Snapshots);
        }
    }
}
