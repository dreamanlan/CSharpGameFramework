input("*.anim")
{
    int("maxKeyFrameCount", 500);
    stringlist("filter", "");
    stringlist("notfilter", "");
	float("pathwidth",240){range(20,4096);};
    feature("source", "project");
    feature("menu", "1.Project Resources/Animation Clip");
    feature("description", "just so so");
}
filter
{
    if(stringcontains(assetpath, filter) && stringnotcontains(assetpath, notfilter)){
        var(0) = getanimationclipinfo();
        order = var(0).maxKeyFrameCount;
        if(var(0).maxKeyFrameCount>=maxKeyFrameCount){
            info = format("clip_name:{0},max_keyframe_count:{1},curve:{2}",
                var(0).clipName, var(0).maxKeyFrameCount, var(0).maxKeyFrameCurveName
                );
            1;
        };
    }else{
        0;
    };
};