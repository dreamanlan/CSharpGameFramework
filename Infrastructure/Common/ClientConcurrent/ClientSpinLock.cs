using System;
using System.Collections.Concurrent;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace System.Threading
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct ClientTicketType {
		[FieldOffset(0)]
		public long TotalValue;
		[FieldOffset(0)]
		public int Value;
		[FieldOffset(4)]
		public int Users;
	}

	/* Implement the ticket SpinLock algorithm described on http://locklessinc.com/articles/locks/
	 * This lock is usable on both endianness.
	 * All the try/finally patterns in this class and various extra gimmicks compared to the original
	 * algorithm are here to avoid problems caused by asynchronous exceptions.
	 */
	[System.Diagnostics.DebuggerDisplay ("IsHeld = {IsHeld}")]
	[System.Diagnostics.DebuggerTypeProxy ("System.Threading.SpinLock+SystemThreading_SpinLockDebugView")]
	public struct ClientSpinLock
	{
		ClientTicketType ticket;

		int threadWhoTookLock;
		readonly bool isThreadOwnerTrackingEnabled;

		static readonly ClientWatch sw = ClientWatch.StartNew ();

		object stallTickets;

		public bool IsThreadOwnerTrackingEnabled {
			get {
				return isThreadOwnerTrackingEnabled;
			}
		}

		public bool IsHeld {
			get {
				// No need for barrier here
				long totalValue = ticket.TotalValue;
				return (totalValue >> 32) != (totalValue & 0xFFFFFFFF);
			}
		}

		public bool IsHeldByCurrentThread {
			get {
				if (isThreadOwnerTrackingEnabled)
					return IsHeld && Thread.CurrentThread.ManagedThreadId == threadWhoTookLock;
				else
					return IsHeld;
			}
		}

		public ClientSpinLock (bool enableThreadOwnerTracking)
		{
			this.isThreadOwnerTrackingEnabled = enableThreadOwnerTracking;
			this.threadWhoTookLock = 0;
			this.ticket = new ClientTicketType ();
			this.stallTickets = null;
		}

		public void Enter (ref bool lockTaken)
		{
			if (lockTaken)
				throw new ArgumentException ("lockTaken", "lockTaken must be initialized to false");
			if (isThreadOwnerTrackingEnabled && IsHeldByCurrentThread)
				throw new LockRecursionException ();

			int slot = -1;

			RuntimeHelpers.PrepareConstrainedRegions ();
			try {
				slot = ClientInterlocked.Increment (ref ticket.Users) - 1;

				ClientSpinWait wait = new ClientSpinWait ();
				while (slot != ticket.Value) {
					wait.SpinOnce ();

          while (stallTickets != null && ((ClientConcurrentOrderedList<int>)stallTickets).TryRemove(ticket.Value))
						++ticket.Value;
				}
			} finally {
				if (slot == ticket.Value) {
					lockTaken = true;
					threadWhoTookLock = Thread.CurrentThread.ManagedThreadId;
				} else if (slot != -1) {
					// We have been interrupted, initialize stallTickets
					if (stallTickets == null)
            ClientInterlocked.CompareExchange(ref stallTickets, new ClientConcurrentOrderedList<int>(), null);
					((ClientConcurrentOrderedList<int>)stallTickets).TryAdd (slot);
				}
			}
		}

		public void TryEnter (ref bool lockTaken)
		{
			TryEnter (0, ref lockTaken);
		}

		public void TryEnter (TimeSpan timeout, ref bool lockTaken)
		{
			TryEnter ((int)timeout.TotalMilliseconds, ref lockTaken);
		}

		public void TryEnter (int millisecondsTimeout, ref bool lockTaken)
		{
			if (millisecondsTimeout < -1)
				throw new ArgumentOutOfRangeException ("milliSeconds", "millisecondsTimeout is a negative number other than -1");
			if (lockTaken)
				throw new ArgumentException ("lockTaken", "lockTaken must be initialized to false");
			if (isThreadOwnerTrackingEnabled && IsHeldByCurrentThread)
				throw new LockRecursionException ();

			long start = millisecondsTimeout == -1 ? 0 : sw.ElapsedMilliseconds;
			bool stop = false;

      do {
				while (stallTickets != null && ((ClientConcurrentOrderedList<int>)stallTickets).TryRemove (ticket.Value))
					++ticket.Value;

				long u = ticket.Users;
				long totalValue = (u << 32) | u;
				long newTotalValue
					= BitConverter.IsLittleEndian ? (u << 32) | (u + 1) : ((u + 1) << 32) | u;
				
				RuntimeHelpers.PrepareConstrainedRegions ();
				try {}
				finally {
					lockTaken = ClientInterlocked.CompareExchange (ref ticket.TotalValue, newTotalValue, totalValue) == totalValue;
				
					if (lockTaken) {
						threadWhoTookLock = Thread.CurrentThread.ManagedThreadId;
						stop = true;
					}
				}
	        } while (!stop && (millisecondsTimeout == -1 || (sw.ElapsedMilliseconds - start) < millisecondsTimeout));
		}

		[ReliabilityContract (Consistency.WillNotCorruptState, Cer.Success)]
		public void Exit ()
		{
			Exit (false);
		}

		[ReliabilityContract (Consistency.WillNotCorruptState, Cer.Success)]
		public void Exit (bool useMemoryBarrier)
		{
			RuntimeHelpers.PrepareConstrainedRegions ();
			try {}
			finally {
				if (isThreadOwnerTrackingEnabled && !IsHeldByCurrentThread)
					throw new SynchronizationLockException ("Current thread is not the owner of this lock");

				threadWhoTookLock = int.MinValue;
				do {
					if (useMemoryBarrier)
						ClientInterlocked.Increment (ref ticket.Value);
					else
            ticket.Value++;
        } while (stallTickets != null && ((ClientConcurrentOrderedList<int>)stallTickets).TryRemove(ticket.Value));
			}
		}
	}

  public struct ClientSimpleRwLock
  {
    const int RwWait = 1;
    const int RwWrite = 2;
    const int RwRead = 4;

    int rwlock;

    public void EnterReadLock()
    {
      ClientSpinWait sw = new ClientSpinWait();
      do {
        while ((rwlock & (RwWrite | RwWait)) > 0)
          sw.SpinOnce();

        if ((ClientInterlocked.Add(ref rwlock, RwRead) & (RwWait | RwWait)) == 0)
          return;

        ClientInterlocked.Add(ref rwlock, -RwRead);
      } while (true);
    }

    public void ExitReadLock()
    {
      ClientInterlocked.Add(ref rwlock, -RwRead);
    }

    public void EnterWriteLock()
    {
      ClientSpinWait sw = new ClientSpinWait();
      do {
        int state = rwlock;
        if (state < RwWrite) {
          if (ClientInterlocked.CompareExchange(ref rwlock, RwWrite, state) == state)
            return;
          state = rwlock;
        }
        // We register our interest in taking the Write lock (if upgradeable it's already done)
        while ((state & RwWait) == 0 && ClientInterlocked.CompareExchange(ref rwlock, state | RwWait, state) != state)
          state = rwlock;
        // Before falling to sleep
        while (rwlock > RwWait)
          sw.SpinOnce();
      } while (true);
    }

    public void ExitWriteLock()
    {
      ClientInterlocked.Add(ref rwlock, -RwWrite);
    }
  }
}
