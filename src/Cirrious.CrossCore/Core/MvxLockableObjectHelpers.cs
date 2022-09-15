// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Core.MvxLockableObjectHelpers
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.Threading;

namespace Cirrious.CrossCore.Core
{
  public static class MvxLockableObjectHelpers
  {
    public static void RunSyncWithLock(object lockObject, Action action)
    {
      lock (lockObject)
        action();
    }

    public static void RunAsyncWithLock(object lockObject, Action action) => MvxAsyncDispatcher.BeginAsync((Action) (() =>
    {
      lock (lockObject)
        action();
    }));

    public static void RunSyncOrAsyncWithLock(
      object lockObject,
      Action action,
      Action whenComplete = null)
    {
      if (Monitor.TryEnter(lockObject))
      {
        try
        {
          action();
        }
        finally
        {
          Monitor.Exit(lockObject);
        }
        if (whenComplete == null)
          return;
        whenComplete();
      }
      else
        MvxAsyncDispatcher.BeginAsync((Action) (() =>
        {
          lock (lockObject)
            action();
          if (whenComplete == null)
            return;
          whenComplete();
        }));
    }
  }
}
