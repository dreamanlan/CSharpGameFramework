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
	int("resultopt",1){
		toggle(["all","onlyalone","onlynotalone"],[1,2,3]);
	};
	float("pathwidth",240){range(20,4096);};
    feature("source", "table");
    feature("menu", "9.Table/compare managed heap");
    feature("description", "just so so");
}
filter
{
    String = gettype("System.String");
    $header = sheet.GetRow(0);
    $ix = 1;
    $ix2 = 2;
    
    $size = getcellnumeric(row, $ix);
    $addr = getcellstring(row, $ix2);
    $vaddr = str2ulong($addr);
    
    var(99) = 0;
    $row = findrowfromhashtable(dict2, [$addr]);
    if(!isnull($row)){
        if(resultopt!=2){
            $size2 = getcellnumeric($row, $ix);
            info=format("{0:X}, size {1}=>{2}",$vaddr,$size,$size2);
            var(99)=1;
        };
    }elseif(resultopt!=3){
        info=format("{0:X}, size {1}=>",$vaddr,$size);
        var(99)=1;
    };
    order = $vaddr;
    var(99);
};

script(buildHashtable)args($paramInfo)
{
    $dict = tabletohashtable(hashtableget(params,"table2").Value,1,[2]);
    return($dict);
};