// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.EqualityComparer.EqualsOverride`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Utilities.EqualityComparer
{
  public class EqualsOverride<T> : IEqualityComparer<T>
  {
    private readonly Func<T, T, bool> equalOverrideFunction;

    public EqualsOverride(Func<T, T, bool> equalOverrideFunction) => this.equalOverrideFunction = equalOverrideFunction != null ? equalOverrideFunction : throw new ArgumentNullException(nameof (equalOverrideFunction));

    public bool Equals(T instance1, T instance2) => this.equalOverrideFunction(instance1, instance2);

    public int GetHashCode(T instance) => System.Collections.Generic.EqualityComparer<T>.Default.GetHashCode(instance);
  }
}
