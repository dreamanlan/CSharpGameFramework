using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyPolyMap
{
	using Core;

	public class EPMPreDefinedTools
	{
		public static Vector2 TranslateVector2d(Vector2d v2d)
		{
			return new Vector2((float)v2d.x, (float)v2d.y);
		}

		public static List<Vector2> TranslateVector2dList(List<Vector2d> v2dList)
		{
			List<Vector2> vlist = new List<Vector2>();
			for(int i=0;i<v2dList.Count;i++)
			{
				Vector2d v2 = v2dList[i];
				vlist.Add(new Vector2((float)v2.x,(float)v2.y));
			}
			return vlist;
		}

		public static List<List<Vector2>> TranslateVector2dLists(List<List<Vector2d>> v2dLists)
		{
			List<List<Vector2>> vlists = new List<List<Vector2>>();
			for (int i = 0; i < v2dLists.Count; i++)
			{
				vlists.Add(TranslateVector2dList(v2dLists[i]));
			}
			return vlists;
		}

		public static void CheckAndSetTileTypeByNeighbor(EPMDelaunayTerrain terrain, EPMPoint.PointType checkType, EPMPoint.PointType neighborType, EPMPoint.PointType newTypeForPassCheck, int minNeighborCount = 2, bool shareEdge = true, bool alsoSetPointsType = false)
		{
			CheckAndSetTileTypeByNeighbor(terrain,EPMPoint.GenerateTypesInt(false, checkType), EPMPoint.GenerateTypesInt(false, neighborType), newTypeForPassCheck, minNeighborCount, shareEdge, alsoSetPointsType);
		}

		public static void CheckAndSetTileTypeByNeighbor(EPMDelaunayTerrain terrain,EPMPoint.PointType checkType, EPMPoint.PointType neighborType, EPMPoint.PointType newTypeForPassCheck, EPMPoint.PointType newTypeForNotPassCheck, int minNeighborCount = 2, bool shareEdge = true, bool alsoSetPointsType = false)
		{
			CheckAndSetTileTypeByNeighbor(terrain,EPMPoint.GenerateTypesInt(false, checkType), EPMPoint.GenerateTypesInt(false, neighborType), newTypeForPassCheck, newTypeForNotPassCheck, true, minNeighborCount, shareEdge, alsoSetPointsType);
		}

		public static void CheckAndSetTileTypeByNeighbor(EPMDelaunayTerrain terrain, int checkTypeEnum, int neighborTypeEnum, EPMPoint.PointType newTypeForPassCheck, int minNeighborCount = 2, bool shareEdge = true, bool alsoSetPointsType = false)
		{
			CheckAndSetTileTypeByNeighbor(terrain, checkTypeEnum, neighborTypeEnum, newTypeForPassCheck, EPMPoint.PointType.Ground, false, minNeighborCount, shareEdge, alsoSetPointsType);
		}

		public static void CheckAndSetTileTypeByNeighbor(EPMDelaunayTerrain terrain, int checkTypeEnum, int neighborTypeEnum, EPMPoint.PointType newTypeForPassCheck, EPMPoint.PointType newTypeForNotPassCheck, bool processNotPassChcek = true, int minNeighborCount = 2, bool shareEdge = true, bool alsoSetPointsType = false)
		{
			List<EPMTriangle> triangleList = terrain.ExtractTriangles_ByTypes(null, checkTypeEnum);
			if (shareEdge)
			{
				for (int i = 0; i < triangleList.Count; i++)
				{
					EPMTriangle t = (EPMTriangle)triangleList[i];
					int count = 0;
					for (int j = 0; j < t.g_shareEdgeNeighbors.Count; j++)
					{
						if (t.g_shareEdgeNeighbors[j].HasShapeType(neighborTypeEnum)) count++;
						if (count >= minNeighborCount)
						{
							t.g_visited = true;
							break;
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < triangleList.Count; i++)
				{
					EPMTriangle t = (EPMTriangle)triangleList[i];
					int count = 0;
					for (int j = 0; j < t.g_sharePointNeighbors.Count; j++)
					{
						if (t.g_sharePointNeighbors[j].HasShapeType(neighborTypeEnum)) count++;
						if (count >= minNeighborCount)
						{
							t.g_visited = true;
							break;
						}
					}
				}
			}

			for (int i = 0; i < triangleList.Count; i++)
			{
				EPMTriangle t = triangleList[i];
				if (t.g_visited)
				{
					t.g_visited = false;
					t.SetType(newTypeForPassCheck);
					if (alsoSetPointsType)
					{
						t.g_pointList[0].g_type = newTypeForPassCheck;
						t.g_pointList[1].g_type = newTypeForPassCheck;
						t.g_pointList[2].g_type = newTypeForPassCheck;
					}

				}
				else if (processNotPassChcek)
				{
					t.SetType(newTypeForNotPassCheck);
					if (alsoSetPointsType)
					{
						t.g_pointList[0].g_type = newTypeForNotPassCheck;
						t.g_pointList[1].g_type = newTypeForNotPassCheck;
						t.g_pointList[2].g_type = newTypeForNotPassCheck;
					}
				}
			}
		}

		public static void ExtractSide(EPMDelaunayTerrain terrain, EPMPoint.PointType oldType, EPMPoint.PointType sideType, double inLength = 2f)
		{
			List<EPMTriangle> list = terrain.ExtractTriangles_ByTypes(null, EPMPoint.GenerateTypesInt(false, oldType));
			List<EPMPoint> allPoints = terrain.ExtractPoints_All();
			List<EPMTriangle> sideList = new List<EPMTriangle>();
			int groundEnum = EPMPoint.GetGroundEnums();
			for (int i = 0; i < list.Count; i++)
			{
				EPMTriangle t = (EPMTriangle)list[i];
				int count = 0;
				int ep1, ep2;
				ep1 = ep2 = 0;
				for (int j = 0; j < t.g_shareEdgeNeighbors.Count; j++)
				{
					//The edge has an ground tile neighbor.
					if (t.g_shareEdgeNeighbors[j].HasShapeType(groundEnum))
					{
						count++;
						t.GetShareEdge((EPMTriangle)t.g_shareEdgeNeighbors[j], out ep1, out ep2);
					}
				}

				if (count == 1)
				{
					if (!t.g_visited)
					{
						sideList.Add(t);
						t.g_visited = true;
					}

					int ep3 = t.g_sortedIndexList[0];
					if (ep3 == ep1 || ep3 == ep2) ep3 = t.g_sortedIndexList[1];
					if (ep3 == ep1 || ep3 == ep2) ep3 = t.g_sortedIndexList[2];

					Vector2d p1, p2, p3;
					p1 =allPoints[ep1].pos2d;
					p2 = allPoints[ep2].pos2d;
					p3 = allPoints[ep3].pos2d;

					Vector2d dir = p2 - p1;
					Vector2d normal = new Vector2d(-dir.y, dir.x);
					normal.Normalize();
					if ((p3 - p1)* normal < 0) normal = -normal;

					for (int k = 0; k < t.g_shareEdgeNeighbors.Count; k++)
					{
						if (t.g_shareEdgeNeighbors[k].HasShapeType(oldType))
						{
							EPMTriangle nei = (EPMTriangle)t.g_shareEdgeNeighbors[k];
							if (nei.g_visited == false)
							{
								nei.g_visited = true;
								sideList.Add(nei);
							}
							for (int j = 0; j < nei.g_sharePointNeighbors.Count; j++)
							{
								EPMTriangle t2 = (EPMTriangle)nei.g_sharePointNeighbors[j];
								if (t2.HasShapeType(oldType) && t2.g_visited == false)
								{
									Vector2d v2 = t2.g_pointList[0].pos2d;
									double dot = (v2 - p1) * normal;
									if (dot >= inLength) continue;

									v2 = t2.g_pointList[1].pos2d;
									dot = (v2 - p1) * normal;
									if (dot >= inLength) continue;

									v2 = t2.g_pointList[2].pos2d;
									dot = (v2 - p1) * normal;
									if (dot >= inLength) continue;

									t2.g_visited = true;
									sideList.Add((EPMTriangle)nei.g_sharePointNeighbors[j]);
								}
							}
						}
					}
				}
			}

			for (int i = 0; i < sideList.Count; i++)
			{
				sideList[i].SetType(sideType);
			}

			for (int i = 0; i < list.Count; i++)
			{
				EPMTriangle t = (EPMTriangle)list[i];
				if (t.HasShapeType(sideType)) continue;
				bool finded = true;
				for (int j = 0; j < t.g_shareEdgeNeighbors.Count; j++)
				{
					if (t.g_shareEdgeNeighbors[j].HasShapeType(oldType))
					{
						finded = false;
						break;
					}
				}
				if (finded)
				{
					t.SetType(sideType);
				}
			}

			for (int i = 0; i < list.Count; i++)
			{
				list[i].g_visited = false;
			}
		}

		public static void ExtractCenter(EPMDelaunayTerrain terrain,List<List<Vector2d>> pointList, EPMPoint.PointType oldType, EPMPoint.PointType newType, double halfWidth)
		{
			List<EPMTriangle> intersectList = new List<EPMTriangle>();
			List<EPMTriangle> oldTypeList = terrain.ExtractTriangles_ByTypes(null, EPMPoint.GenerateTypesInt(false, oldType));
			for (int i = 0; i < pointList.Count; i++)
			{
				List<Vector2d> lineSet = pointList[i];
				for (int j = 0; j < lineSet.Count; j++)
				{
					intersectList.AddRange(terrain.ExtractTriangles_PointsInCircle(lineSet[j], halfWidth, 1, oldTypeList));
					if (j >= 1)
					{
						Vector2d dir = lineSet[j] - lineSet[j - 1];
						dir.Normalize();
						Vector2d normal = new Vector2d(-dir.y, dir.x);
						intersectList.AddRange(terrain.ExtractTriangles_PointsInLineWithDistance(lineSet[j - 1] - dir * halfWidth, lineSet[j] + dir * halfWidth, halfWidth, 1, oldTypeList));
						intersectList.AddRange(terrain.ExtractTriangles_IntersectLine(lineSet[j - 1] + normal * halfWidth - dir * halfWidth, lineSet[j] + normal * halfWidth + dir * halfWidth, oldTypeList));
						intersectList.AddRange(terrain.ExtractTriangles_IntersectLine(lineSet[j - 1] - normal * halfWidth - dir * halfWidth, lineSet[j] - normal * halfWidth + dir * halfWidth, oldTypeList));
					}
				}
			}

			for (int i = 0; i < intersectList.Count; i++)
			{
				EPMTriangle t = (EPMTriangle)intersectList[i];
				t.g_pointList[0].g_type = newType;
				t.g_pointList[1].g_type = newType;
				t.g_pointList[2].g_type = newType;
				t.SetType(newType);
			}
			intersectList.Clear();
			CheckAndSetTileTypeByNeighbor(terrain, oldType, newType, newType);
		}

		public static void SampleHeightByPoint(EPMBaseTerrain terrain, int pointNum, Vector2d radiusRange, EPMHeightSampler.CurveMode curve, EPMHeightSampler.CombineMode combine, double limitation)
		{
			List<EPMPoint> pointList = terrain.ExtractPoints_ByTypes(null, EPMPoint.GenerateTypesInt(true, EPMPoint.PointType.Ocean));
			EPMPoint.HighestHeight = EPMPoint.LowestHeight = 0;
			for (int i = 0; i < pointNum; i++)
			{
				double radius = (double)terrain.RandomDouble(radiusRange.x, radiusRange.y);
				double height = (double)terrain.RandomDouble(-1f, 1f);
				Vector2d point = new Vector2d(terrain.RandomDouble(0, terrain.getXLength()), terrain.RandomDouble(0f, terrain.getZlength()));
				EPMHeightSampler.DistanceToPoint(height, point, radius, pointList, curve, combine, limitation);
			}
		}

		public static void SampleHeightByLine(EPMBaseTerrain terrain,int lineNum, Vector2d radiusRange, EPMHeightSampler.CurveMode curve, EPMHeightSampler.CombineMode combine, double limitation)
		{
			List<EPMPoint> points = terrain.ExtractPoints_ByTypes(null, EPMPoint.GenerateTypesInt(true, EPMPoint.PointType.Ocean));
			EPMPoint.HighestHeight = EPMPoint.LowestHeight = 0;
			for (int i = 0; i < lineNum; i++)
			{
				double radius = terrain.RandomDouble(radiusRange.x, radiusRange.y);
				double height = terrain.RandomDouble(-1f,1f);
				Vector2d point1 = new Vector2d(terrain.RandomDouble(0, terrain.getXLength()), terrain.RandomDouble(0f, terrain.getZlength()));
				Vector2d point2 = new Vector2d(terrain.RandomDouble(0, terrain.getXLength()), terrain.RandomDouble(0f, terrain.getZlength()));
				EPMHeightSampler.DistanceToLine(height, point1, point2, radius, points, curve, combine, limitation);
			}
		}
	}
	
}
