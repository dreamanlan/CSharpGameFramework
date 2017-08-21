input("*.tga","*.png","*.jpg","*.fbx","*.mat","*.controller")
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