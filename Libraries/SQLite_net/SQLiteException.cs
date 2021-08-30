// Decompiled with JetBrains decompiler
// Type: SQLite.SQLiteException
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using System;

namespace SQLite
{
  public class SQLiteException : Exception
  {
    protected SQLiteException(SQLite3.Result r, string message)
      : base(message)
    {
      this.Result = r;
    }

    public SQLite3.Result Result { get; private set; }

    public static SQLiteException New(SQLite3.Result r, string message) => new SQLiteException(r, message);
  }
}
