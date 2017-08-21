input("t:texture","t:mesh","t:model")
{
	string("filter", "");
	feature("source", "allassets");
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