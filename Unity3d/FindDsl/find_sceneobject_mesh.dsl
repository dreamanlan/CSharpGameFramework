input("MeshRenderer")
{
  int("maxTriangleCount", 1000);
	string("filter", "");
	feature("source", "sceneobjects");
}
filter
{ 
	var(1) = collectmeshinfo(object, importer);
	order = var(1).triangleCount;
	if(var(1).triangleCount >= maxTriangleCount && assetpath.Contains(filter)){
	  info = format("vertex count:{0}, triangle count:{1}, material count:{2}",
	    var(1).vertexCount, var(1).triangleCount, var(1).materialCount
	    );
	  1;
	}else{
	  0;
	};
};