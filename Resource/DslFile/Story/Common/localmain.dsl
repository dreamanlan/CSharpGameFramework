story(local_main)
{
  onmessage("start")
  {
  	startstory("skill_main");    
    startstory("auto_main");
  };
  onmessage("open_battle")
  {
    $sceneId = $0;
    openbattle($sceneId);
  };
  onmessage("on_battle_closed")
  {
    $objId = $0;
    $unitId = $1;
    camerafollow($unitId);
    publishgfxevent("loading_complete", "ui");
    wait(3000);
    firemessage("findnpc");
  };
  onmessage("quit")
  {
    changescene(1);
  };
};