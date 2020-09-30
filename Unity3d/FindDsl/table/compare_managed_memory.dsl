input
{   
    string("table", ""){
        file("csv");
    };
    table("table2", ""){
        encoding("utf-8");
        file("csv");
    };
    script("dict2","buildHashtable");
    string("encoding", "utf-8");
    int("skiprows", 1);
    stringlist("classcontainsany", "");
    stringlist("classnotcontains", "");
	int("resultopt",1){
		toggle(["all","onlyalone","onlynotalone"],[1,2,3]);
	};
	float("pathwidth",240){range(20,4096);};
    feature("source", "table");
    feature("menu", "9.Table/compare managed memory");
    feature("description", "just so so");
}
filter
{
    String = gettype("System.String");
    $header = sheet.GetRow(0);
    $ix = 1;
    $ix2 = 3;        
    $ix3 = 4;
    
    $class = getcellstring(row, $ix);
    $size = getcellnumeric(row, $ix2);
    $addr = getcellnumeric(row, $ix3);
    $addrStr = getcellstring(row, $ix3);
    
    var(99) = 0;
    if(stringcontainsany($class, classcontainsany) && stringnotcontains($class, classnotcontains)){
        $row = findrowfromhashtable(dict2, [$class,$addrStr]);
        if(!isnull($row)){
            if(resultopt!=2){
                $size2 = getcellnumeric($row, $ix3);
                info=format("{0}, {1}, size {2}=>{3}",$class,$name,$size,$size2);
                var(99)=1;
            };
        }elseif(resultopt!=3){
            info=format("{0}, {1}, size {2}=>",$class,$name,$size);
            var(99)=1;
        };
    };
    if(var(99)==1){
        assetpath = format("[{0}]",$addr);
        scenepath = $class;
        order = $size;
        value = $size;
    	extraobject = $addr;
    	extralistbuild = "BuildExtraList";
    };
    var(99);
};

script(buildHashtable)args($paramInfo)
{
    $dict = tabletohashtable(hashtableget(params,"table2").Value,1,[2,5]);
    return($dict);
};
script(BuildExtraList)args($item)
{
	$r = findshortestpathtoroot($item.ExtraObject);
	return($r);
};