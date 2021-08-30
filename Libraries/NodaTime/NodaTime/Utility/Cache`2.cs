// Decompiled with JetBrains decompiler
// Type: NodaTime.Utility.Cache`2
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;
using System.Collections.Generic;

namespace NodaTime.Utility
{
  internal sealed class Cache<TKey, TValue>
  {
    private readonly int size;
    private readonly object mutex = new object();
    private readonly Func<TKey, TValue> valueFactory;
    private readonly LinkedList<TKey> keyList;
    private readonly Dictionary<TKey, TValue> dictionary;

    internal Cache(int size, Func<TKey, TValue> valueFactory, IEqualityComparer<TKey> keyComparer)
    {
      this.size = size;
      this.valueFactory = valueFactory;
      this.dictionary = new Dictionary<TKey, TValue>(keyComparer);
      this.keyList = new LinkedList<TKey>();
    }

    internal TValue GetOrAdd(TKey key)
    {
      lock (this.mutex)
      {
        TValue obj;
        if (this.dictionary.TryGetValue(key, out obj))
          return obj;
        if (this.dictionary.Count == this.size)
        {
          TKey key1 = this.keyList.First.Value;
          this.keyList.RemoveFirst();
          this.dictionary.Remove(key1);
        }
        obj = this.valueFactory(key);
        this.keyList.AddLast(key);
        this.dictionary[key] = obj;
        return obj;
      }
    }

    internal int Count
    {
      get
      {
        lock (this.mutex)
          return this.dictionary.Count;
      }
    }

    internal List<TKey> Keys
    {
      get
      {
        lock (this.mutex)
          return new List<TKey>((IEnumerable<TKey>) this.keyList);
      }
    }

    internal void Clear()
    {
      lock (this.mutex)
      {
        this.keyList.Clear();
        this.dictionary.Clear();
      }
    }
  }
}
