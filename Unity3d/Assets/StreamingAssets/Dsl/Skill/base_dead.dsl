skill(9)
{
  section(10000)
  {
    animation(@castAnim);
  	//time_scale(0, 0.1, 4000);
    replace_shader_and_fade_color(0,10000000,"Legacy Shaders/Transparent/Diffuse",color(1,1,1,1),color(0,0,0,-1),0);
    self_effect(@selfEffect,0,@selfEffectBone,0);    
  };
  section(100)
  { 
  	dead_finish(0);
  };
};