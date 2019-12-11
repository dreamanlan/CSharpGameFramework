input
{
    int("maxSize", 8);
    string("category", "native");
    string("class", "Texture2D"){
        native_memory_group;
    };
	string("filter", "");
	feature("source", "snapshot");
	feature("menu", "6.Memory/slowly find memory assets");
	feature("description", "just so so");
}
filter
{
    String = gettype("System.String");
	order = memory.size;
	if(memory.size >= maxSize && memory.className == class && memory.name.Contains(filter)){
		var(0) = findasset(memory.name, memory.className);
		assetpath = var(0)[0];
		scenepath = var(0)[1];
		if(isnullorempty(assetpath)){
			assetpath = memory.name;
		};
		info = format("name:{0} class:{1} size:{2} refby:{3} refother:{4}",
            memory.name, memory.className, memory.size, memory.refCount, memory.refOtherCount
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