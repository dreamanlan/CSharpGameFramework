story(ai_member)
{
  onmessage(start)
  {
    $objid = @objid;
    while(1){
      ai_do_member($objid);
      wait(100);
    };
  };
};