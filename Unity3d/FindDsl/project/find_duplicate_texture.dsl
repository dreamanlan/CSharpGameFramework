input("*.tga","*.png","*.jpg")
{
	int("maxSize",256){
		range(1,1024);
	};
	string("prop",""){
		multiple(["readable","mipmap"],[1,2]);
	};
	string("filter", "");
	string("notfilter", "");
	string("style", "itemlist"){
		popup(["itemlist", "grouplist"]);
	};
	int("duptype",1){
		toggle(["md5","guid"],[1,2]);
	};
	float("pathwidth",240){range(20,4096);};
	feature("source", "project");
	feature("menu", "1.Project Resources/Duplicate Textures");
	feature("description", "just so so");
}
filter
{
	var(0) = loadasset(assetpath);
	if(isnull(var(0))){
		0;
	} else {;
		var(1) = var(0).width;
		var(2) = var(0).height;
		var(3) = importer.isReadable;
		var(4) = importer.mipmapEnabled;
		//unloadasset(var(0));
		order = var(1) < var(2) ? var(2) : var(1);
		if((var(1) > maxSize || var(2) > maxSize) && assetpath.Contains(filter) && (isnullorempty(notfilter) || !assetpath.Contains(notfilter)) && (prop.Contains("1") && var(3) || !prop.Contains("1")) && (prop.Contains("2") && var(4) || !prop.Contains("2"))){
			info = format("{0} size:{1},{2} guid:{3}", assetpath, var(1), var(2), assetpath2guid(assetpath));
			value = calcassetsize(assetpath);
			if(duptype==1){
				group = format("{0}|{1}", value, calcassetmd5(assetpath));				
			}else{
				group = format("{0}", assetpath2guid(assetpath));		
			};
			1;
		} else {
			0;
		};
	};
}
group
{
	if(count>1){
		info = format("{0} count:{1} ref by count:{2}", group, count, calcrefbycount(assetpath));
		1;
	}else{
		0;	
	};
};