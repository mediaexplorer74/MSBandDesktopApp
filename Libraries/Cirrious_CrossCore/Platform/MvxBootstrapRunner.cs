// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Platform.MvxBootstrapRunner
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using System;
using System.Reflection;

namespace Cirrious.CrossCore.Platform
{
  public class MvxBootstrapRunner
  {
    public virtual void Run(Assembly assembly)
    {
      foreach (Type inherit in assembly.CreatableTypes().Inherits<IMvxBootstrapAction>())
        this.Run(inherit);
    }

    public virtual void Run(Type type)
    {
      try
      {
        if (!(Activator.CreateInstance(type) is IMvxBootstrapAction instance2))
          Mvx.Warning("Could not run startup task {0} - it's not a startup task", (object) type.Name);
        else
          instance2.Run();
      }
      catch (Exception ex)
      {
        Mvx.Warning("Error running startup task {0} - error {1}", (object) type.Name, (object) ex.ToLongString());
      }
    }
  }
}
