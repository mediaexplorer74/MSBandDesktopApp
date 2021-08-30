// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Sync.CloudNotificationUpdatesSyncTask
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Debugging;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Sync
{
  [SyncTask(SyncTaskType.Background)]
  public class CloudNotificationUpdatesSyncTask : IBandSyncTask
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Sync\\CloudNotificationUpdatesSyncTask.cs");
    private ICloudNotificationUpdateService cloudNotificationUpdateService;

    public CloudNotificationUpdatesSyncTask(
      ICloudNotificationUpdateService cloudNotificationUpdateService)
    {
      this.cloudNotificationUpdateService = cloudNotificationUpdateService;
    }

    public async Task RunAsync(
      BandSyncContext context,
      CancellationToken cancellationToken,
      SyncDebugResult debugResults = null)
    {
      CloudNotificationUpdatesSyncTask.Logger.Info((object) "<START> scheduled task : updates sync");
      try
      {
        await this.cloudNotificationUpdateService.HandleAvailableUpdatesAsync(cancellationToken);
      }
      finally
      {
        CloudNotificationUpdatesSyncTask.Logger.Info((object) "<END> scheduled task : updates sync");
      }
    }
  }
}
