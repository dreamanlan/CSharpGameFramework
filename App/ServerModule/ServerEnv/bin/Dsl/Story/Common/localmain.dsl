story(local_main)
{
  onmessage("start")
  {
  	startstory("skill_main");
    startstory("auto_story");
  };
  onmessage("open_battle")params($sceneId)
  {
    log("open_battle");
    openbattle($sceneId);
    firemessage("set_map_image", "UITexture/Map1");
  };
  onmessage("on_battle_closed")params($objId,$unitId)
  {
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