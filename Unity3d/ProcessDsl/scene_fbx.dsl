input("*.fbx")
{
	feature("source", "sceneassets");
}
process
{
  importer.isReadable = changetype(0,"bool");
  setmeshcompression("high");
  setanimationcompression("optimal");
  clearanimationscalecurve();
  closemeshanimationifnoanimation();
  saveandreimport();
};