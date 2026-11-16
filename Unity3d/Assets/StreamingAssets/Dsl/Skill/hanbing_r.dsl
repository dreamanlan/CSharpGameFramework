skill(104)
{  
  section(1000)
  {
    animation(@castAnim);
    lock_frame(0, false, 0, 300, 1, 50, true, 1, 1, 1);
    adjust_section_duration("anim",100);
    self_effect(@selfEffect,0,@selfEffectBone,0);
    face_to_target(0,100);
    emit_effect(@emitEffect,@emitEffectBone,0,0);
  };
};