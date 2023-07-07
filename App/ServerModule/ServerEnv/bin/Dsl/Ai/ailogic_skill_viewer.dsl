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
  		@npcData = getentityinfo(@objid);
  		if(!isnull(@npcData)){
  			@skillId = @npcData.ManualSkillId;
  			if(@skillId>0 && cancastskill(@objid,@skillId)){
  				objsetaitarget(@objid,unitid2objid(1));
  				objcastskill(@objid,@skillId);
				@npcData.ManualSkillId = 0;
  			};
  		};
  		wait(100);
  	};
  };
};