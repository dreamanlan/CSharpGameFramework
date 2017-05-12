input
{
  resettrees(true);
  rect(11,11,116,116);
  sampler("img", "F:/OpenSource/CSharpGameFramework_local/spiral.png");
}
height
{
  $x=arg(0);
  $y=arg(1);
  height = clamp(0,0.01,rndfloat(0,1))+samplered("img", $x, $y)/750.0;
  if(height>0.1 && rndint(0,100)<5){
    addtree(0,$x/128,height,$y/128,0,1,1,0x00ff00,0xff0000);
  };
}
alphamap
{
  $x=arg(0);
  $y=arg(1);
  setalpha(0, sin($x/180.0));
  setalpha(1, cos($y/180.0));
}
detail
{
  $x=arg(0);
  $y=arg(1);
  $layer=arg(2);
  detail = 0;
};