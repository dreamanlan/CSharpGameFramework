input("*.tga","*.png","*.jpg","*.exr")
{
	stringlist("filter", "");
	stringlist("notfilter", "/Select/");
	bool("sizeLE",false);
	int("maxSize",512);
	float("bias",1);
	bool("eq",true);
	float("pathwidth",240){range(20,4096);};
	feature("source", "project");
	feature("menu", "1.Project Resources/Texture Mipmap Bias");
	feature("description", "just so so");
}
filter
{
    var(99)=0;
    if(stringcontains(assetpath, filter) && stringnotcontains(assetpath, notfilter)){
        var(0) = loadasset(assetpath);
        var(1) = var(0).width;
        var(2) = var(0).height;
        var(3) = var(0).mipMapBias;
        var(4) = gettexturesetting("iPhone");
        var(5) = gettexturesetting("Android");
        //unloadasset(var(0));
        if(eq && var(3)==bias || !eq && var(3)<bias){
            if(sizeLE && (var(1) <= maxSize || var(2) <= maxSize) && (var(4).maxTextureSize <= maxSize || var(5).maxTextureSize <= maxSize) || 
                !sizeLE && (var(1) > maxSize || var(2) > maxSize) && (var(4).maxTextureSizesizeq > maxSize || var(5).maxTextureSize > maxSize)){
                info = "size:" + var(1) + "," + var(2) + " bias:" + var(3) + " ios_size:"+var(4).maxTextureSize+" android_size:"+var(5).maxTextureSize;
                order = var(1)<var(2) ? var(2) : var(1);
                value = var(3);
                var(99)=1;
            };
        };
    };
    var(99);
};