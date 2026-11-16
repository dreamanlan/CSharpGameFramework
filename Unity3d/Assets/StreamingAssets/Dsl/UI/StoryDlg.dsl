story(main)
{
  local
  {
    @dlgId(0);
    @index(0);
    @dlgItem(0);
    @tag(0);
    @isFinish(0);
  };
  onmessage("start")
  {
    bind_ui(@window){
      onevent("button","btn","Panel/Button");
    };
    install_plugin("StoryDlg/Panel/ScrollView/Viewport/Content", "UiScrollInfo", 1, 0);
    @window.SetActive(change_type(0,"bool"));
  };
  onmessage("show_dlg")params($dlgId)
  {
    publish_gfx_event("ui_hide","ui");
    @isFinish=0;
    @dlgId = $dlgId;
    @index = 0;
    local_namespaced_message("show_dlg_item",@dlgId,@index);
    log("First,show_dlg_item:{0} {1}",@dlgId,@index);
    while(@isFinish<=0){
      wait_local_namespaced_message("on_click")set("@tag",0)timeoutset(5000,"@tag",1);
      if(@tag>0 && @isFinish<=0){
        inc(@index);
        local_namespaced_message("show_dlg_item",@dlgId,@index);
        log("Timeout,show_dlg_item:{0} {1}",@dlgId,@index);
      };
    };
    clear_message("show_dlg");
  };
  onnamespacedmessage("on_click")
  {
    inc(@index);
    local_namespaced_message("show_dlg_item",@dlgId,@index);
    log("on_click,show_dlg_item:{0} {1}",@dlgId,@index);
  };
  onnamespacedmessage("show_dlg_item")params($dlgId,$index)
  {
    @dlgId = $dlgId;
    @index = $index;

    @dlgItem = get_dialog_item(@dlgId,@index);
    if(is_null(@dlgItem)){
      if(@isFinish<=0){
        @window.SetActive(change_type(0,"bool"));
        publish_gfx_event("ui_show","ui");
        fire_message("dialog_over:"+@dlgId);
      };
      log("Finish,show_dlg_item:{0} {1}",@dlgId,@index);
      @isFinish=1;
    }else{
      /*
      if(@dlgItem.leftOrRight>0){
        @left.SetActive(change_type(0,"bool"));
        @right.SetActive(change_type(1,"bool"));
        @right_Image.sprite=get_actor_icon(@dlgItem.speaker);
      }else{
        @left.SetActive(change_type(1,"bool"));
        @right.SetActive(change_type(0,"bool"));
        @left_Image.sprite=get_actor_icon(@dlgItem.speaker);
      };
      */
      $txt = @dlgItem.dialog;
      @window.SetActive(change_type(1,"bool"));
      send_gfx_message("StoryDlg/Panel/ScrollView/Viewport/Content", "CallScript", "PushInfo", $txt);
      log("show_dlg:{0} {1} {2}",@dlgId,@index,$txt);
    };
  };
};