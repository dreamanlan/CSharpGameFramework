story(ai_boss)
{  
  onmessage(start)
  {
    $objid = @objid;

    $skillInfoC = ai_get_skill($objid,1);
    $skillInfoD = ai_get_skill($objid,4);

    $countOfSkill = 0;
    $countOfD = 0;
    $lastTime = time();
    while(1){
      //每隔12~18秒释放技能c
      $curTime = time();    
      $var = rndint(9000,15000);  
      if($curTime>$lastTime+$var){
        $lastTime=$curTime;
        //log("cast skill C");
        if(ai_need_chase($objid,$skillInfoC)){
          ai_chase($objid,$skillInfoC);
        };
        ai_cast_skill($objid, $skillInfoC);
      };

      $hp = gethp($objid);
      $maxHp = getmaxhp($objid);
      //血量少于75%、50%、25%释放一次技能d
      if($hp<$maxHp*1/4 && $countOfD<3){
        log("cast skill D count {0} ({1} - {2})", $countOfD, $hp, $maxHp);
        if(ai_need_chase($objid,$skillInfoD)){
          ai_chase($objid,$skillInfoD);
        };
        ai_cast_skill($objid, $skillInfoD);
        inc($countOfD);
      };
      if($hp<$maxHp*2/4 && $countOfD<2){
        log("cast skill D count {0} ({1} - {2})", $countOfD, $hp, $maxHp);
        if(ai_need_chase($objid,$skillInfoD)){
          ai_chase($objid,$skillInfoD);
        };
        ai_cast_skill($objid, $skillInfoD);
        inc($countOfD);
      };
      if($hp<$maxHp*3/4 && $countOfD<1){
        log("cast skill D count {0} ({1} - {2})", $countOfD, $hp, $maxHp);
        if(ai_need_chase($objid,$skillInfoD)){
          ai_chase($objid,$skillInfoD);
        };
        ai_cast_skill($objid, $skillInfoD);
        inc($countOfD);
      };
      
      $target = ai_select_target($objid, 0);
      if(!isnull($target)){
        $skillInfo = ai_select_skill_by_distance($objid);
        if(!isnull($skillInfo)){
          $needChase = ai_need_chase($objid,$skillInfo);
          $needKeepAway = ai_need_keep_away($objid,$skillInfo,0.5);
          if($needChase){
            ai_chase($objid,$skillInfo);
            ai_cast_skill($objid,$skillInfo);     
          }elseif($needKeepAway){
            ai_keep_away($objid,$skillInfo,0.8);
            ai_cast_skill($objid,$skillInfo);
          }else{
            ai_cast_skill($objid,$skillInfo);
            inc($countOfSkill);
          };
        };
      };
      
      wait(100);
    };
  };
};