story(story_main)
{
  onmessage("start")
  {
  	publishgfxevent("ge_select_server", "lobby", "ws://127.0.0.1:9001");
    highlightprompt(0,"Tip_0");
    firemessage("show_login");
  };
  onmessage("version_verify_failed")
  {
    highlightprompt(0,"Err_VerifyVersion");
    wait(3000);
    quit();
  };
  onmessage("dialog_over",1)
  {
    //changescene(2);
    changescene(3);
  };
  onmessage("do_login")
  {  	
  	publishgfxevent("ge_account_login", "lobby", $0, $1, "");
  };
  onmessage("do_nickname")
  {
  	publishgfxevent("ge_change_name", "lobby", $0);
  };
  onmessage("start_game")
  {
  	showdlg(1);
  };
};