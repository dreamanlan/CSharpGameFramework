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
    @window.active=changetype(0,"bool");
  };
  onmessage("ShowDlg")
  {
    publishgfxevent("ui_hide","ui");
    @isFinish=0;
    @dlgId = $0;
    @index = 0;
    localnamespacedmessage("ShowDlgItem",@dlgId,@index);
    log("First,ShowDlgItem:{0} {1}",@dlgId,@index);
    while(@isFinish<=0){
      waitlocalnamespacedmessage("OnClick")set("@tag",0)timeoutset(5000,"@tag",1);
      if(@tag>0 && @isFinish<=0){
        inc(@index);
        localnamespacedmessage("ShowDlgItem",@dlgId,@index);
        log("Timeout,ShowDlgItem:{0} {1}",@dlgId,@index);
      };
    };
    clearmessage("ShowDlg");
  };  
  onnamespacedmessage("OnClick")
  {
    inc(@index);
    localnamespacedmessage("ShowDlgItem",@dlgId,@index);
    log("OnClick,ShowDlgItem:{0} {1}",@dlgId,@index);
  };
  onnamespacedmessage("ShowDlgItem")
  {
    @dlgId = $0;
    @index = $1;
    
    @dlgItem = getdialogitem(@dlgId,@index);
    if(isnull(@dlgItem)){
      if(@isFinish<=0){
        @window.active=changetype(0,"bool");
        publishgfxevent("ui_show","ui");
        firemessage("DlgFinish:"+@dlgId);
      };
      log("Finish,ShowDlgItem:{0} {1}",@dlgId,@index);
      @isFinish=1;
    }else{
      if(@dlgItem.leftOrRight>0){
        //@left.active=changetype(0,"bool");
        //@right.active=changetype(1,"bool");
        @right_Image.sprite=getactoricon(@dlgItem.speaker);
      }else{
        //@left.active=changetype(1,"bool");
        //@right.active=changetype(0,"bool");
        @left_Image.sprite=getactoricon(@dlgItem.speaker);
      };        
      @text_Text.text = @dlgItem.dialog;
      @window.active=changetype(1,"bool");
    };
  };
};