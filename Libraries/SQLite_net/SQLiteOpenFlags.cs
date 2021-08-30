// Decompiled with JetBrains decompiler
// Type: SQLite.SQLiteOpenFlags
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using System;

namespace SQLite
{
  [Flags]
  public enum SQLiteOpenFlags
  {
    ReadOnly = 1,
    ReadWrite = 2,
    Create = 4,
    NoMutex = 32768, // 0x00008000
    FullMutex = 65536, // 0x00010000
    SharedCache = 131072, // 0x00020000
    PrivateCache = 262144, // 0x00040000
    ProtectionComplete = 1048576, // 0x00100000
    ProtectionCompleteUnlessOpen = 2097152, // 0x00200000
    ProtectionCompleteUntilFirstUserAuthentication = ProtectionCompleteUnlessOpen | ProtectionComplete, // 0x00300000
    ProtectionNone = 4194304, // 0x00400000
  }
}
