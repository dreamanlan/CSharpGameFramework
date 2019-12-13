input("*.prefab")
{
	string("filter", "");
	ulong("minFileId","65536");
	int("pathwidth","480");
    feature("source", "project");
    feature("menu", "1.Project Resources/Check Prefab File ID");
	feature("description", "just so so");
}
filter
{
    if(assetpath.Contains(filter)){
        var(0) = loadasset(assetpath);
        var(1) = getcomponentsinchildren(var(0),"MonoBehaviour");
        looplist(var(1)){
            var(2) = $$;
            if(isnull(var(2))){
                var(99) = newitem();
                var(99).AssetPath = assetpath;
                var(99).Info = "component is null.";  
            }else{
                var(3) = getguidandfileid(var(2));
                if(var(3).Value<minFileId){
                    var(99) = newitem();
                    var(99).AssetPath = assetpath;
                    var(99).Info = format("name:{0} guid:{1} file_id:{2}", var(2).name, var(3).Key, var(3).Value);  
                };
            };
        };
    };
    0;
};