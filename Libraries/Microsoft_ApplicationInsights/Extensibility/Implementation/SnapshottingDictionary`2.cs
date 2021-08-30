// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.SnapshottingDictionary`2
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.Collections;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  internal class SnapshottingDictionary<TKey, TValue> : 
    SnapshottingCollection<KeyValuePair<TKey, TValue>, IDictionary<TKey, TValue>>,
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable
  {
    public SnapshottingDictionary()
      : base((IDictionary<TKey, TValue>) new Dictionary<TKey, TValue>())
    {
    }

    public ICollection<TKey> Keys => this.GetSnapshot().Keys;

    public ICollection<TValue> Values => this.GetSnapshot().Values;

    public TValue this[TKey key]
    {
      get => this.GetSnapshot()[key];
      set
      {
        lock (this.Collection)
        {
          this.Collection[key] = value;
          this.snapshot = (IDictionary<TKey, TValue>) null;
        }
      }
    }

    public void Add(TKey key, TValue value)
    {
      lock (this.Collection)
      {
        this.Collection.Add(key, value);
        this.snapshot = (IDictionary<TKey, TValue>) null;
      }
    }

    public bool ContainsKey(TKey key) => this.GetSnapshot().ContainsKey(key);

    public bool Remove(TKey key)
    {
      lock (this.Collection)
      {
        bool flag = this.Collection.Remove(key);
        if (flag)
          this.snapshot = (IDictionary<TKey, TValue>) null;
        return flag;
      }
    }

    public bool TryGetValue(TKey key, out TValue value) => this.GetSnapshot().TryGetValue(key, out value);

    protected override sealed IDictionary<TKey, TValue> CreateSnapshot(
      IDictionary<TKey, TValue> collection)
    {
      return (IDictionary<TKey, TValue>) new Dictionary<TKey, TValue>(collection);
    }
  }
}
