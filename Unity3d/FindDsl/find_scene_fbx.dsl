input("*.fbx")
{
    int("maxTriangleCount", 1000);
    bool("hasAnimation", false);
    stringlist("filter", "");
    stringlist("notfilter", "");
	float("pathwidth",240){range(20,4096);};
    feature("source", "sceneassets");
    feature("menu", "2.Current Scene Resources/Fbx");
    feature("description", "just so so");
}
filter
{
    if(stringcontains(assetpath, filter) && stringnotcontains(assetpath, notfilter)){
        var(0) = loadasset(assetpath);  
        var(1) = collectmeshinfo(var(0), importer);
        //unloadasset(var(0));
        order = var(1).triangleCount;
        if(var(1).triangleCount >= maxTriangleCount && (!hasAnimation || var(1).clipCount>0)){
            info = format("skinned:{0},mesh:{1},vertex:{2},triangle:{3},bone:{4},material:{5},max_tex_size:({6},{7}),max_tex_name:{8}={9},clip:{10},max_keyframe:{11}",
                var(1).skinnedMeshCount, var(1).meshFilterCount, var(1).vertexCount, var(1).triangleCount, var(1).boneCount, var(1).materialCount, var(1).maxTexWidth, var(1).maxTexHeight, var(1).maxTexPropName, var(1).maxTexName, var(1).clipCount, var(1).maxKeyFrameCount
                );
            1;
        }else{
            0;
        };
    }else{
        0;
    };
};