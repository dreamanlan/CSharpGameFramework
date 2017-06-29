story(auto_story)
{
  local
  {
    @sceneKey("");
  };
  onmessage("start")
  {
    @sceneKey="_"+getsceneid();
    wait(3000);
    if(getplayerid()>0 && getleaderid()<=0){
      setleaderid(getplayerid());
    };
    firemessage("findnpc");
  };
  onmessage("findnpc")
  {
    $ct = getchildcount("Config");
    $index=propget(@sceneKey, 0);
    if($index<=0){
      $index=1;
      propset(@sceneKey, $index);
    };
    $pos = getposition("Config/"+$index);
    log("findnpc, scene key:{0}, index:{1}, pos:{2}", @sceneKey, $index, $pos);
    objmove(getplayerid(), $pos, "touchnpc");
    inc($index);
    propset(@sceneKey, $index);
    if($index>=$ct){
      $index=1;
    };
    wait(1000);
    clearmessage("findnpc");
  };
  onmessage("touchnpc")
  {
    log("touchnpc");
    sendroomstorymessage("touchnpc");
  };
};