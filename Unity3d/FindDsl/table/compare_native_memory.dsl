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
    stringlist("namecontainsany", "");
    stringlist("classnotcontains", "");
    stringlist("namenotcontains", "");
	int("resultopt",1){
		toggle(["all","onlyalone","onlynotalone"],[1,2,3]);
	};
	float("pathwidth",240){range(20,4096);};
    feature("source", "table");
    feature("menu", "9.Table/compare native memory");
    feature("description", "just so so");
}
filter
{
    String = gettype("System.String");
    $header = sheet.GetRow(0);
    $ix = 2;
    $ix2 = 1;        
    $ix3 = 5;
    $ix4 = 6;
    
    $class = getcellstring(row, $ix);
    $name = getcellstring(row, $ix2);
    $size = getcellnumeric(row, $ix3);
    $addr = getcellnumeric(row, $ix4);
    
    var(99) = 0;
    if(stringcontainsany($class, classcontainsany) && stringcontainsany($name, namecontainsany) && stringnotcontains($class, classnotcontains) && stringnotcontains($name, namenotcontains)){
        $row = findrowfromhashtable(dict2, [$class, $name]);
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
        assetpath = format("{0}({1})",$name,$addr);
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
    $dict = tabletohashtable(hashtableget(params,"table2").Value,1,[2,1]);
    return($dict);
};
script(BuildExtraList)args($item)
{
	$r = findshortestpathtoroot($item.ExtraObject);
	return($r);
};