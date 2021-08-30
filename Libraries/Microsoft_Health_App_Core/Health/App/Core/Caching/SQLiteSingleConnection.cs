// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Caching.SQLiteSingleConnection
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using SQLite;

namespace Microsoft.Health.App.Core.Caching
{
  internal class SQLiteSingleConnection : ISQLiteSingleConnection
  {
    private int openCount;
    private SQLiteConnection sqlLiteConnection;
    private ISQLiteService sqlLiteservice;
    private object lockObject = new object();

    public SQLiteSingleConnection(ISQLiteService sqlLiteservice) => this.sqlLiteservice = sqlLiteservice;

    public SQLiteConnection GetConnection()
    {
      lock (this.lockObject)
      {
        if (this.sqlLiteConnection == null)
        {
          this.sqlLiteConnection = this.sqlLiteservice.GetConnection();
          this.openCount = 0;
        }
        ++this.openCount;
        return this.sqlLiteConnection;
      }
    }

    public void ReleaseConnection()
    {
      lock (this.lockObject)
      {
        --this.openCount;
        if (this.openCount != 0)
          return;
        this.sqlLiteConnection.Dispose();
        this.sqlLiteConnection = (SQLiteConnection) null;
      }
    }
  }
}
