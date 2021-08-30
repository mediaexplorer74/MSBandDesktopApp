// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ThreadSafeStore`2
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
  internal class ThreadSafeStore<TKey, TValue>
  {
    private readonly object _lock = new object();
    private Dictionary<TKey, TValue> _store;
    private readonly Func<TKey, TValue> _creator;

    public ThreadSafeStore(Func<TKey, TValue> creator)
    {
      this._creator = creator != null ? creator : throw new ArgumentNullException(nameof (creator));
      this._store = new Dictionary<TKey, TValue>();
    }

    public TValue Get(TKey key)
    {
      TValue obj;
      return !this._store.TryGetValue(key, out obj) ? this.AddValue(key) : obj;
    }

    private TValue AddValue(TKey key)
    {
      TValue obj1 = this._creator(key);
      lock (this._lock)
      {
        if (this._store == null)
        {
          this._store = new Dictionary<TKey, TValue>();
          this._store[key] = obj1;
        }
        else
        {
          TValue obj2;
          if (this._store.TryGetValue(key, out obj2))
            return obj2;
          Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>((IDictionary<TKey, TValue>) this._store);
          dictionary[key] = obj1;
          Thread.MemoryBarrier();
          this._store = dictionary;
        }
        return obj1;
      }
    }
  }
}
