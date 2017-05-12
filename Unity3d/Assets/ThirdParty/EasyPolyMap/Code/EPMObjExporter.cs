using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace EasyPolyMap
{
	using Core;

	public class EPMObjExporter
	{
		//When importing obj mesh to unity, unity will flip the x coordinate, so... I flipped the x coordinate in advance for Unity to flip it back........ T_T
		public static void Export(string path,Mesh mesh,string meshName,bool flipX=false)
		{
			FileStream fstream = new FileStream(path, FileMode.Create);
			Vector3[] vertices = mesh.vertices;
			Vector2[] uvs = mesh.uv;
			Vector3[] normals = mesh.normals;
			int[] indices = mesh.triangles;

			StreamWriter writer = new StreamWriter(fstream);
			writer.WriteLine("# OBJ File exported by EasyPolyMap");

			if(flipX)
			{
				for (int i = 0; i < vertices.Length; i++)
				{
					Vector3 v3 = vertices[i];
					writer.WriteLine(string.Format("v {0} {1} {2}", (-v3.x).ToString("g8"), v3.y.ToString("g8"), v3.z.ToString("g8")));
				}
				for (int i = 0; i < normals.Length; i++)
				{
					Vector3 v3 = normals[i];
					writer.WriteLine(string.Format("vn {0} {1} {2}", (-v3.x).ToString("g8"), v3.y.ToString("g8"), v3.z.ToString("g8")));
				}
			}
			else
			{
				for (int i = 0; i < vertices.Length; i++)
				{
					Vector3 v3 = vertices[i];
					writer.WriteLine(string.Format("v {0} {1} {2}", v3.x.ToString("g8"), v3.y.ToString("g8"), v3.z.ToString("g8")));
				}

				for (int i = 0; i < normals.Length; i++)
				{
					Vector3 v3 = normals[i];
					writer.WriteLine(string.Format("vn {0} {1} {2}", v3.x.ToString("g8"), v3.y.ToString("g8"), v3.z.ToString("g8")));
				}
			}


			for (int i = 0; i < uvs.Length; i++)
			{
				Vector2 vt = uvs[i];
				writer.WriteLine(string.Format("vt {0} {1}", vt.x.ToString("g8"), vt.y.ToString("g8")));
			}

			writer.WriteLine(string.Format("g {0}", meshName));

			if(flipX)
			{
				for (int i = 0; i < indices.Length; i += 3)
				{
					string temp = string.Format("f {0}/{1}/{2}", indices[i] + 1, indices[i] + 1, indices[i] + 1);
					temp += string.Format(" {0}/{1}/{2}", indices[i + 2] + 1, indices[i + 2] + 1, indices[i + 2] + 1);
					temp += string.Format(" {0}/{1}/{2}", indices[i + 1] + 1, indices[i + 1] + 1, indices[i + 1] + 1);
					writer.WriteLine(temp);
				}
			}
			else
			{
				for (int i = 0; i < indices.Length; i += 3)
				{
					string temp = string.Format("f {0}/{1}/{2}", indices[i] + 1, indices[i] + 1, indices[i] + 1);
					temp += string.Format(" {0}/{1}/{2}", indices[i + 1] + 1, indices[i + 1] + 1, indices[i + 1] + 1);
					temp += string.Format(" {0}/{1}/{2}", indices[i + 2] + 1, indices[i + 2] + 1, indices[i + 2] + 1);
					writer.WriteLine(temp);
				}
			}

			writer.Flush();

			fstream.Flush();
			fstream.Close();
		}
	}
}

