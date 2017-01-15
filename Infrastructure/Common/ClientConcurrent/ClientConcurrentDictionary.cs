using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Diagnostics;
using GameFramework;

namespace System.Collections.Concurrent
{
  public class ClientConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>,
    IDictionary, ICollection, IEnumerable
  {
    class Node
    {
      public bool Marked;
      public ulong Key;
      public TKey SubKey;
      public KeyValuePair<TKey, TValue> Data;
      public object Next;

      public Node Init(ulong key, TKey subKey, KeyValuePair<TKey, TValue> data)
      {
        this.Key = key;
        this.SubKey = subKey;
        this.Data = data;

        this.Marked = false;
        this.Next = null;

        return this;
      }

      // Used to create dummy node
      public Node Init(ulong key)
      {
        this.Key = key;
        this.Data = default(KeyValuePair<TKey, TValue>);

        this.Next = null;
        this.Marked = false;
        this.SubKey = default(TKey);

        return this;
      }

      // Used to create marked node
      public Node Init(Node wrapped)
      {
        this.Marked = true;
        this.Next = wrapped;

        this.Key = 0;
        this.Data = default(KeyValuePair<TKey, TValue>);
        this.SubKey = default(TKey);

        return this;
      }
    }

    const int MaxLoad = 5;
    const uint BucketSize = 512;

    object head;
    object tail;

    object[] buckets = new object[BucketSize];
    int count;
    int size = 2;

    ClientSimpleRwLock slim = new ClientSimpleRwLock();

    readonly IEqualityComparer<TKey> comparer;

    public ClientConcurrentDictionary()
    {
      this.comparer = MyDefaultComparer<TKey>.Instance;
      head = new Node().Init(0);
      tail = new Node().Init(ulong.MaxValue);
      SetBucket(0, (Node)head);
      ((Node)head).Next = (Node)tail;
    }

    public ClientConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
      : this()
    {
      foreach (KeyValuePair<TKey, TValue> pair in collection)
        Add(pair.Key, pair.Value);
    }

    void CheckKey(TKey key)
    {
      if (key == null)
        throw new ArgumentNullException("key");
    }

    void Add(TKey key, TValue value)
    {
      while (!TryAdd(key, value)) ;
    }

    void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
    {
      Add(key, value);
    }

    public bool TryAdd(TKey key, TValue value)
    {
      CheckKey(key);
      Node current;
      return InsertInternal(Hash(key), key, Make(key, value), out current);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair)
    {
      Add(pair.Key, pair.Value);
    }
    
    public TValue AddOrUpdate(TKey key, TValue addValue, TValue updateValue)
    {
      CheckKey(key);

      Node current;
      if (InsertInternal(Hash(key), key, Make(key, addValue), out current))
        return current.Data.Value;

      // FIXME: this should have a CAS-like behavior
      current.Data = Make(key, updateValue);
      return updateValue;
    }

    TValue GetValue(TKey key)
    {
      TValue temp;
      if (!TryGetValue(key, out temp))
        throw new KeyNotFoundException(key.ToString());
      return temp;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      CheckKey(key);
      KeyValuePair<TKey, TValue> pair;
      bool result = Find(Hash(key), key, out pair);
      value = pair.Value;

      return result;
    }

    public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
    {
      CheckKey(key);
      return CompareExchange(Hash(key), key, Make(key, newValue), comparisonValue);
    }

    public TValue this[TKey key]
    {
      get
      {
        return GetValue(key);
      }
      set
      {
        AddOrUpdate(key, value, value);
      }
    }

    public TValue GetOrAdd(TKey key, TValue value)
    {
      CheckKey(key);

      Node current;
      InsertInternal(Hash(key), key, Make(key, value), out current);
      return current.Data.Value;
    }

    public bool TryRemove(TKey key, out TValue value)
    {
      CheckKey(key);
      KeyValuePair<TKey, TValue> data;
      bool result = Delete(Hash(key), key, out data);
      value = data.Value;
      return result;
    }

    bool Remove(TKey key)
    {
      TValue dummy;

      return TryRemove(key, out dummy);
    }

    bool IDictionary<TKey, TValue>.Remove(TKey key)
    {
      return Remove(key);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> pair)
    {
      return Remove(pair.Key);
    }

    public bool ContainsKey(TKey key)
    {
      CheckKey(key);
      KeyValuePair<TKey, TValue> dummy;
      return Find(Hash(key), key, out dummy);
    }

    bool IDictionary.Contains(object key)
    {
      if (!(key is TKey))
        return false;

      return ContainsKey((TKey)key);
    }

    void IDictionary.Remove(object key)
    {
      if (!(key is TKey))
        return;

      Remove((TKey)key);
    }

    object IDictionary.this[object key]
    {
      get
      {
        TValue obj;
        if (key is TKey && TryGetValue((TKey)key, out obj))
          return obj;
        return null;
      }
      set
      {
        if (!(key is TKey) || !(value is TValue))
          throw new ArgumentException("key or value aren't of correct type");

        this[(TKey)key] = (TValue)value;
      }
    }

    void IDictionary.Add(object key, object value)
    {
      if (!(key is TKey) || !(value is TValue))
        throw new ArgumentException("key or value aren't of correct type");

      Add((TKey)key, (TValue)value);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
    {
      TValue value;
      if (!TryGetValue(pair.Key, out value))
        return false;

      return MyDefaultComparer<TValue>.Instance.Equals(value, pair.Value);
    }

    public KeyValuePair<TKey, TValue>[] ToArray()
    {
      // This is most certainly not optimum but there is
      // not a lot of possibilities

      return new List<KeyValuePair<TKey, TValue>>(this).ToArray();
    }

    public void Clear()
    {
      // Pronk
      buckets = new Node[BucketSize];
      size = 2;
      head = new Node().Init(0);
      tail = new Node().Init(ulong.MaxValue);
      buckets[0] = head;
      //SetBucket(0, (Node)head);
      ((Node)head).Next = tail;
    }

    public int Count
    {
      get
      {
        return count;
      }
    }

    public bool IsEmpty
    {
      get
      {
        return Count == 0;
      }
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
    {
      get
      {
        return false;
      }
    }

    bool IDictionary.IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public ICollection<TKey> Keys
    {
      get
      {
        List<TKey> temp = new List<TKey>();

        foreach (KeyValuePair<TKey, TValue> kvp in this)
          temp.Add(kvp.Key);

        return temp.AsReadOnly();
      }
    }

    public ICollection<TValue> Values
    {
      get
      {
        List<TValue> temp = new List<TValue>();

        foreach (KeyValuePair<TKey, TValue> kvp in this)
          temp.Add(kvp.Value);

        return temp.AsReadOnly();
      }
    }

    ICollection IDictionary.Keys
    {
      get
      {
        return (ICollection)Keys;
      }
    }

    ICollection IDictionary.Values
    {
      get
      {
        return (ICollection)Values;
      }
    }

    void ICollection.CopyTo(Array array, int startIndex)
    {
      KeyValuePair<TKey, TValue>[] arr = array as KeyValuePair<TKey, TValue>[];
      if (arr == null)
        return;

      CopyTo(arr, startIndex, Count);
    }

    void CopyTo(KeyValuePair<TKey, TValue>[] array, int startIndex)
    {
      CopyTo(array, startIndex, Count);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int startIndex)
    {
      CopyTo(array, startIndex);
    }

    void CopyTo(KeyValuePair<TKey, TValue>[] array, int startIndex, int num)
    {
      foreach (var kvp in this) {
        array[startIndex++] = kvp;

        if (--num <= 0)
          return;
      }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      return GetEnumeratorInternal();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator)GetEnumeratorInternal();
    }

    IEnumerator<KeyValuePair<TKey, TValue>> GetEnumeratorInternal()
    {
      Node node = (Node)((Node)head).Next;

      while (node != tail) {
        while (node.Marked || (node.Key & 1) == 0) {
          node = (Node)node.Next;
          if (node == tail)
            yield break;
        }
        yield return node.Data;
        node = (Node)node.Next;
      }
    }

    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
      return new ConcurrentDictionaryEnumerator(GetEnumeratorInternal());
    }

    class ConcurrentDictionaryEnumerator : IDictionaryEnumerator
    {
      IEnumerator<KeyValuePair<TKey, TValue>> internalEnum;

      public ConcurrentDictionaryEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> internalEnum)
      {
        this.internalEnum = internalEnum;
      }

      public bool MoveNext()
      {
        return internalEnum.MoveNext();
      }

      public void Reset()
      {
        internalEnum.Reset();
      }

      public object Current
      {
        get
        {
          return Entry;
        }
      }

      public DictionaryEntry Entry
      {
        get
        {
          KeyValuePair<TKey, TValue> current = internalEnum.Current;
          return new DictionaryEntry(current.Key, current.Value);
        }
      }

      public object Key
      {
        get
        {
          return internalEnum.Current.Key;
        }
      }

      public object Value
      {
        get
        {
          return internalEnum.Current.Value;
        }
      }
    }

