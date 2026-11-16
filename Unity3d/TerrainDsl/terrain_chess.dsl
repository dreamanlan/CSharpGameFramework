input
{
  rect(10,10,116,116);
  sampler("img", "F:/OpenSource/CSharpGameFramework_local/chess.png");
}
height
{
  $x=arg(0);
  $y=arg(1);
  height = clamp(0,0.01,rnd_float(0,1))+sample_red("img", $x, $y)/1000.0+sample_green("img", $x, $y)/1000.0+sample_blue("img", $x, $y)/1000.0;
}
alphamap
{
  $x=arg(0);
  $y=arg(1);
  set_alpha(0, sin($x/180.0));
  set_alpha(1, cos($y/180.0));
  set_alpha(2, sin($x/180.0));
  set_alpha(3, cos($y/180.0));
};