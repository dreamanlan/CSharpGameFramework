input("dummy")
{   
    string("maps1", ""){
        file("txt");
    };
    string("maps2", ""){
        file("txt");
    };
    string("encoding", "utf-8");
	int("resultopt",1){
		toggle(["all","onlyalone","onlynotalone"],[1,2,3]);
	};
	float("pathwidth",240){range(20,4096);};
    feature("source", "list");
    feature("menu", "9.Table/compare maps");
    feature("description", "just so so");
}
filter
{
    $maps1 = loadmaps(maps1);
    $maps2 = loadmaps(maps2);
    $results = matchmaps($maps1,$maps2);
    looplist($results){
        $row = $$;
        $left = $row[0];
        $right = $row[1];
        if(resultopt==1 || resultopt==2 && (isnull($left) || isnull($right)) || resultopt==3 && !isnull($left) && !isnull($right)){
            $leftInfo = "";
            $rightInfo = "";
            $val = 0;
            $module = "";
            $item = newitem();
            if(!isnull($left)){
                $leftInfo = format("{0:X}-{1:X}", $left.vm_start, $left.vm_end);
                if(resultopt==2){
                    $val = $val - $left.size;
                };
                $module=$left.module;
            };
            if(!isnull($right)){
                $rightInfo = format("{0:X}-{1:X}", $right.vm_start, $right.vm_end);            
                $val = $val + $right.size;
                $module=$right.module;
            };
            $item.AssetPath = $leftInfo+"<=>"+$rightInfo;
            $item.ScenePath = $module;
            $item.Info = $leftInfo + " <=> " + $rightInfo + " " + $val + " " + $module;
            $item.Order = $val;
            $item.Value = $val;
        };
    };
    0;
};