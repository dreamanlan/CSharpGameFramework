story(auto_battle)
{
  // Stage 2: the client reports its battle-layer position on a timer and asks
  // for an exploration check only when the player actively uses the explore
  // skill (E key -> cast_explore). The client never receives event ids, exact
  // event positions, radii or rewards.
  onmessage("start")
  {
    sendroomstorymessage("enter_parallel", getsceneid());
    firemessage("report_pos");
  };
  onmessage("report_pos")
  {
    $obj = getleaderid();
    sendroomstorymessage("battle_pos", getpositionx($obj), getpositionz($obj));
    wait(300);
    firemessage("report_pos");
  };
  onmessage("cast_explore")
  {
    $obj = getleaderid();
    objcastskill($obj, 100);
    firemessage("push_tip_info", "正在调查周围…");
    sendroomstorymessage("explore", getpositionx($obj), getpositionz($obj));
  };
  onmessage("explore_skill")
  {
    firemessage("cast_explore");
  };
  // GM diagnostic path: simulate a player standing at the given spot.
  onmessage("cast_explore_at")params($x,$z)
  {
    objcastskill(getleaderid(), 100);
    sendroomstorymessage("battle_pos", $x, $z);
    sendroomstorymessage("explore", $x, $z);
  };
};
