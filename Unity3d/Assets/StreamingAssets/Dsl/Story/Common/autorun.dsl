story(auto_story)
{
  local
  {
    @sceneKey("");
  };
  onmessage("start")
  {
    @sceneKey="_"+get_scene_id();
    wait(3000);
    if(get_player_id()>0 && get_leader_id()<=0){
      set_leader_id(get_player_id());
    };
    fire_message("findnpc");
  };
  onmessage("findnpc")
  {
    $ct = get_child_count("Config");
    $index=prop_get(@sceneKey, 0);
    if($index<=0){
      $index=1;
      prop_set(@sceneKey, $index);
    };
    $pos = get_position("Config/"+$index);
    log("findnpc, scene key:{0}, index:{1}, pos:{2}", @sceneKey, $index, $pos);
    obj_move(get_player_id(), $pos, "touchnpc");
    inc($index);
    prop_set(@sceneKey, $index);
    if($index>=$ct){ $index=1; prop_set(@sceneKey, $index); };
    wait(1000);
    clear_message("findnpc");
  };
  onmessage("touchnpc")
  {
    log("touchnpc");
    send_room_story_message("touchnpc");
  };
};