using System;
using ScriptableFramework;

namespace System.Threading
{
	public struct ClientSpinWait
	{
		// The number of step until SpinOnce yield on multicore machine
		const           int  step = 10;
		const           int  maxTime = 200;
		static readonly bool isSingleCpu = (Environment.ProcessorCount == 1);

		int ntime;

		public void SpinOnce ()
		{
			ntime += 1;

			if (isSingleCpu) {
				// On a single-CPU system, spinning does no good
				Thread.Sleep(0);
			} else {
				if (ntime % step == 0)
					Thread.Sleep(0);
				else
					// Multi-CPU system might be hyper-threaded, let other thread run
					Thread.SpinWait (Math.Min (ntime, maxTime) << 1);
			}
		}

		public static void SpinUntil (MyFunc<bool> condition)
		{
			ClientSpinWait sw = new ClientSpinWait ();
			while (!condition ())
				sw.SpinOnce ();
		}

		public static bool SpinUntil (MyFunc<bool> condition, TimeSpan timeout)
		{
			return SpinUntil (condition, (int)timeout.TotalMilliseconds);
		}

		public static bool SpinUntil (MyFunc<bool> condition, int millisecondsTimeout)
		{
			ClientSpinWait sw = new ClientSpinWait ();
			ClientWatch watch = ClientWatch.StartNew ();

			while (!condition ()) {
				if (watch.ElapsedMilliseconds > millisecondsTimeout)
					return false;
				sw.SpinOnce ();
			}

			return true;
		}

		public void Reset ()
		{
			ntime = 0;
		}

		public bool NextSpinWillYield {
			get {
				return isSingleCpu ? true : ntime % step == 0;
			}
		}

		public int Count {
			get {
				return ntime;
			}
		}
	}  
}
