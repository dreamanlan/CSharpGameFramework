input("*.prefab")
{
	float("pathwidth",240){range(20,4096);};
	feature("source", "project");
	feature("menu", "1.Project Resources/Find Mesh Collider");
	feature("description", "just so so");
}
filter
{
	object = loadasset(assetpath);
	var(0) = getcomponent(object, "MeshCollider");
	if(isnull(var(0))){
		0;
	}else{
		1;
	};
};