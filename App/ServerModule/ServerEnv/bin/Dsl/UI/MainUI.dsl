story(main)
{
  local
  {
  };
  onmessage("start")
  {
    bind_ui(@window){
      var("@btn1","Panel/Button1/Text");
      var("@btn2","Panel/Button2/Text");
      var("@btn3","Panel/Button3/Text");
      var("@btn4","Panel/Button4/Text");
      onevent("button","btn1","Panel/Button1");
      onevent("button","btn2","Panel/Button2");
      onevent("button","btn3","Panel/Button3");
      onevent("button","btn4","Panel/Button4");
    };
    @window.SetActive(change_type(1,"bool"));
    install_plugin("MainUI/Panel/TextBkg/ScrollView/Viewport/Content", "UiScrollInfo", 1, 0);
    install_plugin("MainUI/Panel/Image/RawImage", "MiniMap", 1, 0);
  };
  onmessage("set_map_image")
  {
    send_gfx_message("MainUI/Panel/Image/RawImage", "CallScript", "SetImage", arg(0));
  };
  onmessage("push_tip_info")
  {
    send_gfx_message("MainUI/Panel/TextBkg/ScrollView/Viewport/Content", "CallScript", "PushInfo", arg(0));
  };
  onmessage("set_button_text")
  {
  	@btn1_Text.text = arg(0);
  	@btn2_Text.text = arg(1);
  	@btn3_Text.text = arg(2);
  	@btn4_Text.text = arg(3);
  };
  onnamespacedmessage("on_click")
  {
  	log("MainUI on click {0}",arg(0));
	  fire_message("on_main_ui_button", arg(0));
  };
};