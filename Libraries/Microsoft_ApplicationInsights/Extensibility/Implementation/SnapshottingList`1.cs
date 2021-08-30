// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.SnapshottingList`1
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.Collections;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  internal class SnapshottingList<T> : 
    SnapshottingCollection<T, IList<T>>,
    IList<T>,
    ICollection<T>,
    IEnumerable<T>,
    IEnumerable
  {
    public SnapshottingList()
      : base((IList<T>) new List<T>())
    {
    }

    public T this[int index]
    {
      get => this.GetSnapshot()[index];
      set
      {
        lock (this.Collection)
        {
          this.Collection[index] = value;
          this.snapshot = (IList<T>) null;
        }
      }
    }

    public int IndexOf(T item) => this.GetSnapshot().IndexOf(item);

    public void Insert(int index, T item)
    {
      lock (this.Collection)
      {
        this.Collection.Insert(index, item);
        this.snapshot = (IList<T>) null;
      }
    }

    public void RemoveAt(int index)
    {
      lock (this.Collection)
      {
        this.Collection.RemoveAt(index);
        this.snapshot = (IList<T>) null;
      }
    }

    protected override sealed IList<T> CreateSnapshot(IList<T> collection) => (IList<T>) new List<T>((IEnumerable<T>) collection);
  }
}
