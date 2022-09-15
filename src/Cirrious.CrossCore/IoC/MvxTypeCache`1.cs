// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.IoC.MvxTypeCache`1
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.CrossCore.IoC
{
  public class MvxTypeCache<TType> : IMvxTypeCache<TType>
  {
    public Dictionary<string, Type> LowerCaseFullNameCache { get; private set; }

    public Dictionary<string, Type> FullNameCache { get; private set; }

    public Dictionary<string, Type> NameCache { get; private set; }

    public Dictionary<Assembly, bool> CachedAssemblies { get; private set; }

    public MvxTypeCache()
    {
      this.LowerCaseFullNameCache = new Dictionary<string, Type>();
      this.FullNameCache = new Dictionary<string, Type>();
      this.NameCache = new Dictionary<string, Type>();
      this.CachedAssemblies = new Dictionary<Assembly, bool>();
    }

    public void AddAssembly(Assembly assembly)
    {
      if (this.CachedAssemblies.ContainsKey(assembly))
        return;
      Type viewType = typeof (TType);
      foreach (Type type in assembly.ExceptionSafeGetTypes().Where<Type>((Func<Type, bool>) (type => ReflectionExtensions.IsAssignableFrom(viewType, type))))
      {
        if (!string.IsNullOrEmpty(type.FullName))
        {
          this.FullNameCache[type.FullName] = type;
          this.LowerCaseFullNameCache[type.FullName.ToLowerInvariant()] = type;
        }
        if (!string.IsNullOrEmpty(type.Name))
          this.NameCache[type.Name] = type;
      }
      this.CachedAssemblies[assembly] = true;
    }
  }
}
