input("*.dds")
{
	feature("source", "project");
	feature("menu", "1.Project Resources/Delete DDS Texture");
	feature("description", "just so so");
}
process
{
	deleteasset(assetpath);
};