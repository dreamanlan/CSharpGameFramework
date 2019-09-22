input("*.fbx")
{
	feature("source", "project");
	feature("menu", "Project Resources/Fbx");
	feature("description", "just so so");	
}
process
{
  importer.isReadable = changetype(0,"bool");
  setmeshcompression("high");
  setanimationcompression("optimal");
  clearanimationscalecurve();
  setanimationtype("generic");
  saveandreimport();
};