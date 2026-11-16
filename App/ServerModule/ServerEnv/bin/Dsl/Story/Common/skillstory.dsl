story(skill_main)
{
  local
  {
    @killObjId(0);
    @anniId(0);
    @anniTargetId(0);
    @bearId(0);
    @anniPos(0); 
    @anniDir(0);
    @hanbingId(0);
    @hanbingArrowId(0);
    @hanbingPos(0);
    @hanbingDir(0);
    @ezId(0);
    @ezArrowId(0);
    @ezPos(0);
    @ezDir(0);
  };
  onmessage("kill_summon_npc")params($senderObj)
  {
    @killObjId=$senderObj.ActorId;
    destroy_npc_with_obj_id(@killObjId);
  };
  onmessage("kill_npc")params($objId,$unitId)
  {
    log("kill_npc");
    set_hp(unit_id_2_obj_id($unitId),0);
  };
  onmessage("summon_bear")params($senderObj,$tableId,$ai)
  {
    @anniId=$senderObj.ActorId;
    @anniTargetId=$senderObj.TargetActorId;
    if(@anniTargetId<=0){
      @anniTargetId = @anniId;
      @anniPos = calc_offset(@anniTargetId,0,vector_3(0,0,2));
      @anniDir=calc_dir(@anniId,0);
    }else{
      @anniPos = calc_offset(@anniTargetId,@anniId,vector_3(0,0,-1));
      @anniDir=calc_dir(@anniTargetId,@anniId);
    };
    log("summon_bear:{0} {1} {2}",@anniId,@anniPos,@anniDir);
    create_npc(0,@anniPos,@anniDir,get_camp(@anniId),str_2_int($tableId),str_2_int($ai),string_list(""))objid("@bearId");
    obj_set_summoner_id(@bearId,@anniId);
    obj_set_formation(@bearId,5);
    obj_set_ai_target(@bearId,@anniTargetId);
    obj_add_impact(@bearId,12);
  };
  onmessage("summon_hanbing_arrow")params($senderObj,$tableId,$ai)
  {
    @hanbingId=$senderObj.ActorId;
    @hanbingPos=get_position(@hanbingId);
    @hanbingDir=calc_dir(@hanbingId,0);
    log("summon_hanbing_arrow:{0} {1} {2}",@hanbingId,@hanbingPos,@hanbingDir);
    create_npc(0,@hanbingPos,@hanbingDir,get_camp(@hanbingId),str_2_int($tableId),str_2_int($ai),string_list(""))objid("@hanbingArrowId");
    obj_set_summoner_id(@hanbingArrowId,@hanbingId);
  };
  onmessage("ez_skill_r")params($senderObj)
  {
    @ezId=$senderObj.ActorId;
    @ezPos=get_position(@ezId);
    @ezDir=calc_dir(@ezId,0);
    log("ez_skill_r:{0} {1} {2}",@ezId,@ezPos,@ezDir);
    create_npc(0,@ezPos,@ezDir,get_camp(@ezId),33,1,string_list("ez_r_arrow Ai/ailogic_ez_r_arrow.dsl"))objid("@ezArrowId");
    obj_set_summoner_id(@ezArrowId,@ezId);
    obj_cast_skill(@ezArrowId,20);
  };
};