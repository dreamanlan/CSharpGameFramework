input("*.tga","*.png","*.jpg")
{
	int("maxSize",256);
	int("postprocessindex", 1){
	  option(1..16);
	};	
	feature("postprocessclass","PostProcessDataOfIos");
	feature("postprocessmethod","GetTextureSet");	
}
filter
{
	var(0) = loadasset(assetpath);
	var(1) = var(0).width;
	var(2) = var(0).height;
	unloadasset(var(0));
	if(var(1) > maxSize || var(2) > maxSize){
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
	var(0).maxTextureSize = changetype(maxSize, "int");
	importer.isReadable = false;
	settexturecompression(var(0),"lowquality");
	settexturesetting(var(0));
  saveandreimport();
};