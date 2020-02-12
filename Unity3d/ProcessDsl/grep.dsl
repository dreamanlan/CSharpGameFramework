input("MapSymbols")
{
    string("txtfile", ""){
        file("txt");
    };
    string("filter1", "");
    string("filter2", "");
    string("filter3", "");
    string("filter4", "");
    string("filter5", "");
    string("filter6", "");
    string("filter7", "");
    string("filter8", "");
	feature("source", "list");
	feature("menu", "6.Tools/Grep");
	feature("description", "just so so");
}
process
{
    $lines=readalllines(txtfile);
    if(!isnullorempty(filter1)){
        $lines = grep($lines, filter1);
    };
    if(!isnullorempty(filter2)){
        $lines = grep($lines, filter2);
    };
    if(!isnullorempty(filter3)){
        $lines = grep($lines, filter3);
    };
    if(!isnullorempty(filter4)){
        $lines = grep($lines, filter4);
    };
    if(!isnullorempty(filter5)){
        $lines = grep($lines, filter5);
    };
    if(!isnullorempty(filter6)){
        $lines = grep($lines, filter6);
    };
    if(!isnullorempty(filter7)){
        $lines = grep($lines, filter7);
    };
    if(!isnullorempty(filter8)){
        $lines = grep($lines, filter8);
    };
    $dir = getdirectoryname(txtfile);
    $filename = getfilenamewithoutextension(txtfile);
    $file = combinepath($dir,$filename+"_grep.txt");
    writealllines($file,$lines);
};