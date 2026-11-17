skill(6)
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
    impact(0,0,0,0,false);
  };
	hitskill
	{
		section(1000)
		{
	    animation("Hit")adjustduration();
	    self_effect(hitEffect, 1000, hit, 0);
	    damage(0);
		};
	};
};