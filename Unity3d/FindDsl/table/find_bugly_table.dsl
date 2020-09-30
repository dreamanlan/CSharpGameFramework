input
{	
    string("table", "")
    {
        file("csv");
    };
    string("encoding", "utf-8");
    int("skiprows", 0);
    int("ft",1)
    {
      toggle("fields1",0);
      toggle("fields2",1);  
    };
    string("fields1", "user,product_version,crash_id,elapsed_time,crash_time,issue_id,is_root,ram,rom,is_new_issue");
    string("fields2", "product_version,user,issue_id,retrace_status,exception_type,process_name,crash_time,crash_id,type,device_id,elapsed_time,is_new_issue,ram,rom,cpu_name,is_root,cpu_type");
    stringlist("contains", "");
    stringlist("containsany", "");
    stringlist("notcontains", "");
    stringlist("notcontainsany", "");
    float("pathwidth",160){range(20,4096);};
    feature("source", "table");
    feature("menu", "9.Table/find bugly table");
    feature("description", "just so so");
}
filter
{
    order = row.RowIndex;
    if(ft==0){
        $fields = fields1;
    }else{
        $fields = fields2;  
    };
	if(isnullorempty($fields)){
		var(0) = row.GetLine();
    	if(stringcontains(var(0),contains) && stringcontainsany(var(0),containsany) && stringnotcontains(var(0),notcontains) && stringnotcontainsany(var(0), notcontainsany)){
    		info = var(0);
    	    value = 0;
    	    1;
    	}else{
    	    0;
    	};
	}else{	    
	    $header = sheet.GetRow(0);
		var(1) = stringsplit($fields,[","]);
		var(2) = findcellindexes($header, var(1));
		var(3) = callscript("getfieldstring", $header, row, "kv");
		var(4) = parseurlargs(var(3), "+");
		var(5) = hashtableget(var(4), "C03_B2");
		var(6) = hashtableget(var(4), "C03_B3");
		var(7) = hashtableget(var(4), "C03_B4");
		var(8) = callscript("getfieldstring", $header, row, "crash_id");
		var(9) = callscript("getfieldstring", $header, row, "issue_id");
		
		$f_kv = var(3);
		$ukv = stringreplace(unescapeurl($f_kv), ";", "\n");
		
		//device_id,hardware,os,cpu_name,exception_type,stack,exception_message
		$txt = callscript("getfieldstring", $header, row, "device_id");
		$device_id = unescapeurl($txt, "+");
				
		$txt = callscript("getfieldstring", $header, row, "hardware");
		$hardware = unescapeurl($txt, "+");
		
		$txt = callscript("getfieldstring", $header, row, "os");
		$uos = stringreplace(unescapeurl($txt, "+"), ",", "|");
				
		$txt = callscript("getfieldstring", $header, row, "cpu_name");
		$cpu_name = unescapeurl($txt, "+");
		
		$txt = callscript("getfieldstring", $header, row, "exception_type");
		$exception_type = unescapeurl($txt, "+");
				
		$txt = callscript("getfieldstring", $header, row, "stack");
		$f_stack = $txt;
		$cstack = unescapeurl($txt, "+");
		if(isnull($cstack)){
		    $cstack = "";  
		};
		
		$txt = callscript("getfieldstring", $header, row, "exception_message");
		$f_exception = $txt;
		$exception_message = unescapeurl($txt, "+");
		if(isnull($exception_message)){
		    $exception_message = "";  
		};
		
		if($f_kv=="kv" && $f_stack=="stack" && $f_exception=="exception_message"){
		    var(0) = rowtoline(row, 0, var(2))+","+$device_id+","+$hardware+","+$uos+","+$cpu_name+","+$exception_type+",scene,hz,native,graphics,unknown,pss,vss,mono,"+$f_kv+","+$f_stack+","+$f_exception;
		}elseif(!isnullorempty(var(5)) && !isnullorempty(var(6)) && !isnullorempty(var(7))){
		    var(5) = stringreplace(var(5), "scene_", "");
		    var(5) = stringreplace(var(5), "hz_", "");
		    var(6) = stringreplace(var(6), "native_", "");
		    var(6) = stringreplace(var(6), "graphics_", "");
		    var(6) = stringreplace(var(6), "unknown_", "");
		    var(7) = stringreplace(var(7), "pss_", "");
		    var(7) = stringreplace(var(7), "vss_", "");
		    var(7) = stringreplace(var(7), "mono_", "");
    		var(0) = rowtoline(row, 0, var(2))+","+$device_id+","+$hardware+","+$uos+","+$cpu_name+","+$exception_type+","+var(5)+","+var(6)+","+var(7)+","+$f_kv+","+$f_stack+","+$f_exception;
    	}else{
    	    var(0) = rowtoline(row, 0, var(2))+","+$device_id+","+$hardware+","+$uos+","+$cpu_name+","+$exception_type+",,,,,,,,,"+$f_kv+","+$f_stack+","+$f_exception;
    	};
		
    	if(stringcontains(var(0),contains) && stringcontainsany(var(0),containsany) && stringnotcontains(var(0),notcontains) && stringnotcontainsany(var(0), notcontainsany)){
    		info = var(0);
    	    value = 0;
    	    assetpath = var(8);
    	    scenepath = var(9);
    	    $exinfo = $uos+"\n\n"+$ukv+"\n\n"+$exception_message+"\n\n"+$cstack;
    	    extralist = newextralist($exinfo => $exinfo);
    	    1;
    	}else{
    	    0;
    	};
	};
};

script(getfieldstring)args($header, $row, $name)
{
    $ix = findcellindex($header, $name);
    $str = getcellstring($row, $ix); 
    return($str);
};