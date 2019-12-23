input("*.tga","*.png","*.jpg","*.exr")
{
	int("maxSize",1){
		range(1,1024);
	};
	string("prop",""){
		multiple(["readable","mipmap"],[1,2]);
	};
	stringlist("filter", "");
	stringlist("notfilter", "");
	stringlist("anyfilter", "t_XueNu|t_GaoYue_luoli|t_DuanMuRong|t_ShaoSiMing|t_DaSiMing|t_XingHun|t_XiangShaoYu|m3a0_ChuLingEr|t_ChiLian|t_GeNie|t_ShiLan|t_TianMing|t_YanDan_DouLi|t_GaoJianLi|t_ZhangLiang|t_WeiZhuang|t_GongSunLingLong|t_LongJu_JiaZhou|t_GaoYue_YinYangJia");
	stringlist("anynotfilter", "");
	float("pathwidth",240){range(20,4096);};
	feature("source", "project");
	feature("menu", "1.Project Resources/Revert Sized Textures");
	feature("description", "just so so");
}
filter
{
    if(stringcontains(assetpath, filter) && stringnotcontains(assetpath, notfilter) && stringcontainsany(assetpath, anyfilter) && stringnotcontainsany(assetpath, anynotfilter)){
    	var(0) = loadasset(assetpath);
    	if(isnull(var(0))){
    		0;
    	} else {
    		var(1) = var(0).width;
    		var(2) = var(0).height;
    		var(3) = importer.isReadable;
    		var(4) = importer.mipmapEnabled;
    		
    		var(5) = gettexturesetting("iPhone");
        	var(6) = gettexturesetting("Android");
	
    		//unloadasset(var(0));
    		order = var(1) < var(2) ? var(2) : var(1);
    		if((var(1) > maxSize || var(2) > maxSize) && (var(5).maxTextureSize<var(1) && var(5).maxTextureSize<var(2) || var(6).maxTextureSize<var(1) && var(6).maxTextureSize<var(2)) && (prop.Contains("1") && var(3) || !prop.Contains("1")) && (prop.Contains("2") && var(4) || !prop.Contains("2"))){
    			info = format("size:{0},{1} readable:{2} mipmap:{3}", var(1), var(2), var(3), var(4));
    			1;
    		} else {
    		    0;
    		};
    	};
    }else{
        0;  
    };
}
process
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
	var(2).maxTextureSize = 2048;
	setastctexture(var(2));
	settexturesetting(var(2));
	
	var(3) = gettexturesetting("Android");
	var(3).overridden=true;
	var(3).maxTextureSize = 2048;
	setastctexture(var(3));
	settexturesetting(var(3));
	
  saveandreimport();
};