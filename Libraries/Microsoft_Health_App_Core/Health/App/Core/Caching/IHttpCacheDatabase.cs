// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Caching.IHttpCacheDatabase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Caching
{
  public interface IHttpCacheDatabase
  {
    Task<HttpCacheItem> GetAsync(string key);

    Task<IList<HttpCacheItem>> GetAllAsync();

    Task InsertAsync(HttpCacheItem item);

    Task UpdateAsync(HttpCacheItem item);

    Task RemoveAsync(string key);

    Task RemoveAllAsync();

    Task RemoveTagsAsync(string[] tags);

    Task RemoveAllBeforeAsync(DateTimeOffset now);
  }
}
