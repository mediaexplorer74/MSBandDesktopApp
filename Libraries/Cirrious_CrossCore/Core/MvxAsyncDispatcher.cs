// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Core.MvxAsyncDispatcher
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.Threading.Tasks;

namespace Cirrious.CrossCore.Core
{
  public static class MvxAsyncDispatcher
  {
    public static void BeginAsync(Action action) => Task.Run((Action) (() => action()));

    public static void BeginAsync(Action<object> action, object state) => Task.Run((Action) (() => action(state)));
  }
}
