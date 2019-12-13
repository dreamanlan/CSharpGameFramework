input("*.prefab")
{
    int("maxTriangleCount", 1000);
    bool("hasAnimation", false);
    bool("hasOffscreenUpdate", false);
    bool("hasAlwaysAnimate", false);
    stringlist("filter", "");
    stringlist("notfilter", "");
	float("pathwidth",240){range(20,4096);};
    feature("source", "project");
    feature("menu", "1.Project Resources/Prefab");
    feature("description", "just so so");
}
filter
{
    if(stringcontains(assetpath, filter) && stringnotcontains(assetpath, notfilter)){
        var(0) = loadasset(assetpath);  
        var(1) = collectprefabinfo(var(0));
        var(2) = getcomponentinchildren(var(0),"Playables.PlayableDirector").gameObject.name;
        //unloadasset(var(0));
        order = var(1).triangleCount;
        if(var(1).triangleCount >= maxTriangleCount && (!hasAnimation || var(1).clipCount>0) && (!hasOffscreenUpdate || var(1).updateWhenOffscreenCount>0) && (!hasAlwaysAnimate || var(1).alwaysAnimateCount>0)){
            info = format("timeline:{0},skinned:{1},mesh:{2},vertex:{3},triangle:{4},bone:{5},material:{6},max_tex_size:({7},{8}),max_tex_name:{9}={10},clip:{11},max_keyframe:{12}",
                var(2), var(1).skinnedMeshCount, var(1).meshFilterCount, var(1).vertexCount, var(1).triangleCount, var(1).boneCount, var(1).materialCount, var(1).maxTexWidth, var(1).maxTexHeight, var(1).maxTexPropName, var(1).maxTexName, var(1).clipCount, var(1).maxKeyFrameCount
                );
            1;
        }else{
            0;
        };
    }else{
        0;
    };
};