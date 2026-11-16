skill(121)
{
	section(10)
	{
    hit_effect(hitEffect, "eyes", 0, 1000, "Stand", 100);
	};
  section(1000)
  {
    animation("Attack");
    adjust_section_duration("anim",100);
    aoe_impact(100,0,0,0,false);
  };
};