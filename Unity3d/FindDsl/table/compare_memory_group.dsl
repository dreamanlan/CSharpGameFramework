input
{   
    string("table", "E:/work/workdoc/性能数据/20191129/镜湖/auto_2019_11_29_21_44_31_900_MANAGED_SnapshotExport_30_11_2019_02_47_49_groups_forcmp.csv"){
        file("csv");
    };
    table("table2", "E:/work/workdoc/性能数据/20191129/镜湖/auto_2019_11_29_22_29_57_436_MANAGED_SnapshotExport_30_11_2019_07_31_59_groups_forcmp.csv"){
        encoding("utf-8");
        file("csv");
    };
    string("encoding", "utf-8");
    int("skiprows", 0);
    stringlist("contains", "");
    stringlist("notcontains", "");
    int("mindiff", -100000);
    float("minmultiple", 0);
    int("minsizediff", -100000000);
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
            if($count+mindiff<=$count2 && $count*minmultiple<=$count2 && $size+minsizediff<=$size2){
                info=format("{0}, count {1}=>{2}, size {3}=>{4}",$type,$count,$count2,$size,$size2);
                var(99)=1;
            };
        };
    };
    order = row.RowIndex;
    var(99);
};
