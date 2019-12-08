input
{   
    string("table", "E:/work/workdoc/性能数据/20191129/镜湖/auto_2019_11_29_22_29_57_436_NATIVE_SnapshotExport_30_11_2019_07_32_20.csv"){
        file("csv");
    };
    table("table2", "E:/work/workdoc/性能数据/20191129/镜湖/auto_2019_11_29_21_44_31_900_NATIVE_SnapshotExport_30_11_2019_02_48_08.csv"){
        encoding("utf-8");
        file("csv");
    };
    script("dict2","buildHashtable");
    string("encoding", "utf-8");
    int("skiprows", 1);
    stringlist("classcontains", "");
    stringlist("namecontains", "");
    stringlist("classnotcontains", "");
    stringlist("namenotcontains", "");
	int("resultopt",1){
		toggle(["all","onlyalone","onlynotalone"],[1,2,3]);
	};
    feature("source", "table");
    feature("menu", "9.Table/compare native memory");
    feature("description", "just so so");
}
filter
{
    String = gettype("System.String");
    $header = sheet.GetRow(0);
    $ix = 3;
    $ix2 = 4;        
    $ix3 = 1;
    
    $class = getcellstring(row, $ix);
    $name = getcellstring(row, $ix2);
    $size = getcellnumeric(row, $ix3);
    
    var(99) = 0;
    if(stringcontains($class, classcontains) && stringcontains($name, namecontains) && stringnotcontains($class, classnotcontains) && stringnotcontains($name, namenotcontains)){
        $row = findrowfromhashtable(dict2, [$class, $name]);
        if(!isnull($row)){
            if(resultopt!=2){
                $size2 = getcellnumeric($row, $ix3);
                if($size+minsizediff<=$size2){
                    info=format("{0}, {1}, size {2}=>{3}",$class,$name,$size,$size2);
                    var(99)=1;
                };
            };
        }elseif(resultopt!=3){
            info=format("{0}, {1}, size {2}=>",$class,$name,$size);
            var(99)=1;
        };
    };
    order = row.RowIndex;
    var(99);
};

script(buildHashtable)args($paramInfo)
{
    $dict = tabletohashtable(hashtableget(params,"table2").Value,1,[3,4]);
    return($dict);
};