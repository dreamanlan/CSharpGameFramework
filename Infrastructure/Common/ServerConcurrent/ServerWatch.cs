using System;
using ScriptableFramework;

namespace System.Threading
{
	internal struct ServerWatch
	{
		long startTicks;
		
		public static ServerWatch StartNew ()
		{
			ServerWatch watch = new ServerWatch ();
			watch.Start ();
			return watch;
		}
		
		public void Start ()
		{
      startTicks = TimeUtility.GetElapsedTimeUs();
		}
		
		public void Stop ()
		{			
		}
		
		public long ElapsedMilliseconds {
			get {
        return (TimeUtility.GetElapsedTimeUs() - startTicks) / 1000;
			}
		}
	}
}
