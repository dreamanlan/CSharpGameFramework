story(main)
{
  local
  {
  	@user(0);
  	@pass(0);
  };
  onmessage("start")
  {
    bindui(@window){
      inputs("Panel/Account","Panel/Passwd");
      onevent("button","ok","Panel/Button");
    };
  	log("LoginDlg start");
    @window.SetActive(changetype(0,"bool"));
  };
  onmessage("show_login")
  {
    publishgfxevent("ui_hide","ui");
    @window.SetActive(changetype(1,"bool"));
  };  
  onnamespacedmessage("on_click")
  {
  	log("LoginDlg on click");
  	@user=listget($1,0,"");
  	@pass=listget($1,1,"");
  	if(@user=="" || @pass==""){
    	highlightprompt(0,"Err_LoginNameOrPass");
  	}else{
	  	firemessage("do_login",@user,@pass);
    	@window.SetActive(changetype(0,"bool"));
	  };
  };
};