// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.IWorkoutsProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public interface IWorkoutsProvider
  {
    DateTimeOffset? LastSubscriptionChanged { get; }

    event EventHandler SubscriptionChanged;

    Task<IList<BrowseCategory>> GetWorkoutBrandsAsync();

    Task<IList<DisplaySubType>> GetWorkoutTypesAsync();

    Task<WorkoutSearch> GetWorkoutsAsync(
      CancellationToken cancellationToken,
      string query,
      WorkoutPublisher workoutPublisher,
      IList<WorkoutSearchFilter> filters);

    Task<WorkoutPlanDetail> GetWorkoutAsync(string id);

    Task<IList<FavoriteWorkout>> GetFavoriteWorkoutsAsync(
      CancellationToken cancellationToken);

    Task FavoriteWorkoutAsync(string id);

    Task UnFavoriteWorkoutAsync(string id);

    Task SubscribeWorkoutAsync(string id);

    Task UnsubscribeWorkoutAsync(string id);

    Task<IList<WorkoutStatus>> GetWorkoutsInPlanAsync(
      string workoutPlanId,
      string instanceId);

    Task<WorkoutStatus> GetLastSyncedWorkoutAsync(
      CancellationToken cancellationToken);

    Task SetLastSyncedWorkoutAsync(
      string workoutPlanId,
      int workoutIndex,
      int weekId,
      int dayId,
      int workoutPlanInstanceId);

    Task SetNextWorkoutAsync(string workoutPlanId, int workoutIndex, int weekId, int dayId);

    Task SkipWorkoutAsync(string workoutPlanId, int workoutIndex, int week, int day);

    Task<IList<FeaturedWorkout>> GetFeaturedWorkoutsAsync();

    Task<GuidedWorkoutState> GetWorkoutStateAsync(
      CancellationToken cancellationToken);

    Task UploadWorkoutBandFileAsync(
      string workoutPlanId,
      int workoutIndex,
      int weekId,
      int dayId,
      int workoutPlanInstanceId,
      CancellationToken cancellationToken,
      GuidedWorkoutSyncMode mode = GuidedWorkoutSyncMode.SyncPlan);

    Task<string> GetVideoUrlAsync(string id, double screenWidth, double screenHeight);

    Task<WorkoutEvent> GetWorkoutEventAsync(string eventId);

    Task PatchWorkoutEventAsync(string eventId, string name);

    Task DeleteWorkoutEventAsync(string eventId);

    Task<WorkoutEvent> GetLastCompletedWorkoutAsync();

    Task<IList<WorkoutWeekGrouping>> GetScheduleAsync(
      string workoutPlanId,
      int workoutPlanInstanceId = -1,
      bool isSubscribed = false);

    Task<GuidedWorkoutTileState> GetGuidedWorkoutTileStateAsync();

    Task<string> GetWorkoutNameAsync(string workoutPlanId, int weekId, int dayId);

    Task<GuidedWorkoutInfo> GetSubscribedWorkoutAsync();

    Task SyncNextWorkoutAsync(CancellationToken cancellationToken);
  }
}
