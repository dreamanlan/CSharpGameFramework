input("*.prefab")
{
    int("totalTriangleCount", 1000);
    int("triangleCount", 200);
    bool("onlyParticle", false);
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
        var(1) = collectprefabinfo(var(0));
        //unloadasset(var(0));
        $totalTriangleCount = var(1).triangleCount;
        if($totalTriangleCount>=totalTriangleCount){
            looplist(var(1).meshes){
                $meshInfo = $$;
                $name = $meshInfo.meshName;
                $count = $meshInfo.meshCount;
                $vertexCount = $meshInfo.vertexCount;
                $triangleCount = $meshInfo.triangleCount;
                $tvc = $meshInfo.totalVertexCount;
                $ttc = $meshInfo.totalTriangleCount;
                $isps = $meshInfo.isParticle;
                if(stringcontains($name, meshfilter) && stringnotcontains($name, meshnotfilter) && $ttc>=triangleCount && (!onlyParticle || $isps)){
                    var(3) = newitem();
                    var(3).AssetPath = assetpath;
                    var(3).ScenePath = getassetpath($mesh);
                    var(3).Info = format("mesh:{0} vertex:{1} triangle:{2} count:{3} total_vertex:{4} total_triangle:{5} total_prefab_triangle:{6}",$name,$vertexCount,$triangleCount,$count,$tvc,$ttc,$totalTriangleCount);
                    var(3).Order = $totalTriangleCount*100000+$ttc;
                    var(3).Value = $ttc;
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