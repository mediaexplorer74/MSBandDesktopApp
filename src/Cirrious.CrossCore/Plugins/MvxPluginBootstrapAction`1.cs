// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Plugins.MvxPluginBootstrapAction`1
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Platform;
using System;

namespace Cirrious.CrossCore.Plugins
{
  public class MvxPluginBootstrapAction<TPlugin> : IMvxBootstrapAction
  {
    public virtual void Run() => Mvx.CallbackWhenRegistered<IMvxPluginManager>(new Action(this.RunAction));

    protected virtual void RunAction() => this.Load(Mvx.Resolve<IMvxPluginManager>());

    protected virtual void Load(IMvxPluginManager manager) => manager.EnsurePluginLoaded<TPlugin>();
  }
}
