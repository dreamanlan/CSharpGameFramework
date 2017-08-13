input("*.tga","*.png","*.jpg")
{
	int("startSize",512);
}
filter
{
	var(0) = loadasset(assetpath);
	var(1) = var(0).width;
	var(2) = var(0).height;
	unloadasset(var(0));
	if(var(1) >= startSize || var(2) >= startSize){
		info = "size:" + var(1) + "," + var(2);
		1;
	} else {
		0;
	};
}
process
{
	//var(0) = gettexturesetting("iPhone");
	var(0) = getdefaulttexturesetting();
	var(0).overridden = changetype(1,"bool");
	var(0).maxTextureSize = changetype(256, "int");
	var(0).readable = false;
	settexturecompression(var(0),"lowquality");
	settexturesetting(var(0));
};