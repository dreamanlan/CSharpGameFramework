using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Collections.Concurrent
{
	public class ClientConcurrentOrderedList<T>: ICollection<T>, IEnumerable<T>
  {
		class Node
		{
			public T Data;
			public int Key;
			public object Next;
			public bool Marked;		   

			public Node ()
			{

			}

			public Node (Node wrapped)
			{
				Marked = true;
				Next = wrapped;
			}
		}

		object head;
		object tail;

		IEqualityComparer<T> comparer;

		int count;

    public ClientConcurrentOrderedList()
      : this(MyDefaultComparer<T>.Instance)
		{
			
		}

		public ClientConcurrentOrderedList (IEqualityComparer<T> comparer)
		{
			if (comparer == null)
				throw new ArgumentNullException ("comparer");

			this.comparer = comparer;

			head = new Node ();
			tail = new Node ();
			((Node)head).Next = tail;
		}

		public bool TryAdd (T data)
		{
			Node node = new Node ();
			node.Data = data;
			node.Key = comparer.GetHashCode (data);

			if (ListInsert (node)) {
				ClientInterlocked.Increment (ref count);
				return true;
			}

			return false;
		}

		public bool TryRemove (T data)
		{
			T dummy;
			return TryRemoveHash (comparer.GetHashCode (data), out dummy);
		}

		public bool TryRemoveHash (int key, out T data)
		{
			if (ListDelete (key, out data)) {
				ClientInterlocked.Decrement (ref count);
				return true;
			}

			return false;
		}

		public bool TryPop (out T data)
		{
			return ListPop (out data);
		}

		public bool Contains (T data)
		{
			return ContainsHash (comparer.GetHashCode (data));
		}

		public bool ContainsHash (int key)
		{
			Node node;

			if (!ListFind (key, out node))
				return false;

			return true;

		}

		public bool TryGetFromHash (int key, out T data)
		{
			data = default (T);
			Node node;

			if (!ListFind (key, out node))
				return false;

			data = node.Data;
			return true;
		}

		public void Clear ()
		{
			((Node)head).Next = tail;
		}

		public void CopyTo (T[] array, int startIndex)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (startIndex < 0)
				throw new ArgumentOutOfRangeException ("startIndex");
			if (count > array.Length - startIndex)
				throw new ArgumentException ("array", "The number of elements is greater than the available space from startIndex to the end of the destination array.");

			foreach (T item in this) {
				if (startIndex >= array.Length)
					break;

				array[startIndex++] = item;
			}
		}

		public IEqualityComparer<T> Comparer {
			get {
				return comparer;
			}
		}

		public int Count {
			get {
				return count;
			}
		}

		Node ListSearch (int key, ref Node left)
		{
			Node leftNodeNext = null, rightNode = null;

			do {
				Node t = (Node)head;
				Node tNext = (Node)t.Next;
				do {
					if (!tNext.Marked) {
						left = t;
						leftNodeNext = tNext;
					}
					t = tNext.Marked ? (Node)tNext.Next : tNext;
					if (t == tail)
						break;
					
					tNext = (Node)((Node)t).Next;
				} while (tNext.Marked || t.Key < key);

				rightNode = t;
				
				if (leftNodeNext == rightNode) {
					if (rightNode != tail && ((Node)rightNode.Next).Marked)
						continue;
					else 
						return rightNode;
				}
				
				if (ClientInterlocked.CompareExchange (ref left.Next, rightNode, leftNodeNext) == leftNodeNext) {
					if (rightNode != tail && ((Node)rightNode.Next).Marked)
						continue;
					else
						return rightNode;
				}
			} while (true);
		}

		bool ListDelete (int key, out T data)
		{
			Node rightNode = null, rightNodeNext = null, leftNode = null;
			data = default (T);
			
			do {
				rightNode = ListSearch (key, ref leftNode);
				if (rightNode == tail || rightNode.Key != key)
					return false;

				data = rightNode.Data;
				
				rightNodeNext = ((Node)rightNode.Next);
				if (!rightNodeNext.Marked)
					if (ClientInterlocked.CompareExchange (ref rightNode.Next, new Node (rightNodeNext), rightNodeNext) == rightNodeNext)
						break;
			} while (true);
			
			if (ClientInterlocked.CompareExchange (ref leftNode.Next, rightNodeNext, rightNode) != rightNodeNext)
				ListSearch (rightNode.Key, ref leftNode);
			
			return true;
		}

		bool ListPop (out T data)
		{
			Node rightNode = null, rightNodeNext = null, leftNode = (Node)head;
			data = default (T);

			do {
				rightNode = (Node)((Node)head).Next;
				if (rightNode == tail)
					return false;

				data = rightNode.Data;

				rightNodeNext = (Node)rightNode.Next;
				if (!rightNodeNext.Marked)
					if (ClientInterlocked.CompareExchange (ref rightNode.Next, new Node (rightNodeNext), rightNodeNext) == rightNodeNext)
						break;
			} while (true);

			if (ClientInterlocked.CompareExchange (ref leftNode.Next, rightNodeNext, rightNode) != rightNodeNext)
				ListSearch (rightNode.Key, ref leftNode);

			return true;
		}
		
		bool ListInsert (Node newNode)
		{
			int key = newNode.Key;
			Node rightNode = null, leftNode = null;
			
			do {
				rightNode = ListSearch (key, ref leftNode);
				if (rightNode != tail && rightNode.Key == key)
					return false;
				
				newNode.Next = rightNode;
				if (ClientInterlocked.CompareExchange (ref leftNode.Next, newNode, rightNode) == rightNode)
					return true;
			} while (true);
		}
		
		bool ListFind (int key, out Node data)
		{
			Node rightNode = null, leftNode = null;
			data = null;
			
			data = rightNode = ListSearch (key, ref leftNode);
			
			return rightNode != tail && rightNode.Key == key;
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator ()
		{
			return GetEnumeratorInternal ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumeratorInternal ();
		}

		IEnumerator<T> GetEnumeratorInternal ()
		{
			Node node = (Node)((Node)head).Next;

			while (node != tail) {
				while (node.Marked) {
					node = (Node)node.Next;
					if (node == tail)
						yield break;
				}
				yield return node.Data;
				node = (Node)node.Next;
			}
		}

		bool ICollection<T>.IsReadOnly {
			get {
				return false;
			}
		}

		void ICollection<T>.Add (T item)
		{
			TryAdd (item);
		}

		bool ICollection<T>.Remove (T item)
		{
			return TryRemove (item);
		}
	}
}

