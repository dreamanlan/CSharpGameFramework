input
{
    int("maxSize", 1000000);
    string("category", "ngroup");
	stringlist("containsany", "");
	stringlist("notcontains", "");
	string("startswith", "");
	string("endswith", "");
	int("mincount",2);
	float("pathwidth",240){range(20,4096);};
	feature("source", "snapshot");
	feature("menu", "6.Memory/find native groups");
	feature("description", "just so so");
}
filter
{
    String = gettype("System.String");
	order = group_info.size;
	var(0) = group_info.group;
	var(1) = group_info.count;
	if(group_info.size >= maxSize && stringcontainsany(var(0),containsany) && stringnotcontains(var(0),notcontains) && (String.IsNullOrEmpty(startswith) || var(0).StartsWith(startswith)) && (String.IsNullOrEmpty(endswith) || var(0).EndsWith(endswith)) && var(1)>=mincount){
		info = format("group:{0} count:{1} size:{2}",
            group_info.group, group_info.count, group_info.size
	        );
        value = group_info.size;
        setredirect("FindDsl/memory/find_native_objects.dsl", "class", group_info.group);
        1;
	}else{
        0;
	};
};