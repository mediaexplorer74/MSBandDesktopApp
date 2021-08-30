// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.SnapshottingCollection`2
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  internal abstract class SnapshottingCollection<TItem, TCollection> : 
    ICollection<TItem>,
    IEnumerable<TItem>,
    IEnumerable
    where TCollection : class, ICollection<TItem>
  {
    protected readonly TCollection Collection;
    protected TCollection snapshot;

    protected SnapshottingCollection(TCollection collection) => this.Collection = collection;

    public int Count => this.GetSnapshot().Count;

    public bool IsReadOnly => false;

    public void Add(TItem item)
    {
      bool lockTaken = false;
      object collection;
      try
      {
        Monitor.Enter((object) (__Boxed<TCollection>) (collection = (object) this.Collection), ref lockTaken);
        this.Collection.Add(item);
        this.snapshot = default (TCollection);
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(collection);
      }
    }

    public void Clear()
    {
      bool lockTaken = false;
      object collection;
      try
      {
        Monitor.Enter((object) (__Boxed<TCollection>) (collection = (object) this.Collection), ref lockTaken);
        this.Collection.Clear();
        this.snapshot = default (TCollection);
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(collection);
      }
    }

    public bool Contains(TItem item) => this.GetSnapshot().Contains(item);

    public void CopyTo(TItem[] array, int arrayIndex) => this.GetSnapshot().CopyTo(array, arrayIndex);

    public bool Remove(TItem item)
    {
      bool lockTaken = false;
      object collection;
      try
      {
        Monitor.Enter((object) (__Boxed<TCollection>) (collection = (object) this.Collection), ref lockTaken);
        bool flag = this.Collection.Remove(item);
        if (flag)
          this.snapshot = default (TCollection);
        return flag;
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(collection);
      }
    }

    public IEnumerator<TItem> GetEnumerator() => this.GetSnapshot().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    protected abstract TCollection CreateSnapshot(TCollection collection);

    protected TCollection GetSnapshot()
    {
      TCollection snapshot = this.snapshot;
      if ((object) snapshot == null)
      {
        bool lockTaken = false;
        object collection;
        try
        {
          Monitor.Enter((object) (__Boxed<TCollection>) (collection = (object) this.Collection), ref lockTaken);
          this.snapshot = this.CreateSnapshot(this.Collection);
          snapshot = this.snapshot;
        }
        finally
        {
          if (lockTaken)
            Monitor.Exit(collection);
        }
      }
      return snapshot;
    }
  }
}
