// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Plugins.MvxLoaderPluginManager
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Exceptions;
using System;
using System.Collections.Generic;

namespace Cirrious.CrossCore.Plugins
{
  public class MvxLoaderPluginManager : MvxPluginManager, IMvxLoaderPluginManager, IMvxPluginManager
  {
    private readonly Dictionary<string, Func<IMvxPlugin>> _finders = new Dictionary<string, Func<IMvxPlugin>>();

    public IDictionary<string, Func<IMvxPlugin>> Finders => (IDictionary<string, Func<IMvxPlugin>>) this._finders;

    protected override IMvxPlugin FindPlugin(Type toLoad)
    {
      string key = toLoad.Namespace;
      if (string.IsNullOrEmpty(key))
        throw new MvxException("Invalid plugin type {0}", new object[1]
        {
          (object) toLoad
        });
      Func<IMvxPlugin> func;
      if (!this._finders.TryGetValue(key, out func))
        throw new MvxException("plugin not registered for type {0}", new object[1]
        {
          (object) key
        });
      return func();
    }
  }
}
