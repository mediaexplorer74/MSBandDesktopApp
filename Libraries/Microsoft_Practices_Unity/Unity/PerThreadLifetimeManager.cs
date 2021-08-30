// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.PerThreadLifetimeManager
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Practices.Unity
{
  public class PerThreadLifetimeManager : LifetimeManager
  {
    [ThreadStatic]
    private static Dictionary<Guid, object> values;
    private readonly Guid key;

    public PerThreadLifetimeManager() => this.key = Guid.NewGuid();

    public override object GetValue()
    {
      PerThreadLifetimeManager.EnsureValues();
      object obj;
      PerThreadLifetimeManager.values.TryGetValue(this.key, out obj);
      return obj;
    }

    public override void SetValue(object newValue)
    {
      PerThreadLifetimeManager.EnsureValues();
      PerThreadLifetimeManager.values[this.key] = newValue;
    }

    public override void RemoveValue()
    {
    }

    private static void EnsureValues()
    {
      if (PerThreadLifetimeManager.values != null)
        return;
      PerThreadLifetimeManager.values = new Dictionary<Guid, object>();
    }
  }
}
