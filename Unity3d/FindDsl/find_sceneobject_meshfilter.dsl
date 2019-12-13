input("Transform")
{
	string("filter", "");
	float("pathwidth",240){range(20,4096);};
	feature("source", "sceneobjects");
	feature("menu", "3.Current Scene Objects/Mesh Filter");
	feature("description", "just so so");
}
filter
{
    var(0) = getcomponentinchildren(object, "MeshFilter");
    var(1) = 0;
    if(!isnull(var(0))){
        if(isnull(var(0).sharedMesh)){
        	var(1)=1;
        };
    };
    var(1);
};