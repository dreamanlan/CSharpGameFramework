input("Transform")
{
	string("filter", "");
	feature("source", "sceneobjects");
	feature("menu", "3.Current Scene Objects/Tranforms");
	feature("description", "just so so");
}
filter
{ 
	if(scenepath.Contains(filter)){
	  1;
	}else{
	  0;
	};
};