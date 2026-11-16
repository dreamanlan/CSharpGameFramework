story(story_main)
{
  onmessage("start")
  {
    publish_gfx_event("loading_complete", "ui");
    //change_scene(3);
  	publish_gfx_event("ge_select_server", "lobby", "ws://127.0.0.1:9001");
    highlight_prompt(0,"Tip_0");
    fire_message("show_login");
  };
  onmessage("version_verify_failed")
  {
    highlight_prompt(0,"Err_VerifyVersion");
    wait(3000);
    quit();
  };
  onmessage("dialog_over",1)
  {
    //change_scene(3);
    change_scene(4);
  };
  onmessage("do_login")params($user,$pwd)
  {
  	publish_gfx_event("ge_account_login", "lobby", $user, $pwd, "");
  };
  onmessage("do_nickname")params($nickname)
  {
  	publish_gfx_event("ge_change_name", "lobby", $nickname);
  };
  onmessage("start_game")
  {
  	show_dlg(1);
  };
};