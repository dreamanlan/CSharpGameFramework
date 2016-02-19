skill(109)
{
	section(10)
	{
    hiteffect(hitEffect, "eyes", 0, 1000, "stand", 100);
	};
  section(3000)
  {
    animation("attack");
    selfeffect(selfEffect,1000,"eyes",0)
    {
      transform(vector3(0,1,0));
    };
    selfeffect(selfEffect,1000,"eyes",500)
    {
      transform(vector3(0,1,0));
    };
    selfeffect(selfEffect,1000,"eyes",1000)
    {
      transform(vector3(0,1,0));
    };
    periodicallyaoeimpact(-3, 0, 0, 0, false, 3000, 400);
  };
  section(100)
  {
    animation("stand");
  };
};