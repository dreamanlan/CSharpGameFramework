input("*.dds")
{
	feature("source", "project");
	feature("menu", "Project Resources/Delete DDS Texture");
	feature("description", "just so so");
}
process
{
	deleteasset(assetpath);
};