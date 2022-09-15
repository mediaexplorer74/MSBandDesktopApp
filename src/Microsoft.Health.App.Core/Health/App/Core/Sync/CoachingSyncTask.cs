// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Sync.CoachingSyncTask
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Debugging;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Sync
{
  [SyncTask(SyncTaskType.Foreground | SyncTaskType.Background)]
  public class CoachingSyncTask : IBandSyncTask
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Sync\\CoachingSyncTask.cs");
    private readonly ICoachingService coachingService;
    private readonly ICloudNotificationUpdateService cloudNotificationService;

    public CoachingSyncTask(
      ICoachingService coachingService,
      ICloudNotificationUpdateService cloudNotificationService)
    {
      this.coachingService = coachingService;
      this.cloudNotificationService = cloudNotificationService;
    }

    public async Task RunAsync(
      BandSyncContext context,
      CancellationToken cancellationToken,
      SyncDebugResult debugResults = null)
    {
      CoachingSyncTask.Logger.Info((object) "<START> scheduled task : Coaching sync");
      try
      {
        int num = await this.coachingService.RefreshIsPlanActiveAsync(cancellationToken).ConfigureAwait(false) ? 1 : 0;
        await this.coachingService.ClearSpecialRemindersCacheAsync(cancellationToken).ConfigureAwait(false);
        ConfiguredTaskAwaitable configuredTaskAwaitable = this.coachingService.SyncSpecialRemindersAsync(cancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable;
        Stopwatch timer = Stopwatch.StartNew();
        configuredTaskAwaitable = this.coachingService.SyncGoalsToBandAsync(cancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable;
        timer.Stop();
        if (debugResults != null)
          debugResults.Goals = timer.ElapsedMilliseconds;
        configuredTaskAwaitable = this.cloudNotificationService.ClearUpdatesAsync((IList<string>) null, cancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable;
        timer = (Stopwatch) null;
      }
      finally
      {
        CoachingSyncTask.Logger.Info((object) "<END> scheduled task : Coaching sync");
      }
    }
  }
}
