// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Plugins.MvxLoaderPluginRegistry
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Exceptions;
using System;
using System.Collections.Generic;

namespace Cirrious.CrossCore.Plugins
{
  public class MvxLoaderPluginRegistry
  {
    private readonly string _pluginPostfix;
    private readonly IDictionary<string, Func<IMvxPlugin>> _loaders;

    public MvxLoaderPluginRegistry(
      string expectedPostfix,
      IDictionary<string, Func<IMvxPlugin>> loaders)
    {
      this._pluginPostfix = expectedPostfix;
      this._loaders = loaders;
    }

    public void AddUnconventionalPlugin(string pluginName, Func<IMvxPlugin> loader) => this._loaders[pluginName] = loader;

    public void AddConventionalPlugin<TPlugin>() where TPlugin : IMvxPlugin => this.AddConventionalPlugin(typeof (TPlugin));

    public void AddConventionalPlugin(Type plugin)
    {
      string str = plugin.Namespace ?? string.Empty;
      if (!str.EndsWith(this._pluginPostfix))
        throw new MvxException("You must pass in the type of a plugin instance - like 'typeof(Cirrious.MvvmCross.Plugins.Visibility{0}.Plugin)'", new object[1]
        {
          (object) this._pluginPostfix
        });
      this._loaders.Add(str.Substring(0, str.Length - this._pluginPostfix.Length), (Func<IMvxPlugin>) (() => (IMvxPlugin) Activator.CreateInstance(plugin)));
    }
  }
}
