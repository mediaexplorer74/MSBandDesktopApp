// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.SynchronizedLifetimeManager
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using System.Threading;

namespace Microsoft.Practices.Unity
{
  public abstract class SynchronizedLifetimeManager : LifetimeManager, IRequiresRecovery
  {
    private object lockObj = new object();

    public override object GetValue()
    {
      Monitor.Enter(this.lockObj);
      object obj = this.SynchronizedGetValue();
      if (obj != null)
        Monitor.Exit(this.lockObj);
      return obj;
    }

    protected abstract object SynchronizedGetValue();

    public override void SetValue(object newValue)
    {
      this.SynchronizedSetValue(newValue);
      this.TryExit();
    }

    protected abstract void SynchronizedSetValue(object newValue);

    public override void RemoveValue()
    {
    }

    public void Recover() => this.TryExit();

    private void TryExit()
    {
      if (!Monitor.IsEntered(this.lockObj))
        return;
      try
      {
        Monitor.Exit(this.lockObj);
      }
      catch (SynchronizationLockException ex)
      {
      }
    }
  }
}
