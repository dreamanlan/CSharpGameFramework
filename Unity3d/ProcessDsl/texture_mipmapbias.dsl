input("*.tga","*.png","*.jpg","*.exr")
{
	stringlist("filter", "");
	stringlist("notfilter", "/Select/");
	int("maxSize",512);
	float("bias",1);
	float("pathwidth",240){range(20,4096);};
	feature("source", "project");
	feature("menu", "1.Project Resources/Texture Mipmap Bias");
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
    	var(5) = var(0).mipMapBias;
    	//unloadasset(var(0));
    	if((var(1) > maxSize || var(2) > maxSize) && (var(3).maxTextureSize > maxSize || var(4).maxTextureSize > maxSize) && var(5)!=bias){
    		info = "size:" + var(1) + "," + var(2);
    		1;
    	} else {
    		0;
    	};
    }else{
        0;  
    };
}
process
{
	importer.mipMapBias = bias;
    saveandreimport();
};