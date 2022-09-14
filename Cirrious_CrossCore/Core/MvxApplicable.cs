// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Core.MvxApplicable
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;

namespace Cirrious.CrossCore.Core
{
  public abstract class MvxApplicable : IMvxApplicable
  {
    private bool _finalizerSuppressed;

    ~MvxApplicable() => Mvx.Trace("Finaliser called on {0} - suggests that  Apply() was never called", (object) this.GetType().Name);

    protected void SuppressFinalizer()
    {
      if (this._finalizerSuppressed)
        return;
      this._finalizerSuppressed = true;
      GC.SuppressFinalize((object) this);
    }

    public virtual void Apply() => this.SuppressFinalizer();
  }
}
