input("*.tga","*.png","*.jpg","*.exr")
{
	stringlist("filter", "");
	stringlist("notfilter", "/Select/");
	int("maxSize",128);
	float("bias",0);
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
    	var(3) = var(0).mipMapBias;
		var(4) = gettexturesetting("iPhone");
    	var(5) = gettexturesetting("Android");
    	//unloadasset(var(0));
    	if((var(1) > maxSize || var(2) > maxSize) && var(3)==bias && (var(4).maxTextureSize > maxSize || var(5).maxTextureSize > maxSize)){
    		info = "size:" + var(1) + "," + var(2) + " bias:" + var(3) + " ios_size:"+var(4).maxTextureSize+" android_size:"+var(5).maxTextureSize;
    		order = var(1)<var(2) ? var(2) : var(1);
    		value = var(3);
    		1;
    	} else {
    		0;
    	};
    }else{
        0;      
    };
};