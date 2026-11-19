story(auto_battle)
{
  // Stage 2: the client reports its battle-layer position on a timer and asks
  // for an exploration check only when the player actively uses the explore
  // skill (E key -> cast_explore). The client never receives event ids, exact
  // event positions, radii or rewards.
  onmessage("start")
  {
    // The grid id is sent by each per-scene story (it knows its own tile).
    fire_message("report_pos");
  };
  // Clicking the ground sends "move_to" from Game.cs. All 640 battlefield
  // scenes share this ClientDslFile, so one handler here covers every one.
  // Resolve the leader by unitId at click time: PluginFramework.LeaderId is a
  // cached objId that survives entity teardown/rebuild between battle rounds,
  // so get_leader_id() can point at an already destroyed actor (obj_move then
  // silently no-ops on GetEntityById). unit_id_2_obj_id(1000) looks up the
  // live player each time, matching how battle_main sets the leader.
  // obj_move switches the AI to MoveCommand; it returns to Idle on arrival.
  onmessage("move_to")params($x,$y,$z)
  {
    obj_move(unit_id_2_obj_id(1000),vector3($x,$y,$z));
  };
  onmessage("report_pos")
  {
    $obj = get_leader_id();
    send_room_story_message("battle_pos", get_position_x($obj), get_position_z($obj));
    wait(300);
    fire_message("report_pos");
  };
  onmessage("cast_explore")
  {
    $obj = get_leader_id();
    obj_cast_skill($obj, 100);
    fire_message("push_tip_info", "正在调查周围…");
    send_room_story_message("explore", get_position_x($obj), get_position_z($obj));
  };
  onmessage("explore_skill")
  {
    fire_message("cast_explore");
  };
  // GM diagnostic path: simulate a player standing at the given spot.
  onmessage("cast_explore_at")params($x,$z)
  {
    obj_cast_skill(get_leader_id(), 100);
    send_room_story_message("battle_pos", $x, $z);
    send_room_story_message("explore", $x, $z);
  };
};
