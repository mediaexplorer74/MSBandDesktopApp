// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Sync.UpdateGuidedWorkoutTask
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Caching;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Services.Debugging;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Caching;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Sync
{
  [SyncTask(SyncTaskType.Foreground)]
  public class UpdateGuidedWorkoutTask : IBandSyncTask
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Sync\\UpdateGuidedWorkoutTask.cs");
    private readonly IDebuggableHttpCacheService cacheService;
    private readonly IWorkoutsProvider workoutsProvider;

    public UpdateGuidedWorkoutTask(
      IDebuggableHttpCacheService cacheService,
      IWorkoutsProvider workoutsProvider)
    {
      this.cacheService = cacheService;
      this.workoutsProvider = workoutsProvider;
    }

    public async Task RunAsync(
      BandSyncContext context,
      CancellationToken cancellationToken,
      SyncDebugResult debugResults = null)
    {
      UpdateGuidedWorkoutTask.Logger.Info((object) "<START> task : UpdateGuidedWorkout");
      try
      {
        await this.cacheService.RemoveTagsAsync("WorkoutPlan");
        await this.cacheService.RemoveTagsAsync(CloudCacheTag.ForEventType(EventType.GuidedWorkout));
        GuidedWorkoutState workoutStateAsync = await this.workoutsProvider.GetWorkoutStateAsync(CancellationToken.None);
        if (workoutStateAsync.State == GuidedWorkoutSyncState.SyncRequired)
          await this.workoutsProvider.UploadWorkoutBandFileAsync(workoutStateAsync.WorkoutInfo.WorkoutPlanId, workoutStateAsync.WorkoutInfo.WorkoutIndex, workoutStateAsync.WorkoutInfo.WeekId, workoutStateAsync.WorkoutInfo.DayId, workoutStateAsync.WorkoutInfo.WorkoutPlanInstanceId, CancellationToken.None).ConfigureAwait(false);
        else
          UpdateGuidedWorkoutTask.Logger.Debug((object) "<FLAG> update tiles : guided workouts : no next guided workouts found");
        UpdateGuidedWorkoutTask.Logger.Debug((object) "<END> update tiles : guided workouts");
      }
      catch (Exception ex)
      {
        UpdateGuidedWorkoutTask.Logger.Error((object) "<FAILED> task : UpdateGuidedWorkout", ex);
        throw;
      }
      finally
      {
        UpdateGuidedWorkoutTask.Logger.Info((object) "<END> scheduled task : UpdateGuidedWorkout");
      }
    }
  }
}
