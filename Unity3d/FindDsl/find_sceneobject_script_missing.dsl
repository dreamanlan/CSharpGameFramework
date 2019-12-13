input("Transform")
{
	string("filter", "");
	float("pathwidth",240){range(20,4096);};
	feature("source", "sceneobjects");
	feature("menu", "3.Current Scene Objects/Missing Script");
	feature("description", "just so so");
}
filter
{   
	if(scenepath.Contains(filter)){
	  var(0) = getcomponents(object, "Component");
	  var(1) = 0;
	  looplist(var(0)){
	    if(isnull($$)){
	      var(1)=1;
	    };
	  };
	  var(1);
	}else{
	  0;
	};
};