using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace EasyPolyMap
{
	using Core;

	public class EPMMaterialAgent
	{
		public Material mat = null;
		Texture2D texture = null;

		public EPMMaterialAgent()
		{
			texture = new Texture2D(64, 64);
			mat = new Material(Shader.Find("Legacy Shaders/Diffuse"));
			mat.mainTexture = texture;
			mat.color = Color.white;
		}

		public void SetColor(EPMPoint.PointType type,Color color)
		{
			int t = Mathf.RoundToInt(Mathf.Log((int)type,2f));
			int row = t / 8;
			int column = t % 8;
			int start_x = column * 8;
			int start_y = row * 8;
			for(int i=0;i<8;i++)
			{
				for(int j=0;j<8;j++)
				{
					texture.SetPixel(start_x + i, start_y + j, color);
				}
			}
			texture.Apply();
		}

		public Vector2 getUV(EPMPoint.PointType type)
		{
			int t = Mathf.RoundToInt(Mathf.Log((int)type, 2f));
			int row = t / 8;
			int column = t % 8;

			Vector2 ret = new Vector2();
			ret.x = column / 8f + 1/16f;
			ret.y = row / 8f + 1/16f;
			return ret;
		}

		public void ExportTexture(string path)
		{
			texture.EncodeToPNG();
			File.WriteAllBytes(path, texture.EncodeToPNG());
		}

		public static Color GetDefaultColor(EPMPoint.PointType type)
		{
			switch (type)
			{
				case EPMPoint.PointType.Ground:
					return new Color(75 / 255f, 161 / 255f, 75 / 255f);
				case EPMPoint.PointType.Mountain:
					return new Color(0.3f, 0.1f, 0.1f);
				case EPMPoint.PointType.MountainSummit:
					return new Color(1f, 1f, 1f);
				case EPMPoint.PointType.Hill:
					return new Color(178 / 255f, 126 / 255f, 126 / 255f);
				case EPMPoint.PointType.Road:
					return new Color(200 / 255f, 200 / 255f, 200 / 255f);
				case EPMPoint.PointType.RoadSide:
					return new Color(143 / 255f, 122 / 255f, 85 / 255f);
				case EPMPoint.PointType.River:
					return new Color(7 / 255f, 152 / 255f, 253 / 255f);
				case EPMPoint.PointType.RiverSide:
					return new Color(75 / 255f, 63 / 255f, 23 / 255f);
				case EPMPoint.PointType.Sand:
					return new Color(165 / 255f, 165 / 255f, 33 / 255f);
				case EPMPoint.PointType.Soil:
					return new Color(73 / 255f, 53 / 255f, 49 / 255f);
				case EPMPoint.PointType.Ocean:
					return new Color(0, 100 / 255f, 200 / 255f);
				case EPMPoint.PointType.OceanSide:
					return new Color(139 / 255f, 125 / 255f, 78 / 255f);
				default:
					return new Color(1,0,0);
			}
		}
	}
}
