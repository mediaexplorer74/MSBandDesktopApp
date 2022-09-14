// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Core.MvxMainThreadDispatcher
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using System;
using System.Reflection;

namespace Cirrious.CrossCore.Core
{
  public abstract class MvxMainThreadDispatcher : MvxSingleton<IMvxMainThreadDispatcher>
  {
    protected static void ExceptionMaskedAction(Action action)
    {
      try
      {
        action();
      }
      catch (TargetInvocationException ex)
      {
        MvxTrace.Trace("TargetInvocateException masked " + ex.InnerException.ToLongString());
      }
      catch (Exception ex)
      {
        MvxTrace.Warning("Exception masked " + ex.ToLongString());
      }
    }
  }
}
