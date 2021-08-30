// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Sync.UpdateTilesTask
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Debugging;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Sync
{
  [SyncTask(SyncTaskType.Foreground | SyncTaskType.Background)]
  public class UpdateTilesTask : IBandSyncTask
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Sync\\UpdateTilesTask.cs");
    private ITileUpdateService tileUpdateService;

    public UpdateTilesTask(ITileUpdateService tileUpdateService) => this.tileUpdateService = tileUpdateService;

    public async Task RunAsync(
      BandSyncContext context,
      CancellationToken cancellationToken,
      SyncDebugResult debugResults = null)
    {
      UpdateTilesTask.Logger.Debug((object) "<START> scheduled task : update tiles");
      try
      {
        using (ITimedTelemetryEvent timedEvent = ApplicationTelemetry.TimeTilesUpdateSync())
          await this.tileUpdateService.UpdateTilesAsync(cancellationToken, debugResults, timedEvent);
      }
      finally
      {
        UpdateTilesTask.Logger.Debug((object) "<END> scheduled task : update tiles");
      }
    }
  }
}
