story(main)
{
  local
  {
  	@user(0);
  	@pass(0);
  };
  onmessage("start")
  {
  	log("LoginDlg start");
    @window.active=changetype(0,"bool");
  };
  onmessage("show_login")
  {
    publishgfxevent("ui_hide","ui");
    @window.active=changetype(1,"bool");
  };  
  onnamespacedmessage("on_click")
  {
  	log("LoginDlg on click");
  	@user=listget($2,0,"");
  	@pass=listget($2,1,"");
  	if(@user=="" || @pass==""){
    	highlightprompt(0,"Err_LoginNameOrPass");
  	}else{
	  	firemessage("do_login",@user,@pass);
	    @window.active=changetype(0,"bool");
	  };
  };
};