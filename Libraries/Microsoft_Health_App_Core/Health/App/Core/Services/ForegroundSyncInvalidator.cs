// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ForegroundSyncInvalidator
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Caching;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class ForegroundSyncInvalidator : ISyncInvalidator
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\ForegroundSyncInvalidator.cs");
    private IDebuggableHttpCacheService cacheService;
    private INetworkService networkService;

    public ForegroundSyncInvalidator(
      IDebuggableHttpCacheService cacheService,
      INetworkService networkService)
    {
      this.cacheService = cacheService;
      this.networkService = networkService;
    }

    public async Task InvalidateAsync(int deviceCount, bool succeeded)
    {
      if (!this.networkService.IsInternetAvailable)
        return;
      ForegroundSyncInvalidator.Logger.Debug((object) "Removing sync invalidated items from cache");
      await this.cacheService.RemoveTagsAsync("Sync", "WorkoutFavorites");
    }
  }
}
