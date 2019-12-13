input("*.controller")
{
    int("maxKeyFrameCount", 500);
    stringlist("filter", "");
    stringlist("notfilter", "");
	float("pathwidth",240){range(20,4096);};
    feature("source", "project");
    feature("menu", "1.Project Resources/Animation Controller");
    feature("description", "just so so");
}
filter
{
    if(stringcontains(assetpath, filter) && stringnotcontains(assetpath, notfilter)){
        var(0) = loadasset(assetpath);  
        var(1) = collectanimatorcontrollerinfo(var(0));
        //unloadasset(var(0));
        order = var(1).maxKeyFrameCount;
        if(var(1).maxKeyFrameCount >= maxKeyFrameCount){
            var(2) = var(1).clips.orderbydesc($$.maxKeyFrameCount).top(4);
            var(3) = newstringbuilder();
            appendlineformat(var(3), "clip name:{0}, total max keyframe count:{1}, layer count:{2}, state count:{3}, sub state machine count:{4}",
                var(1).maxKeyFrameClipName, var(1).maxKeyFrameCount, var(1).layerCount, var(1).stateCount, var(1).subStateMachineCount
                );
            looplist(var(2)){
                appendlineformat(var(3), "clip name:{0}, max keyframe count:{1}", $$.clipName, $$.maxKeyFrameCount);
            };
            info = stringbuildertostring(var(3));
            1;
        }else{
            0;
        };
    }else{
        0;
    };
};