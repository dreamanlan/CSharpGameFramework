input("*.*")
{
	string("assetfilter", "");
	string("dependencefilter", "");
	string("style", "grouplist"){
		popup(["itemlist", "grouplist"]);
	};
	float("pathwidth",240){range(20,4096);};
	feature("source", "project");
	feature("menu", "1.Project Resources/Group Dependencies");
	feature("description", "just so so");
}
filter
{
	if(assetpath.Contains(assetfilter)){
		var(0) = getdependencies(assetpath);
		looplist(var(0)){		
			if($$.Contains(dependencefilter)){
				var(1) = newitem();
				var(1).AssetPath = $$;
				var(1).Info = assetpath;
				var(1).Order = 0;
				var(1).Value = 0;
				var(1).Group = $$;
			};
		};
	};
	0;
}
group
{
		order = count;
		value = count;
		info = format("{0}=>{1}", group, count);
	  1;
};