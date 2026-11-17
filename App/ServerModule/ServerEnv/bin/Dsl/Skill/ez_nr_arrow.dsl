skill(118)
{
	section(10)
	{
    hit_effect(hitEffect, "eyes", 0, 1000, "Stand", 100);
	};
  section(10000)
  {
  	enable_move_agent(false);
    charge(10000,50,0,vector3(0,0,0),0);
    collider_impact(0, 0, 0, 0, 10000, false, false);
  };
  section(100)
  {
  	enable_move_agent(true);
    send_story_message(0,"ez_r_arrow_kill");
  };
};