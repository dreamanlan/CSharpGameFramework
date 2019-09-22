input("*.tga","*.png","*.jpg","*.exr")
{
	string("filter", "");
	string("notfilter", "/Select/");
	int("maxSize",128);
	float("bias",0);
	feature("source", "project");
	feature("menu", "1.Project Resources/Texture Mipmap Bias");
	feature("description", "just so so");
}
filter
{
	var(0) = loadasset(assetpath);
	var(1) = var(0).width;
	var(2) = var(0).height;
	var(3) = var(0).mipMapBias;
	//unloadasset(var(0));
	if((var(1) > maxSize || var(2) > maxSize) && assetpath.Contains(filter) && (notfilter=="" || !assetpath.Contains(notfilter)) && var(3)>=bias){
		info = "size:" + var(1) + "," + var(2) + " bias:" + var(3);
		value = var(3);
		1;
	} else {
		0;
	};
};