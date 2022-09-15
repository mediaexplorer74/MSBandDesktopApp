// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Plugins.MvxPluginManager
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cirrious.CrossCore.Plugins
{
  public abstract class MvxPluginManager : IMvxPluginManager
  {
    private readonly Dictionary<Type, IMvxPlugin> _loadedPlugins = new Dictionary<Type, IMvxPlugin>();

    public Func<Type, IMvxPluginConfiguration> ConfigurationSource { get; set; }

    public bool IsPluginLoaded<T>() where T : IMvxPluginLoader
    {
      lock (this)
        return this._loadedPlugins.ContainsKey(typeof (T));
    }

    public void EnsurePluginLoaded<TType>() => this.EnsurePluginLoaded(typeof (TType));

    public virtual void EnsurePluginLoaded(Type type)
    {
      FieldInfo field = type.GetField("Instance", Cirrious.CrossCore.BindingFlags.Public | Cirrious.CrossCore.BindingFlags.Static);
      if ((object) field == null)
      {
        MvxTrace.Trace("Plugin Instance not found - will not autoload {0}", (object) type.FullName);
      }
      else
      {
        object obj = field.GetValue((object) null);
        if (obj == null)
          MvxTrace.Trace("Plugin Instance was empty - will not autoload {0}", (object) type.FullName);
        else if (!(obj is IMvxPluginLoader pluginLoader4))
          MvxTrace.Trace("Plugin Instance was not a loader - will not autoload {0}", (object) type.FullName);
        else
          this.EnsurePluginLoaded(pluginLoader4);
      }
    }

    protected virtual void EnsurePluginLoaded(IMvxPluginLoader pluginLoader)
    {
      if (pluginLoader is IMvxConfigurablePluginLoader configurablePluginLoader)
      {
        MvxTrace.Trace("Configuring Plugin Loader for {0}", (object) pluginLoader.GetType().FullName);
        IMvxPluginConfiguration configuration = this.ConfigurationFor(pluginLoader.GetType());
        configurablePluginLoader.Configure(configuration);
      }
      MvxTrace.Trace("Ensuring Plugin is loaded for {0}", (object) pluginLoader.GetType().FullName);
      pluginLoader.EnsureLoaded();
    }

    public void EnsurePlatformAdaptionLoaded<T>() where T : IMvxPluginLoader
    {
      lock (this)
      {
        if (this.IsPluginLoaded<T>())
          return;
        Type type = typeof (T);
        this._loadedPlugins[type] = this.ExceptionWrappedLoadPlugin(type);
      }
    }

    public bool TryEnsurePlatformAdaptionLoaded<T>() where T : IMvxPluginLoader
    {
      lock (this)
      {
        if (this.IsPluginLoaded<T>())
          return true;
        try
        {
          Type type = typeof (T);
          this._loadedPlugins[type] = this.ExceptionWrappedLoadPlugin(type);
          return true;
        }
        catch (Exception ex)
        {
          Mvx.Warning("Failed to load plugin adaption {0} with exception {1}", (object) typeof (T).FullName, (object) ex.ToLongString());
          return false;
        }
      }
    }

    private IMvxPlugin ExceptionWrappedLoadPlugin(Type toLoad)
    {
      try
      {
        IMvxPlugin plugin = this.FindPlugin(toLoad);
        if (plugin is IMvxConfigurablePlugin configurablePlugin2)
        {
          IMvxPluginConfiguration configuration = this.ConfigurationSource(configurablePlugin2.GetType());
          configurablePlugin2.Configure(configuration);
        }
        plugin.Load();
        return plugin;
      }
      catch (MvxException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw ex.MvxWrap();
      }
    }

    protected IMvxPluginConfiguration ConfigurationFor(Type toLoad) => this.ConfigurationSource == null ? (IMvxPluginConfiguration) null : this.ConfigurationSource(toLoad);

    protected abstract IMvxPlugin FindPlugin(Type toLoad);
  }
}
