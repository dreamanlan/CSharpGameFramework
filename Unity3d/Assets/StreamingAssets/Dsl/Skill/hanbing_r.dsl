skill(104)
{  
  section(1000)
  {
    animation(@castAnim);
    lockframe(0, false, 0, 300, 1, 50, true, 1, 1, 1);
    adjustsectionduration("anim",100);
    selfeffect(@selfEffect,0,@selfEffectBone,0);
    facetotarget(0,100);
    emiteffect(@emitEffect,@emitEffectBone,0,0);
  };
};