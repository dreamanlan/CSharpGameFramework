script(main)
{
    fileecho(true);

    cfg=arg(1);
    Configuration=arg(1);
    
    paused=true;
    if(argnum()>=3){
    	paused=changetype(arg(2), "bool");
    };

    call("setcolor");
    $encoding = getinputencoding();
    setencoding(936);
    echo("encoding:{0} {1}", $encoding.EncodingName, $encoding.EncodingName);
    echo("commandline:{0}", cmdline());
    echo("commandline args:{0}", stringjoin(",",cmdlineargs()));
    echo("args:{0}", stringjoin(",",args()));
    echo("current directory:{0}", pwd());
    echo("script directory:{0}", getscriptdir());

    rootdir=getscriptdir()+"/../..";
    plugindir=rootdir+"/Unity3d/Assets/Plugins";
    logdir=rootdir+"/BuildLog";
    libdir=rootdir+"/ExternLibrary";
    
    clidsl=expand("%rootdir%/Unity3d/Assets/StreamingAssets/Dsl");
    dsldir=expand("%rootdir%/Resource/DslFile");
    clitable=expand("%rootdir%/Unity3d/Assets/StreamingAssets");
    tabledir=expand("%rootdir%/Resource/Tables");
    
    Platform="Any CPU";
    xbuild=rootdir+"/Tools/msbuild/msbuild.exe";

    echo();
    echo("we'd better use vs2022 msbuild, vs2019 or vs2017 may fail to compile.");

    msbuild2022="C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Msbuild\\Current\\Bin\\msbuild.exe";
    msbuild2019="C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe";
    msbuild2017="C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Community\\MSBuild\\15.0\\Bin\\MSBuild.exe";

    if(fileexist(msbuild2022)){
        xbuild=msbuild2022;
    }
    elseif(fileexist(msbuild2019)){
        xbuild=msbuild2019;
    }
    elseif(fileexist(msbuild2017)){
        xbuild=msbuild2017;
    }
    else{
        echo("Can't find vs2022/vs2019/vs2017, we may fail to compile.");
    };

    mono=expand("%rootdir%/Tools/mono/mono.exe");
    pdb2mdb=expand("%rootdir%/Tools/lib/mono/4.5/pdb2mdb.exe");

    dslcopy=expand("%rootdir%/Tools/DslCopy/bin/Release/DslCopy.exe");
    tablecopy=expand("%rootdir%/Tools/TableReaderGenerator/bin/Debug/TableReaderGenerator.exe");

    unity3dexe="C:\\Program Files\\Unity\\Hub\\Editor\\2021.3.14f1c1\\Editor\\Unity.exe";
    unity3dmac="/Applications/Unity/Unity.app/Contents/MacOS/Unity";
    
    echo();
    looplist(listhashtable(envs())){
        echo("{0}={1}",$$.Key,$$.Value);
    };
    echo();
    
    /*********************************************************************************
    * 生成lua api
    **********************************************************************************/

    deleteallfiles(rootdir+"/SluaExport/LuaObject", "*.cs");
    command{
        win
        {:
            %unity3dexe% -batchmode -quit -projectPath %rootdir%/Unity3d -executeMethod SLua.LuaCodeGen.GenerateAll
        :};
        unix
        {:
            %unity3dmac% -batchmode -quit -projectPath %rootdir%/Unity3d -executeMethod SLua.LuaCodeGen.GenerateAll
        :};
    };
    		
    /*********************************************************************************
    * 生成工程文件
    **********************************************************************************/

    cd(rootdir+"/SluaExport/LuaObject");
    echo("curdir:{0}",pwd());
    $sb = newstringbuilder();
    looplist(listallfiles(rootdir+"/SluaExport/LuaObject","*.cs")){
        $filename = getfilename($$);
        $dirname = getfilename(getdirectoryname($$));
        appendlineformat($sb, "\t\t<Compile Include=\"LuaObject\\{0}\\{1}\" />", $dirname, $filename);
    };
    $txt = stringbuildertostring($sb);
    $prjtxt = readalltext(rootdir+"/SluaExport/SluaExportTempl.csproj");
    $prjtxt = stringreplace($prjtxt, "$GeneratedCSharpFiles$", $txt);
    movefile(rootdir+"/SluaExport/SluaExport.csproj", rootdir+"/SluaExport/SluaExport2.csproj");
    writealltext(rootdir+"/SluaExport/SluaExport.csproj", $prjtxt);
		
    /*********************************************************************************
    * dll编译部分
    **********************************************************************************/

    cd(rootdir);
    command{
        win
        {:
            %xbuild% /version
        :};
        unix
        {:
            msbuild /version
        :};
    };
    
    createdir(logdir);

    echo("building SluaExport.sln ...");
    $var0 = command{
        win
        {:
            %xbuild% /m /nologo /noconsolelogger /p:Configuration=%cfg% /flp:LogFile=%logdir%/SluaExport.sln.log;Encoding=UTF-8 /t:clean;rebuild %rootdir%/SluaExport/SluaExport.sln /p:Platform="Any CPU"
        :};
        unix
        {:
            msbuild /m /nologo /noconsolelogger /p:Configuration=%cfg% /flp:LogFile=%logdir%/SluaExport.sln.log;Encoding=UTF-8 /t:clean;rebuild %rootdir%/SluaExport/SluaExport.sln /p:Platform="Any CPU"
        :};
    };
    
    echo("result:{0}", $var0);
    if($var0){
        setfgcolor("Red");
        echo("compile SluaExport.sln failed, see BuildLog/SluaExport.sln.log");
        call("setcolor");
    };

    cd(rootdir+"/SluaExport/bin/%cfg%");
    echo("curdir:{0}",pwd());
    looplist(listfiles(rootdir+"/SluaExport/bin/%cfg%","*.dll")){
        $filename = getfilename($$);
        if(osplatform()=="Unix"){
			process(mono, pdb2mdb + " " + $filename);
        }else{
            echo("{0} {1}", pdb2mdb, $filename);
            process(mono, pdb2mdb + " " + $filename);
        };
    };
    copyfile(rootdir+"/SluaExport/bin/%cfg%/SluaExport.dll", plugindir+"/SluaExport.dll");
    copyfile(rootdir+"/SluaExport/bin/%cfg%/SluaExport.dll.mdb", plugindir+"/SluaExport.dll.mdb");
    copyfile(rootdir+"/SluaExport/bin/%cfg%/SluaManaged.dll", plugindir+"/SluaManaged.dll");
    copyfile(rootdir+"/SluaExport/bin/%cfg%/SluaManaged.dll.mdb", plugindir+"/SluaManaged.dll.mdb");

    cd(rootdir);

    if($var0){
        setfgcolor("Red");
        echo("build failed !");
        call("setcolor");
    };

    resetcolor();
    if(paused){
	    echo("press any key ...");
	    pause();
	  };
    return($var0);
};

script(setcolor)
{
    if(osplatform()=="Unix"){
        setfgcolor("Blue");
    }else{
        setfgcolor("Yellow");
    };
};