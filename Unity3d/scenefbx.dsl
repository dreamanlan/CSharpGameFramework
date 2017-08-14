input("*.fbx")
process
{
  importer.isReadable = changetype(0,"bool");
  setmeshcompression("high");
  setanimationcompression("optimal");
  clearanimationscalecurve();
  closemeshanimationifnoanimation();
  saveandreimport();
};