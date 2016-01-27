using UnityEditor;
using UnityEngine;
using System.Collections;

public class PostprocessTexture : AssetPostprocessor {

	void OnPostprocessTexture(Texture2D texture)
	{
    /*
		string path = assetPath.ToLower();
		if(path == null ||path == "")
		{
			return;
		}
		TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
		if(textureImporter == null)
		{
			return;
		}
		textureImporter.textureType = TextureImporterType.Advanced;
		textureImporter.isReadable = false;
		textureImporter.mipmapEnabled = false;
        textureImporter.wrapMode = TextureWrapMode.Clamp;*/
		/*
		textureImporter.maxTextureSize = 1024;
		textureImporter.textureFormat = TextureImporterFormat.RGBA16;
		*/
	}
}
