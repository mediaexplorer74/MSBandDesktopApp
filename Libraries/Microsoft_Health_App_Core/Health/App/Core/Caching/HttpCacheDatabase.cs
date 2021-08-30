// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Caching.HttpCacheDatabase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Database;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Caching
{
  public sealed class HttpCacheDatabase : CoreDatabase, IHttpCacheDatabase
  {
    private readonly string tableName = typeof (HttpCacheItem).Name;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Caching\\HttpCacheDatabase.cs");
    private IFileSystemService fileSystemService;

    protected override int CurrentDatabaseVersion => 1;

    public HttpCacheDatabase(
      ISQLiteSingleConnection sqlLiteSingleConnection,
      IFileSystemService fileSystemService)
      : base(sqlLiteSingleConnection)
    {
      this.fileSystemService = fileSystemService;
    }

    protected override bool ConstructIfNeeded(SQLiteConnection connection)
    {
      if (this.TableExists<HttpCacheItem>(connection))
        return false;
      connection.CreateTable<HttpCacheItem>();
      connection.CreateCommand("PRAGMA user_version = " + (object) this.CurrentDatabaseVersion).ExecuteNonQuery();
      return true;
    }

    protected override async Task UpgradeDatabaseAsync(
      int fromVersion,
      int toVersion,
      SQLiteConnection connection)
    {
      await this.DropAndCreateTableAsync(connection).ConfigureAwait(false);
    }

    private async Task DropAndCreateTableAsync(SQLiteConnection connection)
    {
      if (this.TableExists<HttpCacheItem>(connection))
      {
        connection.DropTable<HttpCacheItem>();
        await this.CleanupCacheFolderAsync().ConfigureAwait(false);
      }
      connection.CreateTable<HttpCacheItem>();
    }

    public async Task<HttpCacheItem> GetAsync(string key)
    {
      HttpCacheItem cacheItem = (HttpCacheItem) null;
      HttpCacheItem httpCacheItem = await this.RunAsync<HttpCacheItem>((Func<SQLiteConnection, Task<HttpCacheItem>>) (async connection =>
      {
        cacheItem = connection.Find<HttpCacheItem>((object) key);
        await this.ReadCacheFileAsync(cacheItem).ConfigureAwait(false);
        return cacheItem;
      })).ConfigureAwait(false);
      return cacheItem;
    }

    public async Task InsertAsync(HttpCacheItem item)
    {
      int num = await this.RunAsync<bool>((Func<SQLiteConnection, Task<bool>>) (async connection =>
      {
        await this.WriteCacheFileAsync(item).ConfigureAwait(false);
        connection.RunInTransaction((Action) (() => connection.InsertOrReplace((object) item)));
        return true;
      })).ConfigureAwait(false) ? 1 : 0;
    }

    public async Task UpdateAsync(HttpCacheItem item)
    {
      int num = await this.RunAsync<bool>((Func<SQLiteConnection, Task<bool>>) (async connection =>
      {
        ConfiguredTaskAwaitable configuredTaskAwaitable = this.DeleteCacheFileAsync(item).ConfigureAwait(false);
        await configuredTaskAwaitable;
        configuredTaskAwaitable = this.WriteCacheFileAsync(item).ConfigureAwait(false);
        await configuredTaskAwaitable;
        connection.RunInTransaction((Action) (() => connection.Update((object) item)));
        return true;
      })).ConfigureAwait(false) ? 1 : 0;
    }

    public async Task RemoveAsync(string key)
    {
      int num = await this.RunAsync<bool>((Func<SQLiteConnection, Task<bool>>) (async connection =>
      {
        await this.DeleteCacheFileAsync(await this.GetAsync(key)).ConfigureAwait(false);
        connection.RunInTransaction((Action) (() => connection.Delete<HttpCacheItem>((object) key)));
        return true;
      })).ConfigureAwait(false) ? 1 : 0;
    }

    public async Task<IList<HttpCacheItem>> GetAllAsync()
    {
      List<HttpCacheItem> result = (List<HttpCacheItem>) null;
      List<HttpCacheItem> httpCacheItemList = result;
      result = await this.RunAsync<List<HttpCacheItem>>((Func<SQLiteConnection, Task<List<HttpCacheItem>>>) (async connection =>
      {
        connection.RunInTransaction((Action) (() => result = connection.Query<HttpCacheItem>("SELECT * FROM " + this.tableName)));
        foreach (HttpCacheItem httpCacheItem in result)
          await this.ReadCacheFileAsync(httpCacheItem).ConfigureAwait(false);
        return result;
      })).ConfigureAwait(false);
      return (IList<HttpCacheItem>) result;
    }

    public async Task RemoveAllAsync()
    {
      int num = await this.RunAsync<bool>((Func<SQLiteConnection, Task<bool>>) (async connection =>
      {
        connection.RunInTransaction((Action) (() => connection.DeleteAll<HttpCacheItem>()));
        await this.CleanupCacheFolderAsync().ConfigureAwait(false);
        return true;
      })).ConfigureAwait(false) ? 1 : 0;
    }

    private async Task CleanupCacheFolderAsync() => await (await this.fileSystemService.GetCacheFolderAsync()).DeleteAsync().ConfigureAwait(false);

    public async Task RemoveTagsAsync(params string[] tags)
    {
      if (!((IEnumerable<string>) tags).Any<string>())
        return;
      string[] likeTags = ((IEnumerable<string>) tags).Select<string, string>((Func<string, string>) (tag => "%" + tag + "%")).ToArray<string>();
      int num = await this.RunAsync<bool>((Func<SQLiteConnection, Task<bool>>) (async connection =>
      {
        List<HttpCacheItem> items = (List<HttpCacheItem>) null;
        string queryString = string.Format("SELECT * FROM {0} WHERE ", new object[1]
        {
          (object) this.tableName
        }) + string.Join(" OR ", ((IEnumerable<string>) likeTags).Select<string, string>((Func<string, string>) (tag => string.Format("TagsStorage LIKE '{0}'", new object[1]
        {
          (object) tag
        }))));
        connection.RunInTransaction((Action) (() => items = connection.Query<HttpCacheItem>(queryString)));
        if (items != null)
        {
          foreach (HttpCacheItem httpCacheItem in items)
          {
            HttpCacheItem item = httpCacheItem;
            await this.DeleteCacheFileAsync(item).ConfigureAwait(false);
            connection.RunInTransaction((Action) (() => connection.Delete<HttpCacheItem>((object) item.Url)));
          }
        }
        return true;
      })).ConfigureAwait(false) ? 1 : 0;
    }

    public async Task RemoveAllBeforeAsync(DateTimeOffset now)
    {
      int num = await this.RunAsync<bool>((Func<SQLiteConnection, Task<bool>>) (async connection =>
      {
        List<HttpCacheItem> items = (List<HttpCacheItem>) null;
        connection.RunInTransaction((Action) (() => items = connection.Query<HttpCacheItem>("SELECT * FROM " + this.tableName + " WHERE Expiration < ?", (object) now.ToString())));
        if (items != null)
        {
          foreach (HttpCacheItem httpCacheItem in items)
          {
            HttpCacheItem item = httpCacheItem;
            await this.DeleteCacheFileAsync(item).ConfigureAwait(false);
            connection.RunInTransaction((Action) (() => connection.Delete<HttpCacheItem>((object) item.Url)));
          }
        }
        return true;
      })).ConfigureAwait(false) ? 1 : 0;
    }

    private new bool TableExists<T>(SQLiteConnection connection) => connection.CreateCommand("SELECT name FROM sqlite_master WHERE type='table' AND name=?", (object) typeof (T).Name).ExecuteScalar<string>() != null;

    internal async Task WriteCacheFileAsync(HttpCacheItem item)
    {
      if (item == null)
        return;
      IFile file = await (await this.fileSystemService.GetCacheFolderAsync().ConfigureAwait(false)).CreateFileAsync(Guid.NewGuid().ToString(), CreationCollisionOption.GenerateUniqueName).ConfigureAwait(false);
      await file.WriteAllBytesAsync(item.ResponseBytes).ConfigureAwait(false);
      item.CacheFileName = file.Name;
      file = (IFile) null;
    }

    internal async Task ReadCacheFileAsync(HttpCacheItem item)
    {
      if (item == null || item.CacheFileName == null)
        return;
      IFile file = await (await this.fileSystemService.GetCacheFolderAsync().ConfigureAwait(false)).GetFileAsync(item.CacheFileName).ConfigureAwait(false);
      HttpCacheItem httpCacheItem = item;
      CancellationToken token = new CancellationToken();
      byte[] numArray = await file.ReadAllBytesAsync(token).ConfigureAwait(false);
      httpCacheItem.ResponseBytes = numArray;
      httpCacheItem = (HttpCacheItem) null;
    }

    internal async Task DeleteCacheFileAsync(HttpCacheItem item)
    {
      if (item == null || item.CacheFileName == null)
        return;
      IFile file = await (await this.fileSystemService.GetCacheFolderAsync().ConfigureAwait(false)).TryGetFileAsync(item.CacheFileName).ConfigureAwait(false);
      if (file != null)
        await file.DeleteAsync().ConfigureAwait(false);
      else
        HttpCacheDatabase.Logger.Info("File {0} does not exist for deletion", (object) item.CacheFileName);
    }
  }
}
