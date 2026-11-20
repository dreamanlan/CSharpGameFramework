story(local_main)
{
  onmessage("start")
  {
  	start_story("skill_main");
    start_story("auto_story");
  };
  onmessage("open_battle")params($sceneId)
  {
    log("open_battle");
    open_battle($sceneId);
    fire_message("set_map_image", "UITexture/WorldMap");
  };
  onmessage("on_battle_closed")params($objId,$unitId)
  {
    set_leader_id($objId);
    camera_follow($unitId);
    publish_gfx_event("loading_complete", "ui");
    fire_message("set_map_image", "UITexture/WorldMap");
    wait(3000);
    fire_message("findnpc");
  };
  // Server-authoritative exploration result. A miss, a cooldown rejection and a
  // closed event are deliberately indistinguishable to the client.
  onmessage("explore_result")params($found,$token,$delta,$total)
  {
    log("explore_result found={0} token={1} delta={2} total={3}", $found, $token, $delta, $total);
    if($found>0){
      fire_message("push_tip_info", "发现新的线索，真知累积中");
    } else {
      fire_message("push_tip_info", "这里似乎没有发现");
    };
  };
  // World-map visible state marker: the player stays at the portal while
  // "diving" into the past layer of the same location.
  onmessage("parallel_state")params($roleId,$state,$grid,$era)
  {
    log("parallel_state role={0} state={1} grid={2} era={3}", $roleId, $state, $grid, $era);
    fire_message("push_tip_info", "时空锚点亮起：有调查者正在穿越时代层");
  };
  onmessage("quit")
  {
    change_scene(1);
  };
};