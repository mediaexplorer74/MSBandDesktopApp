// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Core.MvxSingleton
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.Collections.Generic;

namespace Cirrious.CrossCore.Core
{
  public abstract class MvxSingleton : IDisposable
  {
    private static readonly List<MvxSingleton> Singletons = new List<MvxSingleton>();

    ~MvxSingleton() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected abstract void Dispose(bool isDisposing);

    protected MvxSingleton()
    {
      lock (MvxSingleton.Singletons)
        MvxSingleton.Singletons.Add(this);
    }

    public static void ClearAllSingletons()
    {
      lock (MvxSingleton.Singletons)
      {
        foreach (MvxSingleton singleton in MvxSingleton.Singletons)
          singleton.Dispose();
        MvxSingleton.Singletons.Clear();
      }
    }
  }
}
