skill(113)
{  
	section(10)
	{
    hit_effect(hitEffect, "eyes", 0, 1000, "Stand", 100);
	};
  section(1000)
  {
    animation("Attack");
    adjust_section_duration("anim",100);
    self_effect(selfEffect,1000,"eyes",0)
    {
      transform(vector3(0,1,0));
    };
  };
  section(1100)
  {
    publish_gfx_event(0, "ui_hp_hud_visible_for_skill", "ui", 0);
    publish_gfx_event(1000, "ui_hp_hud_visible_for_skill", "ui", 1);
    replace_shader_and_fade_color(0,1000,"Legacy Shaders/Transparent/Diffuse",color(1,1,1,1),color(0,0,0,-1),100);   
  	chain_aoe_impact(0, 0, 0, 0, false, 1000, 250);
    add_impact_to_self(0,10003);
    stop_impact(1000,10003);
  };
  onstop
  {
    stop_impact(0,10003);
  };
};