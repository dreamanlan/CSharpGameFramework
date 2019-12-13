input("*.prefab")
{
	feature("source", "project");
	feature("menu", "1.Project Resources/Prefab Setting");
	feature("description", "just so so");	
}
filter
{
	object = loadasset(assetpath);
	var(0) = getcomponentinchildren(object,"SkinnedMeshRenderer");
	if(!isnull(var(0))){
		1;
	}else{
		0;
	};
}
process
{
	looplist(getcomponentsinchildren(object,"SkinnedMeshRenderer")){
		$$.skinnedMotionVectors=false;
	};	
	looplist(getcomponentsinchildren(object,"Playables.PlayableDirector")){
		$$.playOnAwake=false;
	};
};