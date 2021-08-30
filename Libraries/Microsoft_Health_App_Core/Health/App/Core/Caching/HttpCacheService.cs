// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Caching.HttpCacheService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Caching
{
  public class HttpCacheService : IDebuggableHttpCacheService, IHttpCacheService, IAppUpgradeListener
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Caching\\HttpCacheService.cs");
    private static readonly TimeSpan ScavengeInterval = TimeSpan.FromDays(2.0);
    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromDays(7.0);
    private static readonly Dictionary<string, TimeSpan> CacheDurations = new Dictionary<string, TimeSpan>()
    {
      {
        "Sync",
        TimeSpan.FromHours(1.0)
      },
      {
        "Events",
        TimeSpan.FromDays(1.0)
      },
      {
        "UserProfile",
        TimeSpan.FromDays(1.0)
      },
      {
        "Goals",
        TimeSpan.FromDays(1.0)
      },
      {
        "Insights",
        TimeSpan.FromHours(1.0)
      },
      {
        "WorkoutGeneral",
        TimeSpan.FromDays(1.0)
      },
      {
        "WorkoutFavorites",
        TimeSpan.FromDays(1.0)
      },
      {
        "WorkoutPlan",
        TimeSpan.FromHours(1.0)
      },
      {
        "Golf",
        TimeSpan.FromDays(1.0)
      },
      {
        "GolfRecent",
        TimeSpan.FromMinutes(30.0)
      },
      {
        "WellnessSchedule",
        TimeSpan.FromHours(1.0)
      },
      {
        "WellnessPlan",
        TimeSpan.FromHours(1.0)
      },
      {
        "Weights",
        TimeSpan.FromDays(1.0)
      }
    };
    private readonly IHttpCacheDatabase httpCacheDatabase;
    private readonly IDateTimeService dateTimeService;
    private AsyncLock sync = new AsyncLock();
    private DateTimeOffset lastScavengeTime = DateTimeOffset.MinValue;

    public HttpCacheService(IHttpCacheDatabase httpCacheDatabase, IDateTimeService dateTimeService)
    {
      Assert.ParamIsNotNull((object) httpCacheDatabase, nameof (httpCacheDatabase));
      Assert.ParamIsNotNull((object) dateTimeService, nameof (dateTimeService));
      this.httpCacheDatabase = httpCacheDatabase;
      this.dateTimeService = dateTimeService;
    }

    public DateTimeOffset LastInvalidatedSyncTime { get; set; }

    public async Task InsertAsync(
      Uri url,
      IHttpResponseContent item,
      string cacheArea,
      IEnumerable<string> tags = null)
    {
      try
      {
        AsyncLock.Releaser releaser = await this.sync.LockAsync();
        try
        {
          await this.ScavengeIfNeededAsync();
          DateTimeOffset dateTimeOffset = this.dateTimeService.Now + (cacheArea == null || !HttpCacheService.CacheDurations.ContainsKey(cacheArea) ? HttpCacheService.DefaultCacheDuration : HttpCacheService.CacheDurations[cacheArea]);
          List<string> stringList = new List<string>();
          if (cacheArea != null)
            stringList.Add(cacheArea);
          if (tags != null)
            stringList.AddRange(tags);
          await this.httpCacheDatabase.InsertAsync(new HttpCacheItem()
          {
            Url = url.ToString(),
            Tags = (IList<string>) stringList,
            CharacterSet = item.CharacterSet,
            ContentEncoding = item.ContentEncoding,
            ResponseBytes = item.ResponseBytes,
            Expiration = dateTimeOffset
          });
        }
        finally
        {
          releaser.Dispose();
        }
        releaser = new AsyncLock.Releaser();
      }
      catch (Exception ex)
      {
        HttpCacheService.Logger.Warn((object) "Unable to insert item due to exception.", ex);
      }
    }

    public async Task<IHttpResponseContent> GetAsync(Uri url)
    {
      try
      {
        using (await this.sync.LockAsync())
        {
          HttpCacheItem item = await this.httpCacheDatabase.GetAsync(url.ToString());
          if (item == null)
            return (IHttpResponseContent) null;
          if (!(item.Expiration < this.dateTimeService.Now))
            return (IHttpResponseContent) item;
          await this.httpCacheDatabase.RemoveAsync(url.ToString());
          return (IHttpResponseContent) null;
        }
      }
      catch (Exception ex)
      {
        HttpCacheService.Logger.Warn((object) "Unable to get item due to exception.", ex);
        return (IHttpResponseContent) null;
      }
    }

    public async Task RemoveAsync(Uri url)
    {
      try
      {
        AsyncLock.Releaser releaser = await this.sync.LockAsync();
        try
        {
          await this.httpCacheDatabase.RemoveAsync(url.ToString());
        }
        finally
        {
          releaser.Dispose();
        }
        releaser = new AsyncLock.Releaser();
      }
      catch (Exception ex)
      {
        HttpCacheService.Logger.Warn((object) "Unable to remove item due to exception.", ex);
      }
    }

    public async Task RemoveTagsAsync(params string[] tags)
    {
      try
      {
        AsyncLock.Releaser releaser = await this.sync.LockAsync();
        try
        {
          await this.httpCacheDatabase.RemoveTagsAsync(tags);
        }
        finally
        {
          releaser.Dispose();
        }
        releaser = new AsyncLock.Releaser();
      }
      catch (Exception ex)
      {
        HttpCacheService.Logger.Warn((object) "Unable to remove tags due to exception.", ex);
      }
    }

    public async Task AddTagAsync(Uri url, string tag)
    {
      try
      {
        AsyncLock.Releaser releaser = await this.sync.LockAsync();
        try
        {
          HttpCacheItem async = await this.httpCacheDatabase.GetAsync(url.ToString());
          if (async != null)
          {
            IList<string> tags = async.Tags;
            if (!tags.Contains(tag))
            {
              tags.Add(tag);
              async.Tags = tags;
              await this.httpCacheDatabase.UpdateAsync(async);
            }
          }
        }
        finally
        {
          releaser.Dispose();
        }
        releaser = new AsyncLock.Releaser();
      }
      catch (Exception ex)
      {
        HttpCacheService.Logger.Warn((object) "Unable to add tag to item due to exception.", ex);
      }
    }

    public async Task RemoveAllAsync()
    {
      try
      {
        AsyncLock.Releaser releaser = await this.sync.LockAsync();
        try
        {
          await this.httpCacheDatabase.RemoveAllAsync();
        }
        finally
        {
          releaser.Dispose();
        }
        releaser = new AsyncLock.Releaser();
      }
      catch (Exception ex)
      {
        HttpCacheService.Logger.Warn((object) "Unable to remove all items due to exception.", ex);
      }
    }

    public async Task<IList<IHttpCacheItem>> GetAllAsync()
    {
      try
      {
        using (await this.sync.LockAsync())
          return (IList<IHttpCacheItem>) (await this.httpCacheDatabase.GetAllAsync()).Cast<IHttpCacheItem>().ToList<IHttpCacheItem>();
      }
      catch (Exception ex)
      {
        HttpCacheService.Logger.Warn((object) "Unable to get all items due to exception.", ex);
        return (IList<IHttpCacheItem>) new List<IHttpCacheItem>();
      }
    }

    protected async Task ScavengeIfNeededAsync()
    {
      DateTimeOffset now = this.dateTimeService.Now;
      if (this.lastScavengeTime + HttpCacheService.ScavengeInterval > now)
        return;
      await this.httpCacheDatabase.RemoveAllBeforeAsync(now);
      this.lastScavengeTime = now;
    }

    async Task IAppUpgradeListener.OnAppUpgradeAsync(
      Version newVersion,
      Version oldVersion,
      CancellationToken token)
    {
      HttpCacheService.Logger.Debug((object) "<START> enforcing clear cache on app upgrade policy");
      await this.RemoveAllAsync();
      HttpCacheService.Logger.Debug((object) "<END> enforcing clear cache on app upgrade policy");
    }
  }
}
