input("*.prefab")
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
};