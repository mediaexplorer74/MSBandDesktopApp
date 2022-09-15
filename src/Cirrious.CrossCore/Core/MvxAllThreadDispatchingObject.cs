// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Core.MvxAllThreadDispatchingObject
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;

namespace Cirrious.CrossCore.Core
{
  public abstract class MvxAllThreadDispatchingObject : MvxMainThreadDispatchingObject
  {
    private readonly object _lockObject = new object();

    protected void RunSyncWithLock(Action action) => MvxLockableObjectHelpers.RunSyncWithLock(this._lockObject, action);

    protected void RunAsyncWithLock(Action action) => MvxLockableObjectHelpers.RunAsyncWithLock(this._lockObject, action);

    protected void RunSyncOrAsyncWithLock(Action action, Action whenComplete = null) => MvxLockableObjectHelpers.RunSyncOrAsyncWithLock(this._lockObject, action, whenComplete);
  }
}
