using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace EasyPolyMap
{
	using Core;

	[CustomEditor(typeof(EPMPolyTerrainCreator))]
	public class EPMPolyTerrainCreatorInspector : Editor
	{
		class GeneratingType
		{
			public string name = "";
			public bool expand = false;
			public int currentIndex = -1;
			public GeneratingType(string name_) { name = name_; }
		}

		GeneratingType m_road, m_river, m_hill, m_mountain, m_sandGround,m_soilGround,m_island;
		EPMPolyTerrainCreator m_creator = null;
		GUIStyle sectionStyle = null;
		void Awake()
		{
			m_creator = serializedObject.targetObject as EPMPolyTerrainCreator;
			m_creator.Init();
			sectionStyle = new GUIStyle();
			Texture2D tex = new Texture2D(1, 1);
			tex.SetPixel(0, 0, EditorGUIUtility.isProSkin ? new Color(0.18f, 0.18f, 0.18f) : new Color(0.88f, 0.88f, 0.88f));
			tex.Apply();
			sectionStyle.normal.background = tex;

			m_road = new GeneratingType("Road");
			m_river = new GeneratingType("River");
			m_hill = new GeneratingType("Hill");
			m_mountain = new GeneratingType("Mountain");
			m_sandGround = new GeneratingType("SandGround");
			m_soilGround = new GeneratingType("SoilGround");
			m_island = new GeneratingType("Island");
		}

		void OnEnable()
		{
			
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical(sectionStyle);

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("TerrainName:");
			m_creator.g_customData.terrainName = EditorGUILayout.TextField(m_creator.g_customData.terrainName);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("TerrainWidth:");
			m_creator.g_customData.terrainWidth = EditorGUILayout.IntField(m_creator.g_customData.terrainWidth, GUILayout.MinWidth(100));
			GUILayout.Label("TerrainLength:");
			m_creator.g_customData.terrainLength = EditorGUILayout.IntField(m_creator.g_customData.terrainLength, GUILayout.MinWidth(100));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("RowSplit:");
			m_creator.g_customData.rowSplit = EditorGUILayout.IntField(m_creator.g_customData.rowSplit, GUILayout.MinWidth(100));
			GUILayout.Label("ColumnSplit:");
			m_creator.g_customData.columnSplit = EditorGUILayout.IntField(m_creator.g_customData.columnSplit, GUILayout.MinWidth(100));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Random Seed:",GUILayout.Width(100));
			m_creator.g_customData.seed = EditorGUILayout.IntField(m_creator.g_customData.seed, GUILayout.Width(100));
			if (GUILayout.Button("R", GUILayout.Width(20)))
			{
				UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
				m_creator.g_customData.seed = UnityEngine.Random.Range(0, 1000000);
				m_creator.CreateTerrain();
			}
			GUILayout.Label("SeaLevel:");
			m_creator.g_customData.seaLevel = EditorGUILayout.FloatField((float)m_creator.g_customData.seaLevel, GUILayout.MinWidth(100));
			EditorGUILayout.EndHorizontal();

			DrawDataReadWrite();
			EditorGUILayout.EndVertical();


			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical(sectionStyle);
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("OBJECTS SECTION:");
			GUILayout.FlexibleSpace();
			if(GUILayout.Button("Visual Editor"))
			{
				EPMDrawMapWindow.CreateWindow(m_creator);
			}
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel++;
			for (int i = 0; i < 7; i++) DrawPointListSection(i);
			EditorGUI.indentLevel--;
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("CLEAR All OBJECTS", GUILayout.Width(150)))
			{
				if (EditorUtility.DisplayDialog("Caution!", "All the objects data will be cleard.", "OK", "Cancel"))
				{
					m_creator.g_customData.roadLists.Clear();
					m_creator.g_customData.riverLists.Clear();
					m_creator.g_customData.hillLists.Clear();
					m_creator.g_customData.mountainLists.Clear();
					m_creator.g_customData.sandGroundLists.Clear();
					m_creator.g_customData.soilGroundLists.Clear();
					m_creator.g_customData.islandLists.Clear();
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();


			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical(sectionStyle);
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("HEIGHT SECTION");
			EditorGUILayout.EndHorizontal();

			for (int i = 0; i < m_creator.g_customData.heightSamplerList.Count; i++) DrawSingleHeightSampler(i);
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Add New Sampler",GUILayout.Width(150)))
			{
				m_creator.g_customData.heightSamplerList.Add(new EPMPolyTerrainCreator.HeightSampleData());
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();



			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical(sectionStyle);
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("TWIST SECTION");
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			m_creator.g_customData.twistSeed = EditorGUILayout.IntField("Twist Seed:", m_creator.g_customData.twistSeed,GUILayout.Width(300));
			if(GUILayout.Button("R",GUILayout.Width(20)))
			{
				UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
				m_creator.g_customData.twistSeed = UnityEngine.Random.Range(0, 1000000);
				m_creator.CreateTerrain();
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			m_creator.g_customData.twistNum = EditorGUILayout.IntField("Number:", m_creator.g_customData.twistNum, GUILayout.Width(300));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			m_creator.g_customData.twistStrength = EditorGUILayout.FloatField("Strength:", (float)m_creator.g_customData.twistStrength, GUILayout.Width(300));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("CreateTerrain"))
			{
				m_creator.CreateTerrain();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();



			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical(sectionStyle);
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("COLOR SECTION:");
			EditorGUILayout.EndHorizontal();
			foreach (EPMPoint.PointType pt in System.Enum.GetValues(typeof(EPMPoint.PointType)))
			{
				DrawSingleTypeColor(pt);
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical(sectionStyle);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("ExportTerrain"))
			{
				m_creator.ExportTerrain();
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}

		private void DrawPointListSection(int type)
		{
			List<List<Vector2d>> pointList = null;
			GeneratingType currentType = null;
			switch(type)
			{
				case 0:
					pointList = m_creator.g_customData.roadLists;
					currentType = m_road;
					break;
				case 1:
					pointList = m_creator.g_customData.riverLists;
					currentType = m_river;
					break;
				case 2:
					pointList = m_creator.g_customData.hillLists;
					currentType = m_hill;
					break;
				case 3:
					pointList = m_creator.g_customData.mountainLists;
					currentType = m_mountain;
					break;
				case 4:
					pointList = m_creator.g_customData.sandGroundLists;
					currentType = m_sandGround;
					break;
				case 5:
					pointList = m_creator.g_customData.soilGroundLists;
					currentType = m_soilGround;
					break;
				case 6:
					pointList = m_creator.g_customData.islandLists;
					currentType = m_island;
					break;

			}
			if (currentType == null) return;
			EditorGUILayout.BeginHorizontal();
			currentType.expand = EditorGUILayout.Foldout(currentType.expand, currentType.name + " Section");
			EditorGUI.indentLevel++;
			EditorGUILayout.EndHorizontal();;

			if (currentType.expand)
			{
				bool boolTemp = false;
				for (int i = 0; i < pointList.Count; i++)
				{
					EditorGUILayout.BeginHorizontal();
					boolTemp = EditorGUILayout.Foldout(currentType.currentIndex == i, currentType.name + i.ToString());
					if (boolTemp != (currentType.currentIndex == i))
					{
						if (boolTemp == false)
						{
							currentType.currentIndex = -1;
						}
					}
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Delete", GUILayout.Width(100)))
					{
						pointList.RemoveAt(i);
						return;
					}
					EditorGUILayout.EndHorizontal();

					if (boolTemp == true)
					{
						currentType.currentIndex = i;
						DrawVector2ListField(pointList[i]);
					}

				}

				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("", GUILayout.Width(15));  //why? indentLevel doesn't work...
				if (GUILayout.Button("Add "+ currentType.name, GUILayout.Width(150)))
				{
					pointList.Add(new List<Vector2d>());
				}
				EditorGUILayout.EndHorizontal();
			}

			EditorGUI.indentLevel--;
		}

		private void DrawVector2ListField(List<Vector2d> list)
		{
			EditorGUI.indentLevel++;
			for (int i=0;i<list.Count;i++)
			{
				EditorGUILayout.BeginHorizontal();
				Vector2 v2=(EditorGUILayout.Vector2Field("Point " + i.ToString(),new Vector2((float)list[i].x,(float)list[i].y)));
				list[i].x = v2.x;
				list[i].y = v2.y;
				if(GUILayout.Button("Del",GUILayout.Width(50)))
				{
					list.RemoveAt(i);
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("",GUILayout.Width(30)); //why? indentLevel doesn't work...
			if(GUILayout.Button("Add Point",GUILayout.Width(80)))
			{
				list.Add(new Vector2d(0, 0));
			}
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel--;

		}

		private void DrawDataReadWrite()
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Data File:", GUILayout.Width(80));
			TextAsset ta = (TextAsset)EditorGUILayout.ObjectField(m_creator.g_dataFile, typeof(TextAsset),false);
			if (m_creator.g_dataFile!=ta)
			{
				m_creator.g_dataFile = ta;
				m_creator.g_dataFilePath = ta != null ? AssetDatabase.GetAssetPath(ta) : "";
				Debug.Log(m_creator.g_dataFilePath);
			}

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();

			if(GUILayout.Button("Create Data"))
			{
				string path = m_creator.CreateData();
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
				m_creator.g_dataFile = (TextAsset)AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));
				m_creator.g_dataFilePath = path;
			}
			if(GUILayout.Button("Import Data"))
			{
				if (EditorUtility.DisplayDialog("Caution!", "The imported data will replace current data.", "OK", "Cancel"))
				{
					if (m_creator.g_dataFile == null) Debug.LogError("Data File is Null!!!!");
					else m_creator.ImportData();
				}
			}
			if(GUILayout.Button("Save Data"))
			{
				m_creator.g_dataFilePath = ta != null ? AssetDatabase.GetAssetPath(ta) : "";
				m_creator.ExportData();
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
			EditorGUILayout.EndHorizontal();
		}

		private void DrawSingleHeightSampler(int index)
		{
			EPMPolyTerrainCreator.HeightSampleData data = m_creator.g_customData.heightSamplerList[index];
			if (data == null) return;
			EditorGUILayout.Separator();
			string[] samplerTypes = new string[2];
			samplerTypes[0] = "Point";
			samplerTypes[1] = "Line";
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("SampleMode",GUILayout.Width(100));
			data.type=EditorGUILayout.Popup(data.type, samplerTypes, GUILayout.Width(120));
			GUILayout.FlexibleSpace();
			if(GUILayout.Button("Remove"))
			{
				m_creator.g_customData.heightSamplerList.RemoveAt(index);
				return;
			}
			EditorGUILayout.EndHorizontal();
			
			
			EditorGUILayout.BeginHorizontal();
			data.number = EditorGUILayout.IntField(data.type == 0 ? "Point Number:" : "Line Number", data.number,GUILayout.Width(200));
			GUILayout.Label("Curve Mode:", GUILayout.Width(100));
			data.curveMode = (EPMHeightSampler.CurveMode)EditorGUILayout.EnumPopup(data.curveMode, GUILayout.Width(100));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			Vector2 v2 = (EditorGUILayout.Vector2Field("Height Range:" , new Vector2((float)data.heightRange.x, (float)data.heightRange.y)));
			data.heightRange.x = v2.x;
			data.heightRange.y = v2.y;
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			v2 = (EditorGUILayout.Vector2Field("Radius Range:", new Vector2((float)data.radiusRange.x, (float)data.radiusRange.y)));
			data.radiusRange.x = v2.x;
			data.radiusRange.y = v2.y;
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Combine Mode:", GUILayout.Width(100));
			data.combineMode = (EPMHeightSampler.CombineMode)EditorGUILayout.EnumPopup(data.combineMode, GUILayout.Width(100));
			GUILayout.Label("Limitation:", GUILayout.Width(120));
			data.limitation = EditorGUILayout.FloatField((float)data.limitation,GUILayout.Width(80));
			EditorGUILayout.EndHorizontal();
		}


		private void DrawSingleTypeColor(EPMPoint.PointType type)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(type.ToString(),GUILayout.Width(150));
			Color c = m_creator.GetColorByType(type);
			Color newColor = EditorGUILayout.ColorField(c,GUILayout.Width(50));
			if(c.r!=newColor.r||c.g!=newColor.g||c.b!=newColor.b)
			{
				m_creator.SetColorByType(type, newColor);
			}
			EditorGUILayout.EndHorizontal();
		}
	}

}