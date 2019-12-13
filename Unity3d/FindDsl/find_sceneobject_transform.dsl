input("Transform")
{
	string("filter", "");
	float("pathwidth",240){range(20,4096);};
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