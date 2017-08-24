input("*.prefab")
{
	string("filter", "");
	feature("source", "sceneassets");
}
filter
{
  object = loadasset(assetpath);
	var(0) = getcomponentsinchildren(object,"ParticleSystem");
	order = var(0).Length;
	if(var(0).Length > 0 && assetpath.Contains(filter)){
	  info = format("particle system count:{0}", var(0).Length);
		1;
	}else{
		0;
	};
};