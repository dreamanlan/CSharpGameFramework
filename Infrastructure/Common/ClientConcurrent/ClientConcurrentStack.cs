using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Collections.Concurrent
{
	public class ClientConcurrentStack<T> : IEnumerable<T>, ICollection, IEnumerable
	{
		class Node
		{
			public T Value = default (T);
			public object Next;
		}
		
		object head;
		
		int count;
		
		public ClientConcurrentStack ()
		{
		}
		
		public ClientConcurrentStack (IEnumerable<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException ("collection");

			foreach (T item in collection) 
				Push (item);
		}
		
		public void Push (T item)
		{
			Node temp = new Node ();
			temp.Value = item;
			do {
			  temp.Next = head;
			} while (ClientInterlocked.CompareExchange (ref head, temp, temp.Next) != temp.Next);
			
			ClientInterlocked.Increment (ref count);
		}

		public void PushRange (T[] items)
		{
			if (items == null)
				throw new ArgumentNullException ("items");

			PushRange (items, 0, items.Length);
		}
		
		public void PushRange (T[] items, int startIndex, int count)
		{
			RangeArgumentsCheck (items, startIndex, count);

			Node insert = null;
			Node first = null;
			
			for (int i = startIndex; i < count; i++) {
				Node temp = new Node ();
				temp.Value = items[i];
				temp.Next = insert;
				insert = temp;
				
				if (first == null)
					first = temp;
			}
			
			do {
				first.Next = head;
			} while (ClientInterlocked.CompareExchange (ref head, insert, first.Next) != first.Next);
			
			ClientInterlocked.Add (ref this.count, count);
		}
		
		public bool TryPop (out T result)
		{
			Node temp;
			do {
				temp = (Node)head;
				// The stak is empty
				if (temp == null) {
					result = default (T);
					return false;
				}
			} while (ClientInterlocked.CompareExchange (ref head, temp.Next, temp) != temp);
			
			ClientInterlocked.Decrement (ref count);
			
			result = temp.Value;

			return true;
		}

		public int TryPopRange (T[] items)
		{
			if (items == null)
				throw new ArgumentNullException ("items");
			return TryPopRange (items, 0, items.Length);
		}

		public int TryPopRange (T[] items, int startIndex, int count)
		{
			RangeArgumentsCheck (items, startIndex, count);

			object temp;
			object end;
			
			do {
				temp = head;
				if (temp == null)
					return 0;
				end = temp;
				for (int j = 0; j < count; j++) {
					end = ((Node)end).Next;
					if (end == null)
						break;
				}
			} while (ClientInterlocked.CompareExchange (ref head, end, temp) != temp);
			
			int i;
			for (i = startIndex; i < startIndex + count && temp != null; i++) {
				items[i] = ((Node)temp).Value;
				end = temp;
				temp = ((Node)temp).Next;
			}
			ClientInterlocked.Add (ref this.count, -(i - startIndex));
			
			return i - startIndex;
		}
		
		public bool TryPeek (out T result)
		{
			Node myHead = head as Node;
			if (myHead == null) {
				result = default (T);
				return false;
			}
			result = myHead.Value;
			return true;
		}
		
		public void Clear ()
		{
			// This is not satisfactory
			count = 0;
			head = null;
		}
		
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return (IEnumerator)InternalGetEnumerator ();
		}
		
		public IEnumerator<T> GetEnumerator ()
		{
			return InternalGetEnumerator ();
		}

		IEnumerator<T> InternalGetEnumerator ()
		{
			Node my_head = head as Node;
			if (my_head == null) {
				yield break;
			} else {
				do {
					yield return my_head.Value;
				} while ((my_head = my_head.Next as Node) != null);
			}
		}
		
		void ICollection.CopyTo (Array array, int index)
		{
			ICollection ic = new List<T> (this);
			ic.CopyTo (array, index);
		}
		
		public void CopyTo (T[] array, int index)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (index < 0)
				throw new ArgumentOutOfRangeException ("index");
			if (index > array.Length)
				throw new ArgumentException ("index is equals or greather than array length", "index");

			IEnumerator<T> e = InternalGetEnumerator ();
			int i = index;
			while (e.MoveNext ()) {
				if (i == array.Length - index)
					throw new ArgumentException ("The number of elememts in the collection exceeds the capacity of array", "array");
				array[i++] = e.Current;
			}
		}
		
		bool ICollection.IsSynchronized {
			get { return false; }
		}
		
		object ICollection.SyncRoot {
			get {
				throw new NotSupportedException ();
			}
		}
		
		public T[] ToArray ()
		{
			return new List<T> (this).ToArray ();
		}
		
		public int Count {
			get {
				return count;
			}
		}
		
		public bool IsEmpty {
			get {
				return count == 0;
			}
		}

		static void RangeArgumentsCheck (T[] items, int startIndex, int count)
		{
			if (items == null)
				throw new ArgumentNullException ("items");
			if (startIndex < 0 || startIndex >= items.Length)
				throw new ArgumentOutOfRangeException ("startIndex");
			if (count < 0)
				throw new ArgumentOutOfRangeException ("count");
			if (startIndex + count > items.Length)
				throw new ArgumentException ("startIndex + count is greater than the length of items.");
		}
	}
}

