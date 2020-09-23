input
{   
    string("table", "blocks.txt")
    {
        file("txt");
    };
    string("encoding", "utf-8");
    int("skiprows", 0);
    string("fields", "size_in_bytes,marks_set,objs");
    string("contains", "");
    string("notcontains1", "");
    string("notcontains2", "");
    string("encoding", "utf-8");
    string("style", "grouplist"){
        popup(["itemlist", "grouplist"]);
    };
    float("pathwidth",240){range(20,4096);};
    feature("source", "table");
    feature("menu", "9.Table/group table");
    feature("description", "just so so");
}
filter
{
    order = row.RowIndex;
    if(!isnullorempty(fields)){
        var(1) = stringsplit(fields,[","]);
        var(2) = findcellindexes(sheet.GetRow(0), var(1));
        var(0) = rowtoline(row, 0, var(2));
        if(listsize(var(2))>=3 && var(0).Contains(contains) && (isnullorempty(notcontains1) || !var(0).Contains(notcontains1)) && (isnullorempty(notcontains2) || !var(0).Contains(notcontains2))){
            assetpath = getcellstring(row, var(2)[0]);        
            scenepath = "";    
            info = var(0);
            value = getcellnumeric(row, var(2)[2]);    
            group = assetpath;
            1;
        }else{
            0;
        };
    }else{
        0;
    };
}
group
{
		order = sum;
		value = sum;
		info = format("{0}=>{1}", group, count);
    1;
};