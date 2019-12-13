input("*.prefab")
{
    int("triangleCount", 1000);
    int("lodTriangleCount", 1000);
    stringlist("filter", "");
    stringlist("notfilter", "");
    stringlist("meshfilter", "");
    stringlist("meshnotfilter", "");
	int("lodtype",0){
		toggle(["all","no lod","lod"],[0,1,2]);
	};
	float("pathwidth",240){range(20,4096);};
    feature("source", "project");
    feature("menu", "1.Project Resources/Find Prefab Lod");
    feature("description", "just so so");
}
filter
{
    if(stringnotcontains(assetpath, "_Lod") && stringcontains(assetpath, filter) && stringnotcontains(assetpath, notfilter)){
        var(0) = getdirectoryname(assetpath);
        var(1) = getfilenamewithoutextension(assetpath);
        var(2) = var(0)+"/"+var(1)+"_Lod.prefab";
        if(fileexist(var(2))){
            if(lodtype==0 || lodtype==2){
                var(3) = loadasset(var(2));  
                var(4) = collectmeshes(var(3), true);
                //unloadasset(var(0));
                looplist(var(4)){
                    $mesh = $$;
                    $name = $mesh.name;
                    $vertexCount = $mesh.vertexCount;
                    $triangleCount = $mesh.triangles.Length/3;
                    if(stringcontains($name, meshfilter) && stringnotcontains($name, meshnotfilter) && $triangleCount>=lodTriangleCount){
                        var(5) = newitem();
                        var(5).AssetPath = assetpath;
                        var(5).Info = format("lod, mesh:{0} vertex:{1} triangle:{2}",$name,$vertexCount,$triangleCount);
                        var(5).Order = $triangleCount;
                        var(5).Value = $triangleCount;
                    };
                };
            };
        }else{
            if(lodtype==0 || lodtype==1){
                var(3) = loadasset(assetpath);  
                var(4) = collectmeshes(var(3), true);
                //unloadasset(var(0));
                looplist(var(4)){
                    $mesh = $$;
                    $name = $mesh.name;
                    $vertexCount = $mesh.vertexCount;
                    $triangleCount = $mesh.triangles.Length/3;
                    if(stringcontains($name, meshfilter) && stringnotcontains($name, meshnotfilter) && $triangleCount>=triangleCount){
                        var(5) = newitem();
                        var(5).AssetPath = assetpath;
                        var(5).Info = format("no lod, mesh:{0} vertex:{1} triangle:{2}",$name,$vertexCount,$triangleCount);
                        var(5).Order = $triangleCount;
                        var(5).Value = $triangleCount;
                    };
                };
            };
        };
    };
    0;
};