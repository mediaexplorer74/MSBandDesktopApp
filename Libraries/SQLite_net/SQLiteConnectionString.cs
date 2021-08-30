// Decompiled with JetBrains decompiler
// Type: SQLite.SQLiteConnectionString
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

namespace SQLite
{
  internal class SQLiteConnectionString
  {
    public SQLiteConnectionString(string databasePath, bool storeDateTimeAsTicks)
    {
      this.ConnectionString = databasePath;
      this.StoreDateTimeAsTicks = storeDateTimeAsTicks;
      this.DatabasePath = databasePath;
    }

    public string ConnectionString { get; private set; }

    public string DatabasePath { get; private set; }

    public bool StoreDateTimeAsTicks { get; private set; }
  }
}
