input("t:shader")
{
	string("filter", "");
	float("pathwidth",240){range(20,4096);};
	feature("source", "allassets");
	feature("menu", "4.All Assets/Shaders");
	feature("description", "just so so");
}
filter
{
  if(assetpath.Contains(filter)){
  	var(0) = loadasset(assetpath);
  	order = getshaderpropertycount(var(0));  	
  	info = format("{0} property count:{1} texture count:{2}", getfilename(assetpath), order, getshaderpropertycount(var(0), 4));
    1;
  }else{
    0;
  };
};