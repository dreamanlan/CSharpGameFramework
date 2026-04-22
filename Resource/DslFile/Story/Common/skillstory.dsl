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
    destroynpcwithobjid(@killObjId);
  };
  onmessage("kill_npc")params($objId,$unitId)
  {
    log("kill_npc");
    sethp(unitid2objid($unitId),0);
  };
  onmessage("summon_bear")params($senderObj,$tableId,$ai)
  {
    @anniId=$senderObj.ActorId;
    @anniTargetId=$senderObj.TargetActorId;
    if(@anniTargetId<=0){
      @anniTargetId = @anniId;
      @anniPos = calcoffset(@anniTargetId,0,vector3(0,0,2));
      @anniDir=calcdir(@anniId,0);
    }else{
      @anniPos = calcoffset(@anniTargetId,@anniId,vector3(0,0,-1));
      @anniDir=calcdir(@anniTargetId,@anniId);
    };
    log("summon_bear:{0} {1} {2}",@anniId,@anniPos,@anniDir);
    createnpc(0,@anniPos,@anniDir,getcamp(@anniId),str2int($tableId),str2int($ai),stringlist(""))objid("@bearId");
    objsetsummonerid(@bearId,@anniId);
    objsetformation(@bearId,5);
    objsetaitarget(@bearId,@anniTargetId);
    objaddimpact(@bearId,12);
  };
  onmessage("summon_hanbing_arrow")params($senderObj,$tableId,$ai)
  {
    @hanbingId=$senderObj.ActorId;
    @hanbingPos=getposition(@hanbingId);
    @hanbingDir=calcdir(@hanbingId,0);
    log("summon_hanbing_arrow:{0} {1} {2}",@hanbingId,@hanbingPos,@hanbingDir);
    createnpc(0,@hanbingPos,@hanbingDir,getcamp(@hanbingId),str2int($tableId),str2int($ai),stringlist(""))objid("@hanbingArrowId");
    objsetsummonerid(@hanbingArrowId,@hanbingId);
  };
  onmessage("ez_skill_r")params($senderObj)
  {
    @ezId=$senderObj.ActorId;
    @ezPos=getposition(@ezId);
    @ezDir=calcdir(@ezId,0);
    log("ez_skill_r:{0} {1} {2}",@ezId,@ezPos,@ezDir);
    createnpc(0,@ezPos,@ezDir,getcamp(@ezId),33,1,stringlist("ez_r_arrow Ai/ailogic_ez_r_arrow.dsl"))objid("@ezArrowId");
    objsetsummonerid(@ezArrowId,@ezId);
    objcastskill(@ezArrowId,20);
  };
};