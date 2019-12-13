input
{
    string("category", "native"){
        popup(["all","native","managed"])
    };
    int("maxSize", 8);
    ulong("sectionAddr",0);
	stringlist("classfilter", "");
	stringlist("filter", "");
	stringlist("classnotfilter", "");
	stringlist("notfilter", "");
	string("mheaps",""){
	    file("csv");
	    script("LoadManagedHeaps");
	};
	string("maps",""){
	    file("txt");
	    script("LoadMaps");
	};
	bool("findasset", false);
	float("pathwidth",240){range(20,4096);};
	feature("source", "snapshot");
	feature("menu", "6.Memory/slowly find memory and smaps");
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
		var(0) = findsmaps(maps, memory.address);
		var(1) = findmanagedheaps(mheaps, memory.address);
		if(isnull(var(0))){
		    var(2) = "unknown";
		    var(3) = 0;  
		    var(4) = 0;
		    var(5) = 0;
		    var(6) = 0;
		}else{
		    var(2) = var(0).module;
		    var(3) = var(0).size;
		    var(4) = var(0).vm_start;
		    var(5) = var(0).rss;
		    var(6) = var(0).pss;
		};
		if(isnull(var(1))){
		    var(7) = 0;
		    var(8) = 0;
		}else{
		    var(7) = var(1).size;
		    var(8) = var(1).vm_start;
		};
		if(sectionAddr==0 || sectionAddr==var(4)){
    		scenepath = format("name:{0} class:{1} size:{2} addr:{3:X}",
    	        memory.name, memory.className, memory.size, memory.address
    	        );
    		info = format("module:{0} section_size:{1} section_start:{2:X} rss:{3} pss:{4} mheap_size:{5} mheap:{6:X}",
    	        var(2), var(3), var(4), var(5), var(6), var(7), var(8)
    	        );
    	    order = var(4);
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
script(LoadMaps)args($paramInfo)
{
    if(!isnullorempty($paramInfo.Value)){
        $paramInfo.Value = loadsmaps($paramInfo.Value);
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