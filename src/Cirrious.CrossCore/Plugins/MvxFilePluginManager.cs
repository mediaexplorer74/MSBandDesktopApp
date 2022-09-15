// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Plugins.MvxFilePluginManager
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.CrossCore.Plugins
{
  public class MvxFilePluginManager : MvxPluginManager
  {
    private readonly List<string> _platformDllPostfixes;
    private readonly string _assemblyExtension;

    public MvxFilePluginManager(string platformDllPostfix, string assemblyExtension = "")
    {
      this._platformDllPostfixes = new List<string>()
      {
        platformDllPostfix
      };
      this._assemblyExtension = assemblyExtension;
    }

    public MvxFilePluginManager(List<string> platformDllPostfixes, string assemblyExtension = "")
    {
      this._platformDllPostfixes = platformDllPostfixes;
      this._assemblyExtension = assemblyExtension;
    }

    protected override IMvxPlugin FindPlugin(Type toLoad) => (IMvxPlugin) Activator.CreateInstance(this.LoadAssembly(toLoad).ExceptionSafeGetTypes().FirstOrDefault<Type>((Func<Type, bool>) (x => ReflectionExtensions.IsAssignableFrom(typeof (IMvxPlugin), x))) ?? throw new MvxException("Could not find plugin type in assembly"));

    protected virtual Assembly LoadAssembly(Type toLoad)
    {
      foreach (string platformDllPostfix in this._platformDllPostfixes)
      {
        string assemblyNameFrom = this.GetPluginAssemblyNameFrom(toLoad, platformDllPostfix);
        MvxTrace.Trace("Loading plugin assembly: {0}", (object) assemblyNameFrom);
        try
        {
          return Assembly.Load(new AssemblyName(assemblyNameFrom));
        }
        catch (Exception ex)
        {
        }
      }
      throw new MvxException(string.Format("could not load plugin assembly for type {0}", new object[1]
      {
        (object) toLoad
      }));
    }

    protected virtual string GetPluginAssemblyNameFrom(Type toLoad, string platformDllPostfix) => string.Format("{0}{1}{2}", new object[3]
    {
      (object) toLoad.Namespace,
      (object) platformDllPostfix,
      (object) this._assemblyExtension
    });
  }
}
