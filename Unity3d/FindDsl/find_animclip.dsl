input("*.anim")
{
	string("filter", "");
}
filter
{
  if(assetpath.Contains(filter)){
  	var(0) = getanimationclipinfo();
	  info = format("clip name:{0}, max keyframe count:{1}, curve:{2}",
	    var(0).clipName, var(0).maxKeyFrameCount, var(0).maxKeyFrameCurveName
	    );
	  1;
	}else{
	  0;
	};
};