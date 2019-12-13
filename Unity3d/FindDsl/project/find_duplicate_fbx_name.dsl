input("*.fbx")
{
	stringlist("filter", "");
	stringlist("notfilter", "");
	string("style", "itemlist"){
		popup(["itemlist", "grouplist"]);
	};
	float("pathwidth",240){range(20,4096);};
	feature("source", "project");
	feature("menu", "1.Project Resources/Duplicate Fbx Name");
	feature("description", "just so so");
}
filter
{
	var(0) = loadasset(assetpath);
	if(isnull(var(0))){
		0;
	} else {
		//unloadasset(var(0));
		if(stringcontains(assetpath,filter) && stringnotcontains(assetpath,notfilter)){
			info = format("{0} guid:{1}", assetpath, assetpath2guid(assetpath));
			order = value;
			value = calcassetsize(assetpath);
			group = getfilename(assetpath);
			1;
		} else {
			0;
		};
	};
}
group
{
	if(count>1){
		order = count;
		info = format("{0} count:{1} ref by count:{2}", group, count, calcrefbycount(assetpath));
		1;
	}else{
		0;	
	};
};