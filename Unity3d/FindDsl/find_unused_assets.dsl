input("*.tga","*.png","*.jpg","*.fbx","*.exr","*.mat","*.controller","*.shader")
{
	string("filter", "");
	feature("source", "unusedassets");
}
filter
{
  if(assetpath.Contains(filter)){
    info = stringjoin(",", getreferencebyassets(assetpath));
    1;
  }else{
    0;
  };
};