using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EasyPolyMap
{
	using Core;

	public class EPMDrawMapWindow : EditorWindow
	{
		Texture2D showTexture,originTexture;
		int lastPosX, lastPosY;
		int textureWidth = 500;
		int textureHeight = 500;
		Vector2 scrollPos = Vector2.zero;
		private EPMPolyTerrainCreator m_creator;

		int sectionIndex;
		List<int> detailIndexList;
		string[] sectionNames = null;
		private List<Vector2d> currentLine = null;
		private bool dirty = true;

		GUIStyle fixedWidthButtonStyle = null;
		private Color lightingColor = Color.black;
		private bool lightingColorStrengthen = true;

		private float scale = 1;

		public static void CreateWindow(EPMPolyTerrainCreator creator)
		{
			EPMDrawMapWindow window = (EPMDrawMapWindow)GetWindow(typeof(EPMDrawMapWindow));
			window.Init(creator);
			window.Show();
		}

		void Init(EPMPolyTerrainCreator creator)
		{
			sectionIndex = 0;
			sectionNames = new string[7];
			sectionNames[0] = "Road";
			sectionNames[1] = "River";
			sectionNames[2] = "Hill";
			sectionNames[3] = "Mountain";
			sectionNames[4] = "SandGround";
			sectionNames[5] = "SoilGround";
			sectionNames[6] = "Island";
			detailIndexList = new List<int>();
			for (int i = 0; i < sectionNames.Length; i++) detailIndexList.Add(0);

			m_creator = creator;
			scale = Mathf.Floor(m_creator.g_customData.terrainWidth / 500 * 2) / 2;
			textureHeight = Mathf.RoundToInt(m_creator.g_customData.terrainLength / scale);
			textureWidth = Mathf.RoundToInt(m_creator.g_customData.terrainWidth / scale);

			int halfWidth = Mathf.Min(textureWidth / 2+100,Screen.currentResolution.width/2);
			int halfHeight = 350;
			position = new Rect(Screen.currentResolution.width / 2 - halfWidth, Screen.currentResolution.height / 2 - halfHeight, halfWidth * 2, halfHeight * 2);
			lastPosX = lastPosY = 0;
			showTexture = new Texture2D(textureWidth,textureHeight);
			originTexture = new Texture2D(textureWidth,textureHeight);
			ReDrawTexture();
			dirty = false;
		}

		void OnGUI()
		{
			if (dirty)
			{
				ReDrawTexture();
				dirty = false;
			}

			UpdateLightingColor();
			fixedWidthButtonStyle = new GUIStyle(GUI.skin.button);
			fixedWidthButtonStyle.fixedWidth = 50;
			if (Event.current != null && Event.current.mousePosition.x - 100 >=0 && Event.current.mousePosition.y - 130 >=0 &&
				Event.current.mousePosition.x + 16 <= position.width && Event.current.mousePosition.y + 16 <= position.height)  //Make sure the mouse postion is in the Draw Region
			{
				int posX, posY;
				posX = Mathf.RoundToInt((int)Event.current.mousePosition.x-100+scrollPos.x);
				posY = Mathf.RoundToInt((int)Event.current.mousePosition.y-130+scrollPos.y);
				posY = textureHeight - posY;
				if (posX>=0&&posX <= 5) posX = 0; //snap to border;
				if (posY>=0&&posY <= 5) posY = 0;
				if (posX<=textureWidth&&(textureWidth-posX) <=5) posX = textureWidth;
				if (posY<=textureHeight&&(textureHeight - posY)<=5) posY = textureHeight;
				if(isPositionInTexture(posX,posY))
				{
					DrawLightingPixel(posX, posY);
					if(sectionIndex<3) DrawLightingLine(posX, posY);
					else if(sectionIndex>3)
					{
						if(!isRegionClosed(currentLine))
						{
							DrawLightingLine(posX, posY);
						}
					}
					if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
					{
						if (currentLine != null)
						{
							if(sectionIndex>3)
							{
								if(!isRegionClosed(currentLine)) currentLine.Add(new Vector2d(posX * scale, posY * scale));
							}
							else
							{
								currentLine.Add(new Vector2d(posX * scale, posY * scale));
							}
							dirty = true;
						}
					}
					else if (Event.current.type == EventType.MouseDown & Event.current.button == 1)
					{
						if (currentLine != null && currentLine.Count > 0)
						{
							for (int i = currentLine.Count-1; i >=0;i--)
							{
								int x, y;
								GetScaledPosition(currentLine[i], out x, out y);
								int md = (x - posX) * (x - posX) + (y - posY) * (y - posY);
								if (md <= 25)
								{
									currentLine.RemoveAt(i);
									dirty = true;
									break;
								}
							}
						}
					}
					lastPosX = posX;
					lastPosY = posY;
				}
			}

			int newSection = GUI.SelectionGrid(new Rect(10, 10, this.position.width-20, 30), sectionIndex, sectionNames, sectionNames.Length);
			if(newSection!=sectionIndex)
			{
				dirty = true;
				sectionIndex = newSection;
			}
			List<List<Vector2d>> plist=GetSectionPointList();
			ShowSectionDetailList(plist);

			if(GUI.Button(new Rect(10,90,80,30),"New"))
			{
				if (plist != null)
				{
					plist.Add(new List<Vector2d>());
					detailIndexList[sectionIndex] = plist.Count-1;
					dirty = true;
				}
			}

			if (GUI.Button(new Rect(100, 90, 80, 30), "Delete"))
			{
				if (plist != null&&plist.Count>0)
				{
					plist.RemoveAt(detailIndexList[sectionIndex]);
					detailIndexList[sectionIndex] = 0;
					dirty = true;
				}
			}

			if(GUI.Button(new Rect(190,90,80,30),"CLEAR ALL"))
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
					currentLine = null;
					dirty = true;
				}
			}


			if(sectionIndex>=4)
			{
				if(GUI.Button(new Rect(280, 90, 100, 30), "CLOSE REGION"))
				{
					FinishRegion();
				}
				GUI.Label(new Rect(390, 90, this.position.width - 440, 30), "To Active region,\nPRESS CLOSE REGION");
			}
			else
			{
				GUI.Label(new Rect(280, 90, this.position.width - 300, 30), "Left Click to add new point.\nRight click ON POINT to delete.");
			}
			float newScale = EditorGUI.Slider(new Rect(position.width - 140, 95, 120, 20),scale, 1, 8);
			newScale = Mathf.Floor(newScale * 2) / 2;
			if (scale!=newScale)
			{
				scale = newScale;
				dirty = true;
			}

			scrollPos=GUI.BeginScrollView(new Rect(0, 130, this.position.width,this.position.height-130), scrollPos, new Rect(0, 0, textureWidth+200, textureHeight+100));
			GUI.DrawTexture(new Rect(100, 0, textureWidth, textureHeight), showTexture);
			GUI.EndScrollView();
			

		}

		List<List<Vector2d>> GetSectionPointList()
		{
			List<List<Vector2d>> plist = null;
			switch (sectionIndex)
			{
				case 0:
					plist = m_creator.g_customData.roadLists;
					break;
				case 1:
					plist = m_creator.g_customData.riverLists;
					break;
				case 2:
					plist = m_creator.g_customData.hillLists;
					break;
				case 3:
					plist = m_creator.g_customData.mountainLists;
					break;
				case 4:
					plist = m_creator.g_customData.sandGroundLists;
					break;
				case 5:
					plist = m_creator.g_customData.soilGroundLists;
					break;
				case 6:
					plist = m_creator.g_customData.islandLists;
					break;
			}
			return plist;
		}

		public void FinishRegion()
		{
			if (currentLine != null)
			{
				if (currentLine.Count < 3) return;
				if(!isRegionClosed(currentLine))
				{
					currentLine.Add(currentLine[0]);
				}
			}
			dirty = true;
		}

		void ShowSectionDetailList(List<List<Vector2d>> plist)
		{
			if (plist.Count == 0)
			{
				if (currentLine != null) RollbackLineList(currentLine);
				currentLine = null;
				return;
			}
			
			string[] indexString = new string[plist.Count];
			for (int i = 0; i < plist.Count; i++) indexString[i] = (i + 1).ToString();
			int selection = GUI.SelectionGrid(new Rect(10, 50, this.position.width - 20, 30), detailIndexList[sectionIndex], indexString, indexString.Length, fixedWidthButtonStyle);
			if(selection!=detailIndexList[sectionIndex])
			{
				dirty = true;
				detailIndexList[sectionIndex] = selection;
			}
			if (currentLine != null) RollbackLineList(currentLine);
			currentLine = plist[detailIndexList[sectionIndex]];
			if (currentLine != null&&sectionIndex!=3) DrawLightingLineList(currentLine);
		}

		void OnInspectorUpdate()
		{

			Repaint();
		}

		public void ReDrawTexture()
		{
			if(m_creator == null) return;

			textureHeight = Mathf.RoundToInt(m_creator.g_customData.terrainLength / scale);
			textureWidth = Mathf.RoundToInt(m_creator.g_customData.terrainWidth / scale);

			originTexture.Resize(textureWidth, textureHeight);
			showTexture.Resize(textureWidth, textureHeight);

			RedrawBase();

			ReDrawLineSections(m_creator.g_customData.hillLists, EPMMaterialAgent.GetDefaultColor(EPMPoint.PointType.Hill));
			ReDrawLineSections(m_creator.g_customData.roadLists, EPMMaterialAgent.GetDefaultColor(EPMPoint.PointType.Road));
			ReDrawLineSections(m_creator.g_customData.riverLists, EPMMaterialAgent.GetDefaultColor(EPMPoint.PointType.River));
			ReDrawCicleSections(m_creator.g_customData.mountainLists, EPMMaterialAgent.GetDefaultColor(EPMPoint.PointType.Mountain),Mathf.RoundToInt(30/scale));
			ReDrawLineSections(m_creator.g_customData.sandGroundLists, EPMMaterialAgent.GetDefaultColor(EPMPoint.PointType.Sand));
			ReDrawLineSections(m_creator.g_customData.soilGroundLists, EPMMaterialAgent.GetDefaultColor(EPMPoint.PointType.Soil));
			showTexture.Apply();
			originTexture.Apply();
		}

		public void RedrawBase()
		{
			Color[] c = new Color[textureHeight * textureWidth];

			List<List<Vector2>> islandLists = new List<List<Vector2>>();
			List<List<Vector2>> islandNormalLists = new List<List<Vector2>>(); //Pre-compute normal list to accelerate speed
			for (int i = 0; i < m_creator.g_customData.islandLists.Count; i++)
			{
				if (!isRegionClosed(m_creator.g_customData.islandLists[i])) continue;

				List<Vector2> island = new List<Vector2>();
				for(int j=0;j< m_creator.g_customData.islandLists[i].Count;j++)
				{
					int x, y;
					GetScaledPosition(m_creator.g_customData.islandLists[i][j],out x,out y);
					island.Add(new Vector2(x, y));
				}
				List<Vector2> normals = new List<Vector2>();
				for (int j = 1; j < island.Count; j++)
				{
					Vector2 dir = island[j] - island[j - 1];
					dir.Normalize();
					normals.Add(new Vector2(-dir.y, dir.x));
				}
				islandLists.Add(island);
				islandNormalLists.Add(normals);
			}

			if(m_creator.g_customData.islandLists.Count>0)
			{
				for (int i = 0; i < textureWidth; i++)
				{
					for (int j = 0; j < textureHeight; j++)
					{
						bool inRegion = false;
						for (int k = 0; k < islandLists.Count; k++)
						{
							if (isPointInRegion(islandLists[k], islandNormalLists[k], new Vector2(i, j)))
							{
								c[i + j * textureWidth] = Color.white;
								inRegion = true;
								break;
							}
						}
						if (!inRegion)
						{
							c[i + j * textureWidth] = Color.blue;
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < c.Length; i++) c[i] = Color.white;
			}

			showTexture.SetPixels(c);
			originTexture.SetPixels(c);
		}

		public void GetScaledPosition(Vector2d v2,out int x,out int y)
		{
			x = Mathf.RoundToInt((float)v2.x / scale);
			y = Mathf.RoundToInt((float)v2.y / scale);
		}

		public bool isRegionClosed(List<Vector2d> region)
		{
			if (region!=null&&region.Count >= 3 && region[0].x == region[region.Count - 1].x && region[0].y == region[region.Count - 1].y) return true;
			return false;
		}

		public bool isPointInRegion(List<Vector2> closedlinkedPointList, List<Vector2> normalList, Vector2 point)
		{
			int count = 0;
			for (int k = 0; k < closedlinkedPointList.Count - 1; k++)
			{
				if (closedlinkedPointList[k].y <= point.y)
				{
					if (closedlinkedPointList[k + 1].y > point.y)
					{
						if (Vector2.Dot(normalList[k], point - closedlinkedPointList[k]) > 0)
						{
							count++;
						}
					}
				}
				else
				{
					if (closedlinkedPointList[k + 1].y <= point.y)
					{
						if (Vector2.Dot(normalList[k], point - closedlinkedPointList[k]) < 0)
						{
							count--;
						}
					}
				}
			}
			if (count != 0)
			{
				return true;
			}
			return false;
		}

		public void ReDrawLineSections(List<List<Vector2d>> pointLists,Color color)
		{
			for(int i=0;i<pointLists.Count;i++)
			{
				List<Vector2d> l = pointLists[i];
				if (l.Count == 0) continue;
				int x1, y1, x2, y2;
				GetScaledPosition(l[0], out x1, out y1);
				DrawSquare(x1,y1, 1,1, color, true);
				for(int j=1;j<l.Count;j++)
				{
					GetScaledPosition(l[j - 1], out x1, out y1);
					GetScaledPosition(l[j], out x2, out y2);
					DrawLine(x1, y1, x2, y2, color, true);
					DrawSquare(x2, y2, 1,1, color, true);
				}
			}
		}

		public void ReDrawCicleSections(List<List<Vector2d>> pointLists,Color color,int radius)
		{
			for (int i = 0; i < pointLists.Count; i++)
			{
				List<Vector2d> l = pointLists[i];
				int x, y;
				for(int j=0;j<l.Count;j++)
				{
					GetScaledPosition(l[j], out x, out y);
					DrawCircle(x, y, radius, color, true);
				}	
			}
		}

		private void UpdateLightingColor()
		{
			float r = lightingColor.r;
			if (lightingColorStrengthen)
			{
				r += Time.deltaTime * 0.5f;
				if (r >= 1)
				{
					lightingColorStrengthen = !lightingColorStrengthen;
					r = 1;
				}
			}
			else
			{
				r -= Time.deltaTime * 0.5f;
				if (r <= 0.4f)
				{
					lightingColorStrengthen = !lightingColorStrengthen;
					r = 0.4f;
				}
			}
			lightingColor = new Color(r, r, r);
		}

		private void DrawLightingPixel(int posX,int posY)
		{
			RollbackCircle(lastPosX, lastPosY,5);
			DrawCircle(posX, posY, 5, lightingColor);
			showTexture.Apply();
		}

		private void DrawLightingLine(int posX,int posY)
		{
			if (currentLine == null || currentLine.Count == 0) return;
			int x, y;
			//if(sectionIndex>3&&currentLine.Count>1)
			//{
			//	GetScaledPosition(currentLine[currentLine.Count - 2].GetValue(), out x, out y);
			//}
			//else
			{
				GetScaledPosition(currentLine[currentLine.Count - 1], out x, out y);
			}
			
			RollbackLine(x, y, lastPosX, lastPosY);
			DrawLine(x, y, posX, posY,lightingColor);
			showTexture.Apply();
		}

		private void DrawLightingLineList(List<Vector2d> pointList)
		{
			if (pointList.Count == 0) return;
			int x1, y1, x2, y2;
			GetScaledPosition(pointList[0], out x1, out y1);
			DrawSquare(x1,y1, 1, 1, lightingColor, false);
			for (int j = 1; j < pointList.Count; j++)
			{
				GetScaledPosition(pointList[j - 1], out x1, out y1);
				GetScaledPosition(pointList[j], out x2, out y2);
				DrawLine(x1, y1, x2, y2, lightingColor, false);
				DrawSquare(x2,y2, 1, 1, lightingColor, false);
			}
			showTexture.Apply();
		}

		private void RollbackLineList(List<Vector2d> pointList)
		{
			if (pointList.Count == 0) return;
			int x1, y1, x2, y2;
			GetScaledPosition(pointList[0], out x1, out y1);
			RollbackSquare(x1, y1, 1, 1);
			for (int j = 1; j < pointList.Count; j++)
			{
				GetScaledPosition(pointList[j - 1], out x1, out y1);
				GetScaledPosition(pointList[j], out x2, out y2);
				RollbackLine(x1, y1, x2, y2);
				RollbackSquare(x2, y2, 1, 1);
			}
		}

		private void DrawSquare(int centerX,int centerY,int halfWidth,int halfHeight,Color color,bool both=false)
		{
			for(int i=centerX- halfWidth; i<=centerX+ halfWidth; i++)
			{
				for(int j=centerY- halfHeight; j<=centerY+ halfHeight; j++)
				{
					if(isPositionInTexture(i,j))
					{
						showTexture.SetPixel(i, j, color);
						if(both)
						{
							originTexture.SetPixel(i, j, color);
						}
					}
				}
			}
		}

		private void RollbackSquare(int centerX, int centerY, int halfWidth, int halfHeight)
		{
			for (int i = centerX - halfWidth; i <= centerX + halfWidth; i++)
			{
				for (int j = centerY - halfHeight; j <= centerY + halfHeight; j++)
				{
					if (isPositionInTexture(i, j))
					{
						showTexture.SetPixel(i, j, originTexture.GetPixel(i, j));
					}
				}
			}
		}

		private bool isPositionInTexture(int posX,int posY)
		{
			if(posX>=0&&posX<=textureWidth&&posY>=0&&posY<=textureHeight)
			{
				return true;
			}
			return false;
		}

		private void DrawCircle(int cx, int cy, int r, Color color,bool both=false)
		{
			int x, y, px, nx, py, ny, d;
			 for (x = 0; x <= r; x++)
			 {
				d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
				for (y = 0; y <= d; y++)
				{
					px = cx + x;
					nx = cx - x;
					py = cy + y;
					ny = cy - y;


					if (isPositionInTexture(px, py)) showTexture.SetPixel(px, py, color);
					if (isPositionInTexture(nx, py)) showTexture.SetPixel(nx, py, color);
					if (isPositionInTexture(px, ny)) showTexture.SetPixel(px, ny, color);
					if (isPositionInTexture(nx, ny)) showTexture.SetPixel(nx, ny, color);

					if(both)
					{
						if (isPositionInTexture(px, py)) originTexture.SetPixel(px, py, color);
						if (isPositionInTexture(nx, py)) originTexture.SetPixel(nx, py, color);
						if (isPositionInTexture(px, ny)) originTexture.SetPixel(px, ny, color);
						if (isPositionInTexture(nx, ny)) originTexture.SetPixel(nx, ny, color);
					}
				}
			 }
		}

		private void RollbackCircle(int cx, int cy, int r)
		{
			int x, y, px, nx, py, ny, d;
			for (x = 0; x <= r; x++)
			{
				d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
				for (y = 0; y <= d; y++)
				{
					px = cx + x;
					nx = cx - x;
					py = cy + y;
					ny = cy - y;


					if (isPositionInTexture(px, py)) showTexture.SetPixel(px, py, originTexture.GetPixel(px,py));
					if (isPositionInTexture(nx, py)) showTexture.SetPixel(nx, py, originTexture.GetPixel(nx, py));
					if (isPositionInTexture(px, ny)) showTexture.SetPixel(px, ny, originTexture.GetPixel(px, ny));
					if (isPositionInTexture(nx, ny)) showTexture.SetPixel(nx, ny, originTexture.GetPixel(nx, ny));
				}
			}
		}

		private void DrawLine(int x0, int y0, int x1, int y1, Color color,bool both=false)
		{
			int dy = (int)(y1 - y0);
			int dx = (int)(x1 - x0);
			int stepx, stepy;

			if (dy < 0) { dy = -dy; stepy = -1; }
			else { stepy = 1; }
			if (dx < 0) { dx = -dx; stepx = -1; }
			else { stepx = 1; }
			dy <<= 1;
			dx <<= 1;

			float fraction = 0;

			showTexture.SetPixel(x0, y0, color);
			if (both) originTexture.SetPixel(x0, y0, color);

			if (dx > dy)
			{
				fraction = dy - (dx >> 1);
				while (Mathf.Abs(x0 - x1) > 1)
				{
					if (fraction >= 0)
					{
						y0 += stepy;
						fraction -= dx;
					}
					x0 += stepx;
					fraction += dy;
					showTexture.SetPixel(x0, y0, color);
					if (both) originTexture.SetPixel(x0, y0, color);
				}
			}
			else
			{
				fraction = dx - (dy >> 1);
				while (Mathf.Abs(y0 - y1) > 1)
				{
					if (fraction >= 0)
					{
						x0 += stepx;
						fraction -= dy;
					}
					y0 += stepy;
					fraction += dx;
					showTexture.SetPixel(x0, y0, color);
					if (both) originTexture.SetPixel(x0, y0, color);
				}
			}
		}

		private void RollbackLine(int x0, int y0, int x1, int y1)
		{
			int dy = (int)(y1 - y0);
			int dx = (int)(x1 - x0);
			int stepx, stepy;

			if (dy < 0) { dy = -dy; stepy = -1; }
			else { stepy = 1; }
			if (dx < 0) { dx = -dx; stepx = -1; }
			else { stepx = 1; }
			dy <<= 1;
			dx <<= 1;

			float fraction = 0;

			showTexture.SetPixel(x0, y0, originTexture.GetPixel(x0,y0));

			if (dx > dy)
			{
				fraction = dy - (dx >> 1);
				while (Mathf.Abs(x0 - x1) > 1)
				{
					if (fraction >= 0)
					{
						y0 += stepy;
						fraction -= dx;
					}
					x0 += stepx;
					fraction += dy;
					showTexture.SetPixel(x0, y0, originTexture.GetPixel(x0, y0));
				}
			}
			else
			{
				fraction = dx - (dy >> 1);
				while (Mathf.Abs(y0 - y1) > 1)
				{
					if (fraction >= 0)
					{
						x0 += stepx;
						fraction -= dy;
					}
					y0 += stepy;
					fraction += dx;
					showTexture.SetPixel(x0, y0, originTexture.GetPixel(x0, y0));
				}
			}
		}
	}
}