input
{
    string("category", "native"){
        popup(["all","native","managed"])
    };
    int("maxSize", 8);
    ulong("sectionAddr", 0);
	stringlist("classfilter", "");
	stringlist("filter", "");
	stringlist("classnotfilter", "");
	stringlist("notfilter", "");
	string("maps","E:\\work\\workdoc\\性能数据\\20191204\\内存数据与maps对应实验\\maps0.txt"){
	    file("txt");
	    script("LoadMaps");
	};
	bool("ref", false);
	float("pathwidth",240);
	feature("source", "snapshot");
	feature("menu", "6.Memory/slowly find memory and maps");
	feature("description", "just so so");
}
filter
{
	if(memory.size >= maxSize && stringcontains(memory.className, classfilter) && stringcontains(memory.name, filter) && stringnotcontains(memory.className, classnotfilter) && stringnotcontains(memory.name, notfilter)){
		//var(0) = findasset(memory.name, memory.className);
		//assetpath = var(0)[0];
		assetpath = memory.name;
		if(isnullorempty(assetpath)){
			assetpath = memory.name;
		};
		var(1) = findmaps(maps, memory.address);
		if(isnull(var(1))){
		    var(2) = "unknown";
		    var(3) = 0;  
		    var(4) = 0;
		}else{
		    var(2) = var(1).module;
		    var(3) = var(1).size;
		    var(4) = var(1).vm_start;
		};
		if(sectionAddr==0 || sectionAddr==var(4)){
    		scenepath = format("name:{0} class:{1} size:{2} addr:{3:X}",
    	        memory.name, memory.className, memory.size, memory.address
    	        );
    		info = format("module:{0} section_size:{1} section_start:{2:X}",
    	        var(2), var(3), var(4)
    	        );
    	    order = var(4);
    	    value = memory.size;
    	    if(ref){
        	    extralist = findshortestpathtoroot(memory.memoryObject);
        	};
    	    1;
    	}else{
    	    0;
    	};
	}else{
	    0;
	};
};

script(LoadMaps)args($paramInfo)
{
    if(!isnullorempty($paramInfo.Value)){
        $paramInfo.Value = loadmaps($paramInfo.Value);
        return(3600.0);
    }else{
        return(1.0);
    };
};