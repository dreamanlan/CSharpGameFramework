story(local_main)
{
  onmessage("start")
  {
  	startstory("skill_main");
    startstory("auto_story");
  };
  onmessage("open_battle")
  {
    log("open_battle");
    $sceneId = $0;
    openbattle($sceneId);
    firemessage("set_map_image", "UITexture/Map1");
  };
  onmessage("on_battle_closed")
  {
    $objId = $0;
    $unitId = $1;
    setleaderid($objId);
    camerafollow($unitId);
    publishgfxevent("loading_complete", "ui");
    firemessage("set_map_image", "UITexture/Story1");
    wait(3000);
    firemessage("findnpc");
  };
  onmessage("quit")
  {
    changescene(1);
  };
};