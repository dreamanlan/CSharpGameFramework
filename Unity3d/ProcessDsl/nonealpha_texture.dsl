input("*.tga","*.png","*.jpg","*.exr")
{
	string("filter", "");
	feature("source", "project");
	feature("menu", "1.Project Resources/Set None Alpha Texture");
	feature("description", "just so so");
}
filter
{
	var(0) = loadasset(assetpath);
	var(1) = var(0).width;
	var(2) = var(0).height;
	//unloadasset(var(0));
	if(!istexturenoalphasource() && assetpath.Contains(filter)){
		info = "size:" + var(1) + "," + var(2);
		1;
	} else {
		0;
	};
}
process
{
	setnonealphatexture();
	debuglog("processed:{0}",assetpath);
	saveandreimport();
};