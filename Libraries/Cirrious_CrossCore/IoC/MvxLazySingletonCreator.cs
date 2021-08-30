// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.IoC.MvxLazySingletonCreator
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;

namespace Cirrious.CrossCore.IoC
{
  public class MvxLazySingletonCreator
  {
    private readonly object _lockObject = new object();
    private readonly Type _type;
    private object _instance;

    public object Instance
    {
      get
      {
        if (this._instance != null)
          return this._instance;
        lock (this._lockObject)
        {
          this._instance = this._instance ?? Mvx.IocConstruct(this._type);
          return this._instance;
        }
      }
    }

    public MvxLazySingletonCreator(Type type) => this._type = type;
  }
}
