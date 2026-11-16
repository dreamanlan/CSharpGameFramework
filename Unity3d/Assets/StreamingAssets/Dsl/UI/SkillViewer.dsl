story(main)
{
  local
  {
    @actorId(0);
    @targetId(0);
    @playerPrefs(0);
  };
  onmessage("start")
  {
    bind_ui(@window){
      var("@actorId","Panel/Actor");
      var("@targetId","Panel/Actor2");
      inputs("Panel/Actor","Panel/Actor2");
      toggles("Panel/SingleStep");
      onevent("toggle","step","Panel/SingleStep");
      onevent("button","reload","Panel/Reload");
      onevent("button","new","Panel/NewHeroSkills");
      onevent("button","review","Panel/Review");
      onevent("button","clipboard","Panel/CopyToClipboard");
    };
    @window.SetActive(change_type(1,"bool"));
    @playerPrefs = get_unity_type("PlayerPrefs");
    @actorId_Input.text = ""+@playerPrefs.GetInt("ActorId",1);
    @targetId_Input.text = ""+@playerPrefs.GetInt("TargetId",2);
  };
  onnamespacedmessage("on_toggle")
  {
    if(arg(0)=="step"){
      send_gfx_message("GameRoot","OnStepChanged",arg(1));
    };
  };
  onnamespacedmessage("on_click")
  {
    log("SkillViewer:on_click:{0} {1} {2}",arg(0),arg(1),arg(2));
    if(arg(0)=="reload"){
      send_gfx_message("GameRoot","LoadViewedSkills", str_2_int(list_get(arg(1),0,"1")), str_2_int(list_get(arg(1),1,"2")));
    }elseif(arg(0)=="new"){
      send_gfx_message("GameRoot","NewEditedSkills");
    }elseif(arg(0)=="review"){
      send_gfx_message("GameRoot","LoadEditedSkills", str_2_int(list_get(arg(1),1,"1")));
    }elseif(arg(0)=="clipboard"){
      send_gfx_message("GameRoot","CopyEditedSkillsToClipboard");
    };
  };
};