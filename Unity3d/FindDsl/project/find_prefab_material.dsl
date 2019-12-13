input("*.prefab")
{
	string("matName","Default");
	string("shaderName","Standard");
	float("pathwidth",240){range(20,4096);};
	feature("source", "project");
	feature("menu", "1.Project Resources/Prefab Material");
	feature("description", "just so so");
}
filter
{
	var(10) = 0;
  var(0) = loadasset(assetpath);
  var(1) = getcomponentsinchildren(var(0), "Renderer");
  looplist(var(1)){
  	var(2) = $$.sharedMaterials;
  	looplist(var(2)){
			if(isnull($$))
				continue;
  		var(3) = $$.name;
  		var(4) = $$.shader.name;
  		if(var(3).Contains(matName) && var(4).Contains(shaderName)){
		  	info = "mat:" + var(3) + " shader:" + var(4);
		  	var(10) = 1;  		
		  };
  	};
  };
  var(10);
};