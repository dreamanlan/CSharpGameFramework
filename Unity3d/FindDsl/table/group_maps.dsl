input("dummy")
{   
    string("maps", ""){
        file("txt");
    };
    string("encoding", "utf-8");
	float("pathwidth",240){range(20,4096);};
    feature("source", "list");
    feature("menu", "9.Table/group maps");
    feature("description", "just so so");
}
filter
{
    $maps = loadmaps(maps);
    looplist($maps){
        $row = $$;
        $item = newitem();
        $item.AssetPath = $row.module;
        $item.ScenePath = $row.vm_start+"-"+$row.vm_end;
        $item.Info = $row.size + " " + $module;
        $item.Order = $row.size;
        $item.Value = $row.size;
        $item.Group = $row.module;
    };
    0;
}
group
{
    order = sum;
    1;
};