input("*.prefab")
{
    int("maxSize",10);
    stringlist("anyfilter", "");
    stringlist("psanyfilter", "");
    stringhash("hashkeys", "");
    stringhash("pshashkeys", "");
    float("pathwidth",240){range(20,4096);};
    feature("source", "project");
    feature("menu", "1.Project Resources/Particle Systems");
    feature("description", "just so so");
}
filter
{
    object = loadasset(assetpath);
    var(0) = getcomponentsinchildren(object,"ParticleSystem");
    var(1) = var(0).Length;
    $name = getfilenamewithoutextension(assetpath);
    if(var(1)>maxSize && stringcontainsany(assetpath, anyfilter) && stringhashcontains(hashkeys, $name)){	    
        $ret = 0;
        $key = "";
        looplist(var(0)){
            var(2) = $$.name;
            if(stringcontainsany(var(2), psanyfilter) && stringhashcontains(pshashkeys, var(2))){
                $ret = 1;
                $key = var(2);
                break;
            };
        };
        if($ret){            
            var(3) = collectprefabinfo(object);
            $totalTriangleCount = var(3).triangleCount;
            scenepath = getfilenamewithoutextension(assetpath);
            info = format("key:{0} particle_count:{1} total_prefab_triangle:{2}",$key,var(1),$totalTriangleCount);    
            order = $totalTriangleCount;            
            value = $totalTriangleCount;
        };
        $ret;
    }else{
        0;
    };
};