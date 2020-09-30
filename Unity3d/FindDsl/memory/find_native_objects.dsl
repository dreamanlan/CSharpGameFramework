input
{
    int("maxSize", 0);
    int("minRefCount",0);
    int("maxRefCount",1024);
    string("category", "native");
    string("class", "Texture2D"){
        native_memory_group;
    };
	stringlist("anyfilter", "");
	float("pathwidth",240){range(20,4096);};
	button("Return", "FindDsl/memory/find_ngroups.dsl");
	feature("source", "snapshot");
	feature("menu", "6.Memory/find native objects");
	feature("description", "just so so");
}
filter
{
    String = gettype("System.String");
	order = memory.size;
	if(memory.size >= maxSize && memory.className == class && stringcontainsany(memory.name,anyfilter) && memory.refCount>=minRefCount && memory.refCount<=maxRefCount){
		assetpath = memory.name;
		info = format("name:{0} class:{1} size:{2} refby:{3}",
            memory.name, memory.className, memory.size, memory.refCount
            );
        value = memory.size;
        extraobject = memory.objectData;
        extralistbuild = "BuildExtraList";
        1;
	}else{
        0;
	};
};

script(BuildExtraList)args($item)
{
	$r = findshortestpathtoroot($item.ExtraObject);
	return($r);
};