skill(114)
{  
  section(10)
  {
    hiteffect(hitEffect, "eyes", 0, 1000, "stand", 100);
    facetotarget(0,10,0,"maxdist");
  };
  section(1000)
  {
    animation("attack");
    adjustsectionduration("anim",100);
    selfeffect(selfEffect,1000,"eyes",0)
    {
      transform(vector3(0,1,0));
    };
    emiteffect(emitEffect,"eyes",0,20,100);
  };
};