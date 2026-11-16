story(ai_player)
{
  onmessage(start)
  {
    $objid = @objid;
    $skillInfoC = ai_get_skill($objid,4);
    $countOfSkill = 0;
    $lastTime = time();
    while(1){
      // 每隔 12~18 秒尝试释放技能 C
      $curTime = time();
      $var = rnd_int(9000,15000);
      if($curTime>$lastTime+$var){
        $lastTime=$curTime;
        ai_cast_skill($objid, $skillInfoC);
      };

      $target = ai_select_target($objid, 0);
      if(!is_null($target)){
        $skillInfo = ai_select_skill_by_distance($objid);
        if(!is_null($skillInfo)){
          // ai_chase / ai_keep_away 会在条件满足时挂起 story，
          // 导致够不着目标时 ai_cast_skill 永远执行不到，主角只剩跑步。
          // 改由 AiCastSkill 自己追一步并出手，story 每轮都能推进。
          ai_cast_skill($objid,$skillInfo);
          inc($countOfSkill);
        };
      };

      wait(100);
    };
  };
};
