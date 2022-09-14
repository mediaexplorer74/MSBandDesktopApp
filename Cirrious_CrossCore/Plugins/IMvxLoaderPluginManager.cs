// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Plugins.IMvxLoaderPluginManager
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.Collections.Generic;

namespace Cirrious.CrossCore.Plugins
{
  public interface IMvxLoaderPluginManager : IMvxPluginManager
  {
    IDictionary<string, Func<IMvxPlugin>> Finders { get; }
  }
}
