// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.BindingFlags
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;

namespace Cirrious.CrossCore
{
  [Flags]
  public enum BindingFlags
  {
    None = 0,
    Instance = 1,
    Public = 2,
    Static = 4,
    FlattenHierarchy = 8,
    SetProperty = 8192, // 0x00002000
  }
}
