input("*.fbx")
{
  int("maxTriangleCount", 1000);
  bool("hasAnimation", false);
  bool("hasOffscreenUpdate", false);
  bool("hasAlwaysAnimate", false);
	string("filter", "");
	feature("source", "project");
	feature("menu", "1.Project Resources/Fbx");
	feature("description", "just so so");
}
filter
{
  var(0) = loadasset(assetpath);  
  var(1) = collectmeshinfo(var(0), importer);
	//unloadasset(var(0));
	order = var(1).triangleCount;
	if(var(1).triangleCount >= maxTriangleCount && assetpath.Contains(filter) && (!hasAnimation || var(1).clipCount>0) && (!hasOffscreenUpdate || var(1).updateWhenOffscreenCount>0) && (!hasAlwaysAnimate || var(1).alwaysAnimateCount>0)){
	  info = format("clip name:{0}, keyframe count:{1}, skinned mesh count:{2}, mesh filter count:{3}, vertex count:{4}, triangle count:{5}, bone count:{6}, material count:{7}, offscreen count:{8} always update count:{9}",
	    var(1).maxKeyFrameClipName, var(1).maxKeyFrameCount, var(1).skinnedMeshCount, var(1).meshFilterCount, var(1).vertexCount, var(1).triangleCount, var(1).boneCount, var(1).materialCount, var(1).updateWhenOffscreenCount, var(1).alwaysAnimateCount
	    );
	  1;
	}else{
	  0;
	};
};