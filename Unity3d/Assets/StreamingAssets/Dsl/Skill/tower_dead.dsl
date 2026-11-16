skill(8)
{
  section(4000000)
  {
    animation(@castAnim);
    //adjust_section_duration("anim",100);
    replace_shader_and_fade_color(0,10000000,"Legacy Shaders/Transparent/Diffuse",color(1,1,1,1),color(0,0,0,-1),0);
    self_effect(@selfEffect,0,@selfEffectBone,0);
  };
  section(100)
  { 
  	dead_finish(0);
  };
};