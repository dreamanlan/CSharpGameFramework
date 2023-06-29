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
    
    echo();
    looplist(listhashtable(envs())){
        echo("{0}={1}",$$.Key,$$.Value);
    };
    echo();
		
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

    echo("building Client.sln ...");
    $var0 = command{
        win
        {:
            %xbuild% /m /nologo /noconsolelogger /p:Configuration=%cfg% /flp:LogFile=%logdir%/Client.sln.log;Encoding=UTF-8 /t:clean;rebuild %rootdir%/Client.sln /p:Platform="Any CPU"
        :};
        unix
        {:
            msbuild /m /nologo /noconsolelogger /p:Configuration=%cfg% /flp:LogFile=%logdir%/Client.sln.log;Encoding=UTF-8 /t:clean;rebuild %rootdir%/Client.sln /p:Platform="Any CPU"
        :};
    };
    
    echo("result:{0}", $var0);
    if($var0){
        setfgcolor("Red");
        echo("compile Client.sln failed, see BuildLog/Client.sln.log");
        call("setcolor");
    };

    cd(rootdir+"/App/ClientModule/ClientPlugins/bin/%cfg%");
    echo("curdir:{0}",pwd());
    looplist(listfiles(rootdir+"/App/ClientModule/ClientPlugins/bin/%cfg%","*.dll")){
        $filename = getfilename($$);
        $targetPath = plugindir+"/"+$filename;
        copyfile($$, $targetPath);
        if(osplatform()=="Unix"){
			process(mono, pdb2mdb + " " + $filename);
        }else{
            echo("{0} {1}", pdb2mdb, $filename);
            process(mono, pdb2mdb + " " + $filename);
        };
        //echo("copy {0} to {1}", $$, $targetPath);
    };
    copydir(rootdir+"/App/ClientModule/ClientPlugins/bin/%cfg%", plugindir, "*.mdb");

    deletefile(plugindir+"/UnityEngine.dll");
    deletefile(plugindir+"/UnityEngine.UI.dll");
    deletefile(plugindir+"/UnityEngine.Timeline.dll");
    deletefile(plugindir+"/UnityEditor.dll");
    cd(rootdir);

    $var1 = command{
        win
        {:
            %dslcopy% %dsldir% %clidsl%
        :};
        unix
        {:
            mono %dslcopy% %dsldir% %clidsl%
        :};
    };

    $var2 = command{
        win
        {:
            %tablecopy% %tabledir% %clitable%
        :};
        unix
        {:
            mono %tablecopy% %tabledir% %clitable%
        :};
    };

    if($var0 || $var1 || $var2){
        setfgcolor("Red");
        echo("compile failed !");
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