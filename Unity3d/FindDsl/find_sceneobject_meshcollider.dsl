input("Transform")
{
	feature("source", "sceneobjects");
	feature("menu", "3.Current Scene Objects/Find Mesh Collider");
	feature("description", "just so so");
}
filter
{
	var(0) = getcomponentinchildren(object, "MeshCollider");
	if(isnull(var(0))){
		0;
	}else{
		1;
	};
};