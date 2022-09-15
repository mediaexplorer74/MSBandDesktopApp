// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Core.MvxApplicableExtensions
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System.Collections.Generic;

namespace Cirrious.CrossCore.Core
{
  public static class MvxApplicableExtensions
  {
    public static void Apply(this IEnumerable<IMvxApplicable> toApply)
    {
      foreach (IMvxApplicable mvxApplicable in toApply)
        mvxApplicable.Apply();
    }

    public static void ApplyTo(this IEnumerable<IMvxApplicableTo> toApply, object what)
    {
      foreach (IMvxApplicableTo mvxApplicableTo in toApply)
        mvxApplicableTo.ApplyTo(what);
    }

    public static void ApplyTo<T>(this IEnumerable<IMvxApplicableTo<T>> toApply, T what)
    {
      foreach (IMvxApplicableTo<T> mvxApplicableTo in toApply)
        mvxApplicableTo.ApplyTo(what);
    }
  }
}
