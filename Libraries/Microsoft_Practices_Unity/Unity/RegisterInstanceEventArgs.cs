// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.RegisterInstanceEventArgs
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;

namespace Microsoft.Practices.Unity
{
  public class RegisterInstanceEventArgs : NamedEventArgs
  {
    private Type registeredType;
    private object instance;
    private LifetimeManager lifetimeManager;

    public RegisterInstanceEventArgs()
    {
    }

    public RegisterInstanceEventArgs(
      Type registeredType,
      object instance,
      string name,
      LifetimeManager lifetimeManager)
      : base(name)
    {
      this.registeredType = registeredType;
      this.instance = instance;
      this.lifetimeManager = lifetimeManager;
    }

    public Type RegisteredType
    {
      get => this.registeredType;
      set => this.registeredType = value;
    }

    public object Instance
    {
      get => this.instance;
      set => this.instance = value;
    }

    public LifetimeManager LifetimeManager
    {
      get => this.lifetimeManager;
      set => this.lifetimeManager = value;
    }
  }
}
