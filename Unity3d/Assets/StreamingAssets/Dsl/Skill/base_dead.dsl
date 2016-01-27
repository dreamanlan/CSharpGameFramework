skill(9)
{
  section(10000)
  {
    animation(@castAnim);
  	//timescale(0, 0.1, 4000);
    replaceshaderandfadecolor(0,10000000,"Legacy Shaders/Transparent/Diffuse",color(1,1,1,1),color(0,0,0,-1),0);
    selfeffect(@selfEffect,0,@selfEffectBone,0);    
  };
  section(100)
  { 
  	deadfinish(0);
  };
};