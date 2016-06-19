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
    bindui(@window){
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
    @window.SetActive(changetype(1,"bool"));
    @playerPrefs = getunitytype("PlayerPrefs");
    @actorId_Input.text = ""+@playerPrefs.GetInt("ActorId",101);
    @targetId_Input.text = ""+@playerPrefs.GetInt("TargetId",103);
  };
  onnamespacedmessage("on_toggle")
  {
    if($0=="step"){
      sendgfxmessage("GameRoot","OnStepChanged",$1);
    };
  };
  onnamespacedmessage("on_click")
  {
    log("SkillViewer:on_click:{0} {1} {2}",$0,$1,$2);
    if($0=="reload"){
      sendgfxmessage("GameRoot","LoadViewedSkills", str2int(listget($1,0,"101")), str2int(listget($1,1,"1")));
    }elseif($0=="new"){
      sendgfxmessage("GameRoot","NewEditedSkills");
    }elseif($0=="review"){
      sendgfxmessage("GameRoot","LoadEditedSkills", str2int(listget($1,1,"1")));
    }elseif($0=="clipboard"){
      sendgfxmessage("GameRoot","CopyEditedSkillsToClipboard");
    };
  };
};