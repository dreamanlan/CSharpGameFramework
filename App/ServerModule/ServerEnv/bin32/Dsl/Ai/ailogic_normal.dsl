story(ai_normal)
{
  onmessage(start)
  {
    $objid = @objid;
    while(1){
      ai_do_normal($objid);
      wait(100);
    };
  };
};