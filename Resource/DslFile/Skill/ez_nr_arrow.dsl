skill(118)
{
	section(10)
	{
    hiteffect(hitEffect, "eyes", 0, 1000, "stand", 100);
	};
  section(10000)
  {
  	enablemoveagent(false);
    charge(10000,50,0,vector3(0,0,0),0);
    colliderimpact(0, 0, 0, 0, 10000, false, false);
  };
  section(100)
  {
  	enablemoveagent(true);
    sendstorymessage(0,"ez_r_arrow_kill");
  };
};