// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.RegisterEventArgs
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;

namespace Microsoft.Practices.Unity
{
  public class RegisterEventArgs : NamedEventArgs
  {
    private Type typeFrom;
    private Type typeTo;
    private LifetimeManager lifetimeManager;

    public RegisterEventArgs(
      Type typeFrom,
      Type typeTo,
      string name,
      LifetimeManager lifetimeManager)
      : base(name)
    {
      this.typeFrom = typeFrom;
      this.typeTo = typeTo;
      this.lifetimeManager = lifetimeManager;
    }

    public Type TypeFrom
    {
      get => this.typeFrom;
      set => this.typeFrom = value;
    }

    public Type TypeTo
    {
      get => this.typeTo;
      set => this.typeTo = value;
    }

    public LifetimeManager LifetimeManager
    {
      get => this.lifetimeManager;
      set => this.lifetimeManager = value;
    }
  }
}
