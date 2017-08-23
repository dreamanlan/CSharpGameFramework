input
{
  int("maxTriangleCount", 1000);
  int("areasize",10){
  	range(8,256);
  };
	feature("source", "sceneareas");
}
filter
{  
	order = area.triangleCount;
	if(area.triangleCount >= maxTriangleCount){
	  info = format("object count:{0}, vertex count:{1}, triangle count:{2}, material count:{3}, bone count:{4}, different material count:{5}", 
	    area.objects.Count, area.vertexCount, area.triangleCount, area.materialCount, area.boneCount, area.differentMaterialCount
	    );
	  1;
	}else{
	  0;
	};
};