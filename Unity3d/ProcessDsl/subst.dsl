input("MapSymbols")
{
    string("txtfile", ""){
        file("txt");
    };
    string("pattern1", "");
    string("subst1", "");
    string("pattern2", "");
    string("subst2", "");
    string("pattern3", "");
    string("subst3", "");
    string("pattern4", "");
    string("subst4", "");
    string("pattern5", "");
    string("subst5", "");
    string("pattern6", "");
    string("subst6", "");
    string("pattern7", "");
    string("subst7", "");
    string("pattern8", "");
    string("subst8", "");
	feature("source", "list");
	feature("menu", "6.Tools/Subst");
	feature("description", "just so so");
}
process
{
    $lines=readalllines(txtfile);
    if(!isnullorempty(pattern1)){
        $lines = subst($lines, pattern1, subst1);
    };
    if(!isnullorempty(pattern2)){
        $lines = subst($lines, pattern2, subst2);
    };
    if(!isnullorempty(pattern3)){
        $lines = subst($lines, pattern3, subst3);
    };
    if(!isnullorempty(pattern4)){
        $lines = subst($lines, pattern4, subst4);
    };
    if(!isnullorempty(pattern5)){
        $lines = subst($lines, pattern5, subst5);
    };
    if(!isnullorempty(pattern6)){
        $lines = subst($lines, pattern6, subst6);
    };
    if(!isnullorempty(pattern7)){
        $lines = subst($lines, pattern7, subst7);
    };
    if(!isnullorempty(pattern8)){
        $lines = subst($lines, pattern8, subst8);
    };
    $dir = getdirectoryname(txtfile);
    $filename = getfilenamewithoutextension(txtfile);
    $file = combinepath($dir,$filename+"_subst.txt");
    writealllines($file,$lines);
};