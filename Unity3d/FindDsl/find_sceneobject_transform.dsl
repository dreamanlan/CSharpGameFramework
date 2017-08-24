input("Transform")
{
	string("filter", "");
	feature("source", "sceneobjects");
}
filter
{ 
	if(scenepath.Contains(filter)){
	  1;
	}else{
	  0;
	};
};