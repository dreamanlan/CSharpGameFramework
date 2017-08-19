input("*.controller")
{
	string("filter", "");
}
filter
{
  var(0) = loadasset(assetpath);  
  var(1) = collectanimatorcontrollerinfo(var(0));
	//unloadasset(var(0));
	order = var(1).maxKeyFrameCount;
	if(assetpath.Contains(filter)){
	  info = format("clip name:{0}, max keyframe count:{1}, layer count:{2}, state count:{3}, sub state machine count:{4}",
	    var(1).maxKeyFrameClipName, var(1).maxKeyFrameCount, var(1).layerCount, var(1).stateCount, var(1).subStateMachineCount
	    );
	  1;
	}else{
	  0;
	};
};