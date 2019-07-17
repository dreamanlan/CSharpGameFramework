using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MemoryProfilerWindowForExtension;
using UnityEditor.MemoryProfiler;
using System;
using System.Linq;

namespace UnityEditor.MemoryProfiler2
{
    public class ProfilerNode : ScriptableObject
    {
        public Rect windowRect;
        public string nodeTitle;
        public int instanceID = -1;
        public Color linkColor;

        public ProfilerNodeObjectInfo myInfo;
        private PrimitiveValueReader _primitiveValueReader;
        private CrawledMemorySnapshot _unpackedCrawl;
        GUILayoutOption labelWidth = GUILayout.Width(150);
        public ProfilerNodeView parent;
        public bool bCreateChildNodes = false;
        public int referencesNum = 0;
        public int referencedByNum = 0;
        public ProfilerNode prevNode;
        private Rect input1Rect;
        private Texture2D _textureObject;
        private int _prevInstance;
        private float _textureSize = 128.0f;
        private Vector2 scrollViewVector = Vector2.zero;
        private ThingInMemory[] _shortestPath;
        private ShortestPathToRootFinder _shortestPathToRootFinder;
        Dictionary<ulong, ThingInMemory> objectCache = new Dictionary<ulong, ThingInMemory>();

        public float originalPosX;
        public float originalPosY;

        public float offsetPosX;
        public float offsetPosY;
        public GUIStyle nodeStyle;
        public bool bIsSelected = false;

        public void SetPannedRect(float panX,float panY)
        {
            //windowRect.x = originalPosX + panX;// +offsetPosX;
            //windowRect.y = originalPosY + panY;// +offsetPosY;
            //originalPosX = windowRect.x;
            //originalPosY = windowRect.y;
            windowRect.x += panX;
            windowRect.y += panY;
            originalPosX = windowRect.x;
            originalPosY = windowRect.y;
        }

        static class Styles
        {
            public static GUIStyle entryEven = "OL EntryBackEven";
            public static GUIStyle entryOdd = "OL EntryBackOdd";
        }

        public ProfilerNode(ProfilerNodeObjectInfo info, CrawledMemorySnapshot newUnpackedCrawl, ProfilerNodeView newParent, bool createChildNodes, ProfilerNode newPrevNode)
        {
            if(info == null)
            {
                return;
            }
            myInfo = info;
            if (myInfo.memObject != null)
            {
                nodeTitle = myInfo.memObject.caption;
            }
            _unpackedCrawl = newUnpackedCrawl;
            parent = newParent;
            bCreateChildNodes = createChildNodes;
            prevNode = newPrevNode;

            _shortestPathToRootFinder = new ShortestPathToRootFinder(_unpackedCrawl);
            _primitiveValueReader = new PrimitiveValueReader(_unpackedCrawl.virtualMachineInformation, _unpackedCrawl.managedHeap);
            _shortestPath = _shortestPathToRootFinder.FindFor(info.memObject);
        }

        public void DrawCurves()
        {
            if (prevNode)
            {
                Rect rect = windowRect;
                rect.x += 20;
                rect.y = windowRect.y + windowRect.height / 2;
                rect.width = 1;
                rect.height = 1;
                ProfilerNodeView.DrawNodeCurve(prevNode.windowRect, rect, linkColor);
            }
        }

