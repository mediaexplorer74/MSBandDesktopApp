// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Sync.GuidedWorkoutsSyncTask
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Services.Debugging;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Sync
{
  [SyncTask(SyncTaskType.Background)]
  public class GuidedWorkoutsSyncTask : IBandSyncTask
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Sync\\GuidedWorkoutsSyncTask.cs");
    private IWorkoutsProvider workoutProvider;

    public GuidedWorkoutsSyncTask(IWorkoutsProvider workoutProvider) => this.workoutProvider = workoutProvider;

    public async Task RunAsync(
      BandSyncContext context,
      CancellationToken cancellationToken,
      SyncDebugResult debugResults = null)
    {
      GuidedWorkoutsSyncTask.Logger.Info((object) "<START> scheduled task : guided workouts sync");
      Stopwatch timer = Stopwatch.StartNew();
      try
      {
        await this.workoutProvider.SyncNextWorkoutAsync(cancellationToken);
      }
      finally
      {
        timer.Stop();
        if (debugResults != null)
          debugResults.GuidedWorkout = timer.ElapsedMilliseconds;
        GuidedWorkoutsSyncTask.Logger.Info((object) "<END> scheduled task : guided workouts sync");
      }
    }
  }
}
