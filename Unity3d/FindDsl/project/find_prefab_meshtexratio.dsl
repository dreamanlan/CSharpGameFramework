input("*.prefab")
{
	string("filter", "");
	float("pathwidth",240){range(20,4096);};
	feature("source", "project");
	feature("menu", "1.Project Resources/Mesh Texture Ratio");
	feature("description", "just so so");
}
filter
{
	var(0) = loadasset(assetpath);
	if(isnull(var(0))){
		0;
	}else{
		var(0) = calcmeshtexratio(var(0), 1);
		var(1) = var(0)[0];
		var(2) = var(0)[1];
		var(3) = var(0)[2];
		var(4) = var(0)[3];
		order = changetype(var(2) * 1000,"int");
		if(var(2) > 0 && assetpath.Contains(filter)){
		  info = var(1);
		  1;
		}else{
		  0;
		};
	};
};