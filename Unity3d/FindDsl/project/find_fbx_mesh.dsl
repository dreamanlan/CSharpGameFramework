input("*.fbx")
{
    int("triangleCount", 1000);
    int("componentCount", 3);
    stringlist("filter", "");
    stringlist("notfilter", "");
    stringlist("uvfilter", "");
	float("pathwidth",240){range(20,4096);};
    feature("source", "project");
    feature("menu", "1.Project Resources/Check Fbx Mesh");
    feature("description", "just so so");
}
filter
{
    if(stringcontains(assetpath, filter) && stringnotcontains(assetpath, notfilter)){
        var(0) = loadasset(assetpath);  
        var(1) = collectmeshinfo(var(0), importer);
        //unloadasset(var(0));
        order = var(1).triangleCount;
        if(var(1).triangleCount >= triangleCount){
            var(2) = calcmeshvertexcomponentcount(var(0),true);
            looplist(var(2)){
                $key = $$.Key;
                $val = $$.Value;
                if($val >= componentCount && stringcontains($key, uvfilter)){                    
                    var(3) = newitem();
                    var(3).AssetPath = assetpath;
                    var(3).Info = format("skinned:{0},mesh:{1},vertex:{2},triangle:{3},vertex components:{4} {5}",
                        var(1).skinnedMeshCount, var(1).meshFilterCount, var(1).vertexCount, var(1).triangleCount, $val, $key
                        );
                    var(3).Order = $val;
                    var(3).Value = 0;
                };
            };
        };
    };
    0;
};