    object ICollection.SyncRoot
    {
      get
      {
        return this;
      }
    }

    bool IDictionary.IsFixedSize
    {
      get
      {
        return false;
      }
    }

    bool ICollection.IsSynchronized
    {
      get { return true; }
    }

    static KeyValuePair<U, V> Make<U, V>(U key, V value)
    {
      return new KeyValuePair<U, V>(key, value);
    }

    uint Hash(TKey key)
    {
      return (uint)comparer.GetHashCode(key);
    }

    bool InsertInternal(uint key, TKey subKey, KeyValuePair<TKey, TValue> data, out Node current)
    {
      Node node = new Node().Init(ComputeRegularKey(key), subKey, data);

      uint b = key % (uint)size;
      Node bucket;

      if ((bucket = GetBucket(b)) == null)
        bucket = InitializeBucket(b);

      if (!ListInsert(node, bucket, out current))
        return false;

      int csize = size;
      if (ClientInterlocked.Increment(ref count) / csize > MaxLoad && (csize & 0x40000000) == 0)
        ClientInterlocked.CompareExchange(ref size, 2 * csize, csize);

      current = node;

      return true;
    }

    bool Find(uint key, TKey subKey, out KeyValuePair<TKey, TValue> data)
    {
      Node node;
      uint b = key % (uint)size;
      data = default(KeyValuePair<TKey, TValue>);
      Node bucket;

      if ((bucket = GetBucket(b)) == null)
        bucket = InitializeBucket(b);

      if (!ListFind(ComputeRegularKey(key), subKey, bucket, out node))
        return false;

      data = node.Data;

      return !node.Marked;
    }

