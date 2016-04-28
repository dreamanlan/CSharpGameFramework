skill(100)
{
	section(10)
	{
    hiteffect(hitEffect, "eyes", 0, 1000, "Stand", 100);
	};
  section(3000)
  {
  	rotate(0, 3000, vector3(0, 1440, 0));
    selfeffect(selfEffect,3000,"eyes",0)
    {
      transform(vector3(0,1,0));
    };
    aoeimpact(0,0,0,0,false);
    aoeimpact(500,0,0,0,false);
    aoeimpact(1000,0,0,0,false);
    aoeimpact(1500,0,0,0,false);
    aoeimpact(2000,0,0,0,false);
    aoeimpact(2500,0,0,0,false);
  };
  section(100)
  {
    animation("Stand");
  };
};