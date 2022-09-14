// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Plugins.MvxLoaderPluginBootstrapAction`2
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;

namespace Cirrious.CrossCore.Plugins
{
  public class MvxLoaderPluginBootstrapAction<TPlugin, TPlatformPlugin> : 
    MvxPluginBootstrapAction<TPlugin>
    where TPlugin : IMvxPluginLoader
    where TPlatformPlugin : IMvxPlugin
  {
    protected override void Load(IMvxPluginManager manager)
    {
      this.PreLoad(manager);
      base.Load(manager);
    }

    protected virtual void PreLoad(IMvxPluginManager manager)
    {
      if (!(manager is IMvxLoaderPluginManager loaderPluginManager))
      {
        Mvx.Warning("You should not register a loader plugin bootstrap action when using a non-loader plugin manager");
      }
      else
      {
        string key = typeof (TPlugin).Namespace;
        if (string.IsNullOrEmpty(key))
          Mvx.Warning("Unable to find namespace for {0} - skipping", (object) typeof (TPlugin).Name);
        else
          loaderPluginManager.Finders[key] = (Func<IMvxPlugin>) (() => (IMvxPlugin) Activator.CreateInstance<TPlatformPlugin>());
      }
    }
  }
}
