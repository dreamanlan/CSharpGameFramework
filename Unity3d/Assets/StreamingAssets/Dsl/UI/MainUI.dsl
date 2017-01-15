story(main)
{
  local
  {
  };
  onmessage("start")
  {
    bindui(@window){
      var("@btn1","Panel/Button1/Text");
      var("@btn2","Panel/Button2/Text");
      var("@btn3","Panel/Button3/Text");
      var("@btn4","Panel/Button4/Text");
      onevent("button","btn1","Panel/Button1");
      onevent("button","btn2","Panel/Button2");
      onevent("button","btn3","Panel/Button3");
      onevent("button","btn4","Panel/Button4");
    };
    installplugin("MainUI/Panel/TextBkg/ScrollView/Viewport/Content", "UiScrollInfo", 1, 0);
    installplugin("MainUI/Panel/Image/RawImage", "MiniMap", 1, 0);
    @window.SetActive(changetype(1,"bool"));
  };
  onmessage("push_tip_info")
  {
    sendgfxmessage("MainUI/Panel/TextBkg/ScrollView/Viewport/Content", "CallScript", "PushInfo", $0);
  };
  onmessage("set_button_text")
  {
  	@btn1_Text.text = $0;
  	@btn2_Text.text = $1;
  	@btn3_Text.text = $2;
  	@btn4_Text.text = $3;
  };
  onnamespacedmessage("on_click")
  {
  	log("MainUI on click {0}",$0);
	  firemessage("on_main_ui_button", $0);
  };
};