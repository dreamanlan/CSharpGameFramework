input("*.prefab")
{
    int("totalTriangleCount", 1000);
    int("triangleCount", 200);
    stringlist("filter", "");
    stringlist("notfilter", "");
    stringlist("meshfilter", "");
    stringlist("meshnotfilter", "");
	float("pathwidth",240){range(20,4096);};
    feature("source", "project");
    feature("menu", "1.Project Resources/Find Particle Meshes");
    feature("description", "just so so");
}
filter
{
    if(stringcontains(assetpath, filter) && stringnotcontains(assetpath, notfilter)){
        var(0) = loadasset(assetpath);  
        var(1) = collectmeshes(var(0), true, 2);
        var(2) = collectprefabinfo(var(0));
        //unloadasset(var(0));
        $totalTriangleCount = var(2).triangleCount;
        if($totalTriangleCount>=totalTriangleCount){
            looplist(var(1)){
                $mesh = $$;
                $name = $mesh.name;
                $vertexCount = $mesh.vertexCount;
                $triangleCount = $mesh.triangles.Length/3;
                if(stringcontains($name, meshfilter) && stringnotcontains($name, meshnotfilter) && $triangleCount>=triangleCount){
                    var(3) = newitem();
                    var(3).AssetPath = assetpath;
                    var(3).ScenePath = getassetpath($mesh);
                    var(3).Info = format("mesh:{0} vertex:{1} triangle:{2} total_triangle:{3}",$name,$vertexCount,$triangleCount,$totalTriangleCount);
                    var(3).Order = $triangleCount;
                    var(3).Value = $triangleCount;
                    var(3).ExtraList = newextralist(var(3).ScenePath => var(3).ScenePath);
                    var(3).ExtraListClickScript = "OnClickExtraListItem";
                };
            };
        };
    };
    0;
};

script(OnClickExtraListItem)args($obj,$item)
{
    selectprojectobject($obj.Value);
};