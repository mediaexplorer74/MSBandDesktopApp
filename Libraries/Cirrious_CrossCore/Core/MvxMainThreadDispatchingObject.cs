// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Core.MvxMainThreadDispatchingObject
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;

namespace Cirrious.CrossCore.Core
{
  public abstract class MvxMainThreadDispatchingObject
  {
    protected IMvxMainThreadDispatcher Dispatcher => MvxSingleton<IMvxMainThreadDispatcher>.Instance;

    protected void InvokeOnMainThread(Action action)
    {
      if (this.Dispatcher == null)
        return;
      this.Dispatcher.RequestMainThreadAction(action);
    }
  }
}
