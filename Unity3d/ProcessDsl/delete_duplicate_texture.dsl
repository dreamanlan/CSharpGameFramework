input("*.tga","*.png","*.jpg")
{
	int("maxSize",256){
		range(1,1024);
	};
	string("prop",""){
		multiple(["readable","mipmap"],[1,2]);
	};
	string("filter", "");
	string("style", "itemlist"){
		popup(["itemlist", "grouplist"]);
	};
	feature("source", "project");
	feature("menu", "1.Project Resources/Delete Duplicate Texture");
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
		if((var(1) > maxSize || var(2) > maxSize) && assetpath.Contains(filter) && (prop.Contains("1") && var(3) || !prop.Contains("1")) && (prop.Contains("2") && var(4) || !prop.Contains("2"))){
			info = format("{0} size:{1},{2}", assetpath, var(1), var(2));
			value = calcassetsize(assetpath);
			group = format("{0}|{1}", value, calcassetmd5(assetpath));
			1;
		} else {
			0;
		};
	};
}
group
{
	var(0) = calcrefbycount(assetpath);
	if(count>1 && var(0)<=0){
		info = format("{0} count:{1}", group, count);
		1;
	}else{
		0;	
	};
}
process
{
	deleteasset(assetpath);
};