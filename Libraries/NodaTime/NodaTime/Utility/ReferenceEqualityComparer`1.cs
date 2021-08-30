// Decompiled with JetBrains decompiler
// Type: NodaTime.Utility.ReferenceEqualityComparer`1
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NodaTime.Utility
{
  internal sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
  {
    public bool Equals(T first, T second) => object.ReferenceEquals((object) first, (object) second);

    public int GetHashCode(T value) => !object.ReferenceEquals((object) value, (object) null) ? RuntimeHelpers.GetHashCode((object) value) : 0;
  }
}
