input("MeshRenderer", "SkinnedMeshRenderer")
{
	string("filter", "");
	float("pathwidth",240){range(20,4096);};
	feature("source", "sceneobjects");
	feature("menu", "3.Current Scene Objects/Mesh Texture Ratio");
	feature("description", "just so so");
}
filter
{ 
	var(0) = calcmeshtexratio(object);
	var(1) = var(0)[0];
	var(2) = var(0)[1];
	var(3) = var(0)[2];
	var(4) = var(0)[3];
	order = changetype(var(2) * 1000,"int");
	if(var(2) > 0 && scenepath.Contains(filter)){
	  info = var(1);
	  1;
	}else{
	  0;
	};
};