input("ParticleSystem")
{
	string("filter", "");
	float("pathwidth",240){range(20,4096);};
	feature("source", "sceneobjects");
	feature("menu", "3.Current Scene Objects/Particles");
	feature("description", "just so so");
}
filter
{
	var(0) = getcomponent(object,"ParticleSystem");
	if(assetpath.Contains(filter)){
	  var(1) = var(0).emission.rateOverTime;
	  var(2) = var(0).main.startDelay;
	  var(3) = var(0).main.startLifetime;
	  info = format("max particles:{0} rate over time:{1}-{2} duration:{3} start delay:{4}-{5} start lifetime:{6}-{7}",var(0).main.maxParticles,var(1).constantMin,var(1).constantMax,var(0).main.duration,var(2).constantMin,var(2).constantMax,var(3).constantMin,var(3).constantMax);
		1;
	}else{
		0;
	};
};