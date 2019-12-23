input("SetSpecialTextures")
{
	string("filter", "");
	feature("source", "list");
	feature("menu", "6.Tools/Set Special Textures");
	feature("description", "just so so");
}
filter
{
	$lines=callscript("GetLines",0);
	looplist($lines){
		var(0) = $$;
		looplist(listallfiles(var(0), "*body_d.*")){
			callscript("NewItem", $$, 0);
		};
		looplist(listallfiles(var(0), "*body_n.*")){
			callscript("NewItem", $$, 0);
		};
		looplist(listallfiles(var(0), "*face_d.*")){
			callscript("NewItem", $$, 1);
		};
		looplist(listallfiles(var(0), "*face_n.*")){
			callscript("NewItem", $$, 1);
		};
	};
	$lines=callscript("GetLines",1);
	looplist($lines){
		var(0) = $$;
		looplist(listallfiles(var(0), "*body*_d.*")){
			callscript("NewItem", $$, 0);
		};
		looplist(listallfiles(var(0), "*body*_n.*")){
			callscript("NewItem", $$, 0);
		};
	};
	$lines=callscript("GetLines",2);
	looplist($lines){
		var(0) = $$;
		looplist(listallfiles(var(0), "*face*_d.*")){
			callscript("NewItem", $$, 1);
		};
		looplist(listallfiles(var(0), "*face*_n.*")){
			callscript("NewItem", $$, 1);
		};
	};
}
process
{
	if(order==0){
		callscript("SetTexture", 1024);
	}else{
		callscript("SetTexture", 512);		
	};
};

script(GetLines)args($type)
{
	$lines = list();
	if($type==0){
		$lines = readalllines("texture_s_npc.txt");
	}elseif($type==1){
		$lines = readalllines("texture_player_body.txt");		
	}elseif($type==2){
		$lines = readalllines("texture_player_face.txt");		
	};
	return($lines);
};

script(NewItem)args($file, $type)
{
	if(!$file.EndsWith(".meta") && $file.Contains(filter)){
		var(0) = $file.Replace("\\","/");
		var(10) = loadasset(var(0));
		var(11) = var(10).width;
		var(12) = var(10).height;
		var(1) = newitem();
		var(1).AssetPath = var(0);
		var(1).Importer = getassetimporter(var(0));
		var(1).Info = "w*h:"+var(11)+","+var(12);
		var(1).Order = $type;
		var(1).Value = 0;
	};
};

script(SetTexture)args($maxSize)
{
	/*
	var(0) = getdefaulttexturesetting();
	var(0).maxTextureSize = changetype(maxSize, "int");
	settexturesetting(var(0));
	
	var(1) = gettexturesetting("Standalone");
	var(1).maxTextureSize = changetype(maxSize, "int");
	settexturesetting(var(1));
	*/
	
	var(2) = gettexturesetting("iPhone");
	var(2).overridden=true;
	var(2).maxTextureSize = $maxSize;
	setastctexture(var(2), 8);
	settexturesetting(var(2));
	
	var(3) = gettexturesetting("Android");
	var(3).overridden=true;
	var(3).maxTextureSize = $maxSize;
	setastctexture(var(3), 8);
	settexturesetting(var(3));
	
	updatetexturedb(assetpath, importer);
};