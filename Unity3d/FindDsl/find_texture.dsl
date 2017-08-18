input("*.tga","*.png","*.jpg")
{
	int("maxSize",256);
	string("filter", "");
}
filter
{
	var(0) = loadasset(assetpath);
	var(1) = var(0).width;
	var(2) = var(0).height;
	unloadasset(var(0));
	order = var(1) < var(2) ? var(2) : var(1);
	if((var(1) > maxSize || var(2) > maxSize) && assetpath.Contains(filter)){
		info = "size:" + var(1) + "," + var(2);
		1;
	} else {
		0;
	};
};