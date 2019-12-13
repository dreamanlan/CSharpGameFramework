input("*.fbx")
{
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
	feature("menu", "1.Project Resources/Duplicate Fbx");
	feature("description", "just so so");
}
filter
{
	var(0) = loadasset(assetpath);
	if(isnull(var(0))){
		0;
	} else {
		//unloadasset(var(0));
		if(assetpath.Contains(filter) && (isnullorempty(notfilter) || !assetpath.Contains(notfilter))){
			info = format("{0} guid:{1}", assetpath, assetpath2guid(assetpath));
			order = value;
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
		order = count;
		info = format("{0} count:{1} ref by count:{2}", group, count, calcrefbycount(assetpath));
		1;
	}else{
		0;	
	};
};