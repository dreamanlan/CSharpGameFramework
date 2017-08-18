input
{
  circle(64,64,32);
}
height
{
  $x=arg(0);
  $y=arg(1);
  $ds = distsqr($x,$y,64,64);
  height = sqrt(32*32-$ds)/32;
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