input("*.*")
{
	string("assetfilter", "");
	string("dependencefilter", "");
	float("pathwidth",240){range(20,4096);};
	feature("source", "project");
	feature("menu", "1.Project Resources/Dependencies");
	feature("description", "just so so");
}
filter
{
	if(assetpath.Contains(assetfilter)){
		var(0) = getdependencies(assetpath);
		looplist(var(0)){		
			if($$.Contains(dependencefilter)){
				var(1) = newitem();
				var(1).AssetPath = assetpath;
				var(1).Info = $$;
				var(1).Order = 0;
				var(1).Value = 0;
			};
		};
	};
	0;
};