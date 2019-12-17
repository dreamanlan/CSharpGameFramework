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
    string("libil2cpp_start", "");
    string("libil2cpp_end", "");
    string("libunity_start", "");
    string("libunity_end", "");
	feature("source", "list");
	feature("menu", "6.Tools/Map Symbols");
	feature("description", "just so so");
}
process
{	
    $syms=loadidaprosymbols(il2cpp);
    $syms2=loadidaprosymbols(unity);    
    $lines=mapmyhooksymbols(readalllines(gcalloc),hex2ulong(libil2cpp_start),hex2ulong(libil2cpp_end),$syms,"libil2cpp");
    $lines=mapmyhooksymbols($lines,hex2ulong(libunity_start),hex2ulong(libunity_end),$syms2,"libunity");
    $dir = getdirectoryname(gcalloc);
    $file = combinepath($dir,"gc_alloc_resize.txt");
    writealllines($file,$lines);
};