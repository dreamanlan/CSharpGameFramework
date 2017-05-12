input
{
  circle(256,256,180);
}
height
{
  $x=arg(0);
  $y=arg(1);
  $ds = distsqr($x,$y,256,256);
  height = sqrt(180*180-$ds)/180;
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