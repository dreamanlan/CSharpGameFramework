input("Renderer")
{
	string("matName","Default");
	string("shaderName","Standard");
	float("pathwidth",240){range(20,4096);};
	feature("source", "sceneobjects");
	feature("menu", "3.Current Scene Objects/Materials");
	feature("description", "just so so");
}
filter
{
	var(0) = 0;
	var(1) = getcomponent(object, "Renderer");
	var(2) = var(1).sharedMaterials;
	looplist(var(2)){
		if(isnull($$))
			continue;
		var(3) = $$.name;
		var(4) = $$.shader.name;
		if(var(3).Contains(matName) && var(4).Contains(shaderName)){
	  	info = "mat:" + var(3) + " shader:" + var(4);
	  	var(0) = 1;  		
	  };
	};
	var(0);
};