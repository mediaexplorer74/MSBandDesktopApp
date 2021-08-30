// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ContainerControlledLifetimeManager
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;

namespace Microsoft.Practices.Unity
{
  public class ContainerControlledLifetimeManager : SynchronizedLifetimeManager, IDisposable
  {
    private object value;

    protected override object SynchronizedGetValue() => this.value;

    protected override void SynchronizedSetValue(object newValue) => this.value = newValue;

    public override void RemoveValue() => this.Dispose();

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.value == null)
        return;
      if (this.value is IDisposable)
        ((IDisposable) this.value).Dispose();
      this.value = (object) null;
    }
  }
}
