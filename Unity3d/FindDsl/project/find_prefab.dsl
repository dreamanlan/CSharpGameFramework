input("*.prefab")
{
    int("maxTriangleCount", 1000);
    bool("hasAnimation", false);
    bool("hasOffscreenUpdate", false);
    bool("hasAlwaysAnimate", false);
    stringlist("anyfilter", "");
    stringlist("notfilter", "");
    stringhash("hashkeys", "");
    stringhash("meshhashkeys", "");
    float("pathwidth",240){range(20,4096);};
    feature("source", "project");
    feature("menu", "1.Project Resources/Prefab");
    feature("description", "just so so");
}
filter
{
    $name=getfilenamewithoutextension(assetpath);
    if(stringcontainsany(assetpath, anyfilter) && stringnotcontains(assetpath, notfilter) && stringhashcontains(hashkeys, $name)){
        var(0) = loadasset(assetpath);  
        var(1) = collectprefabinfo(var(0));
        var(2) = getcomponentinchildren(var(0),"Playables.PlayableDirector").gameObject.name;
        //unloadasset(var(0));
        if(var(1).triangleCount >= maxTriangleCount && (!hasAnimation || var(1).clipCount>0) && (!hasOffscreenUpdate || var(1).updateWhenOffscreenCount>0) && (!hasAlwaysAnimate || var(1).alwaysAnimateCount>0)){
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
                info = format("key:{0},timeline:{1},skinned:{2},mesh:{3},vertex:{4},triangle:{5},bone:{6},material:{7},max_tex_size:({8},{9}),max_tex_name:{10}={11},clip:{12},max_keyframe:{13}",
                        $key, var(2), var(1).skinnedMeshCount, var(1).meshFilterCount, var(1).vertexCount, var(1).triangleCount, var(1).boneCount, var(1).materialCount, var(1).maxTexWidth, var(1).maxTexHeight, var(1).maxTexPropName, var(1).maxTexName, var(1).clipCount, var(1).maxKeyFrameCount
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