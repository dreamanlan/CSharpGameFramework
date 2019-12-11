input
{
    int("maxSize", 8);
    string("category", "mgroup");
	string("contains", "");
	string("notcontains1", "");
	string("notcontains2", "");
	string("startswith", "");
	string("endswith", "View");
	int("mincount",2);
	feature("source", "snapshot");
	feature("menu", "6.Memory/find managed groups");
	feature("description", "just so so");
}
filter
{
    String = gettype("System.String");
	order = group_info.size;
	var(0) = group_info.group;
	var(1) = group_info.count;
	if(group_info.size >= maxSize && var(0).Contains(contains) && (String.IsNullOrEmpty(notcontains1) || !var(0).Contains(notcontains1)) && (String.IsNullOrEmpty(notcontains2) || !var(0).Contains(notcontains2)) && (String.IsNullOrEmpty(startswith) || var(0).StartsWith(startswith)) && (String.IsNullOrEmpty(endswith) || var(0).EndsWith(endswith)) && var(1)>=mincount){
		info = format("group:{0} count:{1} size:{2}",
	        group_info.group, group_info.count, group_info.size
	        );
        value = group_info.size;
        setredirect("FindDsl/memory/find_managed_objects.dsl", "class", group_info.group);
        1;
	}else{
        0;
	};
};