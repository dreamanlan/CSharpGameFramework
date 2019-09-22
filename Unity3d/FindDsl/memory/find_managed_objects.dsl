input
{
  int("maxSize", 8);
  int("minRefCount",1);
  int("maxRefCount",128);
  int("minRefOtherCount",0);
  int("maxRefOtherCount",1);
  string("category", "managed");
  string("class", "System.String");  
	string("filter", "");	
	button("Return", "FindDsl/memory/find_mgroups.dsl");
	feature("source", "snapshot");
	feature("menu", "6.Memory/find managed objects");
	feature("description", "just so so");
}
filter
{
  String = gettype("System.String");
	order = memory.size;
	if(memory.size >= maxSize && (isnullorempty(class) || memory.className == class) && memory.name.Contains(filter) && memory.refCount>=minRefCount && memory.refCount<=maxRefCount && memory.refOtherCount>=minRefOtherCount && memory.refOtherCount<=maxRefOtherCount){
		assetpath = memory.name;
		info = format("name:{0} class:{1} size:{2} refby:{3} refother:{4}",
	      memory.name, memory.className, memory.size, memory.refCount, memory.refOtherCount
	    );
	  value = memory.size;
	  extraobject = memory.memoryObject;
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