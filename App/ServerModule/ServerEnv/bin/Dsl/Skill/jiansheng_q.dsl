skill(113)
{  
	section(10)
	{
    hiteffect(hitEffect, "eyes", 0, 1000, "stand", 100);
	};
  section(1000)
  {
    animation("attack");
    adjustsectionduration("anim",100);
    selfeffect(selfEffect,1000,"eyes",0)
    {
      transform(vector3(0,1,0));
    };
  };
  section(1100)
  {
    publishgfxevent(0, "ui_hp_hud_visible_for_skill", "ui", 0);
    publishgfxevent(1000, "ui_hp_hud_visible_for_skill", "ui", 1);
    replaceshaderandfadecolor(0,1000,"Legacy Shaders/Transparent/Diffuse",color(1,1,1,1),color(0,0,0,-1),100);   
  	chainaoeimpact(0, 0, 0, 0, false, 1000, 250);
  	addstate("hidden");
  	removestate("hidden",1000);
  };
  onstop
  {
    removestate("hidden");
  };
};