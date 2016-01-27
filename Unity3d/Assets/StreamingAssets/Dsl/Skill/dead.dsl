skill(2)
{
  section(1000)
  {
    animation("death");
    adjustsectionduration("anim",100);
  };
  section(1000)
  {
    replaceshaderandfadecolor(0,1000,"Legacy Shaders/Transparent/Diffuse",color(1,1,1,1),color(0,0,0,-1));
  	deadfinish(900);
  };
};