input
{
  float("minFps", 20);
  float("maxTime", 10);
  float("maxGC", 1);
	feature("source", "instruments");
	feature("menu", "7.Profiler/time and gc");
	feature("description", "just so so");
}
filter
{
	order = instrument.index;
	if(instrument.fps <= minFps || instrument.totalCpuTime>=maxTime || instrument.totalGcMemory>=maxGC){
	  info = format("frame:{0} fps:{1} time:{2} gc:{3}",
	  		instrument.frame, instrument.fps, instrument.totalCpuTime, instrument.totalGcMemory
	    );
	  value = memory.totalGcMemory;
	  1;
	}else{
	  0;
	};
};