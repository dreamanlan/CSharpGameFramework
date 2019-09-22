input("*.tga","*.png","*.jpg","*.fbx","*.exr","*.mat","*.controller","*.prefab","*.asset")
{
	string("filter", "");
	feature("source", "unusedassets");
	feature("menu", "5.Unused Resources");
	feature("description", "just so so");
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