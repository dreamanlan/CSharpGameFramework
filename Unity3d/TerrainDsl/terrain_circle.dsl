input
{
  circle(64,64,32);
  sampler("img", "F:/OpenSource/CSharpGameFramework_local/circle.png");
}
height
{
  $x=arg(0);
  $y=arg(1);
  height = clamp(0,0.01,rndfloat(0,1))+samplered("img", $x, $y)/1000.0+samplegreen("img", $x, $y)/1000.0+sampleblue("img", $x, $y)/1000.0;
}
alphamap
{
  $x=arg(0);
  $y=arg(1);
  setalpha(0, sin($x/180.0));
  setalpha(1, cos($y/180.0));
  setalpha(2, sin($x/180.0));
  setalpha(3, cos($y/180.0));
};