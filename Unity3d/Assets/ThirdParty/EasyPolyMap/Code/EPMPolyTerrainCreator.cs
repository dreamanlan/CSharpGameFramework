using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EasyPolyMap
{
	using Core;

	public class EPMPolyTerrainCreator : MonoBehaviour
	{

		public Dictionary<EPMPoint.PointType, Color> g_typeColorDic = new Dictionary<EPMPoint.PointType, Color>();

		public TextAsset g_dataFile = null;
		public string g_dataFilePath = "";

		public class HeightSampleData
		{
			public int type = 0;
			public int number = 200;
			public Vector2d heightRange = new Vector2d(0, 30);
			public Vector2d radiusRange = new Vector2d(50, 200);
			public EPMHeightSampler.CurveMode curveMode = EPMHeightSampler.CurveMode.ReverseLinear;
			public EPMHeightSampler.CombineMode combineMode = EPMHeightSampler.CombineMode.Add;
			public double limitation = 0;
		}

		public class CustomData
		{
			public List<List<Vector2d>> roadLists = new List<List<Vector2d>>();
			public List<List<Vector2d>> riverLists = new List<List<Vector2d>>();
			public List<List<Vector2d>> hillLists = new List<List<Vector2d>>();
			public List<List<Vector2d>> mountainLists = new List<List<Vector2d>>();
			public List<List<Vector2d>> sandGroundLists = new List<List<Vector2d>>();
			public List<List<Vector2d>> soilGroundLists = new List<List<Vector2d>>();
			public List<List<Vector2d>> islandLists = new List<List<Vector2d>>();

			public List<Vector3d> colorList = new List<Vector3d>();

			public List<HeightSampleData> heightSamplerList = new List<HeightSampleData>();

			public int terrainWidth = 500, terrainLength = 500;
			public int rowSplit = 4, columnSplit = 4;
			public int seed = 0;
			public double seaLevel = -10;
			public string terrainName = "EPMDefaultName";

			public int twistSeed = 0;
			public int twistNum = 20;
			public double twistStrength = 2;
		}

		public CustomData g_customData = new CustomData();

		EPMDelaunayTerrain m_terrain = null;
		EPMMaterialAgent m_materialAgent = null;
		GameObject m_meshRoot = null;

		private float m_roadHalfWidth, m_riverHalfWidth, m_hillHalfWidth, m_mountainRadius, m_islandBorderHalfWidth;
		private float m_defaultHillHeight, m_defaultMountainHeight;

		void Awake()
		{

		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		public Color GetColorByType(EPMPoint.PointType type)
		{
			if (m_materialAgent == null) return Color.white;
			return g_typeColorDic[type];
		}

		public void SetColorByType(EPMPoint.PointType type, Color newColor)
		{
			if (m_materialAgent == null) return;
			g_typeColorDic[type] = newColor;
			m_materialAgent.SetColor(type, newColor);
		}

		public void Init()
		{
			if (m_terrain == null)
			{
				m_terrain = new EPMDelaunayTerrain();
			}

			if (m_materialAgent == null)
			{
				m_materialAgent = new EPMMaterialAgent();
				foreach (EPMPoint.PointType t in System.Enum.GetValues(typeof(EPMPoint.PointType)))
				{
					Color color = EPMMaterialAgent.GetDefaultColor(t);
					g_typeColorDic.Add(t, color);
					m_materialAgent.SetColor(t, color);
				}
			}

			m_hillHalfWidth = 10f;
			m_roadHalfWidth = 4f;
			m_riverHalfWidth = 4f;
			m_islandBorderHalfWidth = 5f;
			m_mountainRadius = 30f;
			m_defaultHillHeight = 30f;
			m_defaultMountainHeight = 60f;
		}

		public GameObject CreateTerrain()
		{
			Init();

			if (m_meshRoot == null)
			{
				m_meshRoot = new GameObject();
				m_meshRoot.transform.SetParent(null);
			}
			m_meshRoot.name = g_customData.terrainName;

			m_terrain.InitGenerating(g_customData.terrainName, g_customData.seed, new Vector2d(0, 0), new Vector2d(g_customData.terrainWidth, g_customData.terrainLength), g_customData.islandLists);

			float time = Time.realtimeSinceStartup;
			//First, Generate Basic Terrain, If the map has island, surround the world with ocean.
			if (g_customData.islandLists.Count > 0)
			{
				m_terrain.GeneratingBase(g_customData.terrainWidth * g_customData.terrainLength / 50, EPMPoint.PointType.Ocean);
			}
			else
			{
				m_terrain.GeneratingBase(g_customData.terrainWidth * g_customData.terrainLength / 50, EPMPoint.PointType.Ground);
			}

			for (int i = 0; i < g_customData.islandLists.Count; i++)
			{
				GeneratingSingleIsland(g_customData.islandLists[i]);
			}
			SetIslandGround();


			for (int i = 0; i < g_customData.hillLists.Count; i++)
			{
				GenerateSingleHill(g_customData.hillLists[i]);
			}


			for (int i = 0; i < g_customData.roadLists.Count; i++)
			{
				GenerateSingleRoad(g_customData.roadLists[i]);
			}

			for (int i = 0; i < g_customData.riverLists.Count; i++)
			{
				GenerateSingleRiver(g_customData.riverLists[i]);
			}

			for (int i = 0; i < g_customData.mountainLists.Count; i++)
			{
				GenerateSingleMountain(g_customData.mountainLists[i]);
			}

			Debug.Log("Initial Points Used:" + (Time.realtimeSinceStartup - time).ToString());
			time = Time.realtimeSinceStartup;

			m_terrain.StartGenerating();

			Debug.Log("Generating Used:" + (Time.realtimeSinceStartup - time).ToString());
			time = Time.realtimeSinceStartup;

			//If two island intersect, remove the coastline where they intersect.The oceanSide tiles are tiles share at least one point with ocean tiles.
			EPMPreDefinedTools.CheckAndSetTileTypeByNeighbor(m_terrain, EPMPoint.PointType.OceanSide, EPMPoint.PointType.Ocean, EPMPoint.PointType.OceanSide, EPMPoint.PointType.Ground, 1, false, true);

			//The coastline is the Ground tile and has at least one shared point with oceanSide tiles.
			EPMPreDefinedTools.CheckAndSetTileTypeByNeighbor(m_terrain, EPMPoint.PointType.Ground, EPMPoint.PointType.OceanSide, EPMPoint.PointType.OceanSide, EPMPoint.PointType.Ground, 1, false, true);

			//Then check any abnormal ocean points, All the points belongs to Ocean tile must be Ocean points. The CheckAndSetTileTypeByNeighbor I use to solve the problem is overfitting, but it is convenient....
			EPMPreDefinedTools.CheckAndSetTileTypeByNeighbor(m_terrain, EPMPoint.PointType.Ocean, EPMPoint.PointType.OceanSide, EPMPoint.PointType.Ocean, EPMPoint.PointType.Ocean, 1, false, true);

			EPMPreDefinedTools.ExtractSide(m_terrain, EPMPoint.PointType.Road, EPMPoint.PointType.RoadSide, m_roadHalfWidth * 0.5);
			EPMPreDefinedTools.ExtractCenter(m_terrain, g_customData.roadLists, EPMPoint.PointType.RoadSide, EPMPoint.PointType.Road, m_roadHalfWidth * 0.3);
			EPMPreDefinedTools.ExtractCenter(m_terrain, g_customData.riverLists, EPMPoint.PointType.RiverSide, EPMPoint.PointType.River, m_riverHalfWidth * 0.3);

			

			Debug.Log("Extracting Side Used:" + (Time.realtimeSinceStartup - time).ToString());
			time = Time.realtimeSinceStartup;

			RenderRiver();
			SmoothRiver();

			RenderHill();

			RenderMountain();

			Debug.Log("Rendering Used:" + (Time.realtimeSinceStartup - time).ToString());
			time = Time.realtimeSinceStartup;

			for (int i = 0; i < g_customData.sandGroundLists.Count; i++)
			{
				RenderSingleGround(g_customData.sandGroundLists[i], EPMPoint.PointType.Sand);
			}

			for (int i = 0; i < g_customData.soilGroundLists.Count; i++)
			{
				RenderSingleGround(g_customData.soilGroundLists[i], EPMPoint.PointType.Soil);
			}


			SampleHeight();

			Debug.Log("SampleHeight Used:" + (Time.realtimeSinceStartup - time).ToString());
			time = Time.realtimeSinceStartup;
			TwistTerrain();

			RenderOcean();

			Debug.Log("TwistTerrain Used:" + (Time.realtimeSinceStartup - time).ToString());
			time = Time.realtimeSinceStartup;
			CreateMesh();

			Debug.Log("CreateMesh Used:" + (Time.realtimeSinceStartup - time).ToString());
			time = Time.realtimeSinceStartup;

			return m_meshRoot;
		}

		public void ExportTerrain()
		{
			ExportMesh(@"Assets/EPMTerrainOutput", g_customData.terrainName);
			ExportData(true);
		}

		public string CreateData(string path = null)
		{
			g_customData.colorList.Clear();
			foreach (EPMPoint.PointType key in g_typeColorDic.Keys)
			{
				Color c = g_typeColorDic[key];
				g_customData.colorList.Add(new Vector3d(c.r, c.g, c.b));
			}

			string data = LitJson.JsonMapper.ToJson(g_customData);

			if (path == null || path == "")
			{
				System.DateTime dt = System.DateTime.Now;
				string timeString = dt.Year.ToString() + (dt.Month < 10 ? "0" + dt.Month.ToString() : dt.Month.ToString()) + (dt.Day < 10 ? "0" + dt.Day.ToString() : dt.Day.ToString())
					+ (dt.Hour < 10 ? "0" + dt.Hour.ToString() : dt.Hour.ToString()) + (dt.Minute < 10 ? "0" + dt.Minute.ToString() : dt.Minute.ToString())
					+ (dt.Second < 10 ? "0" + dt.Second.ToString() : dt.Second.ToString());

				if (!Directory.Exists(@"Assets/EPMTerrainOutput")) Directory.CreateDirectory(@"Assets/EPMTerrainOutput");
				path = @"Assets/EPMTerrainOutput" + "/" + g_customData.terrainName + "_data" + timeString + ".txt";
			}
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			StreamWriter writer;
			FileInfo t_fFileInfo = new FileInfo(path);
			writer = t_fFileInfo.CreateText();
			writer.WriteLine(data);
			writer.Close();
			writer.Dispose();

			return path;
		}

		public void ExportData(bool toTerrainPath = false)
		{
			string path = g_dataFilePath;
			if (toTerrainPath)
			{
				path = @"Assets/EPMTerrainOutput/" + g_customData.terrainName + "/" + g_customData.terrainName + "_data.txt";
			}
			CreateData(path);
		}

		public void ImportData()
		{
			if (g_dataFile == null) return;
			string data = g_dataFile.text;
			g_customData = LitJson.JsonMapper.ToObject<CustomData>(data);

			List<EPMPoint.PointType> keys = new List<EPMPoint.PointType>();
			foreach (EPMPoint.PointType key in g_typeColorDic.Keys)
			{
				keys.Add(key);
			}
			for (int i = 0; i < g_customData.colorList.Count; i++)
			{
				Vector3d v3d = g_customData.colorList[i];
				Color c = new Color((float)v3d.x, (float)v3d.y, (float)v3d.z);
				SetColorByType(keys[i], c);
			}
		}

		public void GeneratingSingleIsland(List<Vector2d> islandBorder)
		{
			for (int i = 0; i < islandBorder.Count; i++)
			{
				m_terrain.DeletePoints_Circle(islandBorder[i], m_islandBorderHalfWidth * 1.5f);
				m_terrain.GeneratingPoints_Ring((int)(m_islandBorderHalfWidth * m_islandBorderHalfWidth), EPMPoint.PointType.OceanSide, islandBorder[i], m_islandBorderHalfWidth * 0.7f, m_islandBorderHalfWidth);
				if (i > 0)
				{
					GenerateIslandSegment(islandBorder[i - 1], islandBorder[i], m_islandBorderHalfWidth);
				}
			}
		}

		public void GenerateIslandSegment(Vector2d start, Vector2d end, double halfWidth)
		{
			Vector2d dir = end - start;
			double length = dir.Length() / 4f;
			dir.Normalize();
			Vector2d normal = new Vector2d(-dir.y, dir.x);
			double sideLength = halfWidth * 0.08;
			m_terrain.DeletePoints_Line(start, end, halfWidth * 1.5f);
			m_terrain.GeneratingPoints_Line((int)length, EPMPoint.PointType.OceanSide, start + normal * (halfWidth * 0.8f), end + normal * (halfWidth * 0.8f), sideLength, true);
			m_terrain.GeneratingPoints_Line((int)length, EPMPoint.PointType.OceanSide, start, end, sideLength, true);
			m_terrain.GeneratingPoints_Line((int)length, EPMPoint.PointType.OceanSide, start - normal * (halfWidth * 0.8f), end - normal * (halfWidth * 0.8f), sideLength, true);
		}

		public void SetIslandGround()
		{
			for (int i = 0; i < g_customData.islandLists.Count; i++)
			{
				List<Vector2d> island = g_customData.islandLists[i];
				List<EPMPoint> points = m_terrain.ExtractPoints_ByRegion(island, null, EPMPoint.GenerateTypesInt(false, EPMPoint.PointType.Ocean));
				for (int j = 0; j < points.Count; j++)
				{
					points[j].SetType(EPMPoint.PointType.Ground);
				}
			}
		}

		public void GenerateSingleRoad(List<Vector2d> road)
		{
			for (int i = 0; i < road.Count; i++)
			{
				m_terrain.DeletePoints_Circle(road[i], m_roadHalfWidth * 1.5f);
				m_terrain.GeneratingPoints_Ring((int)(m_roadHalfWidth * m_roadHalfWidth), EPMPoint.PointType.Road, road[i], m_roadHalfWidth * 0.7f, m_roadHalfWidth);
				if (i > 0)
				{
					GenerateRoadSegment(road[i - 1], road[i], m_roadHalfWidth);
				}
			}
		}

		public void GenerateRoadSegment(Vector2d start, Vector2d end, float halfWidth)
		{
			Vector2d dir = end - start;
			double length = dir.Length() / 2f;
			dir.Normalize();
			Vector2d normal = new Vector2d(-dir.y, dir.x);
			float sideLength = halfWidth * 0.08f;
			m_terrain.DeletePoints_Line(start, end, halfWidth * 1.5f);
			m_terrain.GeneratingPoints_Line((int)length, EPMPoint.PointType.Road, start + normal * (halfWidth * 0.9f), end + normal * (halfWidth * 0.9f), sideLength, true);
			m_terrain.GeneratingPoints_Line((int)length, EPMPoint.PointType.Road, start + normal * (halfWidth * 0.7f), end + normal * (halfWidth * 0.7f), sideLength, true);
			m_terrain.GeneratingPoints_Line((int)length, EPMPoint.PointType.Road, start - normal * (halfWidth * 0.9f), end - normal * (halfWidth * 0.9f), sideLength, true);
			m_terrain.GeneratingPoints_Line((int)length, EPMPoint.PointType.Road, start - normal * (halfWidth * 0.7f), end - normal * (halfWidth * 0.7f), sideLength, true);
		}

		public void GenerateSingleRiver(List<Vector2d> river)
		{
			for (int i = 0; i < river.Count; i++)
			{
				m_terrain.DeletePoints_Circle(river[i], m_riverHalfWidth * 2.5f);
				m_terrain.GeneratingPoints_Ring((int)(m_riverHalfWidth * m_riverHalfWidth), EPMPoint.PointType.RiverSide, river[i], m_riverHalfWidth * 0.7f, m_riverHalfWidth * 0.9f);
				m_terrain.GeneratingPoints_Ring((int)(m_riverHalfWidth * m_riverHalfWidth), EPMPoint.PointType.RiverSide, river[i], m_riverHalfWidth * 1.1f, m_riverHalfWidth * 1.8f);
				if (i > 0)
				{
					GenerateRiverSegment(river[i - 1], river[i], m_riverHalfWidth);
				}
			}
		}

		public void GenerateRiverSegment(Vector2d start, Vector2d end, float halfWidth)
		{
			Vector2d dir = end - start;
			double length = dir.Length() / 4f;
			dir.Normalize();
			Vector2d normal = new Vector2d(-dir.y, dir.x);
			double sideLength = halfWidth * 0.05;
			m_terrain.DeletePoints_Line(start, end, halfWidth * 2.5);
			EPMPoint.PointType type = EPMPoint.PointType.RiverSide;
			m_terrain.GeneratingPoints_Line((int)length, type, start + normal * (halfWidth * 1.8f), end + normal * (halfWidth * 1.8f), sideLength, true);
			m_terrain.GeneratingPoints_Line((int)length, type, start + normal * (halfWidth * 1.1f), end + normal * (halfWidth * 1.1f), sideLength, true);
			m_terrain.GeneratingPoints_Line((int)length, type, start + normal * (halfWidth * 0.9f), end + normal * (halfWidth * 0.9f), sideLength, true);
			m_terrain.GeneratingPoints_Line((int)length, type, start + normal * (halfWidth * 0.7f), end + normal * (halfWidth * 0.7f), 0, true);
			m_terrain.GeneratingPoints_Line((int)length, type, start - normal * (halfWidth * 0.7f), end - normal * (halfWidth * 0.7f), 0, true);
			m_terrain.GeneratingPoints_Line((int)length, type, start - normal * (halfWidth * 0.9f), end - normal * (halfWidth * 0.9f), sideLength, true);
			m_terrain.GeneratingPoints_Line((int)length, type, start - normal * (halfWidth * 1.1f), end - normal * (halfWidth * 1.1f), sideLength, true);
			m_terrain.GeneratingPoints_Line((int)length, type, start - normal * (halfWidth * 1.8f), end - normal * (halfWidth * 1.8f), sideLength, true);
		}

		public void GenerateSingleHill(List<Vector2d> hill)
		{
			for (int i = 0; i < hill.Count; i++)
			{
				m_terrain.DeletePoints_Circle(hill[i], m_hillHalfWidth * 1.5f);
				m_terrain.GeneratingPoints_Ring((int)(m_hillHalfWidth * m_hillHalfWidth), EPMPoint.PointType.Hill, hill[i], m_hillHalfWidth * 0.7f, m_hillHalfWidth);
				if (i > 0)
				{
					GenerateHillSegment(hill[i - 1], hill[i], m_hillHalfWidth);
				}
			}
		}

		public void GenerateHillSegment(Vector2d start, Vector2d end, float halfWidth)
		{
			Vector2d dir = end - start;
			double length = dir.Length() / 4f;
			dir.Normalize();
			Vector2d normal = new Vector2d(-dir.y, dir.x);
			double sideLength = halfWidth * 0.15f;
			m_terrain.DeletePoints_Line(start, end, halfWidth * 1.5f);
			m_terrain.GeneratingPoints_Line((int)length, EPMPoint.PointType.Hill, start + normal * (halfWidth * 1.1f), end + normal * (halfWidth * 1.1f), sideLength, true);
			m_terrain.GeneratingPoints_Line((int)length, EPMPoint.PointType.Hill, start + normal * (halfWidth * 0.7f), end + normal * (halfWidth * 0.7f), sideLength, true);
			m_terrain.GeneratingPoints_Line((int)length, EPMPoint.PointType.Hill, start, end, halfWidth * 0.4f, false);
			m_terrain.GeneratingPoints_Line((int)length, EPMPoint.PointType.Hill, start - normal * (halfWidth * 1.1f), end - normal * (halfWidth * 1.1f), sideLength, true);
			m_terrain.GeneratingPoints_Line((int)length, EPMPoint.PointType.Hill, start - normal * (halfWidth * 0.7f), end - normal * (halfWidth * 0.7f), sideLength, true);
		}

		public void GenerateSingleMountain(List<Vector2d> mountain)
		{
			for (int i = 0; i < mountain.Count; i++)
			{
				Vector2d center = mountain[i];
				int number = (int)(m_mountainRadius * m_mountainRadius / 10);
				m_terrain.DeletePoints_Circle(center, m_mountainRadius * 1.2f);
				m_terrain.GeneratingPoints_Ring(number / 2, EPMPoint.PointType.Mountain, center, m_mountainRadius * 0.7f, m_mountainRadius);
				m_terrain.GeneratingPoints_Circle(number / 2, EPMPoint.PointType.Mountain, center, m_mountainRadius * 0.7f);
			}
		}

		public void RenderOcean()
		{
			List<EPMPoint> oceanPoints = m_terrain.ExtractPoints_ByTypes(null, EPMPoint.GenerateTypesInt(false, EPMPoint.PointType.Ocean));
			float height = (float)g_customData.seaLevel;
			for (int i = 0; i < oceanPoints.Count; i++)
			{
				oceanPoints[i].posY = height;
			}
		}

		public void RenderRiver()
		{
			List<EPMPoint> riverPoints = m_terrain.ExtractPoints_ByTypes(null, EPMPoint.GenerateTypesInt(false, EPMPoint.PointType.RiverSide, EPMPoint.PointType.River));
			for (int k = 0; k < g_customData.riverLists.Count; k++)
			{
				List<Vector2d> river = g_customData.riverLists[k];
				for (int i = 1; i < river.Count; i++)
				{
					EPMHeightSampler.DistanceToLine(5, river[i - 1], river[i], m_riverHalfWidth, riverPoints, EPMHeightSampler.CurveMode.CosineQuarter, EPMHeightSampler.CombineMode.SubtractWithLimitation, -3);
					if (i != river.Count - 1)
					{
						EPMHeightSampler.DistanceToPoint(5, river[i], m_riverHalfWidth, riverPoints, EPMHeightSampler.CurveMode.CosineQuarter, EPMHeightSampler.CombineMode.SubtractWithLimitation, -3);
					}
				}
			}

			for (int i = 0; i < riverPoints.Count; i++) riverPoints[i].ApplyHeight();

		}

		public void SmoothRiver()
		{
			List<EPMPoint> plist = m_terrain.ExtractPoints_ByTypes(null, EPMPoint.GenerateTypesInt(false, EPMPoint.PointType.River));
			for (int i = 0; i < plist.Count; i++)
			{
				plist[i].posY = -3;
			}
			for (int i = 0; i < g_customData.riverLists.Count; i++)
			{
				List<Vector2d> river = g_customData.riverLists[i];
				for (int j = 1; j < river.Count; j++)
				{
					EPMHeightSampler.DirectionalDistance(0.5f, river[j] - river[j - 1], 100, plist, -1f);
				}
			}

			for (int i = 0; i < plist.Count; i++) plist[i].ApplyHeight();

		}

		public void RenderHill()
		{
			List<EPMPoint> hillPoints = m_terrain.ExtractPoints_ByTypes(null, EPMPoint.GenerateTypesInt(false, EPMPoint.PointType.Hill));

			for (int k = 0; k < g_customData.hillLists.Count; k++)
			{
				List<Vector2d> hill = g_customData.hillLists[k];
				for (int i = 1; i < hill.Count; i++)
				{
					EPMHeightSampler.DistanceToLine(m_defaultHillHeight, hill[i - 1], hill[i], m_hillHalfWidth * 1.2f, hillPoints, EPMHeightSampler.CurveMode.CosineHalf);
				}
			}

			for (int i = 0; i < hillPoints.Count; i++) hillPoints[i].ApplyHeight();

			for (int k = 0; k < g_customData.hillLists.Count; k++)
			{
				List<Vector2d> hill = g_customData.hillLists[k];
				for (int i = 1; i < hill.Count; i++)
				{
					EPMHeightSampler.DirectionalDistance(m_terrain.RandomDouble(2f, 5f), hill[i] - hill[i - 1], 50f, hillPoints, m_defaultHillHeight * 1.5f, 2);
				}
			}

			List<EPMTriangle> hillTriangles = m_terrain.ExtractTriangles_ByVertex(hillPoints);
			for (int i = 0; i < hillTriangles.Count; i++)
			{
				if (hillTriangles[i].HasShapeType(EPMPoint.GetGroundEnums())) hillTriangles[i].SetType(EPMPoint.PointType.Hill);
			}
		}

		public void RenderMountain()
		{
			List<EPMPoint> mountainPoints = m_terrain.ExtractPoints_ByTypes(null, EPMPoint.GenerateTypesInt(false, EPMPoint.PointType.Mountain));
			List<EPMTriangle> mountainTriangles = m_terrain.ExtractTriangles_ByVertex(mountainPoints);
			for (int i = 0; i < mountainTriangles.Count; i++) mountainTriangles[i].SetType(EPMPoint.PointType.Mountain);

			for (int k = 0; k < g_customData.mountainLists.Count; k++)
			{
				List<Vector2d> mountain = g_customData.mountainLists[k];
				for (int i = 0; i < mountain.Count; i++)
				{
					EPMHeightSampler.DistanceToPoint(m_terrain.RandomDouble(m_defaultMountainHeight / 2, m_defaultMountainHeight * 1.5f), mountain[i], m_mountainRadius * 1.5f, mountainPoints, EPMHeightSampler.CurveMode.ReverseLinear);
				}
			}

			for (int i = 0; i < mountainPoints.Count; i++)
			{
				mountainPoints[i].ApplyHeight();
			}

			//find summit, the summit is the point that all its neighbors height is lower than it.
			List<EPMPoint> summitPoints = new List<EPMPoint>();
			for (int i = 0; i < mountainPoints.Count; i++)
			{
				EPMPoint p = mountainPoints[i];
				if (p.g_neighbors.Count <= 2) continue;
				bool summit = true;
				for (int j = 0; j < p.g_neighbors.Count; j++)
				{
					if (p.posY <= p.g_neighbors[j].posY)
					{
						summit = false;
						break;
					}
				}
				if (summit) summitPoints.Add(p);
			}

			for (int i = 0; i < summitPoints.Count; i++)
			{
				List<EPMTriangle> l = m_terrain.ExtractTriangles_PointsInCircle(summitPoints[i].pos2d, m_mountainRadius * m_terrain.RandomDouble(0.2f, 0.4f), 1, mountainTriangles);
				for (int j = 0; j < l.Count; j++) l[j].SetType(EPMPoint.PointType.MountainSummit);
			}

		}

		public void RenderSingleGround(List<Vector2d> ground, EPMPoint.PointType type)
		{
			int groundEnums = EPMPoint.GetGroundEnums();
			List<EPMPoint> plist = m_terrain.ExtractPoints_ByRegion(ground, null, groundEnums);
			for (int i = 0; i < plist.Count; i++)
			{
				plist[i].SetType(type);
			}

			List<EPMTriangle> triangleList = m_terrain.ExtractTriangles_ByVertex(plist);

			for (int i = 0; i < triangleList.Count; i++)
			{
				if (!triangleList[i].HasShapeType(groundEnums)) continue;
				triangleList[i].TryDetermineType();
			}
			EPMPreDefinedTools.CheckAndSetTileTypeByNeighbor(m_terrain, EPMPoint.GetGroundEnums(), EPMPoint.GenerateTypesInt(false, type), type);
		}

		public void SampleHeight()
		{
			for (int i = 0; i < g_customData.heightSamplerList.Count; i++)
			{
				HeightSampleData data = g_customData.heightSamplerList[i];
				if (data.type == 0)
				{
					EPMPreDefinedTools.SampleHeightByPoint(m_terrain, data.number, data.radiusRange, data.curveMode, data.combineMode, (float)data.limitation);
				}
				else if (data.type == 1)
				{
					EPMPreDefinedTools.SampleHeightByLine(m_terrain, data.number, data.radiusRange, data.curveMode, data.combineMode, (float)data.limitation);
				}
				List<EPMPoint> plist = m_terrain.ExtractPoints_All();
				for(int j=0;j<plist.Count;j++)
				{
					plist[j].ApplyHeight(data.heightRange);
				}
			}
		}

		public void TwistTerrain()
		{
			System.Random random = new System.Random(g_customData.twistSeed);
			List<EPMPoint> pointList = m_terrain.ExtractPoints_ByTypes(null, EPMPoint.GenerateTypesInt(true, EPMPoint.PointType.Ocean));

			//If the Ocean Point is connected with OceanSide, it should be put into pointList!!
			List<EPMTriangle> tlist = m_terrain.ExtractTriangles_ByTypes(null, EPMPoint.GenerateTypesInt(false, EPMPoint.PointType.OceanSide));
			Dictionary<int, EPMPoint> connectedOceanPoints = new Dictionary<int, EPMPoint>();
			for (int i = 0; i < tlist.Count; i++)
			{
				List<EPMPoint> pl = tlist[i].g_pointList;

				for (int j = 0; j < pl.Count; j++)
				{
					if (pl[j].HasType(EPMPoint.PointType.Ocean))
					{
						if (connectedOceanPoints.ContainsKey(pl[j].g_indexInList) == false)
						{
							connectedOceanPoints.Add(pl[j].g_indexInList, pl[j]);
							pointList.Add(pl[j]);
						}
					}
				}
			}

			for (int i = 0; i < g_customData.twistNum; i++)
			{
				double degree = random.NextDouble() * 2 * System.Math.PI;
				if (m_terrain.RandomInt(0, 2) % 2 == 0) EPMPlaneTwister.SineExpander(pointList, new Vector2d(System.Math.Sin(degree), System.Math.Cos(degree)), 100, (float)g_customData.twistStrength);
				else EPMPlaneTwister.SineShifter(pointList, new Vector2d(System.Math.Sin(degree), System.Math.Cos(degree)), 100, (float)g_customData.twistStrength);
			}
		}


		public void ExportMesh(string path,string name)
		{
			ExportTerrainMesh(path,name);
			ExportData(true);
		}

		public void CreateSeaPlane(float seaLevel)
		{
			Vector3[] vertices = new Vector3[6];
			int[] indices = new int[6];
			Vector2[] uvs = new Vector2[6];

			vertices[0] = new Vector3(-10000, seaLevel, -10000);
			vertices[1] = new Vector3(-10000, seaLevel, 10000);
			vertices[2] = new Vector3(10000, seaLevel, -10000);
			vertices[3] = new Vector3(10000, seaLevel, 10000);
			vertices[4] = new Vector3(10000, seaLevel, -10000);
			vertices[5] = new Vector3(-10000, seaLevel, 10000);

			indices[0] = 0; indices[1] = 1; indices[2] = 2; indices[3] = 3; indices[4] = 4; indices[5] = 5;
			uvs[0] = uvs[1] = uvs[2] = uvs[3] = uvs[4] = uvs[5] = m_materialAgent.getUV(EPMPoint.PointType.Ocean);

			GameObject obj = new GameObject();
			Mesh mesh = new Mesh();
			obj.transform.SetParent(m_meshRoot.transform);
			obj.name = "SeaPlane";
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localEulerAngles = Vector3.zero;
			obj.transform.localScale = Vector3.one;
			mesh.vertices = vertices;
			mesh.SetIndices(indices, MeshTopology.Triangles, 0);
			mesh.uv = uvs;
			mesh.RecalculateNormals();
			obj.AddComponent<MeshFilter>().sharedMesh = mesh;
			MeshRenderer mr = obj.AddComponent<MeshRenderer>();
			mr.material = m_materialAgent.mat;
			mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			mr.receiveShadows = false;
		}

		public void CreateMesh()
		{
			int rowSplit = g_customData.rowSplit;
			int columnSplit = g_customData.columnSplit;

			//First, delete the former created Mesh.
			for (int i = m_meshRoot.transform.childCount - 1; i >= 0; i--)
			{
				GameObject.DestroyImmediate(m_meshRoot.transform.GetChild(i).gameObject);
			}
			m_meshRoot.transform.DetachChildren();

			//Split Mesh to smaller meshes.
			List<List<EPMTriangle>> submeshTriangleList = new List<List<EPMTriangle>>();
			for (int i = 0; i < rowSplit * columnSplit; i++) submeshTriangleList.Add(new List<EPMTriangle>());

			//Don't Draw Ocean, use an sea plane to replace it.
			List<EPMTriangle> triangleList = m_terrain.ExtractTriangles_ByTypes(null, EPMPoint.GenerateTypesInt(true, EPMPoint.PointType.Ocean));

			for (int i = 0; i < triangleList.Count; i++)
			{
				EPMTriangle t = triangleList[i];

				EPMPoint p1 = t.g_pointList[0];
				//Check which submesh the point p1 falls in, than put the triangle into this submesh list.
				Vector2d v2 = p1.pos2d;
				int column = (int)((v2.x - m_terrain.g_startPoint.x) / m_terrain.getXLength() * columnSplit);
				if (column >= columnSplit) column = columnSplit - 1;
				int row = (int)((v2.y - m_terrain.g_startPoint.y) / m_terrain.getZlength() * rowSplit);
				if (row >= rowSplit) row = rowSplit - 1;
				submeshTriangleList[column + row * columnSplit].Add(t);
			}

			for (int i = 0; i < submeshTriangleList.Count; i++)
			{
				if (submeshTriangleList[i].Count == 0) continue;
				string name = (i / columnSplit).ToString() + "_" + (i % columnSplit).ToString();
				CreateSubMesh(submeshTriangleList[i], name);
			}
			CreateSeaPlane((float)g_customData.seaLevel);
		}

		public void CreateSubMesh(List<EPMTriangle> triangleList, string name)
		{
			int pointsNum = triangleList.Count * 3;
			int indicesNum = pointsNum;

			Vector3[] vertices = new Vector3[pointsNum];
			int[] indices = new int[indicesNum];
			Vector2[] uvs = new Vector2[pointsNum];
			int currVertices = 0;
			int currIndices = 0;
			Vector2 uv = new Vector2(0, 0);
			for (int i = 0; i < triangleList.Count; i++)
			{
				EPMTriangle t = triangleList[i];
				EPMPoint p1, p2, p3;
				p1 = t.g_pointList[0];
				p2 = t.g_pointList[1];
				p3 = t.g_pointList[2];
				Vector3 v1= new Vector3((float)p1.posX, (float)p1.posY, (float)p1.posZ);
				Vector3 v2= new Vector3((float)p2.posX, (float)p2.posY, (float)p2.posZ);
				Vector3 v3= new Vector3((float)p3.posX, (float)p3.posY, (float)p3.posZ);

				vertices[currVertices] = v1;
				vertices[currVertices + 1] = v2;
				vertices[currVertices + 2] = v3;
				//Vector3 cross = Vector3.Cross(v3 - v1, v2 - v1);
				//if (Vector3.Dot(cross, Vector3.up) > 0)
				//{
					indices[currIndices] = currVertices;
					indices[currIndices + 1] = currVertices + 2;
					indices[currIndices + 2] = currVertices + 1;
				//}
				//else
				//{
				//	indices[currIndices] = currVertices;
				//	indices[currIndices + 1] = currVertices + 1;
				//	indices[currIndices + 2] = currVertices + 2;
				//}

				uv = m_materialAgent.getUV(t.g_shapeType);

				uvs[currVertices] = uv;
				uvs[currVertices + 1] = uv;
				uvs[currVertices + 2] = uv;

				currVertices += 3;
				currIndices += 3;
			}

			GameObject obj = new GameObject();
			Mesh mesh = new Mesh();
			obj.transform.SetParent(m_meshRoot.transform);
			obj.name = name;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localEulerAngles = Vector3.zero;
			obj.transform.localScale = Vector3.one;
			mesh.vertices = vertices;
			mesh.SetIndices(indices, MeshTopology.Triangles, 0);
			mesh.uv = uvs;
			mesh.RecalculateNormals();
			obj.AddComponent<MeshFilter>().sharedMesh = mesh;
			MeshRenderer mr = obj.AddComponent<MeshRenderer>();
			mr.material = m_materialAgent.mat;
			mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			mr.receiveShadows = false;
		}

		public void ExportTerrainMesh(string exportPath, string name)
		{
			if (!Directory.Exists(exportPath))
			{
				Directory.CreateDirectory(exportPath);
			}
			string directoryPath = exportPath + "/" + name;
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			for (int i = 0; i < m_meshRoot.transform.childCount; i++)
			{
				var filter = m_meshRoot.transform.GetChild(i).GetComponent<MeshFilter>();
				string realPath = directoryPath + "/" + name + "_" + filter.gameObject.name + ".obj";
				if (File.Exists(realPath))
				{
					File.Delete(realPath);
				}

				EPMObjExporter.Export(realPath, filter.sharedMesh, name, true);
			}

			m_materialAgent.ExportTexture(directoryPath + "/" + name + ".png");
		}
	}
}