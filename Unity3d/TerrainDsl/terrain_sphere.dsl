input
{
  circle(64,64,32);
}
height
{
  $x=arg(0);
  $y=arg(1);
  $ds = dist_sqr($x,$y,64,64);
  height = sqrt(32*32-$ds)/32;
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