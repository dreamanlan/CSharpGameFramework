skill(3)
{
	section(10)
	{
    hiteffect(hitEffect, "eyes", 0, 1000, "stand", 100);
	};
  section(1000)
  {
    animation("attack");
    adjustsectionduration("anim",200);
    selfeffect(selfEffect,1000,"eyes",0)
    {
      transform(vector3(0,1,0));
    };
    impact(100,0,0,0,false);
  };
};