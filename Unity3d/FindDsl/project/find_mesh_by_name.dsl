input("*.fbx")
{
    int("triangleCount", 1000);
    stringlist("filter", "");
    stringlist("notfilter", "");
    stringlist("meshfilter", "");
    stringlist("meshnotfilter", "");
	float("pathwidth",240){range(20,4096);};
    feature("source", "project");
    feature("menu", "1.Project Resources/Find Fbx Mesh");
    feature("description", "just so so");
}
filter
{
    if(stringcontains(assetpath, filter) && stringnotcontains(assetpath, notfilter)){
        var(0) = loadasset(assetpath);  
        var(1) = collectmeshes(var(0), true);
        //unloadasset(var(0));
        looplist(var(1)){
            $mesh = $$;
            $name = $mesh.name;
            $vertexCount = $mesh.vertexCount;
            $triangleCount = $mesh.triangles.Length/3;
            if(stringcontains($name, meshfilter) && stringnotcontains($name, meshnotfilter) && $triangleCount>=triangleCount){
                var(2) = newitem();
                var(2).AssetPath = assetpath;
                var(2).Info = format("mesh:{0} vertex:{1} triangle:{2}",$name,$vertexCount,$triangleCount);
                var(2).Order = $triangleCount;
                var(2).Value = $triangleCount;
            };
        };
    };
    0;
};