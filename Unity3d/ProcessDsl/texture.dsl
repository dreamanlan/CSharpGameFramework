input("*.tga","*.png","*.jpg","*.exr")
{
	string("filter", "");
	string("notfilter", "/Select/");
	int("maxSize",1024);
	float("pathwidth",240){range(20,4096);};
	feature("source", "project");
	feature("menu", "1.Project Resources/Texture Size");
	feature("description", "just so so");
}
filter
{
    if(stringcontains(assetpath, filter) && stringnotcontains(assetpath, notfilter)){
    	var(0) = loadasset(assetpath);
    	var(1) = var(0).width;
    	var(2) = var(0).height;
		var(3) = gettexturesetting("iPhone");
    	var(4) = gettexturesetting("Android");
    	//unloadasset(var(0));
    	if((var(1) > maxSize || var(2) > maxSize) && (var(3).maxTextureSize > maxSize || var(4).maxTextureSize > maxSize)){
    		info = "size:" + var(1) + "," + var(2);
    		1;
    	} else {
    		0;
    	};
	} else {
		0;
	};
}
process
{
	/*
	var(0) = getdefaulttexturesetting();
	var(0).maxTextureSize = changetype(maxSize, "int");
	settexturesetting(var(0));
	
	var(1) = gettexturesetting("Standalone");
	var(1).maxTextureSize = changetype(maxSize, "int");
	settexturesetting(var(1));
	*/
	var(2) = gettexturesetting("iPhone");
	var(2).overridden=true;
	var(2).maxTextureSize = changetype(maxSize, "int");
	setastctexture(var(2));
	settexturesetting(var(2));
	
	var(3) = gettexturesetting("Android");
	var(3).overridden=true;
	var(3).maxTextureSize = changetype(maxSize, "int");
	setastctexture(var(3));
	settexturesetting(var(3));
	
  saveandreimport();
};