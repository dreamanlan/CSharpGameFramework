input("dummy")
{   
    string("addrs", ""){
        file("txt");
    };
    string("maps1", ""){
        file("txt");
    };
    string("maps2", ""){
        file("txt");
    };
	float("pathwidth",240){range(20,4096);};
    feature("source", "list");
    feature("menu", "9.Table/compare alloc and maps");
    feature("description", "just so so");
}
filter
{
    $maps1 = loadmaps(maps1);
    $maps2 = loadmaps(maps2);
    $results = matchmaps($maps1,$maps2);
    $diffs = calcmatchedmapsdiff($results,1);
    $addrs = loadaddrs(addrs);
    $ct = $addrs.Count;
    looplist($addrs){
        $addr = $$;
        $r = findmaps($diffs,$addr);
        if(!isnull($r)){
            $item = newitem();
            $item.AssetPath = format("{0:X}",$addr);
            $item.ScenePath = $addr;
            $item.Info = $r.vm_start+"-"+$r.vm_end+" "+$r.size+" "+$r.module;
            $item.Order = $addr;
            $item.Value = $r.size;
        };
    };
    0;
};