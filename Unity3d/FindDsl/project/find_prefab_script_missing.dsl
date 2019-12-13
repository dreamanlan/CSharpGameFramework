input("*.prefab")
{
	string("filter", "");
	float("pathwidth",240){range(20,4096);};
	feature("source", "project");
	feature("menu", "1.Project Resources/Missing Script");
	feature("description", "just so so");
}
filter
{
	if(assetpath.Contains(filter)){
    var(0) = loadasset(assetpath);
	  var(1) = getcomponentsinchildren(var(0), "Component");
	  var(2) = 0;
	  looplist(var(1)){
	    if(isnull($$)){
	      var(2)=1;
	    };
	  };
	  var(2);
	}else{
	  0;
	};
};