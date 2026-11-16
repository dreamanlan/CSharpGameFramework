skill(2)
{
  section(1000)
  {
    animation("Death");
    adjust_section_duration("anim",100);
  };
  section(1000)
  {
    replace_shader_and_fade_color(0,1000,"Legacy Shaders/Transparent/Diffuse",color(1,1,1,1),color(0,0,0,-1));
  	dead_finish(900);
  };
};