script(main)
{
    file_echo(true);

    cfg=arg(1);
    Configuration=arg(1);
    
    paused=true;
    if(arg_num()>=3){
    	paused=change_type(arg(2), "bool");
    };

    call("setcolor");
    $encoding = get_input_encoding();
    set_encoding(936);
    echo("encoding:{0} {1}", $encoding.EncodingName, $encoding.EncodingName);
    echo("command_line:{0}", cmd_line());
    echo("command_line args:{0}", string_join(",",cmd_line_args()));
    echo("args:{0}", string_join(",",args()));
    echo("current directory:{0}", pwd());
    echo("script directory:{0}", get_script_directory());

    rootdir=get_script_directory()+"/../..";
    logdir=rootdir+"/BuildLog";
    libdir=rootdir+"/ExternLibrary";
    
    svrbin=expand("%rootdir%/App/ServerModule/ServerEnv/bin");

    svrdsl=expand("%rootdir%/App/ServerModule/ServerEnv/bin/Dsl");
    dsldir=expand("%rootdir%/Resource/DslFile");
    svrtable=expand("%rootdir%/App/ServerModule/ServerEnv/bin/Tables");
    tabledir=expand("%rootdir%/Resource/Tables");
    
    Platform="Any CPU";
    xbuild=rootdir+"/Tools/msbuild/msbuild.exe";

    echo();
    echo("we'd better use vs2022 msbuild, vs2019 or vs2017 may fail to compile.");

    msbuild2022="C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\Msbuild\\Current\\Bin\\msbuild.exe";
    msbuild2019="C:\\Program Files (x86)\\Microsoft Visual Studio\\2019\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe";
    msbuild2017="C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Community\\MSBuild\\15.0\\Bin\\MSBuild.exe";

    if(file_exist(msbuild2022)){
        xbuild=msbuild2022;
    }
    elseif(file_exist(msbuild2019)){
        xbuild=msbuild2019;
    }
    elseif(file_exist(msbuild2017)){
        xbuild=msbuild2017;
    }
    else{
        echo("Can't find vs2022/vs2019/vs2017, we may fail to compile.");
    };

    mono=expand("%rootdir%/Tools/mono/mono.exe");
    pdb2mdb=expand("%rootdir%/Tools/lib/mono/4.5/pdb2mdb.exe");

    dslcopy=expand("%rootdir%/Tools/DslCopy/bin/Release/net9.0/DslCopy.exe");
    tablecopy=expand("%rootdir%/Tools/TableReaderGenerator/bin/Debug/net9.0/TableReaderGenerator.exe");
    
    echo();
    loop_list(list_hashtable(envs())){
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
    
    create_dir(logdir);

    echo("building Server.sln ...");
    $var0 = command{
        win
        {:
            %xbuild% /m /nologo /noconsolelogger /p:Configuration=%cfg% /flp:LogFile=%logdir%/Server.sln.log;Encoding=UTF-8 /t:clean;rebuild %rootdir%/Server.sln /p:Platform="Any CPU"
        :};
        unix
        {:
            msbuild /m /nologo /noconsolelogger /p:Configuration=%cfg% /flp:LogFile=%logdir%/Server.sln.log;Encoding=UTF-8 /t:clean;rebuild %rootdir%/Server.sln /p:Platform="Any CPU"
        :};
    };
    
    echo("result:{0}", $var0);
    if($var0){
        set_fg_color("Red");
        echo("compile Server.sln failed, see BuildLog/Server.sln.log");
        call("setcolor");
    };

    copy_dir("%rootdir%/App/ServerModule/Lobby/bin/%cfg%/net9.0", svrbin, "*.exe");
    copy_dir("%rootdir%/App/ServerModule/Lobby/bin/%cfg%/net9.0", svrbin, "*.dll");
    copy_dir("%rootdir%/App/ServerModule/Lobby/bin/%cfg%/net9.0", svrbin, "*.pdb");
    copy_dir("%rootdir%/App/ServerModule/UserServer/bin/%cfg%/net9.0", svrbin, "*.exe");
    copy_dir("%rootdir%/App/ServerModule/UserServer/bin/%cfg%/net9.0", svrbin, "*.dll");
    copy_dir("%rootdir%/App/ServerModule/UserServer/bin/%cfg%/net9.0", svrbin, "*.pdb");
    copy_dir("%rootdir%/App/ServerModule/RoomServer/bin/%cfg%/net9.0", svrbin, "*.exe");
    copy_dir("%rootdir%/App/ServerModule/RoomServer/bin/%cfg%/net9.0", svrbin, "*.dll");
    copy_dir("%rootdir%/App/ServerModule/RoomServer/bin/%cfg%/net9.0", svrbin, "*.pdb");
    copy_dir("%rootdir%/App/ServerModule/DataCache/bin/%cfg%/net9.0", svrbin, "*.exe");
    copy_dir("%rootdir%/App/ServerModule/DataCache/bin/%cfg%/net9.0", svrbin, "*.dll");
    copy_dir("%rootdir%/App/ServerModule/DataCache/bin/%cfg%/net9.0", svrbin, "*.pdb");

    cd(rootdir+"/App/ClientModule/ClientPlugins/bin/%cfg%/netstandard2.1");
    echo("curdir:{0}",pwd());
    loop_list(list_files(svrbin,"*.dll")){
        $filename = get_file_name($$);
        if(os_platform()=="Unix"){
			process(mono, pdb2mdb + " " + $filename);
        }else{
            echo("{0} {1}", pdb2mdb, $filename);
            process(mono, pdb2mdb + " " + $filename);
        };
    };

    cd(rootdir);

    $var1 = command{
        win
        {:
            %dslcopy% %dsldir% %svrdsl%
        :};
        unix
        {:
            mono %dslcopy% %dsldir% %svrdsl%
        :};
    };

    $var2 = command{
        win
        {:
            %tablecopy% %tabledir% %svrtable% isserver
        :};
        unix
        {:
            mono %tablecopy% %tabledir% %svrtable% isserver
        :};
    };

    if($var0 || $var1 || $var2){
        set_fg_color("Red");
        echo("compile failed !");
        call("setcolor");
    };

    reset_color();
    if(paused){
	    echo("press any key ...");
	    pause();
	  };
    return($var0);
};

script(setcolor)
{
    if(os_platform()=="Unix"){
        set_fg_color("Blue");
    }else{
        set_fg_color("Yellow");
    };
};