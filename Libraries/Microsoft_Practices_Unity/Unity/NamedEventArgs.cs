// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.NamedEventArgs
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;

namespace Microsoft.Practices.Unity
{
  public abstract class NamedEventArgs : EventArgs
  {
    private string name;

    protected NamedEventArgs()
    {
    }

    protected NamedEventArgs(string name) => this.name = name;

    public virtual string Name
    {
      get => this.name;
      set => this.name = value;
    }
  }
}
