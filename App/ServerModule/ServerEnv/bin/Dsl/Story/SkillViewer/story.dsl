story(story_main)
{
  local
  {
    @myNpcPos(0);
    @myNpcDir(0);
    @leaderTableId(101);
    @leaderObjId(0);
    @targetTableId(1);
    @skillIds(0);
  };
  onmessage("start")
  {    
    preload();
    wait(1000);
    publishgfxevent("loading_complete", "ui");
    log("scene2");
    
    @myNpcPos = vector3(5,0,7);
    @myNpcDir = 1.57;        
  };
  onmessage("move_to")
  {
  	npcmove(1000,vector3($0,$1,$2));
  };
  onmessage("reload")
  {
    @leaderTableId = $0;
    @targetTableId = $1;
    @skillIds = $2;
    publishgfxevent("ui_remove_skill_buttons","ui");
    wait(1000);
    destroynpc(1000);
    destroynpc(1);
    wait(1000);

    createnpc(1,vector3(0,0,0),0.785,4,@targetTableId);
    sethp(unitid2objid(1),99999999);
    createnpc(1000,@myNpcPos,@myNpcDir,3,@leaderTableId,"skill_viewer",stringlist("Ai/ailogic_skill_viewer.dsl"));
    @leaderObjId=unitid2objid(1000);
    setleaderid(@leaderObjId);
    npcsetformation(1000,12);
    looplist(@skillIds){
      publishgfxevent("ui_add_skill_button","ui",@leaderTableId,@leaderObjId,$$);
    };
    camerafollow(1000);
  };
  onmessage("objkilled")
  {
    log("objkilled:id={0},enemy={1},friend={2},trigger={3}",$0,$1,$2,$3);
  };
};