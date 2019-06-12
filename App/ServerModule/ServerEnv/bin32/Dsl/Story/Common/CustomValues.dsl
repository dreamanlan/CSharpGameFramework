value(TestVal)args($a,$b,$c)ret($d)
{
  if($a>1){
    $d=$a+TestVal($a-1,$b,$c);
  }else{
    $d=$a+$b+$c;
  };
};