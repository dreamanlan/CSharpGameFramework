input("MapSymbols")
{
    string("il2cpp", ""){
        file("map");
    };
    string("unity", ""){
        file("map");
    };
    string("gcalloc", ""){
        file("txt");
    };
    string("libil2cpp_start", "0");
    string("libil2cpp_end", "0");
    string("libunity_start", "0");
    string("libunity_end", "0");
    bool("reloadmap",false);
	feature("source", "list");
	feature("menu", "6.Tools/Map Symbols");
	feature("description", "just so so");
}
process
{	
    if(isnull(@syms) || isnull(@syms2) || reloadmap){
        @syms=loadidaprosymbols(il2cpp);
        @syms2=loadidaprosymbols(unity);    
    };
    $lines=mapmyhooksymbols(readalllines(gcalloc),hex2ulong(libil2cpp_start),hex2ulong(libil2cpp_end),@syms,"libil2cpp");
    $lines=mapmyhooksymbols($lines,hex2ulong(libunity_start),hex2ulong(libunity_end),@syms2,"libunity");
    $dir = getdirectoryname(gcalloc);
    $filename = getfilenamewithoutextension(gcalloc);
    $file = combinepath($dir,$filename+"_with_sym.txt");
    writealllines($file,$lines);
};