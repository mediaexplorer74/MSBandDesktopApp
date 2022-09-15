// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Plugins.IMvxPluginManager
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;

namespace Cirrious.CrossCore.Plugins
{
  public interface IMvxPluginManager
  {
    Func<Type, IMvxPluginConfiguration> ConfigurationSource { get; set; }

    bool IsPluginLoaded<T>() where T : IMvxPluginLoader;

    void EnsurePluginLoaded<TType>();

    void EnsurePluginLoaded(Type type);

    void EnsurePlatformAdaptionLoaded<T>() where T : IMvxPluginLoader;

    bool TryEnsurePlatformAdaptionLoaded<T>() where T : IMvxPluginLoader;
  }
}
