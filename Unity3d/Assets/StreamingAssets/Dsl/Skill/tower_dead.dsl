skill(8)
{
  section(4000000)
  {
    animation(@castAnim);
    //adjustsectionduration("anim",100);
    replaceshaderandfadecolor(0,10000000,"Legacy Shaders/Transparent/Diffuse",color(1,1,1,1),color(0,0,0,-1),0);
    selfeffect(@selfEffect,0,@selfEffectBone,0);
  };
  section(100)
  { 
  	deadfinish(0);
  };
};