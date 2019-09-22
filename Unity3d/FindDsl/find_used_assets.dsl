input("t:texture","t:mesh","t:model","t:material")
{
	string("filter", "");
	feature("source", "allassets");
	feature("menu", "4.All Assets/Used Resources");
	feature("description", "just so so");
}
filter
{
  if(assetpath.Contains(filter)){
    1;
  }else{
    0;
  };
};