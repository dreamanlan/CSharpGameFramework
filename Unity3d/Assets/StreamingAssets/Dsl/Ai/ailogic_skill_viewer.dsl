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
  		@aiData = getaidata(@objid,"AiData_General");
  		if(!isnull(@aiData)){
  			@skillId = @aiData.ManualSkillId;
  			if(@skillId>0 && cancastskill(@objid,@skillId)){
  				objsetaitarget(@objid,unitid2objid(1));
  				objcastskill(@objid,@skillId);
  				@aiData.ManualSkillId = 0;
  			};
  		};
  		wait(100);
  	};
  };
};