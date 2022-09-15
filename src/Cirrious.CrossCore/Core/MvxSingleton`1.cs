// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Core.MvxSingleton`1
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Exceptions;

namespace Cirrious.CrossCore.Core
{
  public abstract class MvxSingleton<TInterface> : MvxSingleton where TInterface : class
  {
    protected MvxSingleton() => MvxSingleton<TInterface>.Instance = (object) MvxSingleton<TInterface>.Instance == null ? this as TInterface : throw new MvxException("You cannot create more than one instance of MvxSingleton");

    public static TInterface Instance { get; private set; }

    protected override void Dispose(bool isDisposing)
    {
      if (!isDisposing)
        return;
      MvxSingleton<TInterface>.Instance = default (TInterface);
    }
  }
}
