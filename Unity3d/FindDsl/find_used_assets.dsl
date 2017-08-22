input("t:texture","t:mesh","t:model","t:material")
{
	string("filter", "");
	feature("source", "allassets");
}
filter
{
  if(assetpath.Contains(filter)){
    1;
  }else{
    0;
  };
};