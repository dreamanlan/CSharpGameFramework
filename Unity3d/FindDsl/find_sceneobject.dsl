input("Transform")
{
	string("objectfilter", "");
	string("typefilter", "");
	string("style", "itemlist"){
		popup(["itemlist", "grouplist"]);
	};
	float("pathwidth",240){range(20,4096);};
	feature("source", "scenecomponents");
	feature("menu", "3.Current Scene Objects/Component Group");
	feature("description", "just so so");
}
filter
{
	if(scenepath.Contains(objectfilter)){
		1;
	} else {
		0;
	};
}
group
{
	if(group.Contains(typefilter)){
		order = count;
		value = count;
		info = format("{0}=>{1}", group, count);
	  1;
	}else{
	  0;
	};
};