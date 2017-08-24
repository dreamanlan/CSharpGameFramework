input("*.prefab")
{
	string("filter", "");
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