    bool CompareExchange(uint key, TKey subKey, KeyValuePair<TKey, TValue> data, TValue comparisonValue)
    {
      Node node;
      uint b = key % (uint)size;
      Node bucket;

      if ((bucket = GetBucket(b)) == null)
        bucket = InitializeBucket(b);

      if (!ListFind(ComputeRegularKey(key), subKey, bucket, out node))
        return false;

      if(!node.Data.Value.Equals(comparisonValue))
        return false;

      node.Data = data;

      return true;
    }

    bool Delete(uint key, TKey subKey, out KeyValuePair<TKey, TValue> data)
    {
      uint b = key % (uint)size;
      Node bucket;

      if ((bucket = GetBucket(b)) == null)
        bucket = InitializeBucket(b);

      if (!ListDelete(bucket, ComputeRegularKey(key), subKey, out data))
        return false;

      ClientInterlocked.Decrement(ref count);
      return true;
    }

    Node InitializeBucket(uint b)
    {
      Node current;
      uint parent = GetParent(b);
      Node bucket;

      if ((bucket = GetBucket(parent)) == null)
        bucket = InitializeBucket(parent);

      Node dummy = new Node().Init(ComputeDummyKey(b));
      if (!ListInsert(dummy, bucket, out current))
        return current;

      return SetBucket(b, dummy);
    }

    // Turn v's MSB off
    static uint GetParent(uint v)
    {
      uint t, tt;

      // Find MSB position in v
      var pos = (tt = v >> 16) > 0 ?
        (t = tt >> 8) > 0 ? 24 + logTable[t] : 16 + logTable[tt] :
        (t = v >> 8) > 0 ? 8 + logTable[t] : logTable[v];

      return (uint)(v & ~(1 << pos));
    }

    // Reverse integer bits and make sure LSB is set
    static ulong ComputeRegularKey(uint key)
    {
      return ComputeDummyKey(key) | 1;
    }

    // Reverse integer bits
    static ulong ComputeDummyKey(uint key)
    {
      return ((ulong)(((uint)reverseTable[key & 0xff] << 24) |
                      ((uint)reverseTable[(key >> 8) & 0xff] << 16) |
                      ((uint)reverseTable[(key >> 16) & 0xff] << 8) |
                      ((uint)reverseTable[(key >> 24) & 0xff]))) << 1;
    }

    // Bucket storage is abstracted in a simple two-layer tree to avoid too much memory resize
    Node GetBucket(uint index)
    {
      if (index >= buckets.Length)
        return null;
      return (Node)buckets[index];
    }

    Node SetBucket(uint index, Node node)
    {
      try {
        slim.EnterReadLock();
        CheckSegment(index, true);

        ClientInterlocked.CompareExchange(ref buckets[index], node, null);
        return (Node)buckets[index];
      } finally {
        slim.ExitReadLock();
      }
    }

    // When we run out of space for bucket storage, we use a lock-based array resize
    void CheckSegment(uint segment, bool readLockTaken)
    {
      if (segment < buckets.Length)
        return;

      if (readLockTaken)
        slim.ExitReadLock();
      try {
        slim.EnterWriteLock();
        while (segment >= buckets.Length)
          Array.Resize(ref buckets, buckets.Length * 2);
      } finally {
        slim.ExitWriteLock();
      }
      if (readLockTaken)
        slim.EnterReadLock();
    }

