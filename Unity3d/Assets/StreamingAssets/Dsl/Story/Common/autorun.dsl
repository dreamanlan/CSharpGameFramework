story(auto_main)
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
    objmove(getleaderid(), getposition("Config/"+$index), "touchnpc");
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
    sendroomstorymessage("touchnpc");
  };
};