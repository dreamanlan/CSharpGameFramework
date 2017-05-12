using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyPolyMap
{
	public class EPMDemoScene : MonoBehaviour
	{
		EPMPolyTerrainCreator m_terrainCreator = null;
		void Awake()
		{
			m_terrainCreator = GameObject.FindObjectOfType<EPMPolyTerrainCreator>();

			m_terrainCreator.Init();
			m_terrainCreator.ImportData();
			GameObject terrianRoot = m_terrainCreator.CreateTerrain();

			for (int i = 0; i < terrianRoot.transform.childCount; i++)
			{
				terrianRoot.transform.GetChild(i).gameObject.AddComponent<MeshCollider>();
			}
		}
		// Use this for initialization
		void Start()
		{
			
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}

