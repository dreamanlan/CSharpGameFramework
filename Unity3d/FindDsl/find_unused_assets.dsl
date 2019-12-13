input("*.tga","*.png","*.jpg","*.fbx","*.exr","*.mat","*.controller","*.prefab","*.asset")
{
	string("filter", "");
	float("pathwidth",240){range(20,4096);};
	feature("source", "unusedassets");
	feature("menu", "5.Unused Resources");
	feature("description", "just so so");
}
filter
{
  if(assetpath.Contains(filter) && !assetpath.Contains("/SceneLight/") && !assetpath.Contains("/SceneVoxel/") && !assetpath.EndsWith("_split.asset") 
  	 	&& (assetpath.StartsWith("Assets/GUI/") || 
  	 		assetpath.StartsWith("Assets/M1Tool/") || 
  	 		assetpath.StartsWith("Assets/PostProcessing/") || 
  	 		assetpath.StartsWith("Assets/ResourceAB/") || 
  	 		assetpath.StartsWith("Assets/Resources/") || 
  	 		assetpath.StartsWith("Assets/SceneRes/") || 
  	 		assetpath.StartsWith("Assets/Scenes/") || 
  	 		assetpath.StartsWith("Assets/Story/") || 
  	 		assetpath.StartsWith("Assets/timeline/")
  	 		)){
    info = stringjoin(",", getreferencebyassets(assetpath));
    1;
  }else{
    0;
  };
};