        public void DrawNode()
        {
            if (nodeStyle == null)
            {
                GUISkin skin = EditorGUIUtility.Load("MemoryProfiler_EditorResources/MemoryProfiler_NodeGUISkin.guiskin") as GUISkin;
                nodeStyle = new GUIStyle(skin.window);
            }
            int size = 18 * (myInfo.memObject.references.Length + myInfo.memObject.referencedBy.Length + 9 + 128);
           
            if (bIsSelected)
            {
                scrollViewVector = GUI.BeginScrollView(new Rect(6, 31, 348, 323), scrollViewVector, new Rect(0, 18, 360, size));
            }
            else
            {
                scrollViewVector = GUI.BeginScrollView(new Rect(6, 31, 348, 323), scrollViewVector, new Rect(0, 18, 360, size),false,false,GUIStyle.none, GUIStyle.none);
                GUILayout.Space(10);
            }
            var nativeObject = myInfo.memObject as NativeUnityEngineObject;
            if (nativeObject != null)
            {
                GUILayout.Label("NativeUnityEngineObject", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Name", nativeObject.name);
                EditorGUILayout.LabelField("ClassName", nativeObject.className);
                EditorGUILayout.LabelField("ClassID", nativeObject.classID.ToString());
                EditorGUILayout.LabelField("instanceID", nativeObject.instanceID.ToString());
                EditorGUILayout.LabelField("isDontDestroyOnLoad", nativeObject.isDontDestroyOnLoad.ToString());
                EditorGUILayout.LabelField("isPersistent", nativeObject.isPersistent.ToString());
                EditorGUILayout.LabelField("isManager", nativeObject.isManager.ToString());
                EditorGUILayout.LabelField("hideFlags", nativeObject.hideFlags.ToString());
                EditorGUILayout.LabelField("Size", nativeObject.size.ToString());
                instanceID = nativeObject.instanceID;
                DrawSpecificTexture2D(nativeObject);
                
            }

            var managedObject = myInfo.memObject as ManagedObject;
            if (managedObject != null)
            {
                GUILayout.Label("ManagedObject");
                EditorGUILayout.LabelField("Type", managedObject.typeDescription.name);
                EditorGUILayout.LabelField("Address", managedObject.address.ToString("X"));
                EditorGUILayout.LabelField("size", managedObject.size.ToString());

                if (managedObject.typeDescription.name == "System.String")
                    EditorGUILayout.LabelField("value", StringTools.ReadString(_unpackedCrawl.managedHeap.Find(managedObject.address, _unpackedCrawl.virtualMachineInformation), _unpackedCrawl.virtualMachineInformation));
                DrawFields(managedObject);

                if (managedObject.typeDescription.isArray)
                {
                    DrawArray(managedObject);
                }
                if (nodeStyle == null)
                {
                    nodeStyle = new GUIStyle("flow node 1");
                }
            }

            if (myInfo.memObject is GCHandle)
            {
                GUILayout.Label("GCHandle");
                EditorGUILayout.LabelField("size", myInfo.memObject.size.ToString());

            }

            var staticFields = myInfo.memObject as StaticFields;
            if (staticFields != null)
            {

                GUILayout.Label("Static Fields");
                GUILayout.Label("Of type: " + staticFields.typeDescription.name);
                GUILayout.Label("size: " + staticFields.size);

                DrawFields(staticFields.typeDescription, new BytesAndOffset() { bytes = staticFields.typeDescription.staticFieldBytes, offset = 0, pointerSize = _unpackedCrawl.virtualMachineInformation.pointerSize }, true);
            }

            if (managedObject == null)
            {
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.Label("References:", labelWidth);
                GUILayout.BeginVertical();
                DrawLinks(myInfo.memObject.references, 1);
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            GUILayout.Label("Referenced by:");
            DrawLinks(myInfo.memObject.referencedBy, 2);


            GUILayout.Space(10);
            if (_shortestPath != null)
            {
                if (_shortestPath.Length > 1)
                {
                    GUILayout.Label("ShortestPathToRoot");
                    DrawLinks(_shortestPath, 0);
                }
                string reason;
                _shortestPathToRootFinder.IsRoot(_shortestPath.Last(), out reason);
                GUILayout.Label("This is a root because:");
                GUILayout.TextArea(reason);
            }
            else
            {
                GUILayout.TextArea("No root is keeping this object alive. It will be collected next UnloadUnusedAssets() or scene load");
            }

                GUI.EndScrollView();
            
        }



        private void DrawLinks(IEnumerable<ThingInMemory> thingInMemories, int bIsReferences)
        {
            var c = GUI.backgroundColor;
            Color elementColor;
            if (bIsReferences == 1)
            {
                elementColor = Color.white;
            }
            else if (bIsReferences == 2)
            {
                elementColor = new Color(0.8039f, 0.8627f, 0.2235f);
            }
            else
            {
                elementColor = new Color(0, 0.8f, 0.8f);
            }
            GUI.skin.button.alignment = TextAnchor.UpperLeft;
            foreach (var thingInMemory in thingInMemories)
            {
                bool disableGroup = thingInMemory == myInfo.memObject || thingInMemory == null;
                if (bCreateChildNodes)
                {

                    if (referencesNum < thingInMemories.Count<ThingInMemory>())
                    { 
                        if (!disableGroup)
                        {
                            CreateChildNodes(thingInMemory, elementColor);
                            referencesNum++;
                        }
                    }
                }

                {
                    
                    EditorGUI.BeginDisabledGroup(disableGroup);

                    GUI.backgroundColor = ColorFor(thingInMemory);

                    var caption = thingInMemory == null ? "null" : thingInMemory.caption;

                    var managedObject = thingInMemory as ManagedObject;
                    if (managedObject != null && managedObject.typeDescription.name == "System.String")
                        caption = StringTools.ReadString(_unpackedCrawl.managedHeap.Find(managedObject.address, _unpackedCrawl.virtualMachineInformation), _unpackedCrawl.virtualMachineInformation);

                    if (GUILayout.Button(caption))
                    {
                        var nativeObject = thingInMemory as NativeUnityEngineObject;
                        if (nativeObject != null)
                        {
                            int nodeId = parent.FindExistingObjectByInstanceID(nativeObject.instanceID);
                            if (nodeId < 0)
                            {
                                if (!disableGroup)
                                {
                                    CreateChildNodes(thingInMemory, elementColor,false);
                                }
                            }
                            else
                            {
                                parent.SetSelectedNode(nodeId);
                            }
                        }
                        var manObject = thingInMemory as ManagedObject;
                        if (manObject != null)
                        {
                            if (!disableGroup)
                            {
                                int nodeId = parent.FindExistingObjectByAddress(manObject.address);
                                if (nodeId < 0)
                                {
                                    CreateChildNodes(thingInMemory, elementColor,false);
                                }
                                else
                                {
                                    parent.SetSelectedNode(nodeId);
                                }
                            }
                        }
                        if (thingInMemory is GCHandle)
                        {
                            if (!disableGroup)
                            {
                                CreateChildNodes(thingInMemory, elementColor,false);
                            }
                        }
                        var staticFields = thingInMemory as StaticFields;
                        if (staticFields != null)
                        {
                            if (!disableGroup)
                            {
                                CreateChildNodes(thingInMemory, elementColor,false);
                            }
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                }
            }
            GUI.backgroundColor = c;
        }

        public void CreateChildNodes(ThingInMemory thingInMemories, Color elementColor, bool bCreateInGrid = true)
        {
            ProfilerNodeObjectInfo info = new ProfilerNodeObjectInfo();
            info.id = 0;
            info.text = thingInMemories.caption;
            info.memObject = thingInMemories;
            ProfilerNode objectInfoNode = new ProfilerNode(info, _unpackedCrawl, parent, false, this);
            objectInfoNode.linkColor = elementColor;
            parent.CreateObjectInfoNode(objectInfoNode,windowRect.x,windowRect.y, bCreateInGrid);
        }

        private Color ColorFor(ThingInMemory rb)
        {
            if (rb == null)
                return Color.gray;
            if (rb is NativeUnityEngineObject)                  //Red
                return new Color(0.9568f, 0.2627f, 0.2117f);
            if (rb is ManagedObject)                            //Blue
                return new Color(0.1294f, 0.5882f, 0.9529f);    
            if (rb is GCHandle)
                return new Color(0.5411f, 0.7607f, 0.2862f);   //Green
            if (rb is StaticFields)                             
                return new Color(1, 0.9215f, 0.2313f);         //Yellow

            throw new ArgumentException("Unexpected type: " + rb.GetType());
        }

        private void DrawFields(ManagedObject managedObject)
        {
            if (managedObject.typeDescription.isArray)
                return;
            GUILayout.Space(10);
            GUILayout.Label("Fields:");
            DrawFields(managedObject.typeDescription, _unpackedCrawl.managedHeap.Find(managedObject.address, _unpackedCrawl.virtualMachineInformation));
        }

        private void DrawFields(TypeDescription typeDescription, BytesAndOffset bytesAndOffset, bool useStatics = false)
        {
            int counter = 0;

            foreach (var field in TypeTools.AllFieldsOf(typeDescription, _unpackedCrawl.typeDescriptions, useStatics ? TypeTools.FieldFindOptions.OnlyStatic : TypeTools.FieldFindOptions.OnlyInstance))
            {
                counter++;
                var gUIStyle = counter % 2 == 0 ? Styles.entryEven : Styles.entryOdd;
                gUIStyle.margin = new RectOffset(0, 0, 0, 0);
                gUIStyle.overflow = new RectOffset(0, 0, 0, 0);
                gUIStyle.padding = EditorStyles.label.padding;
                GUILayout.BeginHorizontal(gUIStyle);
                GUILayout.Label(field.name, labelWidth);
                GUILayout.BeginVertical();
                DrawValueFor(field, bytesAndOffset.Add(field.offset));
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }

        private void DrawArray(ManagedObject managedObject)
        {
            var typeDescription = managedObject.typeDescription;
            int elementCount = ArrayTools.ReadArrayLength(_unpackedCrawl.managedHeap, managedObject.address, typeDescription, _unpackedCrawl.virtualMachineInformation);
            GUILayout.Label("element count: " + elementCount);
            int rank = typeDescription.arrayRank;
            GUILayout.Label("arrayRank: " + rank);
            if (_unpackedCrawl.typeDescriptions[typeDescription.baseOrElementTypeIndex].isValueType)
            {
                GUILayout.Label("Cannot yet display elements of value type arrays");
                return;
            }
            if (rank != 1)
            {
                GUILayout.Label("Cannot display non rank=1 arrays yet.");
                return;
            }

            var pointers = new List<UInt64>();
            for (int i = 0; i != elementCount; i++)
            {
                pointers.Add(_primitiveValueReader.ReadPointer(managedObject.address + (UInt64)_unpackedCrawl.virtualMachineInformation.arrayHeaderSize + (UInt64)(i * _unpackedCrawl.virtualMachineInformation.pointerSize)));
            }
            GUILayout.Label("elements:");
            DrawLinks(pointers);
        }

        private void DrawLinks(IEnumerable<UInt64> pointers)
        {
            DrawLinks(pointers.Select(p => GetThingAt(p)),0);
        }

        private void DrawValueFor(FieldDescription field, BytesAndOffset bytesAndOffset)
        {
            var typeDescription = _unpackedCrawl.typeDescriptions[field.typeIndex];

            switch (typeDescription.name)
            {
                case "System.Int32":
                    GUILayout.Label(_primitiveValueReader.ReadInt32(bytesAndOffset).ToString());
                    break;
                case "System.Int64":
                    GUILayout.Label(_primitiveValueReader.ReadInt64(bytesAndOffset).ToString());
                    break;
                case "System.UInt32":
                    GUILayout.Label(_primitiveValueReader.ReadUInt32(bytesAndOffset).ToString());
                    break;
                case "System.UInt64":
                    GUILayout.Label(_primitiveValueReader.ReadUInt64(bytesAndOffset).ToString());
                    break;
                case "System.Int16":
                    GUILayout.Label(_primitiveValueReader.ReadInt16(bytesAndOffset).ToString());
                    break;
                case "System.UInt16":
                    GUILayout.Label(_primitiveValueReader.ReadUInt16(bytesAndOffset).ToString());
                    break;
                case "System.Byte":
                    GUILayout.Label(_primitiveValueReader.ReadByte(bytesAndOffset).ToString());
                    break;
                case "System.SByte":
                    GUILayout.Label(_primitiveValueReader.ReadSByte(bytesAndOffset).ToString());
                    break;
                case "System.Char":
                    GUILayout.Label(_primitiveValueReader.ReadChar(bytesAndOffset).ToString());
                    break;
                case "System.Boolean":
                    GUILayout.Label(_primitiveValueReader.ReadBool(bytesAndOffset).ToString());
                    break;
                case "System.Single":
                    GUILayout.Label(_primitiveValueReader.ReadSingle(bytesAndOffset).ToString());
                    break;
                case "System.Double":
                    GUILayout.Label(_primitiveValueReader.ReadDouble(bytesAndOffset).ToString());
                    break;
                case "System.IntPtr":
                    GUILayout.Label(_primitiveValueReader.ReadPointer(bytesAndOffset).ToString("X"));
                    break;
                default:
                    if (!typeDescription.isValueType)
                    {
                        ThingInMemory item = GetThingAt(bytesAndOffset.ReadPointer());
                        if (item == null)
                        {
                            EditorGUI.BeginDisabledGroup(true);
                            GUILayout.Button("Null");
                            EditorGUI.EndDisabledGroup();
                        }
                        else
                        {
                            DrawLinks(new ThingInMemory[] { item }, 0);
                        }
                    }
                    else
                    {
                        DrawFields(typeDescription, bytesAndOffset);
                    }
                    break;
            }
        }

        private ThingInMemory GetThingAt(ulong address)
        {
            if (!objectCache.ContainsKey(address))
            {
                objectCache[address] = _unpackedCrawl.allObjects.OfType<ManagedObject>().FirstOrDefault(mo => mo.address == address);
            }

            return objectCache[address];
        }

        private void DrawSpecificTexture2D(NativeUnityEngineObject nativeObject)
        {
            if (nativeObject.className != "Texture2D")
            {
                _textureObject = null;
                return;
            }
            if (_prevInstance != nativeObject.instanceID)
            {
                _textureObject = EditorUtility.InstanceIDToObject(nativeObject.instanceID) as Texture2D;
                _prevInstance = nativeObject.instanceID;
            }
            if (_textureObject != null)
            {
                EditorGUILayout.LabelField("textureInfo: " + _textureObject.width + "x" + _textureObject.height + " " + _textureObject.format);
                GUILayout.Label(_textureObject, GUILayout.Width(_textureSize), GUILayout.Height(_textureSize * _textureObject.height / _textureObject.width));
            }
            else
            {
                EditorGUILayout.LabelField("Can't instance texture,maybe it was already released.");
            }
        }
    }
}
