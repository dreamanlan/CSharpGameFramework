story(main)
{
  local
  {
  };
  onmessage("start")
  {
    bind_ui(@window){
      inputs("Panel/Account","Panel/Passwd");
      onevent("button","ok","Panel/Button");
    };
  	log("LoginDlg start");
    @window.SetActive(change_type(0,"bool"));
  };
  onmessage("show_login")
  {
    publish_gfx_event("ui_hide","ui");
    @window.SetActive(change_type(1,"bool"));
  };
  onnamespacedmessage("on_click")
  {
  	log("LoginDlg on click");
  	$user=list_get(arg(1),0,"");
  	$pass=list_get(arg(1),1,"");
  	if($user=="" || $pass==""){
    	highlight_prompt(0,"Err_LoginNameOrPass");
  	}else{
	  	fire_message("do_login",$user,$pass);
    	@window.SetActive(change_type(0,"bool"));
	  };
  };
};