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
    bindui(@window){
      onevent("button","btn","Panel/Button");
    };
    installplugin("StoryDlg/Panel/ScrollView/Viewport/Content", "UiScrollInfo", 1, 0);
    @window.SetActive(changetype(0,"bool"));
  };
  onmessage("show_dlg")
  {
    publishgfxevent("ui_hide","ui");
    @isFinish=0;
    @dlgId = $0;
    @index = 0;
    localnamespacedmessage("show_dlg_item",@dlgId,@index);
    log("First,show_dlg_item:{0} {1}",@dlgId,@index);
    while(@isFinish<=0){
      waitlocalnamespacedmessage("on_click")set("@tag",0)timeoutset(5000,"@tag",1);
      if(@tag>0 && @isFinish<=0){
        inc(@index);
        localnamespacedmessage("show_dlg_item",@dlgId,@index);
        log("Timeout,show_dlg_item:{0} {1}",@dlgId,@index);
      };
    };
    clearmessage("show_dlg");
  };  
  onnamespacedmessage("on_click")
  {
    inc(@index);
    localnamespacedmessage("show_dlg_item",@dlgId,@index);
    log("on_click,show_dlg_item:{0} {1}",@dlgId,@index);
  };
  onnamespacedmessage("show_dlg_item")
  {
    @dlgId = $0;
    @index = $1;
    
    @dlgItem = getdialogitem(@dlgId,@index);
    if(isnull(@dlgItem)){
      if(@isFinish<=0){
        @window.SetActive(changetype(0,"bool"));
        publishgfxevent("ui_show","ui");
        firemessage("dialog_over:"+@dlgId);
      };
      log("Finish,show_dlg_item:{0} {1}",@dlgId,@index);
      @isFinish=1;
    }else{
      /*
      if(@dlgItem.leftOrRight>0){        
        @left.SetActive(changetype(0,"bool"));
        @right.SetActive(changetype(1,"bool"));
        @right_Image.sprite=getactoricon(@dlgItem.speaker);
      }else{
        @left.SetActive(changetype(1,"bool"));
        @right.SetActive(changetype(0,"bool"));
        @left_Image.sprite=getactoricon(@dlgItem.speaker);
      };
      */        
      $txt = @dlgItem.dialog;
      @window.SetActive(changetype(1,"bool"));
      sendgfxmessage("StoryDlg/Panel/ScrollView/Viewport/Content", "CallScript", "PushInfo", $txt);
      log("show_dlg:{0} {1} {2}",@dlgId,@index,$txt);
    };
  };
};