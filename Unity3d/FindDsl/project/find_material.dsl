input("*.mat")
{
  string("shaderName","Standard");
	float("pathwidth",240){range(20,4096);};
	feature("source", "project");
	feature("menu", "1.Project Resources/Materials");
	feature("description", "just so so");
}
filter
{
	var(0) = loadasset(assetpath);
	var(1) = var(0).name;
	var(2) = var(0).shader.name;
	unloadasset(var(0));
	if(var(2)==shaderName){
  	info = "mat:" + var(1) + " shader:" + var(2);
  	1;
  }else{
    0;
  };
};