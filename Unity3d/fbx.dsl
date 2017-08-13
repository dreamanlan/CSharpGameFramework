input("*.fbx")
filter
{
	importer.importedTakeInfos.Length <= 0;
}
process
{
  importer.isReadable = changetype(0,"bool");
  setmeshcompression("high");
  setanimationcompression("optimal");
  clearanimationscalecurve();
  closemeshanimationifnoanimation();
};