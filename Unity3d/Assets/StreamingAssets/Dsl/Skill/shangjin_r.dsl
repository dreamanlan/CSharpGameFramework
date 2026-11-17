skill(109)
{
	section(10)
	{
    hit_effect(hitEffect, "eyes", 0, 1000, "Stand", 100);
	};
  section(3000)
  {
    animation("Attack");
    self_effect(selfEffect,1000,"eyes",0)
    {
      transform(vector3(0,1,0));
    };
    self_effect(selfEffect,1000,"eyes",500)
    {
      transform(vector3(0,1,0));
    };
    self_effect(selfEffect,1000,"eyes",1000)
    {
      transform(vector3(0,1,0));
    };
    periodically_aoe_impact(-3, 0, 0, 0, false, 3000, 400);
  };
  section(100)
  {
    animation("Stand");
  };
};