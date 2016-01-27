skill(121)
{
	section(10)
	{
    hiteffect(hitEffect, "eyes", 0, 1000, "stand", 100);
	};
  section(1000)
  {
    animation("attack");
    adjustsectionduration("anim",100);
    aoeimpact(100,0,0,0,false);
  };
};