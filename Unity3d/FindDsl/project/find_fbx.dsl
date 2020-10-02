input("*.fbx")
{
    int("maxTriangleCount", 1000);
    bool("hasAnimation", false);
    bool("hasOffscreenUpdate", false);
    bool("hasAlwaysAnimate", false);
    bool("readable", false);
    stringlist("anyfilter", "");
    stringlist("notfilter", "");
    stringhash("hashkeys", "");
    stringhash("meshhashkeys", "");
    float("pathwidth",240){range(20,4096);};
    feature("source", "project");
    feature("menu", "1.Project Resources/Fbx");
    feature("description", "just so so");
}
filter
{
    $name = getfilenamewithoutextension(assetpath);
    if(stringcontainsany(assetpath, anyfilter) && stringnotcontains(assetpath, notfilter) && stringhashcontains(hashkeys, $name)){
        var(0) = loadasset(assetpath);  
        var(1) = collectmeshinfo(var(0), importer);
        var(2) = importer.isReadable;
        //unloadasset(var(0));
        if(var(1).triangleCount >= maxTriangleCount && (!hasAnimation || var(1).clipCount>0) && (!hasOffscreenUpdate || var(1).updateWhenOffscreenCount>0) && (!hasAlwaysAnimate || var(1).alwaysAnimateCount>0) && (!readable || var(2))){
            $ret = 0;
            $key = "";
            looplist(var(1).meshes){
                var(3) = getfilename($$.meshName);
                if(stringhashcontains(meshhashkeys, var(3))){
                    $ret = 1;
                    $key = $$.meshName;
                    break;
                };
            };
            if($ret){
                scenepath = $name;
                order = var(1).triangleCount;
                value = order;
                info = format("key:{0},skinned:{1},mesh:{2},vertex:{3},triangle:{4},bone:{5},material:{6},max_tex_size:({7},{8}),max_tex_name:{9}={10},clip:{11},max_keyframe:{12}",
                    $key, var(1).skinnedMeshCount, var(1).meshFilterCount, var(1).vertexCount, var(1).triangleCount, var(1).boneCount, var(1).materialCount, var(1).maxTexWidth, var(1).maxTexHeight, var(1).maxTexPropName, var(1).maxTexName, var(1).clipCount, var(1).maxKeyFrameCount
                );
            };
            $ret;
        }else{
            0;
        };
    }else{
        0;
    };
};