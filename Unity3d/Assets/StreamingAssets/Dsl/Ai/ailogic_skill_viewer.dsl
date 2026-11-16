story(skill_viewer)
{
	local
	{
		@aiData(0);
		@skillId(0);
	};
  onmessage(start)
  {
  	while(1){
  		@npcData = get_entity_info(@objid);
  		if(!is_null(@npcData)){
  			@skillId = @npcData.ManualSkillId;
  			if(@skillId>0 && can_cast_skill(@objid,@skillId)){
  				obj_set_ai_target(@objid,unit_id_2_obj_id(1));
  				obj_cast_skill(@objid,@skillId);
				@npcData.ManualSkillId = 0;
  			};
  		};
  		wait(100);
  	};
  };
};