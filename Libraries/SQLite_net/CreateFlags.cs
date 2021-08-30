// Decompiled with JetBrains decompiler
// Type: SQLite.CreateFlags
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using System;

namespace SQLite
{
  [Flags]
  public enum CreateFlags
  {
    None = 0,
    ImplicitPK = 1,
    ImplicitIndex = 2,
    AllImplicit = ImplicitIndex | ImplicitPK, // 0x00000003
    AutoIncPK = 4,
    FullTextSearch3 = 256, // 0x00000100
    FullTextSearch4 = 512, // 0x00000200
  }
}
