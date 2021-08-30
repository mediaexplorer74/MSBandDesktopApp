// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.LifetimeContainer
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class LifetimeContainer : ILifetimeContainer, IEnumerable<object>, IEnumerable, IDisposable
  {
    private readonly List<object> items = new List<object>();

    public int Count
    {
      get
      {
        lock (this.items)
          return this.items.Count;
      }
    }

    public void Add(object item)
    {
      lock (this.items)
        this.items.Add(item);
    }

    public bool Contains(object item)
    {
      lock (this.items)
        return this.items.Contains(item);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      lock (this.items)
      {
        List<object> objectList = new List<object>((IEnumerable<object>) this.items);
        objectList.Reverse();
        foreach (object obj in objectList)
        {
          if (obj is IDisposable disposable3)
            disposable3.Dispose();
        }
        this.items.Clear();
      }
    }

    public IEnumerator<object> GetEnumerator() => (IEnumerator<object>) this.items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public void Remove(object item)
    {
      lock (this.items)
      {
        if (!this.items.Contains(item))
          return;
        this.items.Remove(item);
      }
    }
  }
}