    Node ListSearch(ulong key, TKey subKey, ref Node left, Node h)
    {
      Node leftNodeNext = null, rightNode = null;

      do {
        Node t = h;
        Node tNext = (Node)t.Next;
        do {
          if (!tNext.Marked) {
            left = t;
            leftNodeNext = tNext;
          }
          t = tNext.Marked ? (Node)tNext.Next : tNext;
          if (t == tail)
            break;

          tNext = (Node)t.Next;
        } while (tNext.Marked || t.Key < key || (tNext.Key == key && !comparer.Equals(subKey, t.SubKey)));

        rightNode = t;

        if (leftNodeNext == rightNode) {
          if (rightNode != tail && ((Node)rightNode.Next).Marked)
            continue;
          else
            return rightNode;
        }

        if (ClientInterlocked.CompareExchange(ref left.Next, rightNode, leftNodeNext) == leftNodeNext) {
          if (rightNode != tail && ((Node)rightNode.Next).Marked)
            continue;
          else
            return rightNode;
        }
      } while (true);
    }

    bool ListDelete(Node startPoint, ulong key, TKey subKey, out KeyValuePair<TKey, TValue> data)
    {
      Node rightNode = null, rightNodeNext = null, leftNode = null;
      data = default(KeyValuePair<TKey, TValue>);
      Node markedNode = null;

      do {
        rightNode = ListSearch(key, subKey, ref leftNode, startPoint);
        if (rightNode == tail || rightNode.Key != key || !comparer.Equals(subKey, rightNode.SubKey))
          return false;

        data = rightNode.Data;
        rightNodeNext = (Node)rightNode.Next;

        if (!rightNodeNext.Marked) {
          if (markedNode == null)
            markedNode = new Node();
          markedNode.Init(rightNodeNext);

          if (ClientInterlocked.CompareExchange(ref rightNode.Next, markedNode, rightNodeNext) == rightNodeNext)
            break;
        }
      } while (true);

      if (ClientInterlocked.CompareExchange(ref leftNode.Next, rightNodeNext, rightNode) != rightNode)
        ListSearch(rightNode.Key, subKey, ref leftNode, startPoint);

      return true;
    }

    bool ListInsert(Node newNode, Node startPoint, out Node current)
    {
      ulong key = newNode.Key;
      Node rightNode = null, leftNode = null;

      do {
        rightNode = current = ListSearch(key, newNode.SubKey, ref leftNode, startPoint);
        if (rightNode != tail && rightNode.Key == key && comparer.Equals(newNode.SubKey, rightNode.SubKey))
          return false;

        newNode.Next = rightNode;
        if (ClientInterlocked.CompareExchange(ref leftNode.Next, newNode, rightNode) == rightNode)
          return true;
      } while (true);
    }

    bool ListFind(ulong key, TKey subKey, Node startPoint, out Node data)
    {
      Node rightNode = null, leftNode = null;
      data = null;

      rightNode = ListSearch(key, subKey, ref leftNode, startPoint);
      data = rightNode;

      return rightNode != tail && rightNode.Key == key && comparer.Equals(subKey, rightNode.SubKey);
    }

    static readonly byte[] reverseTable = {
			0, 128, 64, 192, 32, 160, 96, 224, 16, 144, 80, 208, 48, 176, 112, 240, 8, 136, 72, 200, 40, 168, 104, 232, 24, 152, 88, 216, 56, 184, 120, 248, 4, 132, 68, 196, 36, 164, 100, 228, 20, 148, 84, 212, 52, 180, 116, 244, 12, 140, 76, 204, 44, 172, 108, 236, 28, 156, 92, 220, 60, 188, 124, 252, 2, 130, 66, 194, 34, 162, 98, 226, 18, 146, 82, 210, 50, 178, 114, 242, 10, 138, 74, 202, 42, 170, 106, 234, 26, 154, 90, 218, 58, 186, 122, 250, 6, 134, 70, 198, 38, 166, 102, 230, 22, 150, 86, 214, 54, 182, 118, 246, 14, 142, 78, 206, 46, 174, 110, 238, 30, 158, 94, 222, 62, 190, 126, 254, 1, 129, 65, 193, 33, 161, 97, 225, 17, 145, 81, 209, 49, 177, 113, 241, 9, 137, 73, 201, 41, 169, 105, 233, 25, 153, 89, 217, 57, 185, 121, 249, 5, 133, 69, 197, 37, 165, 101, 229, 21, 149, 85, 213, 53, 181, 117, 245, 13, 141, 77, 205, 45, 173, 109, 237, 29, 157, 93, 221, 61, 189, 125, 253, 3, 131, 67, 195, 35, 163, 99, 227, 19, 147, 83, 211, 51, 179, 115, 243, 11, 139, 75, 203, 43, 171, 107, 235, 27, 155, 91, 219, 59, 187, 123, 251, 7, 135, 71, 199, 39, 167, 103, 231, 23, 151, 87, 215, 55, 183, 119, 247, 15, 143, 79, 207, 47, 175, 111, 239, 31, 159, 95, 223, 63, 191, 127, 255
		};

    static readonly byte[] logTable = {
			0xFF, 0, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7
		};
  }
}
