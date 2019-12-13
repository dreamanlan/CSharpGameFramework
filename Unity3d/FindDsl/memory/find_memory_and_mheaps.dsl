input
{
    string("category", "native"){
        popup(["all","native","managed"])
    };
    int("maxSize", 8);
    ulong("heapAddr", 0);
	stringlist("classfilter", "");
	stringlist("filter", "");
	stringlist("classnotfilter", "");
	stringlist("notfilter", "");
	string("mheaps",""){
	    file("csv");
	    script("LoadManagedHeaps");
	};
	bool("findasset", false);
	float("pathwidth",240){range(20,4096);};
	feature("source", "snapshot");
	feature("menu", "6.Memory/slowly find memory and mheaps");
	feature("description", "just so so");
}
filter
{
	if(memory.size >= maxSize && stringcontains(memory.className, classfilter) && stringcontains(memory.name, filter) && stringnotcontains(memory.className, classnotfilter) && stringnotcontains(memory.name, notfilter)){
		if(findasset){
    		assetpath = (findasset(memory.name, memory.className))[0];
	    }else{
		    assetpath = memory.name;
		};
		if(isnullorempty(assetpath)){
			assetpath = memory.name;
		};
		var(0) = findmanagedheaps(mheaps, memory.address);
		if(isnull(var(0))){
		    var(1) = 0;
		    var(2) = 0;
		}else{
		    var(1) = var(0).size;
		    var(2) = var(0).vm_start;
		};
		if(heapAddr==0 || heapAddr==var(2)){
    		scenepath = format("name:{0} class:{1} size:{2} addr:{3:X}",
    	        memory.name, memory.className, memory.size, memory.address
    	        );
    		info = format("mheap_size:{0} mheap:{1:X}",
    	        var(1), var(2)
    	        );
    	    order = var(2);
    	    value = memory.size;
        	extraobject = memory.objectData;
        	extralistbuild = "BuildExtraList";
    	    1;
    	}else{
    	    0;
    	};
	}else{
	    0;
	};
};

script(LoadManagedHeaps)args($paramInfo)
{
    if(!isnullorempty($paramInfo.Value)){
        $paramInfo.Value = loadmanagedheaps($paramInfo.Value);
        return(3600.0);
    }else{
        return(1.0);
    };
};
script(BuildExtraList)args($item)
{
	$r = findshortestpathtoroot($item.ExtraObject);
	return($r);
};