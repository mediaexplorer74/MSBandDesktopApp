// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Database.CoreDatabase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Caching;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using SQLite;
using System;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Database
{
  public abstract class CoreDatabase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Database\\CoreDatabase.cs");
    private ISQLiteSingleConnection sqlLiteSingleConnection;

    protected abstract int CurrentDatabaseVersion { get; }

    public CoreDatabase(ISQLiteSingleConnection sqlLiteSingleConnection)
    {
      this.sqlLiteSingleConnection = sqlLiteSingleConnection;
      this.RunAsync<bool>((Func<SQLiteConnection, Task<bool>>) (async connection =>
      {
        connection.RunInTransaction((Action) (() =>
        {
          if (!this.ConstructIfNeeded(connection))
            return;
          this.SetDatabaseVersion(this.CurrentDatabaseVersion, connection);
        }));
        await this.CheckForDatabaseUpgradeAsync(connection).ConfigureAwait(false);
        return true;
      })).Wait();
    }

    private async Task CheckForDatabaseUpgradeAsync(SQLiteConnection connection)
    {
      int fromVersion = (int) connection.ExecuteScalar<long>("PRAGMA user_version");
      int currentDatabaseVersion = this.CurrentDatabaseVersion;
      if (fromVersion == currentDatabaseVersion)
        return;
      await this.UpgradeDatabaseAsync(fromVersion, currentDatabaseVersion, connection);
      this.SetDatabaseVersion(currentDatabaseVersion, connection);
    }

    private void SetDatabaseVersion(int version, SQLiteConnection connection) => connection.CreateCommand("PRAGMA user_version = " + (object) this.CurrentDatabaseVersion).ExecuteNonQuery();

    protected Task<T> RunAsync<T>(Func<SQLiteConnection, Task<T>> function) => Task.Run<T>((Func<Task<T>>) (async () =>
    {
      T obj;
      try
      {
        obj = await function(this.sqlLiteSingleConnection.GetConnection()).ConfigureAwait(false);
      }
      finally
      {
        this.sqlLiteSingleConnection.ReleaseConnection();
      }
      return obj;
    }));

    protected bool TableExists<T>(SQLiteConnection connection) => connection.CreateCommand("SELECT name FROM sqlite_master WHERE type='table' AND name=?", (object) typeof (T).Name).ExecuteScalar<string>() != null;

    protected abstract bool ConstructIfNeeded(SQLiteConnection connection);

    protected abstract Task UpgradeDatabaseAsync(
      int fromVersion,
      int toVersion,
      SQLiteConnection connection);
  }
}
