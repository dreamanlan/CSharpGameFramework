input
{
  int("maxSize", 10000);
  string("type", "Texture2D"){
    popup("Texture2D","Texture2D");
    popup("Cubemap", "Cubemap");
    popup("Mesh", "Mesh");
    popup("ParticleSystem", "ParticleSystem");
    popup("RenderTexture", "RenderTexture");
    popup("AnimationClip","AnimationClip");
    popup("AudioClip","AudioClip");
    popup("Font", "Font");
  };
	string("filter", "");
	feature("source", "snapshot");
}
filter
{
	order = memory.size;
	if(memory.size >= maxSize && memory.className == type && assetpath.Contains(filter)){
	  info = format("name:{0} class:{1} size:{2} asset:{3} path:{4}",
	      memory.name, memory.className, memory.size, memory.assetPath, memory.scenePath
	    );
	  value = memory.size;
	  1;
	}else{
	  0;
	};
};