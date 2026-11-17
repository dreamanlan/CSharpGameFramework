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
    publish_gfx_event("loading_complete", "ui");
    log("scene2");

    @myNpcPos = vector3(5,0,7);
    @myNpcDir = 1.57;
  };
  onmessage("move_to")params($x,$y,$z)
  {
  	npc_move(1000,vector3($x,$y,$z));
  };
  onmessage("reload")params($leaderTableId,$targetTableId,$skillIds)
  {
    @leaderTableId = $leaderTableId;
    @targetTableId = $targetTableId;
    @skillIds = $skillIds;
    publish_gfx_event("ui_remove_skill_buttons","ui");
    wait(1000);
    destroy_npc(1000);
    destroy_npc(1);
    wait(1000);

    create_npc(1,vector3(0,0,0),0.785,4,@targetTableId);
    set_hp(unit_id_2_obj_id(1),99999999);
    create_npc(1000,@myNpcPos,@myNpcDir,3,@leaderTableId,"skill_viewer",string_list("Ai/ailogic_skill_viewer.dsl"));
    @leaderObjId=unit_id_2_obj_id(1000);
    set_leader_id(@leaderObjId);
    npc_set_formation(1000,12);
    loop_list(@skillIds){
      publish_gfx_event("ui_add_skill_button","ui",@leaderTableId,@leaderObjId,$$);
    };
    camera_follow(1000);
  };
  onmessage("objkilled")params($id,$enemy,$friend,$trigger)
  {
    log("objkilled:id={0},enemy={1},friend={2},trigger={3}",$id,$enemy,$friend,$trigger);
  };
};