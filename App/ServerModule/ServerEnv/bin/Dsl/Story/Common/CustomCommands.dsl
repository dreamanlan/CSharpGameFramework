command(TestCmd)args($a,$b,$c)
{
  if($a>1){
    TestCmd($a-1,$b,$c);
  };
  log("{0} {1} {2}",$a,$b,$c);
};