input("*.tga","*.png","*.jpg","*.exr")
{
	int("maxSize",256){
		range(1,1024);
	};
	int("maxRefBy",3);
	string("prop",""){
		multiple(["readable","mipmap"],[1,2]);
	};
	stringlist("filter", "");
	stringlist("notfilter", "");
	float("pathwidth",240){range(20,4096);};
	feature("source", "project");
	feature("menu", "1.Project Resources/Textures with refby");
	feature("description", "just so so");
}
filter
{
    if(stringcontains(assetpath, filter) && stringnotcontains(assetpath, notfilter)){
    	var(0) = loadasset(assetpath);
    	if(isnull(var(0))){
    		0;
    	} else {
    		var(1) = var(0).width;
    		var(2) = var(0).height;
    		var(3) = importer.isReadable;
    		var(4) = importer.mipmapEnabled;
    		var(5) = gettexturesetting("iPhone");
        	var(6) = gettexturesetting("Android");
			var(7) = calcrefbycount(assetpath);
    		//unloadasset(var(0));
    		//order = var(1) < var(2) ? var(2) : var(1);
    		if((var(1) > maxSize || var(2) > maxSize) && (var(5).maxTextureSize > maxSize || var(6).maxTextureSize > maxSize) && (prop.Contains("1") && var(3) || !prop.Contains("1")) && (prop.Contains("2") && var(4) || !prop.Contains("2")) && var(7)<=maxRefBy){
    		    var(8) = getreferencebyassets(assetpath);
    		    looplist(var(8)){
    		        $asset = $$;
    		        var(9) = newitem();
    		        var(9).AssetPath = assetpath;
    		        var(9).ScenePath = "";
    		        var(9).Info = format("size:{0},{1} readable:{2} mipmap:{3} refby_count:{4} refby_asset:{5}", var(1), var(2), var(3), var(4), var(7), $asset);
    		        var(9).Order = var(7);
    		    };
    		};
    	};
    };
    0;
};