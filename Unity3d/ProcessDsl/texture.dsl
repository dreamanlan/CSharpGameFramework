input("*.tga","*.png","*.jpg","*.exr")
{
	string("filter", "");
	string("notfilter", "/Select/");
	int("maxSize",1024);
	feature("source", "project");
	feature("menu", "Project Resources/Texture Size");
	feature("description", "just so so");
}
filter
{
	var(0) = loadasset(assetpath);
	var(1) = var(0).width;
	var(2) = var(0).height;
	//unloadasset(var(0));
	if((var(1) > maxSize || var(2) > maxSize) && assetpath.Contains(filter) && (notfilter=="" || !assetpath.Contains(notfilter))){
		info = "size:" + var(1) + "," + var(2);
		1;
	} else {
		0;
	};
}
process
{
	var(1) = gettexturesetting("iPhone");
	var(1).overridden=true;
	var(1).maxTextureSize = changetype(maxSize, "int");
	setastctexture(var(1));
	settexturesetting(var(1));
	
	var(2) = gettexturesetting("Android");
	var(2).overridden=true;
	var(2).maxTextureSize = changetype(maxSize, "int");
	setastctexture(var(2));
	settexturesetting(var(2));
	
    saveandreimport();
};