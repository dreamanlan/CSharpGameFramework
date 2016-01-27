using System;

namespace System.Threading
{
	internal struct ClientWatch
	{
		long startTicks;
		
		public static ClientWatch StartNew ()
		{
			ClientWatch watch = new ClientWatch ();
			watch.Start ();
			return watch;
		}
		
		public void Start ()
		{
			startTicks = TicksNow ();
		}
		
		public void Stop ()
		{			
		}
		
		public long ElapsedMilliseconds {
			get {
				return (TicksNow () - startTicks) / TimeSpan.TicksPerMillisecond;
			}
		}
		
		static long TicksNow ()
		{
			return DateTime.Now.Ticks;
		}
	}
}
