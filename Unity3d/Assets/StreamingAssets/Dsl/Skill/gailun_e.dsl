skill(100)
{
	section(10)
	{
    hit_effect(hitEffect, "eyes", 0, 1000, "Stand", 100);
	};
  section(3000)
  {
  	rotate(0, 3000, vector_3(0, 1440, 0));
    self_effect(selfEffect,3000,"eyes",0)
    {
      transform(vector_3(0,1,0));
    };
    aoe_impact(0,0,0,0,false);
    aoe_impact(500,0,0,0,false);
    aoe_impact(1000,0,0,0,false);
    aoe_impact(1500,0,0,0,false);
    aoe_impact(2000,0,0,0,false);
    aoe_impact(2500,0,0,0,false);
  };
  section(100)
  {
    animation("Stand");
  };
};