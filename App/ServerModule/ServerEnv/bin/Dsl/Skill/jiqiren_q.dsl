skill(114)
{  
  section(10)
  {
    hit_effect(hitEffect, "eyes", 0, 1000, "Stand", 100);
    face_to_target(0,10,0,"maxdist");
  };
  section(1000)
  {
    animation("Attack");
    adjust_section_duration("anim",100);
    self_effect(selfEffect,1000,"eyes",0)
    {
      transform(vector_3(0,1,0));
    };
    emit_effect(emitEffect,"eyes",0,20,100);
  };
};