// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.ILifetimeContainer
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder2
{
  public interface ILifetimeContainer : IEnumerable<object>, IEnumerable, IDisposable
  {
    int Count { get; }

    void Add(object item);

    bool Contains(object item);

    void Remove(object item);
  }
}
