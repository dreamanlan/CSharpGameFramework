input("*.anim")
{
	string("filter", "");
	feature("source", "project");
	feature("menu", "1.Project Resources/Animation Clip");
	feature("description", "just so so");
}
filter
{
  if(assetpath.Contains(filter)){
  	var(0) = getanimationclipinfo();
  	order = var(0).maxKeyFrameCount;
  	var(1) = newstringbuilder();
	  appendlineformat(var(1), "clip name:{0}, max keyframe count:{1}, curve:{2}",
	    var(0).clipName, var(0).maxKeyFrameCount, var(0).maxKeyFrameCurveName
	    );
	  info = stringbuildertostring(var(1));
	  1;
	}else{
	  0;
	};
};