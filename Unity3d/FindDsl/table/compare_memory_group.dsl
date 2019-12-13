input
{   
    string("table", ""){
        file("csv");
    };
    table("table2", ""){
        encoding("utf-8");
        file("csv");
    };
    string("encoding", "utf-8");
    int("skiprows", 0);
    stringlist("contains", "");
    stringlist("notcontains", "");
    int("mindiff", 0);
    float("minmultiple", 0);
    int("minsizediff", 0);
	float("pathwidth",240){range(20,4096);};
    feature("source", "table");
    feature("menu", "9.Table/compare memory group");
    feature("description", "just so so");
}
filter
{
    String = gettype("System.String");
    $header = sheet.GetRow(0);
    $ix = findcellindex($header, "type");
    $ix2 = findcellindex($header, "count");        
    $ix3 = findcellindex($header, "size");
    
    $type = getcellstring(row, $ix);
    $count = getcellnumeric(row, $ix2);
    $size = getcellnumeric(row, $ix3);
    
    var(99) = 0;
    if(stringcontains($type, contains) && stringnotcontains($type, notcontains)){
        $rowIndex = findrowindex(table2, $ix, $type);
        if($rowIndex>0){
            $row = table2.GetRow($rowIndex);
            $count2 = getcellnumeric($row, $ix2);
            $size2 = getcellnumeric($row, $ix3);
            if(abs($count-$count2)>=mindiff && ($count*minmultiple<=$count2 || $count2*minmultiple<=$count) && abs($size-$size2)>=minsizediff){
                info=format("{0}, count {1}=>{2}, size {3}=>{4}",$type,$count,$count2,$size,$size2);
                var(99)=1;
            };
        };
    };
    order = row.RowIndex;
    var(99);
};
