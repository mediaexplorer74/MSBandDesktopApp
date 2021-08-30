// Decompiled with JetBrains decompiler
// Type: SQLite.CreateTablesResult
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using System;
using System.Collections.Generic;

namespace SQLite
{
  public class CreateTablesResult
  {
    internal CreateTablesResult() => this.Results = new Dictionary<Type, int>();

    public Dictionary<Type, int> Results { get; private set; }
  }
}
