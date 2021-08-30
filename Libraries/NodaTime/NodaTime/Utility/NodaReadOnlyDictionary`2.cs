// Decompiled with JetBrains decompiler
// Type: NodaTime.Utility.NodaReadOnlyDictionary`2
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace NodaTime.Utility
{
  internal sealed class NodaReadOnlyDictionary<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable
  {
    private readonly IDictionary<TKey, TValue> original;

    internal NodaReadOnlyDictionary(IDictionary<TKey, TValue> original) => this.original = Preconditions.CheckNotNull<IDictionary<TKey, TValue>>(original, nameof (original));

    public bool ContainsKey(TKey key) => this.original.ContainsKey(key);

    public ICollection<TKey> Keys => this.original.Keys;

    public bool TryGetValue(TKey key, out TValue value) => this.original.TryGetValue(key, out value);

    public ICollection<TValue> Values => this.original.Values;

    public TValue this[TKey key]
    {
      get => this.original[key];
      set => throw new NotSupportedException("Cannot set a value in a read-only dictionary");
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => this.original.Contains(item);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => this.original.CopyTo(array, arrayIndex);

    public int Count => this.original.Count;

    public bool IsReadOnly => true;

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this.original.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => throw new NotSupportedException("Cannot add to a read-only dictionary");

    public void Add(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException("Cannot add to a read-only dictionary");

    bool IDictionary<TKey, TValue>.Remove(TKey key) => throw new NotSupportedException("Cannot remove from a read-only dictionary");

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(
      KeyValuePair<TKey, TValue> item)
    {
      throw new NotSupportedException("Cannot remove from a read-only dictionary");
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Clear() => throw new NotSupportedException("Cannot clear a read-only dictionary");
  }
}
